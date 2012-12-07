using System.Xml;
using FubuDocsRunner;
using FubuTestingSupport;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace FubuDocs.Testing.Runner
{
    [TestFixture]
    public class TopicParserTester
    {
        private const string theRootDirectory = "Something\\WidgetPro.Core";
        private XmlDocument document;
        private TopicRequest theTop;

        [SetUp]
        public void SetUp()
        {
            document = new XmlDocument();
            document.Load("Topics.xml");

            var parser = new TopicParser();
            theTop = parser.Parse(theRootDirectory, document);
        }

        [Test]
        public void has_the_right_properties()
        {
            theTop.Title.ShouldEqual("Working with FubuWorld Docs");
            theTop.TopicName.ShouldEqual("FubuWorldRoot");
            theTop.RootDirectory.ShouldEqual(theRootDirectory);
            theTop.Namespace.ShouldBeEmpty();
        }

        [Test]
        public void has_The_right_children()
        {
            theTop.Children.Select(x => x.Title)
                .ShouldHaveTheSameElementsAs(
                "Starting a new FubuWorld Documentation Project",
                "The 'Topic' Navigation Structure",
                "Working with FubuDocsRunner",
                "Documentation View Helpers"
                );
        }

        [Test]
        public void second_level_children()
        {
            theTop.Children[1].Children.Select(x => x.Title)
                .ShouldHaveTheSameElementsAs(
                "Topic Navigation", "Adding a New Topic", "Spark Layout for a Topic"
                
                
                
                );
        }

        [Test]
        public void tracking_namespace_deeper()
        {
            theTop.Children.Select(x => x.Namespace)
                .ShouldHaveTheSameElementsAs("HowTo", "HowTo.Topics", "HowTo.FubuDocsRunner", "HowTo.ViewHelpers");
        }
    }
}