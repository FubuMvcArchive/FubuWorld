using System;
using System.Reflection;
using Fubu.Running;
using FubuCore;
using FubuCore.CommandLine;
using System.Collections.Generic;

namespace FubuDocsRunner.Running
{
    public class RunCommand : FubuCommand<RunInput>
    {
        private RemoteApplication _application;
        private IFileSystem fileSystem = new FileSystem();

        public override bool Execute(RunInput input)
        {
            if (!input.NoBottlingFlag)
            {
                new BottleCommand().Execute(new BottleInput {NoZipFlag = true});
            }

            cleanExplodedBottleContents();

            var request = new ApplicationRequest
            {
                ApplicationFlag = typeof (FubuDocsApplication).Name,
                DirectoryFlag = Assembly.GetExecutingAssembly().Location.ParentDirectory(),
            };

            try
            {
                _application = new RemoteApplication(x => {
                    x.Setup.AppDomainInitializerArguments = new string[]{input.DirectoryFlag ?? Environment.CurrentDirectory};
                });

                _application.Start(request); // Need to pass on a linked package directory (ies)

                Console.WriteLine("Press 'r' to recycle the application, anything else to quit");
                ConsoleKeyInfo key = Console.ReadKey();
                while (key.Key == ConsoleKey.R)
                {
                    _application.RecycleAppDomain();

                    key = Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return true;
        }

        private void cleanExplodedBottleContents()
        {
            new DocActionInput().DetermineDocumentsFolders().Each(dir => {
                string explodedBottlesDirectory = dir.AppendPath("fubu-content");
                Console.WriteLine("Trying to clean out the contents of " + explodedBottlesDirectory);
                fileSystem.CleanDirectory(explodedBottlesDirectory);
            });
        }
    }
}