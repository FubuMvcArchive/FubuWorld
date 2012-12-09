using System.Diagnostics;
using NUnit.Framework;
using FubuTestingSupport;
using System.Linq;
using System.Collections.Generic;

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

        [Test]
        public void got_all()
        {
            var graph = new TopicGraph();
            graph.For<CTopic>().Append<DTopic>();
            graph.For<ATopic>().Append<BTopic>().Append<CTopic>();

            var types = graph.All().Select(x => x.TopicType);

            types.ShouldHaveTheSameElementsAs(typeof(ATopic), typeof(BTopic), typeof(CTopic), typeof(DTopic));
        }

        [Test]
        public void find_by_name()
        {
            var graph = new TopicGraph();
            graph.For<CTopic>().Append<DTopic>();
            graph.For<ATopic>().Append<BTopic>().Append<CTopic>();


            graph.FindByName("atopic").TopicType.ShouldEqual(typeof (ATopic));
            graph.FindByName("ctopic").TopicType.ShouldEqual(typeof(CTopic));
            graph.FindByName("dtopic").TopicType.ShouldEqual(typeof(DTopic));
        }
    }
}