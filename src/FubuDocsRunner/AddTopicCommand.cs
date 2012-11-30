using System;
using System.ComponentModel;
using System.IO;
using Bottles.Commands;
using FubuCore.CommandLine;
using FubuCore;
using System.Linq;
using System.Collections.Generic;
using FubuDocs;

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
            Console.WriteLine("No directory specified, looking in {0} for a 'Docs' folder", path);

            var directories = Directory.GetDirectories(path).Where(x => Path.GetFileName(x).EndsWith("Docs"));
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

        [Description("Optional argument to specify a namespace underneath the assembly folder.  Use either periods or '/' to delimit the namespace")]
        public string Namespace { get; set; }

        [Description("The name of the Topic class.  If not specified, it will be derived from the title")]
        public string NameFlag { get; set; }

        public string GetName()
        {
            if (NameFlag.IsNotEmpty()) return NameFlag;

            return StringTokenizer.Tokenize(Title).Select(x => x.ToLower().Replace(",", "").Replace("-", "").Capitalize()).Join(string.Empty);

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
            var directory = input.DetermineDocumentsFolder();
            Console.WriteLine("Using directory " + directory);

            var @namespace = Path.GetFileName(directory);

            if (input.Namespace.IsNotEmpty())
            {
                var parts = input.Namespace.Replace('.', '/').Split('/');
                @namespace = @namespace + "." + parts.Join(".");

                var fileSystem = new FileSystem();

                parts.Each(folder => {
                    directory = directory.AppendPath(folder);
                    fileSystem.CreateDirectory(directory);
                });
            }

            var topicName = input.GetName();
            Console.WriteLine("The topic name is '{0}'", topicName);

            writeTopicClassFile(input, directory, topicName, @namespace);
            writeTopicSparkFile(directory, topicName, @namespace);


            return true;
        }

        private static void writeTopicSparkFile(string directory, string topicName, string @namespace)
        {
            var sparkFile = directory.AppendPath(topicName + ".spark");
            Console.WriteLine("Writing " + sparkFile);
            using (var writer = new StreamWriter(sparkFile))
            {
                writer.WriteLine("<viewdata model=\"{0}\"/>".ToFormat(@namespace + "." + topicName));
            }
        }

        private static void writeTopicClassFile(AddTopicInput input, string directory, string topicName, string @namespace)
        {
            var classFile = directory.AppendPath(topicName + ".cs");

            Console.WriteLine("Writing " + classFile);
            using (var writer = new StreamWriter(classFile))
            {
                writer.WriteLine("using {0};", typeof (Topic).Namespace);
                writer.WriteLine();
                writer.WriteLine("namespace " + @namespace);
                writer.WriteLine("{");
                writer.WriteLine("    public class {0} : Topic", topicName);
                writer.WriteLine("    {");
                writer.WriteLine("        public {0}() : base(\"{1}\")", topicName, input.Title);
                writer.WriteLine("        {");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("}");
            }
        }
    }
}