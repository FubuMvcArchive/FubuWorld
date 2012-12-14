using FubuDocs;
using FubuMVC.Core.View;
using HtmlTags;

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

        public static TagList TopTopicNavigation(this IFubuPage page)
        {
            return page.Get<TopicTreeBuilder>().BuildTopTopicLinks().ToTagList();
        }

        public static HtmlTag LinkToTopic(this IFubuPage page, string name)
        {
            var node = TopicGraph.AllTopics.FindByName(name);

            return new TopicLinkTag(node);
        }
    }

    public class TopicLinkTag : HtmlTag
    {
        public TopicLinkTag(TopicNode node) : base("a")
        {
            Attr("href", node.Url);
            Text(node.Title);
            Attr("data-key", node.TopicType.Name);
        }
    }
}