using FubuDocs.Topics;
using FubuDocsRunner.Topics;
using NUnit.Framework;
using FubuCore;

namespace FubuWorld.Tests.Commands
{
    [TestFixture]
    public class TopicTextReportSmokeTester
    {
        [Test]
        public void write_report_for_FubuWorld_Docs()
        {
            var folder = ".".ToFullPath().ParentDirectory().ParentDirectory().ParentDirectory()
                            .AppendPath("FubuWorld.Docs");

            var root = TopicLoader.LoadFromFolder(folder);

            var report = new TopicTextReport(root.AllTopics());

            report.WriteToConsole();
        }
    }
}