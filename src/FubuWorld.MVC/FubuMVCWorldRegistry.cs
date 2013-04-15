using FubuMVC.Core;
using FubuWorld.Navigation;

namespace FubuWorld.MVC
{
    public class FubuMVCWorldRegistry : FubuPackageRegistry
    {
        public FubuMVCWorldRegistry()
        {
            Policies.Add<FubuWorldMenu>();
        }
    }
}