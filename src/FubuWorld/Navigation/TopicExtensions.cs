using System.Web;
using FubuMVC.Core.View;
using FubuWorld.Topics;
using HtmlTags;
using FubuMVC.Core.UI;

namespace FubuWorld.Navigation
{
    public static class TopicExtensions
    {
        public static IHtmlString ProjectSummary(this IFubuPage page)
        {
            var project = page.Get<ITopicContext>().Project;
            return page.Partial(project);
        }

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
            Topic topic = page.Get<ITopicContext>().Current;

            return new TopicLinkTag(topic);
        }
    }

    public class TopicLinkTag : HtmlTag
    {
        public TopicLinkTag(Topic topic) : base("a")
        {
            Attr("href", topic.AbsoluteUrl);
            Text(topic.Title);
            Attr("data-key", topic.Name);
        }
    }
}