begin
  require 'bundler/setup'
  require 'fuburake'
rescue LoadError
  puts 'Bundler and all the gems need to be installed prior to running this rake script. Installing...'
  system("gem install bundler --source http://rubygems.org")
  sh 'bundle install'
  system("bundle exec rake", *ARGV)
  exit 0
end

@solution = FubuRake::Solution.new do |sln|
	sln.compile = {
		:solutionfile => 'src/FubuWorld.sln'
	}

	sln.assembly_info = {
		:product_name => "FubuWorld",
		:copyright => 'Copyright 2008-2013 Jeremy D. Miller, Joshua Arnold, et al. All rights reserved.'
	}

	sln.ripple_enabled = true
	sln.fubudocs_enabled = true
    
    sln.assembly_bottle 'FubuWorld'
	
	# TODO -- add this later:  , :include_in_ci => true
	sln.export_docs({:repository => 'git@github.com:DarthFubuMVC/FubuRelease.git', :host => 'src/FubuWorld', :prefix => 'website'})
	sln.export_docs({:repository => 'git@github.com:DarthFubuMVC/darthfubumvc.github.io.git', :host => 'src/FubuWorld', :branch => 'master'})
end

desc "Cleans things up before running the docs publishing"
task :pre_docs do
  sh "git clean -xfd"
  sh "ripple update"
end

Rake::Task["website:export"].enhance [:pre_docs]
Rake::Task["docs:export"].enhance [:pre_docs]
