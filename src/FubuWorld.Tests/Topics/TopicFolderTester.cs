using FubuTestingSupport;
using FubuWorld.Topics;
using NUnit.Framework;

namespace FubuWorld.Tests.Topics
{
    [TestFixture]
    public class TopicFolderTester
    {
        private TopicFolder theFolder;

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
        public void has_the_order()
        {
            theFolder.Order.ShouldEqual(new OrderedString("1.1.2.abc"));
        }

        [Test]
        public void has_the_url()
        {
            theFolder.Url.ShouldEqual("fubumvc/foo/bar/abc");
        }
    }
}