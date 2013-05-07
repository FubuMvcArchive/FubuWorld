using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using FubuCore;
using FubuCore.CommandLine;
using FubuDocsRunner.Running;
using FubuMVC.Core;
using FubuMVC.Katana;

namespace FubuDocsRunner.Exports
{
    public class ExportInput : RunInput
    {
        [Description("The directory to output the application")]
        public string Output { get; set; }
    }

    public class ExportCommand : FubuCommand<ExportInput>
    {
        private readonly IFileSystem _fileSystem = new FileSystem();
        private string _solutionDirectory;

        public override bool Execute(ExportInput input)
        {
            string runnerDirectory = Assembly.GetExecutingAssembly().Location.ParentDirectory();

            Task bottling = Task.Factory.StartNew(() =>
            {
                if (!input.NoBottlingFlag)
                {
                    new BottleCommand().Execute(new BottleInput { NoZipFlag = true });
                }
            });

            Task cleaning = Task.Factory.StartNew(() => { cleanExplodedBottleContents(runnerDirectory); });


            Task.WaitAll(bottling, cleaning);


            _solutionDirectory = Environment.CurrentDirectory;

            try
            {
                _fileSystem.DeleteDirectory(input.Output);

                // TODO -- It sure would be nice to turn off the pre-compile work so we don't get a ton of console errors
                using (var server = new FubuDocsExportingApplication(_solutionDirectory).BuildApplication().RunEmbedded(_solutionDirectory))
                {
                    server.ExportTo(input.Output);
                }  

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        private void cleanExplodedBottleContents(string runnerDirectory)
        {
            string explodedBottlesDirectory = runnerDirectory.AppendPath("fubu-content");
            Console.WriteLine("Trying to clean out the contents of " + explodedBottlesDirectory);
            _fileSystem.CleanDirectory(explodedBottlesDirectory);
        }
    }
}