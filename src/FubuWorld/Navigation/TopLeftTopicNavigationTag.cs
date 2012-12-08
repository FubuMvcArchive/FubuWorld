using FubuDocs;
using FubuMVC.Core.Urls;
using HtmlTags;

namespace FubuWorld.Navigation
{
    public class TopLeftTopicNavigationTag : HtmlTag
    {
        public TopLeftTopicNavigationTag(TopicNode node, IUrlRegistry urls) : base("ul")
        {
            AddClass("nav");

            var current = new NamedTopicLinkTag(node, urls);
            current.AddClass("active");

            Append(current);
            var parent = node.Parent;
            while (parent != null)
            {
                var tag = new NamedTopicLinkTag(parent, urls);
                Children.Insert(0, tag);

                parent = parent.Parent;
            }
        }
    }

    public class NamedTopicLinkTag : HtmlTag
    {
        public NamedTopicLinkTag(TopicNode node, IUrlRegistry urls)
            : base("li")
        {
            var url = urls.UrlFor(node.TopicType);
            Add("a").Attr("href", url).Attr("data-key", node.TopicType.Name).Text(node.Title + " »");
        }
    }

    public class TopRightTopicNavigationTag : HtmlTag
    {
        public TopRightTopicNavigationTag(TopicNode node, IUrlRegistry urls)
            : base("ul")
        {
            AddClass("nav");
            Style("float","right");

            var previous = node.FindPrevious();
            if (previous != null)
            {
                Add("li/a")
                    .Attr("href", urls.UrlFor(previous.TopicType))
                    .Text("Previous")
                    .Attr("title", previous.Title);
            }

            var next = node.FindNext();
            if (next != null)
            {
                Add("li/a")
                    .Attr("href", urls.UrlFor(next.TopicType))
                    .Text("Next")
                    .Attr("title", next.Title);
            }
            
            var index = node.FindIndex();
            if (index != null && !ReferenceEquals(node, index))
            {
                Add("li/a")
                    .Attr("href", urls.UrlFor(index.TopicType))
                    .Text("Index")
                    .Attr("title", index.Title);
            }
        }
    }
}