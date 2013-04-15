using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuCore.Util;
using FubuDocs;
using FubuDocs.Topics;
using FubuMVC.Core;
using FubuMVC.Core.Packaging;
using FubuMVC.Core.Registration;
using FubuMVC.StructureMap;
using StructureMap;

namespace FubuWorld.Tests.Topics
{
    public static class ObjectMother
    {
        public static readonly ProjectRoot ProjectRoot;
        public static readonly Cache<string, Topic> Topics;
        public static readonly IEnumerable<ITopicFile> Files;
        public static BehaviorGraph Behaviors;

        static ObjectMother()
        {
            FubuMvcPackageFacility.PhysicalRootPath = ".".ToFullPath().ParentDirectory().ParentDirectory();
            var registry = new FubuRegistry();
            registry.Import<FubuDocsExtension>();

            FubuRuntime app = FubuApplication
                .For(registry)
                .StructureMap(new Container())
                .Bootstrap();


            Behaviors = app.Factory.Get<BehaviorGraph>();

            ProjectRoot = TopicGraph.AllTopics.ProjectFor("FubuMVC");

            Topics = new Cache<string, Topic>();
            Topics[ProjectRoot.Root.Key] = ProjectRoot.Root;
            ProjectRoot.Root.Descendents().Each(x => Topics[x.Key] = x);

            Files = Topics.Select(x => x.File).ToArray();
        }
    }
}