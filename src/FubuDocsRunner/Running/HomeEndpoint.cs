using FubuDocs;
using FubuMVC.Core.Continuations;

namespace FubuDocsRunner.Running
{
    public class HomeEndpoint
    {
        public FubuContinuation Index()
        {
            return FubuContinuation.RedirectTo<AllTopicsEndpoint>(x => x.get_topics());
        }
    }
}