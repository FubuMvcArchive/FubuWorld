using System;
using System.Collections.Generic;
using System.IO;
using FubuCore;

namespace FubuDocsRunner.Exports
{
    public class DownloadToken
    {
        private static readonly object Lock = new object();
        private readonly FileSystem _fileSystem;

        private DownloadToken(string url, string[] parts, string localPath)
        {
            Url = url;
            Parts = parts;
            LocalPath = localPath;

            _fileSystem = new FileSystem();
        }

        public string BaseUrl { get; private set; }
        public string Url { get; private set; }
        public string[] Parts { get; private set; }
        public string LocalPath { get; private set; }
        public bool TriggersDownloads { get; private set; }

        public string RelativeUrl
        {
            get
            {
                var url = Url.Replace(BaseUrl, "");
                return "/" + url.TrimStart('/');
            }
        }

        public string GetLocalPath(string directory)
        {
            lock (Lock)
            {
                if (!_fileSystem.DirectoryExists(directory))
                {
                    _fileSystem.CreateDirectory(directory);
                }
            }

            var attempts = new List<string>();
            Parts.Each(x =>
            {
                attempts.Add(x);
                lock (Lock)
                {
                    var file = directory;
                    attempts.Each(y => file = file.AppendPath(y));

                    if (!_fileSystem.FileExists(file) && !Path.HasExtension(file))
                    {
                        _fileSystem.CreateDirectory(file);
                    }
                }
            });

            return directory.AppendPath(LocalPath);
        }

        protected bool Equals(DownloadToken other)
        {
            return Url.EqualsIgnoreCase(other.Url);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DownloadToken)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Url.GetHashCode() * 397) ^ LocalPath.GetHashCode();
            }
        }

        public override string ToString()
        {
            return Url;
        }

        public static DownloadToken For(string baseUrl, string relativePath)
        {
            baseUrl = baseUrl.TrimEnd('/');
            relativePath = relativePath.Replace(baseUrl, "");

            var triggers = false;
            var url = baseUrl + relativePath;
            var lastIndex = relativePath.LastIndexOf('.');
            
            if (lastIndex == -1)
            {
                relativePath += "/index.html";
                triggers = true;
            }
            else if (url.EndsWith(".css"))
            {
                triggers = true;
            }

            var parts = relativePath.TrimStart('/').Split(new [] { "/" }, StringSplitOptions.None);

            var path = "";
            parts.Each(part => path = path.AppendPath(part));

            return new DownloadToken(url, parts, path)
            {
                BaseUrl = baseUrl,
                TriggersDownloads = triggers
            };
        }
    }
}