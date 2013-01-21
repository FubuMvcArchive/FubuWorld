using System;
using System.Collections.Generic;
using System.IO;
using FubuCore;
using FubuCore.CommandLine;
using System.Linq;
using FubuDocs;

namespace FubuDocsRunner
{
    public class TopicRequest
    {
        public readonly IList<TopicRequest> Children = new List<TopicRequest>();

        public static readonly string SparkTemplate = "<viewdata model=\"{0}\"/>{1}<use master=\"Topic\" />{1}<content:main>{1}{1}<markdown>{1}{1}</markdown>{1}{1}</content:main>";
        public static readonly string TopicClassTemplate = @"
using {0};

namespace {1}
{{
    public class {2} : Topic
    {{
        public {2}() : base('{3}')
        {{
        }}
    }}
}}".Replace("'", "\"");

        private string _topicName;

        public static string GetNameFromTitle(string title)
        {
            return StringTokenizer.Tokenize(title).Select(x => x.ToLower()
                .Replace(",", "")
                .Replace(".", "")
                .Replace("'", "")
                .Replace("-", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("?", "")
                .Capitalize()).Join(string.Empty);
        }

        public void WriteFiles()
        {
            var fileSystem = new FileSystem();

            var directory = NamespacedDirectory();
            writeSparkFile(directory, fileSystem);

            var classFile = directory.AppendPath(TopicName + ".cs");
            if (!fileSystem.FileExists(classFile))
            {
                Console.WriteLine("Writing " + classFile);
                var content = TopicClassTemplate.ToFormat(typeof (Topic).Namespace, FullNamespace, TopicName, Title);
                fileSystem.WriteStringToFile(classFile, content);
            }
        }

        private void writeSparkFile(string directory, FileSystem fileSystem)
        {
            var sparkFile = directory.AppendPath(TopicName + ".spark");
            if (!fileSystem.FileExists(sparkFile))
            {
                Console.WriteLine("Writing " + sparkFile);
                var content = SparkTemplate.ToFormat(FullTopicClassName, Environment.NewLine);

                fileSystem.WriteStringToFile(sparkFile, content);
            }
        }

        public string FullTopicClassName
        {
            get { return FullNamespace + "." + TopicName; }
        }

        public string FullNamespace
        {
            get
            {
                var @namespace = Path.GetFileName(RootDirectory);
                if (Namespace.IsNotEmpty())
                {
                    @namespace += "." + Namespace;
                }

                return @namespace;
            }
        }

        public string RootDirectory { get; set; }
        public string Namespace { get; set; }
        public string Title { get; set; }
        public string TopicName
        {
            get
            {
                if (_topicName.IsEmpty())
                {
                    return GetNameFromTitle(Title);
                }
                
                
                return _topicName;
            }

            set { _topicName = value; }
        }


        public string NamespacedDirectory()
        {
            if (Namespace.IsEmpty()) return RootDirectory;

            var fileSystem = new FileSystem();

            var directory = RootDirectory;
            var parts = Namespace.Split('.');
            parts.Each(folder =>
            {
                directory = directory.AppendPath(folder);
                fileSystem.CreateDirectory(directory);
            });

            return directory;
        }

        
    }
}