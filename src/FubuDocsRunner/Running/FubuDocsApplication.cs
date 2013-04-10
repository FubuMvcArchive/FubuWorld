using System;
using System.Collections.Generic;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuWorld.Infrastructure;
using StructureMap;

namespace FubuDocsRunner.Running
{
    public class FubuDocsApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            return FubuApplication.For<RunFubuWorldRegistry>()
                                  .StructureMap(new Container())
                                  .Packages(x => {
                                      string[] directories =
                                          AppDomain.CurrentDomain.SetupInformation.AppDomainInitializerArguments;
                                      directories.Each(directory
                                                       => x.Loader(new DocumentPackageLoader(directory)));

                                      x.Loader(new FubuDocsPackageLoader());
                                  });
        }
    }
}