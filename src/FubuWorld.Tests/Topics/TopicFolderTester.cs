using System.Collections.Generic;
using FubuTestingSupport;
using FubuWorld.Topics;
using NUnit.Framework;
using System.Linq;

namespace FubuWorld.Tests.Topics
{
    [TestFixture]
    public class TopicFolderTester
    {
        private TopicFolder theFolder;

        private IEnumerable<ITopicFile> addFiles(params string[] names)
        {
            var files = names.Select(x => new StubTopicFile {Name = x}).ToArray();
            theFolder.OrganizeFiles(files);

            return files;
        }

        [SetUp]
        public void SetUp()
        {
            theFolder = new TopicFolder("1.foo/2.3.1.bar/1.1.2.abc", new ProjectRoot { Url = "fubumvc" });
        }

        [Test]
        public void captures_the_raw_name()
        {
            theFolder.RawName.ShouldEqual("1.foo/2.3.1.bar/1.1.2.abc");
        }

        [Test]
        public void uses_only_the_non_ordered_name_of_the_last_item()
        {
            theFolder.Name.ShouldEqual("abc");
        }

        [Test]
        public void has_the_url()
        {
            theFolder.Url.ShouldEqual("fubumvc/foo/bar/abc");
        }

        [Test]
        public void organizing_by_index_only()
        {
            var file = addFiles("index.spark").Single();

            theFolder.RootTopic().File.ShouldBeTheSameAs(file);
        }

        [Test]
        public void named_files_only()
        {
            addFiles("b", "c", "a");

            theFolder.RootTopic().File.Name.ShouldEqual("a");
            theFolder.RootTopic().NextSibling.File.Name.ShouldEqual("b");
            theFolder.RootTopic().NextSibling.NextSibling.File.Name.ShouldEqual("c");
        }

        [Test]
        public void index_and_named_files()
        {
            addFiles("b", "c", "a", "index");

            var root = theFolder.RootTopic();
            root.File.Name.ShouldEqual("index");
            root.FirstChild.File.Name.ShouldEqual("a");
            root.FirstChild.NextSibling.File.Name.ShouldEqual("b");
            root.FirstChild.NextSibling.NextSibling.File.Name.ShouldEqual("c");
        }

        [Test]
        public void respect_the_numbering()
        {
            addFiles("1.c", "2.a", "3.b");

            theFolder.RootTopic().File.Name.ShouldEqual("1.c");
            theFolder.RootTopic().NextSibling.File.Name.ShouldEqual("2.a");
            theFolder.RootTopic().NextSibling.NextSibling.File.Name.ShouldEqual("3.b");
        }

        [Test]
        public void respect_the_numbering_with_index_too()
        {
            addFiles("1.b", "3.c", "2.a", "index");

            var root = theFolder.RootTopic();
            root.File.Name.ShouldEqual("index");
            root.FirstChild.File.Name.ShouldEqual("1.b");
            root.FirstChild.NextSibling.File.Name.ShouldEqual("2.a");
            root.FirstChild.NextSibling.NextSibling.File.Name.ShouldEqual("3.c");
        }
    }
}