require 'bundler/setup'
require 'rubygems/package_task'

COMPILE_TARGET = ENV['config'].nil? ? "Debug" : ENV['config'] # Keep this in sync w/ VS settings since Mono is case-sensitive
CLR_TOOLS_VERSION = "v4.0.30319"

buildsupportfiles = Dir["#{File.dirname(__FILE__)}/buildsupport/*.rb"]

if( ! buildsupportfiles.any? )
  # no buildsupport, let's go get it for them.
  sh 'git submodule update --init' unless buildsupportfiles.any?
  buildsupportfiles = Dir["#{File.dirname(__FILE__)}/buildsupport/*.rb"]
end

# nope, we still don't have buildsupport. Something went wrong.
raise "Run `git submodule update --init` to populate your buildsupport folder." unless buildsupportfiles.any?

buildsupportfiles.each { |ext| load ext }

include FileTest
require 'albacore'
load "VERSION.txt"

RESULTS_DIR = "results"
PRODUCT = "FubuWorld"
COPYRIGHT = 'Copyright 2012 FubuWorld. All rights reserved.';
COMMON_ASSEMBLY_INFO = 'src/CommonAssemblyInfo.cs';
BUILD_DIR = File.expand_path("build")
ARTIFACTS = File.expand_path("artifacts")

tc_build_number = ENV["BUILD_NUMBER"]
build_revision = tc_build_number || Time.new.strftime('5%H%M')
BUILD_NUMBER = "#{BUILD_VERSION}.#{build_revision}"

props = { :stage => BUILD_DIR, :artifacts => ARTIFACTS }

desc "**Default**, compiles and runs tests"
task :default => [:compile, :unit_test]

desc "Target used for the CI server"
task :ci => [:update_all_dependencies, :default, :package]

desc "Update the version information for the build"
assemblyinfo :version do |asm|
  asm_version = BUILD_VERSION + ".0"
  
  begin
    commit = `git log -1 --pretty=format:%H`
  rescue
    commit = "git unavailable"
  end
  puts "##teamcity[buildNumber '#{BUILD_NUMBER}']" unless tc_build_number.nil?
  puts "Version: #{BUILD_NUMBER}" if tc_build_number.nil?
  asm.trademark = commit
  asm.product_name = PRODUCT
  asm.description = BUILD_NUMBER
  asm.version = asm_version
  asm.file_version = BUILD_NUMBER
  asm.custom_attributes :AssemblyInformationalVersion => asm_version
  asm.copyright = COPYRIGHT
  asm.output_file = COMMON_ASSEMBLY_INFO
end

desc "Prepares the working directory for a new build"
task :clean => [:update_buildsupport] do
	
	FileUtils.rm_rf props[:stage]
    # work around nasty latency issue where folder still exists for a short while after it is removed
    waitfor { !exists?(props[:stage]) }
	Dir.mkdir props[:stage]
    
	Dir.mkdir props[:artifacts] unless exists?(props[:artifacts])
end

def waitfor(&block)
  checks = 0
  until block.call || checks >10 
    sleep 0.5
    checks += 1
  end
  raise 'waitfor timeout expired' if checks > 10
end

desc "Compiles the app"
task :compile => [:clean, :restore_if_missing, :aliases, :version] do
  bottles("assembly-pak src/FubuWorld -p FubuWorld.csproj")
  bottles("assembly-pak src/FubuDocs -p FubuDocs.csproj")
  bottles("assembly-pak src/FubuMVC.Plugin.Docs -p FubuMVC.Plugin.Docs.csproj")
  bottles("assembly-pak src/Sample.Docs -p Sample.Docs.csproj")
  bottles("assembly-pak src/Imported.Docs -p Imported.Docs.csproj")

  MSBuildRunner.compile :compilemode => COMPILE_TARGET, :solutionfile => 'src/FubuWorld.sln', :clrversion => CLR_TOOLS_VERSION

  target = COMPILE_TARGET.downcase
end

def copyOutputFiles(fromDir, filePattern, outDir)
  Dir.glob(File.join(fromDir, filePattern)){|file| 		
	copy(file, outDir, :preserve => true) if File.file?(file)
  } 
end

desc "Runs unit tests"
task :test => [:unit_test]

desc "Sets up the Bottles/Fubu aliases"
task :aliases => [:restore_if_missing] do
	bottles 'alias fubuworld src/FubuWorld'
end 

desc "Run unit tests"
task :unit_test do 
  runner = NUnitRunner.new :compilemode => COMPILE_TARGET, :source => 'src', :platform => 'x86'
  tests = Array.new
  file = File.new("TESTS.txt", "r")
  assemblies = file.readlines()
  assemblies.each do |a|
	test = a.gsub("\r\n", "").gsub("\n", "")
	tests.push(test)
  end
  file.close
  
  runner.executeTests tests
end

desc "Outputs the command line usage"
task :dump_usages do
  sh "src/FubuDocsRunner/bin/Debug/FubuDocsRunner.exe dump-usages fubudocs src/FubuWorld.Docs/fubudocs.cli.xml"
end

def self.bottles(args)
  bottles = 'src/packages/Bottles/tools/BottleRunner.exe'
  sh "#{bottles} #{args}"
end

def self.fubu(args)
  fubu = Platform.runtime(Nuget.tool("fubu", "fubu.exe"))
  sh "#{fubu} #{args}" 
end

desc "Compiles and copies FubuDocsRunner to the /buildsupport directory parallel to this solution"
task :deploy => [:compile] do
  sh "src/DeployRunner/bin/debug/DeployRunner.exe"
end

def cleanDirectory(dir)
  FileUtils.rm_rf dir;
  waitfor { !exists?(dir) }
  Dir.mkdir dir
end

def cleanFile(file)
  File.delete file unless !File.exist?(file)
end

desc "Recreates and installs the fubudocs gem locally for testing"
task :local_gem => [:compile, :create_gem] do
	sh 'gem uninstall fubudocs'
	Dir.chdir 'pkg'
	sh 'gem install fubudocs'
	Dir.chdir '..'
end

desc "Creates the gem for fubudocs.exe"
task :create_gem do
	cleanDirectory 'lib'
	cleanDirectory 'bin'	
	cleanDirectory 'pkg'
	
	dir = "src/fubudocsrunner/bin/#{COMPILE_TARGET}"

	copyOutputFiles dir, '*.dll', 'bin'

	FileUtils.copy "#{dir}/fubu.exe", 'bin'
	FileUtils.copy "#{dir}/FubuDocsRunner.exe", 'bin/fubudocs.exe'
	FileUtils.copy 'fubudocs', 'bin'
	
	Rake::Task[:gem].invoke
end

	spec = Gem::Specification.new do |s|
	  s.platform    = Gem::Platform::RUBY
	  s.name        = 'fubudocs'
	  s.version     = BUILD_NUMBER
	  s.files = Dir['bin/**/*']
	  s.bindir = 'bin'
	  s.executables << 'fubudocs'
	  
	  s.summary     = 'fubudocs runner'
	  s.description = 'FubuDocs is a tool for generating project documentation using the FubuMVC framework'
	  
	  s.authors           = ['Jeremy D. Miller', 'Josh Arnold']
	  s.email             = 'fubumvc-devel@googlegroups.com'
	  s.homepage          = 'http://fubu-project.org'
	  s.rubyforge_project = 'fubudocs'
	end


Gem::PackageTask.new(spec) do |pkg|
  pkg.need_zip = true
  pkg.need_tar = true
end





