using System;
using FubuCore.CommandLine;
using FubuCore.Util;
using FubuCore;
using FubuDocs.Navigation;
using FubuMVC.Core.Runtime.Files;
using FubuMVC.Core.View;
using HtmlTags;

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
        public static CommandSectionTag SectionForCommand(this IFubuPage page, string applicationName, string command)
        {
            throw new NotImplementedException();
        }

        public static string BodyForCommand(this IFubuPage page, string applicationName, string command)
        {
            return "the body";
        }
    }




}