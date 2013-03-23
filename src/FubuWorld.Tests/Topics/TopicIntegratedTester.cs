using FubuTestingSupport;
using NUnit.Framework;

namespace FubuWorld.Tests.Topics
{
    [TestFixture]
    public class TopicIntegratedTester
    {
        [Test]
        public void determines_the_key()
        {
            ObjectMother.Nodes["colors/red"].ShouldNotBeNull();
            ObjectMother.Nodes["colors/blue"].ShouldNotBeNull();
            ObjectMother.Nodes["colors/purple"].ShouldNotBeNull();
            ObjectMother.Nodes["colors/green"].ShouldNotBeNull();
        }

        [Test]
        public void strips_the_order_out_of_folder_names()
        {
            ObjectMother.Nodes["deep/1"].Url.ShouldEqual("deep/1");
        }

        [Test]
        public void strips_the_order_out_of_deep_folder_names()
        {
            ObjectMother.Nodes["deep/b/subjectA/C"]
                .Url.ShouldEqual("deep/b/subjectA/C");
        }


        [Test]
        public void determine_the_url_for_an_index_name()
        {
            var colorsIndex = ObjectMother.Nodes["colors"];
            colorsIndex.File.Name.ShouldEqual("index");

            colorsIndex.Url.ShouldEqual("colors");
        }

        [Test]
        public void determine_the_url_for_a_file_not_the_index()
        {
            ObjectMother.Nodes["colors/red"].Url.ShouldEqual("colors/red");
        }

        [Test]
        public void determine_the_url_for_a_file_overriding_url_in_spark_file()
        {
            ObjectMother.Nodes["colors/green"].Url.ShouldEqual("colors/SeaGreen"); // look at the 1.1.2.green.spark file
        }


        [Test]
        public void get_the_title_if_it_is_not_written_into_the_file()
        {
            ObjectMother.Nodes["colors/green"].Title.ShouldEqual("The green page");
        }

        [Test]
        public void get_the_title_from_file_contents()
        {
            ObjectMother.Nodes["colors/blue"].Title.ShouldEqual("Blue");
        }

    }
}