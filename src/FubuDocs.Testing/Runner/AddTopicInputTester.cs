using FubuDocsRunner;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuDocs.Testing.Runner
{
    [TestFixture]
    public class AddTopicInputTester
    {
        [Test]
        public void determine_the_name_from_the_title()
        {
            new AddTopicInput {Title = "Something"}.ToRequest().TopicName.ShouldEqual("Something");
            new AddTopicInput { Title = "Something else altogether" }.ToRequest().TopicName.ShouldEqual("SomethingElseAltogether");
            new AddTopicInput { Title = "Something else, altogether" }.ToRequest().TopicName.ShouldEqual("SomethingElseAltogether");
            new AddTopicInput { Title = "Something else - altogether" }.ToRequest().TopicName.ShouldEqual("SomethingElseAltogether");
        }

        [Test]
        public void use_the_explicit_name_if_it_is_given()
        {
            var input = new AddTopicInput()
            {
                NameFlag = "SomeName",
                Title = "Something else"
            };

            input.ToRequest().TopicName
                 .ShouldEqual(input.NameFlag);
        }
    }
}