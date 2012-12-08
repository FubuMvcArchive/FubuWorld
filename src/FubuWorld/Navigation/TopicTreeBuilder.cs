using System;
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

        public static TagList LeftTopicNavigation(this IFubuPage page)
        {
            return page.Get<TopicTreeBuilder>().BuildLeftTopicLinks().ToTagList();
        }
    }

    public class TopicTreeBuilder
    {
        private readonly IUrlRegistry _urls;
        private readonly Lazy<TopicNode> _topic;

        public TopicTreeBuilder(IUrlRegistry urls, IFubuRequest request)
        {
            _urls = urls;
            _topic = new Lazy<TopicNode>(() => {
                var topic = request.Find<Topic>().FirstOrDefault();
                if (topic == null) return null;

                var graph = TopicGraph.AllTopics;

                return graph.Find(topic.GetType());
            });
        }

        public IEnumerable<HtmlTag> BuildLeftTopicLinks()
        {
            var next = _topic.Value.FindNext();

            if (next != null)
            {
                yield return new HtmlTag("h4").Text("Next");
                yield return new HtmlTag("p", tag => {
                    tag.Add("a").Text(next.Title).Attr("href", _urls.UrlFor(next.TopicType));
                });
            }

            var previous = _topic.Value.FindPrevious();

            if (previous != null)
            {
                yield return new HtmlTag("h4").Text("Previous");
                yield return new HtmlTag("p", tag => {
                    tag.Add("a").Text(previous.Title).Attr("href", _urls.UrlFor(previous.TopicType));
                });
            }
        }

        public HtmlTag BuildTableOfContents()
        {
            var tag = new HtmlTag("ul").AddClass("table-of-contents");

            if (_topic.Value == null) return new HtmlTag("div").Render(false);

            writeChildNodes(_topic.Value, tag);

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