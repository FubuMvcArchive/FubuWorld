using System;
using System.Web;
using FubuMVC.Core;

namespace FubuWorld
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            FubuApplication.BootstrapApplication<FubuWorldApplication>();
        }
    }
}