using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.Core.View;
using FubuWorld.Infrastructure.Binders;

namespace FubuWorld
{
    public class FubuWorldRegistry : FubuPackageRegistry
    {
    }

    public class AllTopicsEndpoint
    {
        public AllProjectsModel get_topics()
        {
            return new AllProjectsModel();
        }
    }

    public class FubuWorldExtension : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.AlterSettings<CommonViewNamespaces>(x => { x.AddForType<FubuWorldRegistry>(); });

            registry.Services(x => { x.AddService<IPropertyBinder, RequestLogPropertyBinder>(); });
        }
    }
}