using Bottles;
using FubuCore.Binding;
using FubuMVC.Core;
using FubuWorld.Infrastructure;
using FubuWorld.Infrastructure.Binders;
using Spark;

namespace FubuWorld
{
    public class FubuWorldRegistry : FubuPackageRegistry
    {
        public FubuWorldRegistry()
        {

        }
    }

    public class AllTopicsEndpoint
    {
        public AllTopicsModel get_topics()
        {
            return new AllTopicsModel();
        }
    }

    public class FubuWorldExtension : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Policies.Add<TopicUrlPolicy>();

            registry.AlterSettings<SparkSettings>(x =>
            {
                x.AddNamespace(GetType().Namespace);
            });

            registry.Services(x =>
            {
                x.AddService<IPropertyBinder, RequestLogPropertyBinder>();
                x.AddService<IActivator, TopicGraphActivator>();
            });
        }
    }
}