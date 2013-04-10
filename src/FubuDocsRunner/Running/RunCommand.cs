using System;
using System.Reflection;
using System.Threading.Tasks;
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
        private string _solutionDirectory;

        public override bool Execute(RunInput input)
        {
            var runnerDirectory = Assembly.GetExecutingAssembly().Location.ParentDirectory();

            var bottling = Task.Factory.StartNew(() => {
                if (!input.NoBottlingFlag)
                {
                    new BottleCommand().Execute(new BottleInput { NoZipFlag = true });
                }
            });

            var cleaning = Task.Factory.StartNew(() => {
                cleanExplodedBottleContents(runnerDirectory);
            });


            Task.WaitAll(bottling, cleaning);


            _solutionDirectory = Environment.CurrentDirectory;

            try
            {
                _application = new RemoteApplication(x => {
                    x.Setup.AppDomainInitializerArguments = new string[]{_solutionDirectory};
                    
                    x.Setup.ApplicationBase = runnerDirectory;
                });

                _application.Start(input); 

                tellUserWhatToDo();
                ConsoleKeyInfo key = Console.ReadKey();
                while (key.Key != ConsoleKey.Q)
                {
                    if (key.Key == ConsoleKey.R)
                    {
                        _application.RecycleAppDomain();
                        tellUserWhatToDo();
                    }

                    key = Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return true;
        }

        private static void tellUserWhatToDo()
        {
            Console.WriteLine("Press 'q' to quit, 'r' to recycle the application");
        }

        private void cleanExplodedBottleContents(string runnerDirectory)
        {
            string explodedBottlesDirectory = runnerDirectory.AppendPath("fubu-content");
            Console.WriteLine("Trying to clean out the contents of " + explodedBottlesDirectory);
            fileSystem.CleanDirectory(explodedBottlesDirectory);
        }
    }
}