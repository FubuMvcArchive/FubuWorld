using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.View.Rendering;

namespace FubuWorld.Topics
{
    // Tested w/ integration tests only.
    public class TopicBehavior : BasicBehavior
    {
        // TODO -- add DescibesItself stuff to this

        private readonly Topic _node;
        private readonly IViewFactory _factory;
        private readonly IOutputWriter _writer;

        public TopicBehavior(Topic node, IViewFactory factory, IOutputWriter writer) : base(PartialBehavior.Executes)
        {
            _node = node;
            _factory = factory;
            _writer = writer;
        }

        public Topic Node
        {
            get { return _node; }
        }

        protected override DoNext performInvoke()
        {
            var view = _factory.GetView();
            view.Render();

            _writer.ContentType(MimeType.Html);

            return DoNext.Continue;
        }
    }
}