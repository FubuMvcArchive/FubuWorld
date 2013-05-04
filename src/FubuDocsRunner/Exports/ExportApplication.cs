using System.Collections.Generic;
using System.Linq;
using FubuDocs.Exporting;
using FubuMVC.Katana;
using OpenQA.Selenium;

namespace FubuDocsRunner.Exports
{
    public static class ExportApplication
    {
        public static DownloadReport ExportTo(this EmbeddedFubuMvcServer server, string outputDir)
        {
            var runtime = RuntimeFor(outputDir, server);
            var report = runtime.Plan.Execute();

            runtime.Driver.Dispose();

            return report;
        }

        public static DownloadRuntime RuntimeFor(string outputDir, EmbeddedFubuMvcServer server)
        {
            var baseUrl = server.BaseAddress;
            var driver = new OpenQA.Selenium.Firefox.FirefoxDriver();
            var source = new PageSource(driver);

            var plan = new DownloadPlan(outputDir, baseUrl, source);

            var model = server.Endpoints.Get<UrlQueryEndpoint>(x => x.get_urls()).ReadAsJson<UrlList>();
            model.Urls.Distinct().Each(url =>
            {
                var token = DownloadToken.For(baseUrl, url);
                plan.Add(new DownloadUrl(token, source));
            });

            return new DownloadRuntime { Plan = plan, Driver = driver };
        }

        public class DownloadRuntime
        {
            public DownloadPlan Plan { get; set; }
            public IWebDriver Driver { get; set; }
        }
    }
}