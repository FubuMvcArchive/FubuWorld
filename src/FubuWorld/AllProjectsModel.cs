using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using FubuWorld.Navigation;
using FubuWorld.Topics;
using HtmlTags;

namespace FubuWorld
{
    [UrlPattern("projects")]
    public class AllProjectsModel
    {
        public TagList Topics
        {
            get
            {
                List<HtmlTag> tags =
                    TopicGraph.AllTopics.Projects
                              .OrderBy(x => x.Name)
                              .Select(x => new HtmlTag("li").Append(new TopicLinkTag(x.Root)))
                              .ToList();

                return new TagList(tags);
            }
        }
    }
}