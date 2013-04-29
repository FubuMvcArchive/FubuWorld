using FubuDocs.Infrastructure;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Urls;

namespace FubuDocs.Tools
{
    public class ToolViewModel
    {
        public string Editor { get; set; }
    }

    public class ToolsEndpoints
    {
        public ToolViewModel get_tools()
         {
             return new ToolViewModel
             {
                 Editor = EditorLauncher.GetEditor()
             };
         }

        public AjaxContinuation post_tools(ToolViewModel model)
        {
            EditorLauncher.SetEditor(model.Editor);

            return AjaxContinuation.Successful();
        }
    }
}