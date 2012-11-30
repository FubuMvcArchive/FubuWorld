using NUnit.Framework;
using FubuTestingSupport;

namespace FubuDocs.Testing
{
    [TestFixture]
    public class TopicGraphTester
    {
        [Test]
        public void can_find_topic_on_the_fly()
        {
            var graph = new TopicGraph();
            graph.Find<ATopic>().ShouldNotBeNull();
        }

        [Test]
        public void does_return_the_same_topic_per_type()
        {
            var graph = new TopicGraph();
            graph.Find<ATopic>().ShouldBeTheSameAs(graph.Find<ATopic>());
        }

        [Test]
        public void can_find_topic_in_children_just_fine()
        {
            var graph = new TopicGraph();

            graph.For<ATopic>().Append<BTopic>().Append<CTopic>();

            graph.Find<BTopic>().Parent.ShouldBeTheSameAs(graph.Find<ATopic>());
            graph.Find<CTopic>().Parent.ShouldBeTheSameAs(graph.Find<ATopic>());
        
            graph.TopLevelNodes().ShouldHaveTheSameElementsAs(TopicNode.For<ATopic>());
        }

        [Test]
        public void deep_topic_graph()
        {
            var graph = new TopicGraph();
            graph.For<CTopic>().Append<DTopic>();
            graph.For<ATopic>().Append<BTopic>().Append<CTopic>();

            graph.Find<ATopic>().ChildNodes.ShouldHaveTheSameElementsAs(TopicNode.For<BTopic>(), TopicNode.For<CTopic>());
            graph.Find<CTopic>().ChildNodes.ShouldHaveTheSameElementsAs(TopicNode.For<DTopic>());

        }
    }
}