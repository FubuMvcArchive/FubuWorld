using System;
using System.Diagnostics;
using System.IO;
using FubuCore;
using FubuCore.CommandLine;
using FubuMVC.Core;
using FubuMVC.Core.Packaging;
using FubuMVC.Katana;
using FubuMVC.StructureMap;
using FubuWorld.Infrastructure;
using Container = StructureMap.Container;

namespace FubuDocsRunner.Running
{
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

            try
            {
                EmbeddedFubuMvcServer server = buildServer(documentDirectory);
                string url = server.BaseAddress;

                Process.Start(url);

                Console.WriteLine("Press any key to quit");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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
}