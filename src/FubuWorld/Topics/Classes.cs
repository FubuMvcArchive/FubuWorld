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
            // a new Topic for the default.

            // group the files in a hierarchy and order. 
            // Build the Topic's

            // add the new ProjectRoot to 

            throw new NotImplementedException();
        }
    }

    public class OrderedString : IComparable<OrderedString>
    {
        private readonly int[] _order;
        private readonly string _value;
        private readonly int[] _parentRank;
        private string _raw;

        public OrderedString(string text)
        {
            _raw = text;
            var values = text.Split('.');
            _order = values.Reverse().Skip(1).Reverse().Select(int.Parse).ToArray();
            _value = values.Last();

            _parentRank = _order.Any() ? _order.Take(_order.Length - 1).ToArray() : new int[0];
        }

        public int[] Order
        {
            get { return _order; }
        }

        public string Value
        {
            get { return _value; }
        }

        public int[] ParentRank
        {
            get { return _parentRank; }
        }

        protected bool Equals(OrderedString other)
        {
            return _order.SequenceEqual(other._order) && string.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OrderedString) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_order != null ? _order.GetHashCode() : 0)*397) ^ (_value != null ? _value.GetHashCode() : 0);
            }
        }

        public bool OrderStartsWith(int[] order)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(OrderedString other)
        {
            if (Order.Any() && other.Order.Length == Order.Length)
            {
                for (int i = 0; i < Order.Length; i++)
                {
                    var compare = Order[i].CompareTo(other.Order[i]);
                    if (compare != 0) return compare;

                }
            }

            return _raw.CompareTo(other._raw);
        }
    }

    public interface ITopicFile
    {
        string FilePath { get; }
        string Name { get; }
        string Folder { get; }
        IViewToken ToViewToken();
    }


    // TODO -- add DescibesItself stuff to this
    public class TopicBehavior : BasicBehavior
    {
        private readonly Topic _node;
        private readonly IMediaWriter<Topic> _writer;

        public TopicBehavior(Topic node, IMediaWriter<Topic> writer) : base(PartialBehavior.Executes)
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
        private readonly Topic _node;
        private readonly ViewNode _view;

        public TopicBehaviorNode(Topic node, ViewNode view)
        {
            _node = node;
            _view = view;
        }

        public Topic Node
        {
            get { return _node; }
        }

        public override BehaviorCategory Category
        {
            get { return BehaviorCategory.Output; }
        }

        public Type InputType()
        {
            return typeof (Topic);
        }

        protected override ObjectDef buildObjectDef()
        {
            ObjectDef def = ObjectDef.ForType<TopicBehavior>();

            def.DependencyByValue(typeof (Topic), Node);
            ObjectDef writerDef = _view.As<IContainerModel>().ToObjectDef();
            def.Dependency(typeof (IMediaWriter<Topic>), writerDef);

            return def;
        }
    }
}