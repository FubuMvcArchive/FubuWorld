using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FubuCore;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Resources.PathBased;

namespace FubuDocs.Exporting
{
    public class UrlQueryEndpoint
    {
        private readonly BehaviorGraph _graph;

        public UrlQueryEndpoint(BehaviorGraph graph)
        {
            _graph = graph;
        }

        public UrlList get_urls()
        {
            return new UrlList
            {
                Urls = findUrls().ToArray()
            };
        }

        private IEnumerable<string> findUrls()
        {
            foreach (var chain in _graph.Behaviors)
            {
                if (chain.IsPartialOnly) continue;
                if (chain.Route == null) continue;
                if (!chain.Route.RespondsToMethod("GET")) continue;

                
                if (chain.Route.Rank != 0) continue;
                if (chain.InputType() != null && chain.InputType().CanBeCastTo<ResourcePath>()) continue;

                if (chain.Calls.Any(x => x.HandlerType.Assembly == Assembly.GetExecutingAssembly()))
                {
                    if (!chain.Calls.Any(x => x.HasAttribute<ExportAttribute>())) continue;
                }

                if (chain.GetRoutePattern().StartsWith("_fubu")) continue;
                if (chain.GetRoutePattern().StartsWith("_diagnostics")) continue;
                if (chain.GetRoutePattern().StartsWith("_content")) continue;
                if (chain.GetRoutePattern() == "_about") continue;


                yield return chain.GetRoutePattern();
            }
        }
    }
}