using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bottles;
using FubuCore;
using FubuCore.Util;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.View;

namespace FubuWorld.Topics
{
    /*
     * TODO --------------------
     * - find all the ITopicFile's
     * - find Url for a named topic key
     * - show all topics
     * - register DocumentationProjectLoader
     * - going to need a full end to end test for SparkTopicFile
     */

    [ConfigurationType(ConfigurationType.Discovery)]
    public class DocumentationProjectLoader : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            PackageRegistry.Packages.Where(x => x.Name.EndsWith(".Docs"))
                           .Each(
                               pak => { pak.ForFolder(BottleFiles.WebContentFolder, dir => LoadPackage(pak, dir, graph)); });
        }

        private void LoadPackage(IPackageInfo pak, string directory, BehaviorGraph graph)
        {
            // 1.) Load the project file itself and build the project root
            // 2.) go through the folders and build out the ITopicFile's

            //var loader = new TopicFileLoader();
            //IEnumerable<ITopicFile> files = loader.FindFilesFromBottle(pak.Name);

            // find the project.spark file.  If it does not exist, use the default one and create
            // a new TopicNode for the default.

            // group the files in a hierarchy and order. 
            // Build the TopicNode's

            // add the new ProjectRoot to 

            throw new NotImplementedException();
        }
    }

    

    public interface ITopicFile
    {
        string FilePath { get; }
        string Name { get; }
        string Folder { get; }
        IViewToken ToViewToken();
    }

    [ApplicationLevel]
    public class TopicGraph
    {
        private readonly Cache<string, ProjectRoot> _projects = new Cache<string, ProjectRoot>();

        public void AddProject(ProjectRoot project)
        {
            _projects[project.Name] = project;
        }
    }

    // TODO -- add DescibesItself stuff to this
    public class TopicBehavior : BasicBehavior
    {
        private readonly TopicNode _node;
        private readonly IMediaWriter<TopicNode> _writer;

        public TopicBehavior(TopicNode node, IMediaWriter<TopicNode> writer) : base(PartialBehavior.Executes)
        {
            _node = node;
            _writer = writer;
        }

        protected override DoNext performInvoke()
        {
            _writer.Write(MimeType.Html.Value, _node);
            return DoNext.Continue;
        }
    }


    public class TopicBehaviorNode : BehaviorNode, IMayHaveInputType
    {
        private readonly TopicNode _node;
        private readonly ViewNode _view;

        public TopicBehaviorNode(TopicNode node, ViewNode view)
        {
            _node = node;
            _view = view;
        }

        public TopicNode Node
        {
            get { return _node; }
        }

        public override BehaviorCategory Category
        {
            get { return BehaviorCategory.Output; }
        }

        public Type InputType()
        {
            return typeof (TopicNode);
        }

        protected override ObjectDef buildObjectDef()
        {
            ObjectDef def = ObjectDef.ForType<TopicBehavior>();

            def.DependencyByValue(typeof (TopicNode), Node);
            ObjectDef writerDef = _view.As<IContainerModel>().ToObjectDef();
            def.Dependency(typeof (IMediaWriter<TopicNode>), writerDef);

            return def;
        }
    }
}