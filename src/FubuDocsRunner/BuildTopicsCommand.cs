using System;
using System.Collections.Generic;
using System.IO;
using FubuCore.CommandLine;
using FubuCore;
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
            if (request == null) return true; // Nothing to do here, go away

            writeRequest(request);

            return true;
        }

        private void writeRequest(TopicRequest request)
        {
            request.WriteFiles();

            request.Children.Each(writeRequest);
        }

    }

}