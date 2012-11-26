using FubuMVC.Navigation;
using FubuWorld.Navigation;

namespace FubuWorld.MVC
{
    public class FubuMVCWorldMenu : NavigationRegistry
    {
        public FubuMVCWorldMenu()
        {
            ForMenu(FubuWorldKeys.Main);
            var mvc = MenuNode.ForInput<FubuMVCHomeInput>(FubuMVCWorldKeys.MvcHome);
            configureChildren(mvc);
            Add += mvc;
        }

        private void configureChildren(MenuNode parent)
        {
            parent.AddChild(MenuNode.ForInput<MvcGettingStarted>(FubuMVCWorldKeys.GettingStarted));
        }
    }
}