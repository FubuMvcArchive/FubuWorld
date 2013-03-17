using FubuDocs.Testing.Deep.Structure;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuDocs.Testing
{
    [TestFixture]
    public class TopicTester
    {
        [Test]
        public void lookup_the_url_for_topic_in_the_root_of_the_assembly()
        {
            Topic.UrlPatternFor(typeof (StoryAboutABoy))
                 .ShouldEqual("storyaboutaboy");
        }

        [Test]
        public void lookup_the_url_for_topic_type_deeper_into_the_assembly()
        {
            Topic.UrlPatternFor(typeof (StoryAboutAGirl))
                 .ShouldEqual("deep/structure/storyaboutagirl");
        }
    }

    public class StoryAboutABoy : Topic
    {
        public StoryAboutABoy() : base("This is the story of a boy")
        {
        }
    }




}

namespace FubuDocs.Testing.Deep.Structure
{
    public class StoryAboutAGirl : Topic
    {
        public StoryAboutAGirl()
            : base("This is the story of a girl")
        {
        }
    }
}