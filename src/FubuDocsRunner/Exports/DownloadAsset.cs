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

        protected bool Equals(DownloadAsset other)
        {
            return _token.Equals(other._token);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DownloadAsset) obj);
        }

        public override int GetHashCode()
        {
            return _token.GetHashCode();
        }
    }
}