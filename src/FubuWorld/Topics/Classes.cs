using System;
using System.Collections.Generic;
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
using FubuMVC.Core.View.Model;
using FubuMVC.Spark.SparkModel;

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
            PackageRegistry.Packages.Where(x => x.Name.EndsWith(".Docs")).Each(pak => {
                pak.ForFolder(BottleFiles.WebContentFolder, dir => LoadPackage(pak, dir, graph));
            });
        }

        private void LoadPackage(IPackageInfo pak, string directory, BehaviorGraph graph)
        {
            // 1.) Load the project file itself and build the project root
            // 2.) go through the folders and build out the ITopicFile's

            var loader = new TopicFileLoader();
            var files = loader.FindFilesFromBottle(pak.Name);

            // find the project.spark file.  If it does not exist, use the default one and create
            // a new TopicNode for the default.

            // group the files in a hierarchy and order. 
            // Build the TopicNode's

            // add the new ProjectRoot to 

            throw new NotImplementedException();
        }
    }

    public class TopicFileLoader
    {
        private readonly SparkTemplateRegistry _sparkTemplates = new SparkTemplateRegistry();
        // needs to use SparkTemplateRegistry to find descriptors

        public static bool IsTopic<T>(ViewDescriptor<T> descriptor) where T : ITemplateFile
        {
            if (descriptor.HasViewModel()) return false;
            
            if (descriptor.ViewPath.Contains("/Samples/") || descriptor.ViewPath.Contains("/Examples/")) return false;

            return true;
        }

        public IEnumerable<ITopicFile> FindFilesFromBottle(string bottleName)
        {
            return _sparkTemplates.Where(x => x.Origin == bottleName)
                                  .OfType<ViewDescriptor<Template>>()
                                  .Where(IsTopic)
                                  .Select(x => new SparkTopicFile(x));
        } 
    }

    public interface ITopicFile
    {
        string FilePath { get; }
        string Name { get; }
        string RelativePath();
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

    public class ProjectRoot
    {
        public static ProjectRoot LoadFrom(string file)
        {
            throw new NotImplementedException();
        }

        public static void WriteTo(string file)
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }
        public string GitHubPage { get; set; }
        public string UserGroupUrl { get; set; }
        public string BuildServerUrl { get; set; }
        public string BottleName { get; set; }

        public TopicNode RootNode { get; set; }


        /*
         * Extends what?
         * Keywords?
         * Nugets?
         * 
         * 
         * 
         * 
         */
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

        protected override ObjectDef buildObjectDef()
        {
            var def = ObjectDef.ForType<TopicBehavior>();

            def.DependencyByValue(typeof(TopicNode), Node);
            var writerDef = _view.As<IContainerModel>().ToObjectDef();
            def.Dependency(typeof(IMediaWriter<TopicNode>), writerDef);

            return def;
        }

        public override BehaviorCategory Category
        {
            get { return BehaviorCategory.Output; }
        }

        public Type InputType()
        {
            return typeof (TopicNode);
        }
    }

    public class TopicNode
    {
        private readonly ITopicFile _file;
        private readonly ProjectRoot _projectRoot;
        private TopicNode _firstChild;

        private TopicNode _next;
        private TopicNode _parent;
        private TopicNode _previous;

        public TopicNode(ProjectRoot projectRoot, ITopicFile file)
        {
            _projectRoot = projectRoot;
            _file = file;
        }

        public string Key
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Url
        {
            get { throw new NotImplementedException(); }
        }

        public string Title
        {
            get { throw new NotImplementedException(); }
        }


        public TopicNode NextSibling
        {
            get { return _next; }
        }

        public TopicNode PreviousSibling
        {
            get { return _previous; }
        }

        public TopicNode Parent
        {
            get { return _previous == null ? _parent : _previous.Parent; }
        }

        public IEnumerable<TopicNode> ChildNodes
        {
            get
            {
                TopicNode node = FirstChild;
                while (node != null)
                {
                    yield return node;

                    node = node.NextSibling;
                }
            }
        }

        public TopicNode FirstChild
        {
            get { return _firstChild; }
            private set
            {
                if (value != null)
                {
                    value._parent = this;
                }
                _firstChild = value;
            }
        }

        public TopicNode LastChild
        {
            get { return ChildNodes.LastOrDefault(); }
        }

        public void BuildChain(BehaviorGraph graph)
        {
            throw new NotImplementedException();
        }

        public void AppendChild(TopicNode node)
        {
            TopicNode last = LastChild;
            if (last == null)
            {
                FirstChild = node;
            }
            else
            {
                last.InsertAfter(node);
            }
        }

        public void PrependChild(TopicNode node)
        {
            if (_firstChild != null)
            {
                _firstChild._previous = node;
                node._next = _firstChild;
            }

            FirstChild = node;
        }

        public void InsertAfter(TopicNode node)
        {
            if (_next != null)
            {
                _next._previous = node;
                node._next = _next;
            }

            node._previous = this;
            _next = node;
        }

        public void InsertBefore(TopicNode node)
        {
            if (_previous == null)
            {
                if (_parent != null)
                {
                    _parent.PrependChild(node);
                }
                else
                {
                    node._next = this;
                    _previous = node;
                }
            }
            else
            {
                _previous._next = node;
                node._previous = _previous;

                node._next = this;
                _previous = node;
            }
        }

        public void Remove()
        {
            TopicNode parent = Parent;
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }

        public void RemoveChild(TopicNode child)
        {
            List<TopicNode> children = ChildNodes.ToList();
            children.Remove(child);

            child._parent = null;
            child._previous = null;
            child._next = null;


            if (!children.Any())
            {
                _firstChild = null;
                return;
            }

            _firstChild = children.First();
            _firstChild._parent = this;
            _firstChild._previous = null;

            children.Last()._next = null;

            for (int i = 1; i < children.Count; i++)
            {
                children[i]._previous = children[i - 1];
            }

            for (int i = 0; i < children.Count - 1; i++)
            {
                children[i]._next = children[i + 1];
            }
        }


        public override string ToString()
        {
            return string.Format("Topic: {0}", Title);
        }

        public TopicNode FindNext()
        {
            if (_firstChild != null) return _firstChild;

            return findNextTopicNotChild();
        }

        private TopicNode findNextTopicNotChild()
        {
            if (NextSibling != null) return NextSibling;

            if (Parent == null) return null;

            return Parent.findNextTopicNotChild();
        }

        public TopicNode FindPrevious()
        {
            if (PreviousSibling != null) return PreviousSibling;

            return Parent;
        }

        public TopicNode FindIndex()
        {
            if (Parent == null) return null;

            if (Parent != null && Parent.Parent == null) return Parent;

            return Parent.FindIndex();
        }

        public IEnumerable<TopicNode> Descendents()
        {
            foreach (TopicNode childNode in ChildNodes)
            {
                yield return childNode;

                foreach (TopicNode descendent in childNode.Descendents())
                {
                    yield return descendent;
                }
            }
        }
    }
}