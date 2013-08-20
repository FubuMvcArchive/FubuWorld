using FubuMVC.Core;
using FubuWorld.Navigation;

namespace FubuWorld
{
    public class FubuWorldWebsiteRegistry : FubuPackageRegistry
    {
        public FubuWorldWebsiteRegistry()
        {
            Policies.Add<FubuWorldMenu>(); 
        }
    }

    public class FubuWorldExtensions : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Routes.HomeIs<HomeEndpoint>(x => x.Index());
        }
    }
}