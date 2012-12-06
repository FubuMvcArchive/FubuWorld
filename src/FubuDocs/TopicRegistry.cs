namespace FubuDocs
{
    public class TopicRegistry
    {
        public TopicGraph.TopicExpression For<T>() where T : Topic, new()
        {
            return TopicGraph.AllTopics.For<T>();
        }
    }
}