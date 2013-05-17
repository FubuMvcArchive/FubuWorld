using System.Diagnostics;
using FubuDocs.Topics;
using NUnit.Framework;
using FubuCore;
using FubuTestingSupport;
using System.Linq;
using System.Collections.Generic;

namespace FubuWorld.Tests.Topics
{
    [TestFixture]
    public class TopicLoaderTester
    {
        [Test]
        public void smoke_test_can_load_project_root_from_folder_outside_of_fubumvc_app()
        {
            var folder =
                ".".ToFullPath().ParentDirectory().ParentDirectory().ParentDirectory().AppendPath("FubuWorld.Docs");

            var project = TopicLoader.LoadFromFolder(folder);

            project.ShouldNotBeNull();
            project.AllTopics().Count().ShouldBeGreaterThan(5);

            project.AllTopics().Each(x => {
                Debug.WriteLine(x);
            });
        }
    }
}