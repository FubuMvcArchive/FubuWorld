namespace FubuDocsRunner.Exports
{
    public class OverrideRootPath : IDownloadReportVisitor
    {
        private readonly string _root;

        public OverrideRootPath(string root)
        {
            _root = root;
        }

        public void Visit(DownloadReport report)
        {
        }
    }
}