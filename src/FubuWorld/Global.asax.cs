using System;
using Bottles;
using StructureMap;

namespace FubuWorld
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new FubuWorldApplication()
                .BuildApplication()
                .Bootstrap();

            PackageRegistry.AssertNoFailures();
        }

        protected void Application_End(Object sender, EventArgs e)
        {
            ObjectFactory.Container.Dispose();
        }
    }
}