using FubuCore.Binding;
using FubuDocs.Infrastructure.Binders;
using FubuDocs.Topics;
using FubuMVC.Core;
using FubuMVC.Core.View;

namespace FubuDocs
{
    public class FubuDocsExtension : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.AlterSettings<CommonViewNamespaces>(x => {
                x.AddForType<FubuDocsRegistry>();
                x.Add("FubuWorld.Navigation");
                x.Add("FubuMVC.CodeSnippets");
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