using System;
using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using FubuCore;
using FubuDocs;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Urls;

namespace FubuWorld.Infrastructure
{
    public class TopicGraphActivator : IActivator
    {
        private readonly IUrlRegistry _urls;

        public TopicGraphActivator(IUrlRegistry urls)
        {
            _urls = urls;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            var pool = new TypePool();
            pool.AddAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            pool.IgnoreExportTypeFailures = true;

            pool.TypesMatching(type => type.IsConcreteWithDefaultCtor() && type.CanBeCastTo<TopicRegistry>())
                .Each(type => {
                    // All we have to do is create it to work
                    Activator.CreateInstance(type);
                });

            TopicGraph.AllTopics.All().Each(node => {
                node.Url = _urls.UrlFor(node.TopicType);
            });
        }
    }
}