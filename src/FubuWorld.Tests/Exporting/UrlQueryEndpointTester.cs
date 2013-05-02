using System.Diagnostics;
using FubuDocs.Exporting;
using FubuMVC.Core;
using FubuMVC.Core.Packaging;
using FubuMVC.Core.Registration;
using NUnit.Framework;
using FubuMVC.StructureMap;
using StructureMap;
using System.Collections.Generic;
using FubuCore;
using System.Linq;
using FubuTestingSupport;

namespace FubuWorld.Tests.Exporting
{
    [TestFixture]
    public class UrlQueryEndpointTester
    {
        private BehaviorGraph graph;
        private string[] urls;

        [TestFixtureSetUp]
        public void SetUp()
        {
            FubuMvcPackageFacility.PhysicalRootPath = ".".ToFullPath().ParentDirectory().ParentDirectory();

            graph = FubuApplication.DefaultPolicies()
                                   .StructureMap(new Container())
                                   .Bootstrap().Factory.Get<BehaviorGraph>();

            urls = new UrlQueryEndpoint(graph).get_urls().Urls;

            urls.Each(x => Debug.WriteLine(x));
        }

        [Test]
        public void does_not_include_any_diagnostics_links()
        {
            urls.Any(x => x.StartsWith("_fubu")).ShouldBeFalse();
            urls.Any(x => x.StartsWith("_diagnostics")).ShouldBeFalse();
        }

        [Test]
        public void about_page_is_not_exported()
        {
            urls.ShouldNotContain("_about");
        }

        [Test]
        public void asset_is_not_exported()
        {
            urls.Any(x => x.StartsWith("_content")).ShouldBeFalse();
        }

        [Test]
        public void code_file_is_not_exported()
        {
            urls.Any(x => x.StartsWith("code/")).ShouldBeFalse();
        }

        [Test]
        public void all_topics_page_are_exported()
        {
            urls.ShouldContain("fubudocs");
            urls.ShouldContain("fubumvc");
            urls.ShouldContain("fubumvc/deep/b");
            urls.ShouldContain("fubumvc/nested/b/imported1");
            urls.ShouldContain("fubudocs/topics/navigation");
        }

        [Test]
        public void the_all_topics_endpoint_is_exported()
        {
            urls.ShouldContain("topics");
        }
    }
}