using System.IO;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.View;
using FubuWorld.Topics;
using HtmlTags;

namespace FubuWorld.Navigation
{
    public static class FubuWorldPageExtensions
    {

         public static HtmlTag SectionFor(this IFubuPage page, string text, string id)
         {
             return new HtmlTag("section", tag => tag.Add("h4").Text(text).AddClass("section-header")).Id(id).NoClosingTag();
         }

        public static HtmlTag AuthoringTopic(this IFubuPage page)
        {
            var tag = new HtmlTag("div").AddClass("authoring");
            
            if (FubuMode.InDevelopment())
            {
                var context = page.Get<ITopicContext>();
                var url = page.Urls.UrlFor<FileRequest>();
                tag.Add("a").Data("url", url).Data("key", context.Current.Key).Attr("href", "#").AddClass("edit-link").Text(context.File);

                var lastUpdated = File.GetLastWriteTimeUtc(context.File).ToLocalTime();
                tag.Add("span").AddClass("last-updated").Text("File changed at: " + lastUpdated);
                
            }
            else
            {
                tag.Render(false);
            }


            


            return tag;
        }
    }

    public class EditFileEndpoint
    {
        public void post_edit_file(FileRequest request)
        {
            var topic = TopicGraph.AllTopics.Find(request.Key);

            new FileSystem().LaunchEditor(topic.File.FilePath);
        }
    }

    public class FileRequest
    {
        public string Key { get; set; }
    }
}