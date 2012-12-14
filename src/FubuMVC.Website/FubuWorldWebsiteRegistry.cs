using FubuMVC.Core;
using FubuMVC.Website.Navigation;
using FubuWorld;

namespace FubuMVC.Website
{
    public class FubuWorldWebsiteRegistry : FubuRegistry
    {
        public FubuWorldWebsiteRegistry()
        {
            Policies.Add<FubuWorldMenu>();
            Import<FubuWorldExtension>();
        }
    }
}