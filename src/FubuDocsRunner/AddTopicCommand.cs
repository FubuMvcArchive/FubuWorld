using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using FubuCore;
using FubuCore.CommandLine;

namespace FubuDocsRunner
{
    public class DocActionInput
    {
        [Description(
            "The directory holding the docs.  Will try to find a single directory containing the name 'Docs' under an 'src' folder if this flag is not specified"
            )]
        public string DirectoryFlag { get; set; }

        public string DetermineDocumentsFolder()
        {
            if (DirectoryFlag.IsNotEmpty())
            {
                return DirectoryFlag;
            }


            string path = ".".ToFullPath().AppendPath("src");
            if (!Directory.Exists(path))
            {
                return ".".ToFullPath().ParentDirectory().ParentDirectory();
            }

            Console.WriteLine("No directory specified, looking in {0} for a 'Docs' folder", path);

            IEnumerable<string> directories =
                Directory.GetDirectories(path).Where(x => Path.GetFileName(x).EndsWith("Docs"));
            if (directories.Count() == 1)
            {
                return directories.Single();
            }

            throw new ApplicationException("Could not determine the document folder");
        }
    }

    public class AddTopicInput : DocActionInput
    {
        [Description("The title of this topic in navigation links and the page title")]
        public string Title { get; set; }

        [Description(
            "Optional argument to specify a namespace underneath the assembly folder.  Use either periods or '/' to delimit the namespace"
            )]
        public string Namespace { get; set; }

        [Description("The name of the Topic class.  If not specified, it will be derived from the title")]
        public string NameFlag { get; set; }

        public TopicRequest ToRequest()
        {
            return new TopicRequest
            {
                Title = Title,
                RootDirectory = DetermineDocumentsFolder(),
                Namespace = (Namespace ?? string.Empty).Replace('/', '.'),
                TopicName = NameFlag
            };
        }
    }

    [CommandDescription("Add a new topic to the project", Name = "add-topic")]
    public class AddTopicCommand : FubuCommand<AddTopicInput>
    {
        public AddTopicCommand()
        {
            Usage("Add a topic to the root directory of the document folder").Arguments(x => x.Title);
            Usage("Add a topic to the specified namespace of the document folder")
                .Arguments(x => x.Title, x => x.Namespace);
        }

        public override bool Execute(AddTopicInput input)
        {
            TopicRequest request = input.ToRequest();
            Console.WriteLine("Using directory " + request.RootDirectory);
            Console.WriteLine("The topic name is '{0}'", request.TopicName);

            request.WriteFiles();

            return true;
        }
    }
}