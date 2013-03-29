using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bottles;
using FubuCore.Util;
using FubuMVC.Core;
using FubuMVC.Core.Registration;

namespace FubuWorld.Topics
{
    [ConfigurationType(ConfigurationType.Discovery)]
    public class DocumentationProjectLoader : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            PackageRegistry.Packages.Where(x => x.Name.EndsWith(".Docs"))
                           .Each(
                               pak => { pak.ForFolder(BottleFiles.WebContentFolder, dir => LoadPackage(pak, dir, graph)); });
        }

        public void LoadPackage(IPackageInfo pak, string directory, BehaviorGraph graph)
        {
            // 1.) Load the project file itself and build the project root
            // 2.) go through the folders and build out the ITopicFile's

            //var loader = new TopicLoader();
            //IEnumerable<ITopicFile> files = loader.FindFilesFromBottle(pak.Name);

            // find the project.spark file.  If it does not exist, use the default one and create
            // a new Topic for the default.

            // group the files in a hierarchy and order. 
            // Build the Topic's

            // add the new ProjectRoot to 

            throw new NotImplementedException();
        }
    }
}