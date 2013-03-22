using System.Collections.Generic;
using System.Diagnostics;
using FubuMVC.Core;
using FubuMVC.Core.Packaging;
using FubuMVC.Spark.SparkModel;
using FubuWorld.Topics;
using NUnit.Framework;
using FubuMVC.StructureMap;
using StructureMap;
using FubuTestingSupport;
using System.Linq;
using FubuCore;

namespace FubuWorld.Tests.Topics
{
    [TestFixture]
    public class TopicFileLoaderIntegratedTester
    {
        private IEnumerable<ITopicFile> theFiles;

        [SetUp]
        public void SetUp()
        {
            FubuMvcPackageFacility.PhysicalRootPath = ".".ToFullPath().ParentDirectory().ParentDirectory();
            var app = FubuApplication
                .DefaultPolicies()
                .StructureMap(new Container())
                .Bootstrap();

            var registry = app.Factory.Get<ISparkTemplateRegistry>();
            registry.ShouldNotBeNull();

            var loader = new TopicFileLoader(registry);
            theFiles = loader.FindFilesFromBottle("Sample.Docs");
        }

        [Test]
        public void should_find_files()
        {
            theFiles.Any().ShouldBeTrue();
        }

        [Test]
        public void spot_check_a_topic()
        {
            var file = theFiles.First(x => x.Name == "1.3.topic");
            file.ShouldNotBeNull();

            file.RelativePath().ShouldEqual("deep/1.3.topic");
        }

        [Test]
        public void ignores_any_file_in_samples()
        {
            theFiles.Any(x => x.Name == "whatever").ShouldBeFalse();
        }

        [Test]
        public void ignores_any_file_in_examples()
        {
            theFiles.Any(x => x.Name == "anything").ShouldBeFalse();
        }
    }
}