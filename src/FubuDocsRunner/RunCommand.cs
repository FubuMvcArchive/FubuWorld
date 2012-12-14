using System;
using System.Collections.Generic;
using System.ComponentModel;
using Bottles;
using Bottles.Diagnostics;
using Bottles.Manifest;
using FubuCore;
using FubuCore.CommandLine;
using FubuMVC.Core.Packaging;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.SelfHost;
using FubuWorld;
using Process = System.Diagnostics.Process;

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
            var documentDirectory = input.DetermineDocumentsFolder();

            if (!input.NoBottlingFlag)
            {
                new BottleCommand().Execute(new BottleInput { DirectoryFlag = documentDirectory });
            }
            

            var server = buildServer(documentDirectory);
            var url = server.BaseAddress;

            Process.Start(url);

            Console.WriteLine("Press any key to quit");
            while (true)
            {
                var text = Console.ReadLine();
                if (text.Trim().EqualsIgnoreCase("q"))
                {
                    server.Dispose();
                    break;
                }
            }

            return true;
        }

        private static EmbeddedFubuMvcServer buildServer(string documentDirectory)
        {
            FubuMvcPackageFacility.PhysicalRootPath = documentDirectory;

            var application =
                new FubuWorldApplication().BuildApplication()
                                          .Packages(x => { x.Loader(new MainDocumentLinkedPackageLoader(documentDirectory)); });

            return application.RunEmbedded(documentDirectory);
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