using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.Core.View;
using FubuWorld.Infrastructure.Binders;
using FubuWorld.Topics;

namespace FubuWorld
{
    public class FubuWorldExtension : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.AlterSettings<CommonViewNamespaces>(x => {
                x.AddForType<FubuWorldRegistry>();
            });

            registry.Services(x => {
                x.AddService<IPropertyBinder, RequestLogPropertyBinder>();
                x.ReplaceService<ITopicContext, TopicContext>();
            });

            registry.ReplaceSettings(TopicGraph.AllTopics);
            registry.Policies.Add<DocumentationProjectLoader>();
        }
    }
}