using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Bottles;
using Bottles.Diagnostics;
using Bottles.Manifest;
using FubuCore;
using FubuCore.CommandLine;
using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Packaging;
using FubuMVC.Katana;
using FubuMVC.StructureMap;
using FubuWorld;
using FubuWorld.Infrastructure;
using Container = StructureMap.Container;

namespace FubuDocsRunner
{
    public class RunInput : DocActionInput
    {
        [Description("Disables the bottle and code snippet scanning while this command runs")]
        public bool NoBottlingFlag { get; set; }
    }

    public class RunCommand : FubuCommand<RunInput>
    {
        public override bool Execute(RunInput input)
        {
            string documentDirectory = input.DetermineDocumentsFolder();

            if (!input.NoBottlingFlag)
            {
                new BottleCommand().Execute(new BottleInput {DirectoryFlag = documentDirectory, NoZipFlag = true});
            }


            string explodedBottlesDirectory = documentDirectory.AppendPath("fubu-content");
            Console.WriteLine("Trying to clean out the contents of " + explodedBottlesDirectory);
            new FileSystem().CleanDirectory(explodedBottlesDirectory);

            EmbeddedFubuMvcServer server = buildServer(documentDirectory);
            string url = server.BaseAddress;

            Process.Start(url);

            Console.WriteLine("Press any key to quit");
            Console.ReadLine();

            return true;
        }

        private static EmbeddedFubuMvcServer buildServer(string documentDirectory)
        {
            FubuMvcPackageFacility.PhysicalRootPath = documentDirectory;

            FubuApplication application = FubuApplication
                .For<RunFubuWorldRegistry>()
                .StructureMap(new Container())
                .Packages(x => {
                    x.Loader(new MainDocumentLinkedPackageLoader(documentDirectory));
                    x.Loader(new FubuDocsPackageLoader
                    {
                        IgnoreAssembly = Path.GetFileName(documentDirectory)
                    });
                });

            return application.RunEmbedded(documentDirectory);
        }
    }

    public class RunFubuWorldRegistry : FubuRegistry
    {
    }

    public class HomeEndpoint
    {
        public FubuContinuation Index()
        {
            return FubuContinuation.RedirectTo<AllTopicsEndpoint>(x => x.get_topics());
        }
    }

    public class MainDocumentLinkedPackageLoader : IPackageLoader
    {
        private readonly string _directory;

        public MainDocumentLinkedPackageLoader(string directory)
        {
            _directory = directory;
        }

        public IEnumerable<IPackageInfo> Load(IPackageLog log)
        {
            var reader = new PackageManifestReader(new FileSystem(), folder => folder);
            yield return reader.LoadFromFolder(_directory);
        }
    }
}