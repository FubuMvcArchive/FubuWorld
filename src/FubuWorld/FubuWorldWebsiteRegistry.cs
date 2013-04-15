using FubuMVC.Core;
using FubuWorld.Navigation;

namespace FubuWorld
{
    public class FubuWorldWebsiteRegistry : FubuRegistry
    {
        public FubuWorldWebsiteRegistry()
        {
            Policies.Add<FubuWorldMenu>();

            Routes.HomeIs<HomeEndpoint>(x => x.Index());
        }
    }
}