using FubuCore.Descriptions;

namespace FubuDocsRunner.Exports
{
    public class ItemDownloaded : DescribesItself
    {
        private readonly DownloadToken _token;
        private readonly string _fullPath;

        public ItemDownloaded(DownloadToken token, string fullPath)
        {
            _token = token;
            _fullPath = fullPath;
        }

        public void Describe(Description description)
        {
            description.Title = _token.Url;
            description.ShortDescription = "Downloaded to " + _fullPath;
        }

        protected bool Equals(ItemDownloaded other)
        {
            return _token.Equals(other._token) && string.Equals(_fullPath, other._fullPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ItemDownloaded)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_token.GetHashCode() * 397) ^ _fullPath.GetHashCode();
            }
        }
    }
}