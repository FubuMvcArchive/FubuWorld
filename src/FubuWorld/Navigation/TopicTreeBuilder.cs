using System;
using FubuDocs;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using HtmlTags;
using System.Collections.Generic;
using System.Linq;

namespace FubuWorld.Navigation
{
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

        public HtmlTag Title()
        {
            return new HtmlTag("h1").Text(_topic.Value.Title);
        }
        public IEnumerable<HtmlTag> BuildTopTopicLinks()
        {
            yield return new TopLeftTopicNavigationTag(_topic.Value, _urls);
            yield return new TopRightTopicNavigationTag(_topic.Value, _urls);
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