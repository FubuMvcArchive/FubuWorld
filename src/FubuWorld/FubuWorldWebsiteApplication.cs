using System.ComponentModel;
using FubuDocs.Infrastructure;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using Container = StructureMap.Container;

namespace FubuWorld
{
    public class FubuWorldWebsiteApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            return FubuApplication
                .For<FubuWorldWebsiteRegistry>()
                .StructureMap(new Container())
                .Packages(x => x.Loader(new FubuDocsPackageLoader()));
        }
    }
}