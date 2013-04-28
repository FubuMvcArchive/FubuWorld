using System.Collections.Generic;
using System.Linq;
using FubuDocs.Navigation;
using FubuDocs.Topics;
using FubuMVC.Core;
using HtmlTags;

namespace FubuDocs
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
                              .Select(x => new HtmlTag("li").Append(new TopicLinkTag(x.Index)))
                              .ToList();

                return new TagList(tags);
            }
        }
    }
}