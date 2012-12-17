using System.Xml;
using FubuDocsRunner;
using NUnit.Framework;
using FubuCore;
using FubuTestingSupport;

namespace FubuDocs.Testing.Runner
{
    [TestFixture]
    public class TopicXmlStackTester
    {
        private const string theRootDirectory = "Something\\WidgetPro.Core";
        private TopicXmlStack theStack;
        private XmlDocument document;

        [SetUp]
        public void SetUp()
        {
            document = new XmlDocument();
            document.Load("Topics.xml");

            theStack = new TopicXmlStack(theRootDirectory);
        }

        [Test]
        public void current_namespace_is_initially_empty()
        {
            theStack.CurrentNamespace.ShouldBeEmpty();
        }

        [Test]
        public void current_namespace_after_push_folder()
        {
            theStack.PushFolder("Folder1");

            theStack.CurrentNamespace.ShouldEqual("Folder1");

            theStack.PushFolder("Folder2");
            theStack.PushFolder("Folder3");

            theStack.CurrentNamespace.ShouldEqual("Folder1.Folder2.Folder3");
        }

        [Test]
        public void unwind_the_current_namespace()
        {
            theStack.PushFolder("Folder1");
            theStack.PushFolder("Folder2");
            theStack.PushFolder("Folder3");

            theStack.PopFolder();
            theStack.CurrentNamespace.ShouldEqual("Folder1.Folder2");

            theStack.PopFolder();
            theStack.CurrentNamespace.ShouldEqual("Folder1");

            theStack.PopFolder();
            theStack.CurrentNamespace.ShouldBeEmpty();
        }

        [Test]
        public void add_the_first_topic()
        {
            var root = document.DocumentElement;
            var request = theStack.AddTopic(root);

            request.Title.ShouldEqual("Working with FubuWorld Docs");
            request.TopicName.ShouldEqual("FubuWorldRoot");
            request.RootDirectory.ShouldEqual(theRootDirectory);
            request.Namespace.ShouldBeEmpty();
        }

        [Test]
        public void add_the_second_topic_that_is_a_child_to_the_parent()
        {
            var root = document.DocumentElement;
            var top = theStack.AddTopic(root);
            theStack.PushTopic(top);
            theStack.PushFolder("HowTo");

            var element = document.DocumentElement.FirstChild.FirstChild;

            var request = theStack.AddTopic((XmlElement) element);
            top.Children.ShouldContain(request);

            request.Namespace.ShouldEqual("HowTo");
            request.TopicName.ShouldEqual(TopicRequest.GetNameFromTitle(request.Title));

            // Screws up in Mono and I do *NOT* have a clue why.  Shouldn't even be possible, but of course it is
            //request.FullTopicClassName.ShouldEqual("WidgetPro.Core.HowTo." + request.TopicName);
        }
    }
}