using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuWorld;
using FubuWorld.Infrastructure;
using StructureMap;

namespace FubuMVC.Website
{
    public class FubuWorldWebsiteApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            return FubuApplication
                .For<FubuWorldWebsiteRegistry>()
                .StructureMap<StructureMapRegistry>()
                .Packages(x => x.Loader(new FubuDocModuleAttributePackageLoader()));
        }
    }
}