using FubuDocs;
using FubuMVC.Core.Urls;
using HtmlTags;

namespace FubuWorld.Navigation
{
    public class TopLeftTopicNavigationTag : HtmlTag
    {
        public TopLeftTopicNavigationTag(TopicNode node) : base("ul")
        {
            AddClass("nav");

            var current = new NamedTopicLinkTag(node);
            current.AddClass("active");

            Append(current);
            var parent = node.Parent;
            while (parent != null)
            {
                var tag = new NamedTopicLinkTag(parent);
                Children.Insert(0, tag);

                parent = parent.Parent;
            }
        }
    }

    public class NamedTopicLinkTag : HtmlTag
    {
        public NamedTopicLinkTag(TopicNode node)
            : base("li")
        {
            Add("a").Attr("href", node.Url).Attr("data-key", node.TopicType.Name).Text(node.Title + " »");
        }
    }

    public class TopRightTopicNavigationTag : HtmlTag
    {
        public TopRightTopicNavigationTag(TopicNode node)
            : base("ul")
        {
            AddClass("nav");
            Style("float","right");

            var previous = node.FindPrevious();
            if (previous != null)
            {
                Add("li/a")
                    .Attr("href", previous.Url)
                    .Text("Previous")
                    .Attr("title", previous.Title);
            }

            var next = node.FindNext();
            if (next != null)
            {
                Add("li/a")
                    .Attr("href", next.Url)
                    .Text("Next")
                    .Attr("title", next.Title);
            }
            
            var index = node.FindIndex();
            if (index != null && !ReferenceEquals(node, index))
            {
                Add("li/a")
                    .Attr("href", index.Url)
                    .Text("Index")
                    .Attr("title", index.Title);
            }
        }
    }
}