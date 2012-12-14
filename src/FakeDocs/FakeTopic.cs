using FubuDocs;

namespace FakeDocs
{
    public class FakeTopic : Topic
    {
        public FakeTopic() : base("I'm a fake topic")
        {
        }
    }

    public class SecondTopic : Topic
    {
        public SecondTopic()
            : base("The second topic")
        {
        }
    }

    public class FakeTopicRegistry : TopicRegistry
    {
        public FakeTopicRegistry()
        {
            For<FakeTopic>().Append<SecondTopic>();
        }
    }
}