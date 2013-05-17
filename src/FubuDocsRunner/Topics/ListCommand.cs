using System;
using System.Collections.Generic;
using FubuCore.Util.TextWriting;
using FubuDocs.Topics;

namespace FubuDocsRunner.Topics
{
    public class ListCommand
    {
         
    }

    public class TopicTextReport
    {
        private readonly TextReport _report;

        public TopicTextReport(IEnumerable<Topic> topics)
        {
            _report = new TextReport();
            _report.AddDivider('-');
            _report.StartColumns(3);
            _report.AddDivider('-');
            _report.AddColumnData("Url", "Title", "Key");
            _report.AddDivider('-');

            topics.Each(topic => _report.AddColumnData(topic.Url, topic.Title, topic.Key));

            
        }

        public void WriteToConsole()
        {
            _report.WriteToConsole();
        }
    }
}