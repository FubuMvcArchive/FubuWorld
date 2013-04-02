using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using FubuCore;

namespace FubuDocsRunner
{
    public class DocActionInput
    {
        [Description("The directory holding the docs.  Will try to find a single directory containing the name 'Docs' under an 'src' folder if this flag is not specified")]
        public string DirectoryFlag { get; set; }

        public string DetermineDocumentsFolder()
        {
            if (DirectoryFlag.IsNotEmpty())
            {
                return DirectoryFlag;
            }


            var path = ".".ToFullPath().AppendPath("src");
            if (!Directory.Exists(path))
            {
                return ".".ToFullPath().ParentDirectory().ParentDirectory();
            }

            Console.WriteLine("No directory specified, looking in {0} for a 'Docs' folder", path);

            var directories = Directory.GetDirectories(path).Where(x => Path.GetFileName(x).EndsWith("Docs"));
            if (directories.Count() == 1)
            {
                return directories.Single();
            }

            throw new ApplicationException("Could not determine the document folder");
        }
    }
}