using System.ComponentModel;
using System.IO;
using Bottles;
using Bottles.Commands;
using FubuCore.CommandLine;
using FubuCore;
using FubuDocs;
using FubuMVC.Core;
using System.Collections.Generic;
using System.Linq;

namespace FubuDocsRunner
{
    public class BottleInput : DocActionInput
    {
        [Description("If selected, disables the creation of the pak-WebContent.zip file")]
        public bool NoZipFlag { get; set; }
    }

    [CommandDescription("Packages up a documentation project as a FubuWorld bottle")]
    public class BottleCommand : FubuCommand<BottleInput>
    {
        public override bool Execute(BottleInput input)
        {
            var fileSystem = new FileSystem();



            importSnippets(input);

            var directory = input.DetermineDocumentsFolder();
            writeManifestIfNecessary(directory, fileSystem);
            writeFubuDocModuleAttributeIfNecessary(directory, fileSystem);

            if (!input.NoZipFlag)
            {
                bottleItUp(directory);
            }

            return true;
        }

        private static void bottleItUp(string directory)
        {
            new AssemblyPackageCommand().Execute(new AssemblyPackageInput
            {
                RootFolder = directory.ToFullPath()
            });
        }

        private static void writeFubuDocModuleAttributeIfNecessary(string directory, FileSystem fileSystem)
        {
            var assemblyInfoFile = directory.AppendPath("Properties").AppendPath("AssemblyInfo.cs");
            if (File.Exists(assemblyInfoFile))
            {
                fileSystem.AlterFlatFile(assemblyInfoFile, list => {
                    var @using = "using {0};".ToFormat(typeof (FubuDocModuleAttribute).Namespace);
                    if (!list.Contains(@using))
                    {
                        list.Insert(0, @using);
                    }

                    if (!list.Any(x => x.Contains("FubuDocModule")))
                    {
                        list.Add("[assembly: FubuDocModule(\"CHANGEME\")]");
                    }
                });
            }
        }

        private static void writeManifestIfNecessary(string directory, FileSystem fileSystem)
        {
            var manifestFile = directory.AppendPath(PackageManifest.FILE);
            if (!File.Exists(manifestFile))
            {
                var manifest = new PackageManifest {ContentFileSet = FileSet.Deep("*.*")};
                string assemblyName = Path.GetFileName(directory);
                manifest.Name = assemblyName;
                manifest.AddAssembly(assemblyName);

                manifest.ContentFileSet.Exclude = "Properties/*;bin/*;obj/*;*.csproj*;packages.config;repositories.config;pak-*.zip;*.sln";

                fileSystem.WriteObjectToFile(manifestFile, manifest);
            }
        }

        private static void importSnippets(BottleInput input)
        {
            new SnippetsCommand().Execute(new SnippetsInput {DirectoryFlag = input.DetermineDocumentsFolder()});
        }
    }
}