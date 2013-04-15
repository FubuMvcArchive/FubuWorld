using System;
using System.Collections.Generic;
using FubuCore.CommandLine;
using FubuCore.Util;
using FubuWorld.Navigation;
using HtmlTags;
using FubuCore;

namespace FubuWorld.CLI
{
    public static class CommandDocumentation
    {
        private static Cache<string, CommandLineApplication> _applications = new Cache<string, CommandLineApplication>(); 

        public static void AddExecutor<T>(string applicationName) where T : FubuCore.CommandLine.CommandExecutor, new()
        {
            _applications[applicationName] = new CommandLineApplication(applicationName, new T().Factory);
        }

        public static Cache<string, CommandLineApplication> Applications
        {
            get { return _applications; }
        }
    }

    public class CommandLineApplication
    {
        private readonly string _applicationName;
        private readonly ICommandFactory _factory;

        public CommandLineApplication(string applicationName, ICommandFactory factory)
        {
            _applicationName = applicationName;
            _factory = factory;
        }

        public string ApplicationName
        {
            get { return _applicationName; }
        }

        public ICommandFactory Factory
        {
            get { return _factory; }
        }
    }



    public static class CommandUsagePageExtensions
    {
        
    }

    public class CommandUsageEndpoint
    {
        public HtmlTag get_commands_ApplicationName(ApplicationUsage application)
        {
            throw new NotImplementedException();
        }

        public HtmlTag get_commands_ApplicationName_Command(CommandUsage usage)
        {
            throw new NotImplementedException();
        }
    }

    public class CommandDescriptionTag : SectionTag
    {
        public CommandDescriptionTag(string applicationName, UsageGraph graph) : base("{0} {1}".ToFormat(applicationName, graph.CommandName), graph.CommandName)
        {
                        
        }
    }
    

    public class ApplicationUsage
    {
        public string ApplicationName { get; set; }
    }

    public class CommandUsage : ApplicationUsage
    {
        public string Command { get; set; }
    }
}