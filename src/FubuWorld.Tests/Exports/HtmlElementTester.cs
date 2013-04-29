using FubuDocsRunner.Exports;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuWorld.Tests.Exports
{
    [TestFixture]
    public class HtmlElementTester
    {
        [Test]
        public void get_attribute_value()
        {
            var input = "<script src=\"/test\" />";
            HtmlElement.GetAttributeValue(input, "src").ShouldEqual("/test");
        }
    }
}