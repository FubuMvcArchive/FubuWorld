using Bottles;
using FubuCore.Binding;
using FubuWorld.Infrastructure;
using FubuWorld.Infrastructure.Binders;
using StructureMap.Configuration.DSL;

namespace FubuWorld
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
            });

            For<IPropertyBinder>().Add<RequestLogPropertyBinder>();
            For<IActivator>().Add<TopicGraphActivator>();
        }
    }
}