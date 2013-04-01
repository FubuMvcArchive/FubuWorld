using System;
using FubuMVC.Core.View;
using FubuWorld.Topics;
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
            var graph = page.Get<TopicGraph>();

            throw new NotImplementedException("Need to look for the current project first");
            Topic node = graph.Find(name);

            return new TopicLinkTag(node);
        }
    }

    public class TopicLinkTag : HtmlTag
    {
        public TopicLinkTag(Topic topic) : base("a")
        {
            Attr("href", topic.Url);
            Text(topic.Title);
            Attr("data-key", topic.Name);
        }
    }
}