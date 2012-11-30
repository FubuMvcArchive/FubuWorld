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
            graph.For<ATopic>().ShouldNotBeNull();
        }

        [Test]
        public void does_return_the_same_topic_per_type()
        {
            var graph = new TopicGraph();
            graph.For<ATopic>().ShouldBeTheSameAs(graph.For<ATopic>());
        }

        [Test]
        public void can_find_topic_in_children_just_fine()
        {
            var graph = new TopicGraph();
            graph.For<ATopic>().Append<BTopic>().Append<CTopic>();

            graph.For<BTopic>().Parent.ShouldBeTheSameAs(graph.For<ATopic>());
            graph.For<CTopic>().Parent.ShouldBeTheSameAs(graph.For<ATopic>());
        
            graph.TopLevelNodes().ShouldHaveTheSameElementsAs(TopicNode.For<ATopic>());
        }
    }
}