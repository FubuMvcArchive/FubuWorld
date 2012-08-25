using FubuMVC.Core.UI.Navigation;

namespace FubuWorld.Navigation
{
    public class FubuWorldMenu : NavigationRegistry
    {
        public FubuWorldMenu()
        {
            ForMenu(FubuWorldKeys.Main);
            Add += MenuNode.ForAction<HomeController>(FubuWorldKeys.Home, x => x.get_home());
        }
    }
}