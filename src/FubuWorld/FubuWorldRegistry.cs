using FubuMVC.Core;
using FubuWorld.Navigation;

namespace FubuWorld
{
    public class FubuWorldRegistry : FubuRegistry
    {
        public FubuWorldRegistry()
        {
            Policies.Add<FubuWorldMenu>();
        }
    }
}