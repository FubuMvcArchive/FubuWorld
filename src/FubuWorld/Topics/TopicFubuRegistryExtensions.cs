using FubuMVC.Core;

namespace FubuWorld.Topics
{
    public class TopicFubuRegistryExtensions : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.ReplaceSettings(TopicGraph.AllTopics);

            registry.Policies.Add<DocumentationProjectLoader>();
        }
    }
}