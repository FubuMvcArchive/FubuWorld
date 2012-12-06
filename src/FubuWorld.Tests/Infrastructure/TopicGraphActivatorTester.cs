using Bottles.Diagnostics;
using FubuDocs;
using FubuWorld.Infrastructure;
using NUnit.Framework;
using System.Linq;
using FubuTestingSupport;

namespace FubuWorld.Tests.Infrastructure
{
    [TestFixture]
    public class TopicGraphActivatorTester
    {
        [Test]
        public void running_The_activator_applies_all_topic_registries()
        {
            new TopicGraphActivator().Activate(null, new PackageLog());

            TopicGraph.AllTopics.Find<ATopic>().ChildNodes.Select(x => x.Title).ShouldHaveTheSameElementsAs("B", "C");
            TopicGraph.AllTopics.Find<DTopic>().ChildNodes.Select(x => x.Title).ShouldHaveTheSameElementsAs("E");
            TopicGraph.AllTopics.Find<CTopic>().ChildNodes.Select(x => x.Title).ShouldHaveTheSameElementsAs("D");
        }
    }

    public class Topics1 : TopicRegistry
    {
        public Topics1()
        {
            For<ATopic>().Append<BTopic>().Append<CTopic>();
            For<DTopic>().Append<ETopic>();
        }
    }

    public class Topics2 : TopicRegistry
    {
        public Topics2()
        {
            For<CTopic>().Append<DTopic>();
        }
    }

    public class ATopic : Topic
    {
        public ATopic() : base("A")
        {
        }
    }

    public class BTopic : Topic
    {
        public BTopic()
            : base("B")
        {
        }
    }

    public class CTopic : Topic
    {
        public CTopic()
            : base("C")
        {
        }
    }

    public class DTopic : Topic
    {
        public DTopic()
            : base("D")
        {
        }
    }

    public class ETopic : Topic
    {
        public ETopic()
            : base("E")
        {
        }
    }

    public class FTopic : Topic
    {
        public FTopic()
            : base("F")
        {
        }
    }
}