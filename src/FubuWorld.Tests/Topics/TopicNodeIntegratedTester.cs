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
    public static class ObjectMother
    {
        public readonly static ProjectRoot ProjectRoot;
        public readonly static Cache<string, TopicNode> Nodes;
        public readonly static IEnumerable<ITopicFile> Files;

        static ObjectMother()
        {
            ProjectRoot = new ProjectRoot();

            FubuMvcPackageFacility.PhysicalRootPath = ".".ToFullPath().ParentDirectory().ParentDirectory();
            var app = FubuApplication
                .DefaultPolicies()
                .StructureMap(new Container())
                .Bootstrap();

            var registry = app.Factory.Get<ISparkTemplateRegistry>();
            registry.ShouldNotBeNull();

            var loader = new TopicFileLoader(registry);
            Files = loader.FindFilesFromBottle("Sample.Docs");

            Nodes = new Cache<string, TopicNode>();
            Files.Each(file =>
            {
                var node = new TopicNode(ProjectRoot, file);
                Nodes[node.Key] = node;

                Debug.WriteLine(node.Key);
            });
        }
    }

    [TestFixture]
    public class TopicNodeIntegratedTester
    {
        [Test]
        public void determines_the_key()
        {
            ObjectMother.Nodes["colors/red"].ShouldNotBeNull();
            ObjectMother.Nodes["colors/blue"].ShouldNotBeNull();
            ObjectMother.Nodes["colors/purple"].ShouldNotBeNull();
            ObjectMother.Nodes["colors/green"].ShouldNotBeNull();
        }


        [Test]
        public void determine_the_url_for_an_index_name()
        {
            var colorsIndex = ObjectMother.Nodes["colors"];
            colorsIndex.File.Name.ShouldEqual("index");

            colorsIndex.Url.ShouldEqual("colors");
        }

        [Test]
        public void determine_the_url_for_a_file_not_the_index()
        {
            ObjectMother.Nodes["colors/red"].Url.ShouldEqual("colors/red");
        }

        [Test]
        public void determine_the_url_for_a_file_overriding_url_in_spark_file()
        {
            ObjectMother.Nodes["colors/green"].Url.ShouldEqual("colors/SeaGreen"); // look at the 1.1.2.green.spark file
        }


        [Test]
        public void get_the_title_if_it_is_not_written_into_the_file()
        {
            ObjectMother.Nodes["colors/green"].Title.ShouldEqual("The green page");
        }

        [Test]
        public void get_the_title_from_file_contents()
        {
            ObjectMother.Nodes["colors/blue"].Title.ShouldEqual("Blue");
        }

    }
}