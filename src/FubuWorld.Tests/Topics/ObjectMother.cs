using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuCore.Util;
using FubuMVC.Core;
using FubuMVC.Core.Packaging;
using FubuMVC.StructureMap;
using FubuWorld.Topics;
using StructureMap;

namespace FubuWorld.Tests.Topics
{
    public static class ObjectMother
    {
        public static readonly ProjectRoot ProjectRoot;
        public static readonly Cache<string, Topic> Nodes;
        public static readonly IEnumerable<ITopicFile> Files;

        static ObjectMother()
        {
            FubuMvcPackageFacility.PhysicalRootPath = ".".ToFullPath().ParentDirectory().ParentDirectory();
            var registry = new FubuRegistry();
            registry.Import<TopicFubuRegistryExtensions>();

            FubuRuntime app = FubuApplication
                .For(registry)
                .StructureMap(new Container())
                .Bootstrap();




            ProjectRoot = TopicGraph.AllTopics.ProjectFor("FubuMVC");

            Nodes = new Cache<string, Topic>();
            Nodes[ProjectRoot.Root.Key] = ProjectRoot.Root;
            ProjectRoot.Root.Descendents().Each(x => Nodes[x.Key] = x);

            Files = Nodes.Select(x => x.File).ToArray();
        }
    }
}