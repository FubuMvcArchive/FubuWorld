using FubuDocs;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using FubuMVC.Core.View;
using HtmlTags;
using System.Collections.Generic;
using System.Linq;

namespace FubuWorld.Navigation
{
    public static class TopicExtensions
    {
        public static HtmlTag TableOfContents(this IFubuPage page)
        {
            return page.Get<TopicTreeBuilder>().BuildTableOfContents();
        }
    }

    public class TopicTreeBuilder
    {
        private readonly IUrlRegistry _urls;
        private readonly IFubuRequest _request;

        public TopicTreeBuilder(IUrlRegistry urls, IFubuRequest request)
        {
            _urls = urls;
            _request = request;
        }

        public HtmlTag BuildTableOfContents()
        {
            var tag = new HtmlTag("ul").AddClass("table-of-contents");

            var topic = _request.Find<Topic>().FirstOrDefault();
            if (topic == null) return new HtmlTag("div").Render(false);

            var graph = TopicGraph.AllTopics;

            var node = graph.Find(topic.GetType());

            writeChildNodes(node, tag);

            return tag;
        }

        private void writeChildNodes(TopicNode node, HtmlTag tag)
        {
            node.ChildNodes.Each(childTopic => {
                var li = tag.Add("li");
                li.Add("a").Attr("href", _urls.UrlFor(childTopic.TopicType)).Text(childTopic.Title);

                if (childTopic.ChildNodes.Any())
                {
                    var ul = li.Add("ul");
                    writeChildNodes(childTopic, ul);
                }
            });
        }
    }
}