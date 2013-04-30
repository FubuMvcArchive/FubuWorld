using System.Collections.Generic;
using System.Linq;
using FubuDocs.Topics;
using HtmlTags;

namespace FubuDocs.Navigation
{
    public class TopicTreeBuilder
    {
        private readonly Topic _topic;

        public TopicTreeBuilder(ITopicContext context)
        {
            _topic = context.Current;
        }

        public HtmlTag Title()
        {
            return new HtmlTag("h1").Text(_topic.Title);
        }

        public IEnumerable<HtmlTag> BuildTopTopicLinks()
        {
            yield return new TopLeftTopicNavigationTag(_topic);
            yield return new TopRightTopicNavigationTag(_topic);
        }


        public IEnumerable<HtmlTag> BuildLeftTopicLinks()
        {
            Topic next = _topic.FindNext();

            if (next != null)
            {
                yield return new HtmlTag("h4").Text("Next");
                yield return new HtmlTag("p", tag => tag.Append(new TopicLinkTag(next, null)));
            }

            Topic previous = _topic.FindPrevious();

            if (previous != null)
            {
                yield return new HtmlTag("h4").Text("Previous");
                yield return new HtmlTag("p", tag => tag.Append(new TopicLinkTag(previous, null)));
            }
        }

        public HtmlTag BuildTableOfContents()
        {
            if (_topic == null) return new HtmlTag("div").Render(false);

            return new TableOfContentsTag(_topic);
        }
    }

    public class TableOfContentsTag : HtmlTag
    {
        public TableOfContentsTag(Topic topic) : base("ul")
        {
            AddClass("table-of-contents");

            writeChildNodes(topic, this);
        }

        private void writeChildNodes(Topic node, HtmlTag tag)
        {
            node.ChildNodes.Each(childTopic =>
            {
                HtmlTag li = tag.Add("li");
                li.Add("a").Attr("href", childTopic.AbsoluteUrl).Text(childTopic.Title);

                if (childTopic.ChildNodes.Any())
                {
                    HtmlTag ul = li.Add("ul");
                    writeChildNodes(childTopic, ul);
                }
            });
        }
    }
}