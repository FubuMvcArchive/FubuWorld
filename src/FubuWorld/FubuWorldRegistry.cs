using FubuMVC.Core;
using FubuWorld.Infrastructure;

namespace FubuWorld
{
    public class FubuWorldRegistry : FubuRegistry
    {
        public FubuWorldRegistry()
        {
            Import<FubuWorldExtension>();
            Routes.HomeIs<AllTopicsEndpoint>(x => x.get_topics());
        }
    }

    public class AllTopicsEndpoint
    {
        public AllTopicsModel get_topics()
        {
            return new AllTopicsModel();
        }
    }

    public class FubuWorldExtension : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Policies.Add<TopicUrlPolicy>();
        }
    }
}