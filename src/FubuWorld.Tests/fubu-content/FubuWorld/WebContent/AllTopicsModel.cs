using System.Collections.Generic;
using System.Linq;
using FubuDocs;
using FubuMVC.Core;
using FubuWorld.Navigation;
using HtmlTags;

namespace FubuWorld
{
    [UrlPattern("topics")]
    public class AllTopicsModel
    {
        public TagList Topics
        {
            get
            {
                List<HtmlTag> tags =
                    TopicGraph.AllTopics.TopLevelNodes()
                              .OrderBy(x => x.Title)
                              .Select(x => new HtmlTag("li").Append(new TopicLinkTag(x)))
                              .ToList();

                return new TagList(tags);
            }
        }
    }
}