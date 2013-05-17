using System;
using System.IO;
using FubuDocsRunner.Topics;
using NUnit.Framework;
using FubuCore;
using FubuTestingSupport;

namespace FubuWorld.Tests.Commands.Topics
{
    [TestFixture]
    public class TopicFileSystemTester
    {
        private string _folder;
        private TopicFileSystem theTopicFiles;

        [SetUp]
        public void SetUp()
        {
            _folder = Guid.NewGuid().ToString();
            new FileSystem().CreateDirectory(_folder);

            theTopicFiles = new TopicFileSystem(_folder);
        }

        [Test]
        public void write_topic_file_that_is_a_leaf()
        {
            var token = new TopicToken
            {
                Key = "foo",
                Title = "it's my foo",
                Order = 3
            };

            var path = theTopicFiles.WriteFile(token);
            Path.GetFileName(path).ShouldEqual("3.foo.spark");

            Path.GetDirectoryName(path).ShouldEqual(_folder);

            var text = new FileSystem().ReadStringFromFile(path);
            text.ShouldContain("<!--Title: it's my foo-->");
            text.ShouldContain("<markdown>");
            text.ShouldContain("</markdown>");
            text.ShouldContain("TODO(Write content!)");
        }

        [Test]
        public void write_topic_file_that_is_a_new_folder()
        {
            var token = new TopicToken
            {
                Key = "foo",
                Title = "it's my foo",
                Order = 3,
                Type = TopicTokenType.Folder
            };

            var path = theTopicFiles.WriteFile(token);
            path.ShouldEqual(_folder.AppendPath("3.foo", "index.spark"));

            var text = new FileSystem().ReadStringFromFile(path);
            text.ShouldContain("<!--Title: it's my foo-->");
            text.ShouldContain("<markdown>");
            text.ShouldContain("</markdown>");
            text.ShouldContain("TODO(Write content!)");
        }
    }
}