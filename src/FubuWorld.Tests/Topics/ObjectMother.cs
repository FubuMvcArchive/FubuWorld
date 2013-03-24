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
            FubuMvcPackageFacility.PhysicalRootPath = ".".ToFullPath().ParentDirectory().ParentDirectory();
            var app = FubuApplication
                .DefaultPolicies()
                .StructureMap(new Container())
                .Bootstrap();

            var registry = app.Factory.Get<ISparkTemplateRegistry>();
            registry.ShouldNotBeNull();

            var loader = new TopicFileLoader(registry);
            Files = loader.FindFilesFromBottle("Sample.Docs");

            ProjectRoot =
                ProjectRoot.LoadFrom(".".ToFullPath().ParentDirectory()
                                        .ParentDirectory()
                                        .ParentDirectory()
                                        .AppendPath("Sample.Docs", ProjectRoot.File));

            ProjectRoot.OrganizeFiles(Files);

            Nodes = new Cache<string, Topic>();
            Nodes[ProjectRoot.Root.Key] = ProjectRoot.Root;
            ProjectRoot.Root.Descendents().Each(x => Nodes[x.Key] = x);

            Nodes.Each(x => Debug.WriteLine(x.Key));
        }
    }
}