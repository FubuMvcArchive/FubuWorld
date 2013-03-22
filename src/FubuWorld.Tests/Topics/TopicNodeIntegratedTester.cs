using System.Collections.Generic;
using System.Diagnostics;
using FubuCore;
using FubuCore.Util;
using FubuMVC.Core;
using FubuMVC.Core.Packaging;
using FubuMVC.Spark.SparkModel;
using FubuMVC.StructureMap;
using FubuTestingSupport;
using FubuWorld.Topics;
using NUnit.Framework;
using StructureMap;

namespace FubuWorld.Tests.Topics
{
    [TestFixture]
    public class TopicNodeIntegratedTester
    {
        private Cache<string, TopicNode> nodes;
        private ProjectRoot theProjectRoot;

        [SetUp]
        public void SetUp()
        {
            theProjectRoot = new ProjectRoot();

            FubuMvcPackageFacility.PhysicalRootPath = ".".ToFullPath().ParentDirectory().ParentDirectory();
            var app = FubuApplication
                .DefaultPolicies()
                .StructureMap(new Container())
                .Bootstrap();

            var registry = app.Factory.Get<ISparkTemplateRegistry>();
            registry.ShouldNotBeNull();

            var loader = new TopicFileLoader(registry);
            var files = loader.FindFilesFromBottle("Sample.Docs");

            nodes = new Cache<string, TopicNode>();
            files.Each(file => {
                var node = new TopicNode(theProjectRoot, file);
                nodes[node.Key] = node;

                Debug.WriteLine(node.Key);
            });
        }

        [Test]
        public void determines_the_key()
        {
            nodes["colors/red"].ShouldNotBeNull();
            nodes["colors/blue"].ShouldNotBeNull();
            nodes["colors/purple"].ShouldNotBeNull();
            nodes["colors/green"].ShouldNotBeNull();
        }

    }
}