using System;
using System.ComponentModel;
using FubuCore.CommandLine;
using FubuDocs.Topics;

namespace FubuDocsRunner.Topics
{
    [CommandDescription("Lists the topics and/or todo's in a document project directory")]
    public class ListCommand : FubuCommand<ListInput>
    {
        public ListCommand()
        {
            Usage("Lists all the topics under this directory");
            Usage("Lists the selected reports for the topics under this directory").Arguments(x => x.Mode);
        }

        public override bool Execute(ListInput input)
        {
            var folder = Environment.CurrentDirectory;
            var projectFolder = TopicLoader.FindProjectRootFolder(folder);
            if (projectFolder == null)
            {
                Console.WriteLine("Not in a document project folder");
                return false;
            }

            WriteProject(input, projectFolder);


            return true;
        }

        public static void WriteProject(ListInput input, string projectFolder)
        {
            var project = TopicLoader.LoadFromFolder(projectFolder);
            var topics = project.AllTopics();


            ConsoleWriter.Write(ConsoleColor.Cyan, "Report for " + projectFolder);
            Console.WriteLine();
            if (input.Mode == ListMode.topics || input.Mode == ListMode.all)
            {
                new TopicTextReport(topics).WriteToConsole();
            }

            if (input.Mode == ListMode.all)
            {
                Console.WriteLine();
                Console.WriteLine();
            }

            if (input.Mode == ListMode.all || input.Mode == ListMode.todo)
            {
                new TodoTextReport(projectFolder, topics).WriteToConsole();
            }
        }
    }

    public enum ListMode
    {
        topics,
        all,
        todo
    }

    public class ListInput
    {
        public ListInput()
        {
            Mode = ListMode.topics;
        }

        [Description("Choose what gets listed for the current document directory.  Default is 'topics'")]
        public ListMode Mode { get; set; }
    }
}