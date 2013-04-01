﻿using FubuWorld.Topics;
using HtmlTags;

namespace FubuWorld.Navigation
{
    public class TopLeftTopicNavigationTag : HtmlTag
    {
        public TopLeftTopicNavigationTag(Topic node) : base("ul")
        {
            AddClass("nav");

            var current = new NamedTopicLinkTag(node);
            current.AddClass("active");

            Append(current);
            Topic parent = node.Parent;
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
        public NamedTopicLinkTag(Topic node)
            : base("li")
        {
            Add("a").Attr("href", node.AbsoluteUrl).Attr("data-key", node.Name).Text(node.Title + " »");
        }
    }

    public class TopRightTopicNavigationTag : HtmlTag
    {
        public TopRightTopicNavigationTag(Topic node)
            : base("ul")
        {
            AddClass("nav");
            Style("float", "right");

            Topic previous = node.FindPrevious();
            if (previous != null)
            {
                Add("li/a")
                    .Attr("href", previous.AbsoluteUrl)
                    .Text("Previous")
                    .Attr("title", previous.Title);
            }

            Topic next = node.FindNext();
            if (next != null)
            {
                Add("li/a")
                    .Attr("href", next.AbsoluteUrl)
                    .Text("Next")
                    .Attr("title", next.Title);
            }

            Topic index = node.FindIndex();
            if (index != null && !ReferenceEquals(node, index))
            {
                Add("li/a")
                    .Attr("href", index.AbsoluteUrl)
                    .Text("Index")
                    .Attr("title", index.Title);
            }
        }
    }
}