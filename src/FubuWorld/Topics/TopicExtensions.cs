using FubuMVC.Core;

namespace FubuWorld.Topics
{
    public class TopicExtensions : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            var graph = new TopicGraph();
            registry.ReplaceSettings(graph);

            registry.Policies.Add<DocumentationProjectLoader>();
            registry.Services(x => x.ReplaceService<TopicGraph>(graph));
        }
    }
}