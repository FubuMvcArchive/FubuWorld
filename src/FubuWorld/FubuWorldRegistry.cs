using FubuMVC.Core;
using FubuMVC.Core.Registration.Conventions;
using FubuWorld.Navigation;

namespace FubuWorld
{
    public class FubuWorldRegistry : FubuRegistry
    {
        public FubuWorldRegistry()
        {
            Actions.IncludeClassesSuffixedWithController();

            Navigation<FubuWorldMenu>();

            Routes.HomeIs<HomeController>(x => x.get_home());

            Views.TryToAttachWithDefaultConventions();

            ApplyConvention<NavigationRootPolicy>(x =>
            {
                x.ForKey(FubuWorldKeys.Main);
                x.WrapWithChrome<FubuWorldChrome>();
            });
        }
    }
}