using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;

namespace FubuWorld.Topics
{
    // Tested w/ integration tests only.
    public class TopicBehavior : BasicBehavior
    {
        // TODO -- add DescibesItself stuff to this

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
}