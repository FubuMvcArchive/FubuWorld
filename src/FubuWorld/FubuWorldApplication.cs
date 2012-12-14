using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuWorld.Infrastructure;

namespace FubuWorld
{
    public class FubuWorldApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            return FubuApplication
                .For<FubuWorldRegistry>()
                .StructureMap<StructureMapRegistry>()
                .Packages(x => x.Loader(new FubuDocModuleAttributePackageLoader()));
        }
    }
}