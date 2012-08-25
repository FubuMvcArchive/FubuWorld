using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuWorld.StructureMap;

namespace FubuWorld
{
    public class FubuWorldApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            return FubuApplication
                .For<FubuWorldRegistry>()
                .StructureMap(() => WebBootstrapper.BuildContainer());
        }
    }
}