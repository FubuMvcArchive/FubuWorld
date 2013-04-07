using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Bottles;
using Bottles.Commands;
using FubuCore;
using FubuCore.CommandLine;

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
        private static readonly FileSystem fileSystem = new FileSystem();

        public override bool Execute(BottleInput input)
        {
            importSnippets(input);

            string directory = input.DetermineDocumentsFolder();

            bottleize(input, directory);

            return true;
        }

        private static void bottleize(BottleInput input, string directory)
        {
            writeManifestIfNecessary(directory);
            NuspecMaker.CreateNuspecIfMissing(directory);

            if (!input.NoZipFlag)
            {
                bottleItUp(directory);
            }
        }

        private static void bottleItUp(string directory)
        {
            new AssemblyPackageCommand().Execute(new AssemblyPackageInput
            {
                RootFolder = directory.ToFullPath()
            });
        }

        private static void writeManifestIfNecessary(string directory)
        {
            string manifestFile = directory.AppendPath(PackageManifest.FILE);
            if (!File.Exists(manifestFile))
            {
                var manifest = new PackageManifest {ContentFileSet = FileSet.Deep("*.*")};
                string assemblyName = Path.GetFileName(directory);
                manifest.Name = assemblyName;
                manifest.AddAssembly(assemblyName);

                manifest.ContentFileSet.Exclude =
                    "Properties/*;bin/*;obj/*;*.csproj*;packages.config;repositories.config;pak-*.zip;*.sln";

                fileSystem.WriteObjectToFile(manifestFile, manifest);
            }
        }

        private static void importSnippets(BottleInput input)
        {
            new SnippetsCommand().Execute(new SnippetsInput {DirectoryFlag = input.DetermineDocumentsFolder()});
        }
    }

    public static class NuspecMaker
    {
        public static void CreateNuspecIfMissing(string documentationDirectory)
        {
            string name = Path.GetFileName(documentationDirectory);
            string nuspecName = name.ToLower() + ".nuspec";

            string file = ".".ToFullPath().AppendPath("packaging", "nuget", nuspecName);
            var system = new FileSystem();

            if (!system.FileExists(file))
            {
                Console.WriteLine("Creating a nuspec file at " + file);

                string nuspecText = Assembly.GetExecutingAssembly()
                                            .GetManifestResourceStream(typeof (NuspecMaker), "nuspec.txt")
                                            .ReadAllText().Replace("NAME", name);

                Console.WriteLine(nuspecText);

                system.WriteStringToFile(file, nuspecText);
            }
        }
    }
}