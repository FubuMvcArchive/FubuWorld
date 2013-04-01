using System;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using FubuWorld.Topics;
using HtmlTags;
using System.Collections.Generic;
using System.Linq;

namespace FubuWorld.Navigation
{
    public class TopicTreeBuilder
    {
        private readonly IUrlRegistry _urls;
        private readonly Lazy<Topic> _topic;

        public TopicTreeBuilder(IUrlRegistry urls, IFubuRequest request)
        {
            _urls = urls;
            _topic = new Lazy<Topic>(() => {
                throw new NotImplementedException("Need the service that gives you the current topic");
//                var topic = request.Find<Topic>().FirstOrDefault();
//                if (topic == null) return null;
//
//                var graph = TopicGraph.AllTopics;
//
//                return graph.Find(topic.GetType());
            });
        }

        public HtmlTag Title()
        {
            return new HtmlTag("h1").Text(_topic.Value.Title);
        }
        public IEnumerable<HtmlTag> BuildTopTopicLinks()
        {
            yield return new TopLeftTopicNavigationTag(_topic.Value);
            yield return new TopRightTopicNavigationTag(_topic.Value);
        }


        public IEnumerable<HtmlTag> BuildLeftTopicLinks()
        {
            var next = _topic.Value.FindNext();

            if (next != null)
            {
                yield return new HtmlTag("h4").Text("Next");
                yield return new HtmlTag("p", tag => tag.Append(new TopicLinkTag(next)));
            }

            var previous = _topic.Value.FindPrevious();

            if (previous != null)
            {
                yield return new HtmlTag("h4").Text("Previous");
                yield return new HtmlTag("p", tag => tag.Append(new TopicLinkTag(previous)));
            }
        }

        public HtmlTag BuildTableOfContents()
        {
            var tag = new HtmlTag("ul").AddClass("table-of-contents");

            if (_topic.Value == null) return new HtmlTag("div").Render(false);

            writeChildNodes(_topic.Value, tag);

            return tag;
        }

        private void writeChildNodes(Topic node, HtmlTag tag)
        {
            node.ChildNodes.Each(childTopic => {
                var li = tag.Add("li");
                li.Add("a").Attr("href", childTopic.Url).Text(childTopic.Title);

                if (childTopic.ChildNodes.Any())
                {
                    var ul = li.Add("ul");
                    writeChildNodes(childTopic, ul);
                }
            });
        }
    }
}