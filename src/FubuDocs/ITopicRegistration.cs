using FubuCore;

namespace FubuDocs
{
    [MarkedForTermination]
    public interface ITopicRegistration
    {
        void Modify(TopicGraph graph);
    }
}