using FubuMVC.Core;

namespace FubuWorld.MVC
{
    public class FubuMVCWorldRegistry : FubuPackageRegistry
    {
        public FubuMVCWorldRegistry()
        {
            Actions
                .IncludeClassesSuffixedWithController();

            Navigation<FubuMVCWorldMenu>();

            Views
                .TryToAttachWithDefaultConventions();
        }
    }
}