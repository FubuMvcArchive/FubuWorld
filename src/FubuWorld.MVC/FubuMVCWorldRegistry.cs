using FubuMVC.Core;

namespace FubuWorld.MVC
{
    public class FubuMVCWorldRegistry : FubuPackageRegistry
    {
        public FubuMVCWorldRegistry()
        {
            Policies.Add<FubuMVCWorldMenu>();
        }
    }
}