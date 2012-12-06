using System;
using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using FubuDocs;
using FubuMVC.Core.Registration;
using FubuCore;
using System.Linq;

namespace FubuWorld.Infrastructure
{
    public class TopicGraphActivator : IActivator
    {
        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            var pool = new TypePool();
            pool.AddAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            pool.IgnoreExportTypeFailures = true;

            pool.TypesMatching(type => type.IsConcreteWithDefaultCtor() && type.CanBeCastTo<TopicRegistry>()).Each(type => {
                // All we have to do is create it to work
                Activator.CreateInstance(type);
            });
        }
    }
}