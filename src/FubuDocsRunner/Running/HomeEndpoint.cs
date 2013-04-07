using FubuMVC.Core.Continuations;
using FubuWorld;

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