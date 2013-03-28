using System;
using FubuCore;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.View;

namespace FubuWorld.Topics
{
    // Only using integration tests on this
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