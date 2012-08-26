using FubuCore.Binding;
using FubuWorld.Infrastructure.Binders;
using StructureMap.Configuration.DSL;

namespace FubuWorld.StructureMap
{
    public class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<IPropertyBinder>().Add<RequestLogPropertyBinder>();
        }    
    }
}