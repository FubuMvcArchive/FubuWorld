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
using System.Linq;

namespace FubuWorld.Tests.Topics
{
    public static class ObjectMother
    {
        public readonly static ProjectRoot ProjectRoot;
        public readonly static Cache<string, Topic> Nodes;
        public readonly static IEnumerable<ITopicFile> Files;

        static ObjectMother()
        {
            FubuMvcPackageFacility.PhysicalRootPath = ".".ToFullPath().ParentDirectory().ParentDirectory();
            var app = FubuApplication
                .DefaultPolicies()
                .StructureMap(new Container())
                .Bootstrap();

            var registry = app.Factory.Get<ISparkTemplateRegistry>();
            registry.ShouldNotBeNull();

            var loader = new TopicLoader(registry);
            ProjectRoot = loader.LoadProject("Sample.Docs");

            Nodes = new Cache<string, Topic>();
            Nodes[ProjectRoot.Root.Key] = ProjectRoot.Root;
            ProjectRoot.Root.Descendents().Each(x => Nodes[x.Key] = x);

            Files = Nodes.Select(x => x.File).ToArray();
        }
    }
}