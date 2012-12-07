using System;
using System.Collections.Generic;
using System.IO;
using FubuCore.CommandLine;
using FubuCore;
using FubuDocs;
using StringWriter = System.IO.StringWriter;
using System.Linq;

namespace FubuDocsRunner
{
    public class BuildTopicsInput : DocActionInput
    {
        
    }

    [CommandDescription("Looks for a 'Topics.xml' file in the named directory and tries to generate the skeleton files for the topics in the file", Name = "build-topics")]
    public class BuildTopicsCommand : FubuCommand<BuildTopicsInput>
    {
        public override bool Execute(BuildTopicsInput input)
        {
            var directory = input.DetermineDocumentsFolder();
            var parser = new TopicParser();

            var request = parser.Parse(directory);

            writeRequest(request);
            WriteRelationships(request);

            return true;
        }

        private void writeRequest(TopicRequest request)
        {
            request.WriteFiles();

            request.Children.Each(writeRequest);
        }

        public static void WriteRelationships(TopicRequest request)
        {
            var @ns = Path.GetFileName(request.RootDirectory);
            var className = request.TopicName + "TopicRegistry";

            var writer = new StringWriter();

            writer.WriteLine("namespace {0}", @ns);
            writer.WriteLine("{");
            writer.WriteLine("    public class {0} : {1}", className, typeof(TopicRegistry).FullName);
            writer.WriteLine("    {");

            writer.WriteLine("        public {0}()", className);
            writer.WriteLine("        {");

            writeRelationships(writer, request);

            writer.WriteLine("        }");
            writer.WriteLine("    }");
            writer.WriteLine("}");

            var path = request.RootDirectory.AppendPath(className + ".cs");

            Console.WriteLine("Writing the topic registry to " + path);
            new FileSystem().WriteStringToFile(path, writer.ToString());
        }

        private static void writeRelationships(StringWriter writer, TopicRequest request)
        {
            if (!request.Children.Any()) return;

            request.Children.Each(child => {
                writer.WriteLine("            For<{0}>().Append<{1}>();", request.FullTopicClassName, child.FullTopicClassName);
            });

            writer.WriteLine();

            request.Children.Each(child => {
                writeRelationships(writer, child);
            });
        }
    }

}