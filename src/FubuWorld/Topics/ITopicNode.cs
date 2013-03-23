namespace FubuWorld.Topics
{
    public interface ITopicNode
    {
        Topic RootTopic();
        string Url { get; }
        ProjectRoot Project { get; }
    }
}