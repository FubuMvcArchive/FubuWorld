namespace FubuDocsRunner.Exports
{
    public class DownloadAsset : IDownloadStep
    {
        private readonly DownloadToken _token;

        public DownloadAsset(DownloadToken token)
        {
            _token = token;
        }

        public DownloadToken Token { get { return _token; } }

        public void Execute(DownloadContext context)
        {
            var path = _token.EnsureLocalPath(context.Plan.OutputDirectory);
            DownloadManager.Download(_token.Url, path);

            context.Report.ItemDownloaded(new ItemDownloaded(_token, path));
        }
    }
}