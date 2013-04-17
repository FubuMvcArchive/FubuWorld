using System;
using FubuCore.CommandLine;
using FubuCore.Util;
using FubuCore;
using FubuDocs.Navigation;
using FubuMVC.Core.Runtime.Files;
using FubuMVC.Core.View;
using HtmlTags;
using System.Linq;

namespace FubuDocs.CLI
{
    public interface ICommandDocumentationSource
    {
        CommandLineApplicationReport ReportFor(string applicationName);
    }

    public class CommandDocumentationSource : ICommandDocumentationSource
    {
        private readonly Cache<string, CommandLineApplicationReport> _applications;

        public CommandDocumentationSource(IFubuApplicationFiles files, IFileSystem fileSystem)
        {
            _applications = new Cache<string, CommandLineApplicationReport>(name => {
                var filename = "{0}.cli.xml".ToFormat(name);
                var file = files.Find(filename);

                if (file == null)
                {
                    throw new ArgumentOutOfRangeException("name", name, "Unable to find a *.cli.xml file for the requested application name");
                }

                return fileSystem.LoadFromFile<CommandLineApplicationReport>(file.Path);
            });
        }

        public CommandLineApplicationReport ReportFor(string applicationName)
        {
            return _applications[applicationName];
        }
    }

    public class CommandSectionTag : SectionTag
    {
        public CommandSectionTag(string applicationName, CommandReport report)
            : base("{0} {1}".ToFormat(applicationName, report.Name), report.Name)
        {
            
        }
    }



    public static class CommandUsagePageExtensions
    {
        public static CommandSectionTag SectionForCommand(this IFubuPage page, string applicationName, string commandName)
        {
            var command = page.CommandReportFor(applicationName, commandName);

            return new CommandSectionTag(applicationName, command);
        }

        public static CommandReport CommandReportFor(this IFubuPage page, string applicationName, string commandName)
        {
            var application = page.Get<ICommandDocumentationSource>().ReportFor(applicationName);
            var command = application.Commands.FirstOrDefault(x => x.Name == commandName);
            if (command == null)
                throw new ArgumentOutOfRangeException("commandName", "Could not find the named command in this application");

            return command;
        }


        public static HtmlTag BodyForCommand(this IFubuPage page, string applicationName, string commandName)
        {
            var command = page.CommandReportFor(applicationName, commandName);


            return new DivTag().Text(command.Usages.First().Usage);
        }
    }




}