using StructureMap;

namespace FubuWorld.StructureMap
{
    public static class WebBootstrapper
    {
        public static IContainer BuildContainer()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CoreRegistry>();
            });
            return ObjectFactory.Container;
        }
    }
}