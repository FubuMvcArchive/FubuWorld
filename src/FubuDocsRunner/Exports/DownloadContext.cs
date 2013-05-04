using System.Collections.Generic;

namespace FubuDocsRunner.Exports
{
    public class DownloadContext
    {
        private readonly DownloadPlan _plan;
        private readonly DownloadReport _report;
        private readonly IPageSource _source;
        private readonly IList<DownloadToken> _tokens = new List<DownloadToken>();

        public DownloadContext(DownloadPlan plan, IPageSource source)
        {
            _plan = plan;
            _source = source;
            _report = new DownloadReport();

            plan.Steps.Each(x => _tokens.Fill(x.Token));
        }

        public DownloadPlan Plan
        {
            get { return _plan; }
        }

        public DownloadReport Report
        {
            get { return _report; }
        }

        public void QueueDownload(DownloadToken token)
        {
            if (_tokens.Contains(token)) return;

            _tokens.Fill(token);

            if (token.IsAsset)
            {
                _plan.Add(new DownloadAsset(token));
                return;
            }
            
            _plan.Add(new DownloadUrl(token, _source));
        }

        public static DownloadContext For(string outputDirectory, string baseUrl, IPageSource source)
        {
            return new DownloadContext(new DownloadPlan(outputDirectory, baseUrl, source), source);
        }
    }
}