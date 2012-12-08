using FubuMVC.Core.View;
using HtmlTags;

namespace FubuWorld.Navigation
{
    public static class FubuWorldPageExtensions
    {
         public static HtmlTag SectionFor(this IFubuPage page, string text, string id)
         {
             return new HtmlTag("section", tag => tag.Add("h4").Text(text).AddClass("section-header")).Id(id).NoClosingTag();
         }
    }
}