using System.IO;
using FubuCore;
using FubuDocsRunner.Exports;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuWorld.Tests.Exports
{
    [TestFixture]
    public class DownloadTokenTester
    {
        [Test]
        public void asset_target()
        {
            var token = DownloadToken.For("http://localhost:5500", "/_content/styles/default.css");

            token.Url.ShouldEqual("http://localhost:5500/_content/styles/default.css");
            token.Parts.ShouldHaveTheSameElementsAs("_content", "styles", "default.css");
            token.LocalPath.ShouldEqual("_content{0}styles{0}default.css".ToFormat(Path.DirectorySeparatorChar));
        }

        [Test]
        public void url_target()
        {
            var token = DownloadToken.For("http://localhost:5500", "/hello/world");

            token.Url.ShouldEqual("http://localhost:5500/hello/world");
            token.Parts.ShouldHaveTheSameElementsAs("hello", "world", "index.html");
            token.LocalPath.ShouldEqual("hello{0}world{0}index.html".ToFormat(Path.DirectorySeparatorChar));
        }
    }
}