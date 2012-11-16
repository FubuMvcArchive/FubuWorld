using FubuMVC.Core;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Navigation;
using FubuWorld.Navigation;

namespace FubuWorld
{
    public class FubuWorldRegistry : FubuRegistry
    {
        public FubuWorldRegistry()
        {
            Actions
                .IncludeClassesSuffixedWithController();

            Policies.Add<FubuWorldMenu>();
            Policies.Add<NavigationRootPolicy>(x =>
            {
                x.ForKey(FubuWorldKeys.Main);
                x.WrapWithChrome<FubuWorldChrome>();
            });
        }
    }
}