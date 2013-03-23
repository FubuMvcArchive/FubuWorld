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
using StructureMap;

namespace FubuWorld.Tests.Topics
{
    public static class ObjectMother
    {
        public readonly static ProjectRoot ProjectRoot;
        public readonly static Cache<string, Topic> Nodes;
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

            ProjectRoot.OrganizeFiles(Files);

            Nodes = new Cache<string, Topic>();
//            Files.Each(file =>
//            {
//                var node = new Topic(ProjectRoot, file);
//                Nodes[node.Key] = node;
//
//                Debug.WriteLine(node.Key);
//            });
        }
    }
}