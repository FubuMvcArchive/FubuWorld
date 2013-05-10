﻿using System.IO;
using FubuCore;
using FubuDocs.Infrastructure;
using FubuDocs.Snippets;
using FubuDocs.Todos;
using FubuDocs.Tools;
using FubuDocs.Topics;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using FubuMVC.Core.View;
using HtmlTags;

namespace FubuDocs.Navigation
{
    public static class FubuDocsPageExtensions
    {
         public static HtmlTag SectionFor(this IFubuPage page, string text, string id)
         {
             return new SectionTag(text, id).NoClosingTag();
         }

        

        public static HtmlTag AuthoringTopic(this IFubuPage page)
        {
            var tag = new HtmlTag("div").AddClass("alert").AddClass("alert-block").AddClass("authoring");
            
            if (FubuMode.InDevelopment())
            {
                var context = page.Get<ITopicContext>();
                var url = page.Urls.UrlFor<FileRequest>();
                var topic = context.Current;

                var firstLine = tag.Add("div");
                firstLine.Add("a").Data("url", url).Data("key", topic.Key).Attr("href", "#").AddClass("edit-link").Text(context.File);

                var lastUpdated = File.GetLastWriteTimeUtc(context.File).ToLocalTime();
                firstLine.Add("span").AddClass("last-updated").Text("File changed at: " + lastUpdated).AddClass("file-changed");

                var secondLine = tag.Add("div");
                var toolsUrl = page.Urls.UrlFor<ToolsEndpoints>(x => x.get_tools());
                secondLine.Add("a").Text("Tools").Attr("href", toolsUrl);
                secondLine.Add("span").Text(" | ");

                var projectUrl = page.Urls.UrlFor(new ProjectRequest {Name = context.Project.Name});
                secondLine.Add("a").Text(context.Project.Name + " Project Page").Attr("href", projectUrl);
                secondLine.Add("span").Text(" | ");

                var todoUrl = page.Urls.UrlFor<TodoEndpoint>(x => x.get_todos());
                secondLine.Add("a").Text("TODO's").Attr("href", todoUrl);
                secondLine.Add("span").Text(" | ");

                var snippetsUrl = page.Urls.UrlFor<SnippetEndpoints>(x => x.get_snippets());
                secondLine.Add("a").Text("Code Snippets").Attr("href", snippetsUrl);
                secondLine.Add("span").Text(" | ");



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

            EditorLauncher.LaunchFile(topic.File.FilePath);
        }
    }

    public class FileRequest
    {
        public string Key { get; set; }
    }
}