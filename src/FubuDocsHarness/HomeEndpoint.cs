using FubuDocs.Tools;
using FubuMVC.Core.Continuations;

namespace FubuDocsHarness
{
    public class HomeEndpoint
    {
        public FubuContinuation Index()
        {
            return FubuContinuation.RedirectTo<ToolsEndpoints>(x => x.get_tools());
        }
    }
}