using FubuMVC.Core;
using FubuWorld.Infrastructure;
using FubuWorld.Navigation;

namespace FubuWorld
{
    public class FubuWorldRegistry : FubuRegistry
    {
        public FubuWorldRegistry()
        {
            Policies.Add<FubuWorldMenu>();
            Policies.Add<TopicUrlPolicy>();
        }
    }
}