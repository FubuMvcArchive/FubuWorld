using FubuMVC.Navigation;
using FubuWorld.Navigation;

namespace FubuWorld.MVC
{
    public class FubuMVCWorldMenu : NavigationRegistry
    {
        public FubuMVCWorldMenu()
        {
            ForMenu(FubuWorldKeys.Main);
            Add += MenuNode.ForInput<FubuMVCHomeInput>(FubuMVCWorldKeys.MvcHome);
        }
    }
}