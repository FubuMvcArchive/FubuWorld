using System.Collections.Generic;
using System.Linq;
using Bottles;
using FubuMVC.Core;
using FubuMVC.Core.Registration;

namespace FubuDocs.Topics
{
    [ConfigurationType(ConfigurationType.InjectNodes)]
    public class DocumentationProjectLoader : IConfigurationAction
    {
        private TopicLoader _loader;

        public void Configure(BehaviorGraph graph)
        {
            _loader = new TopicLoader(graph);
            PackageRegistry.Packages.Where(x => x.Name.EndsWith(".Docs"))
                           .Each(
                               pak => { pak.ForFolder(BottleFiles.WebContentFolder, dir => LoadPackage(pak, dir, graph)); });
        }

        public void LoadPackage(IPackageInfo pak, string directory, BehaviorGraph graph)
        {
            ProjectRoot root = _loader.LoadProject(pak.Name, directory);
            root.AllTopics().Each(topic => graph.AddChain(topic.BuildChain()));

            TopicGraph.AllTopics.AddProject(root);
        }
    }
}