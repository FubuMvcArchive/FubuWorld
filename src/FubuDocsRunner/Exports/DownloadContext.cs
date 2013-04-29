namespace FubuDocsRunner.Exports
{
    public class DownloadContext
    {
        private readonly DownloadPlan _plan;
        private readonly DownloadReport _report;

        public DownloadContext(DownloadPlan plan)
        {
            _plan = plan;
            _report = new DownloadReport();
        }

        public DownloadPlan Plan
        {
            get { return _plan; }
        }

        public DownloadReport Report
        {
            get { return _report; }
        }

        public void QueueAssetDowload(string url)
        {
            var token = DownloadToken.For(_plan.BaseUrl, url);
            _plan.Add(new DownloadAsset(token));
        }

        public static DownloadContext For(string outputDirectory, string baseUrl)
        {
            return new DownloadContext(new DownloadPlan(outputDirectory, baseUrl));
        }
    }
}