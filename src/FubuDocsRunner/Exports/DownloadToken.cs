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

        public string Url { get; private set; }
        public string[] Parts { get; private set; }
        public string LocalPath { get; private set; }

        public string EnsureLocalPath(string directory)
        {
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
            return string.Equals(Url, other.Url) && string.Equals(LocalPath, other.LocalPath);
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

        public static DownloadToken For(string baseUrl, string relativePath)
        {
            var url = baseUrl.TrimEnd('/') + relativePath;

            var lastIndex = relativePath.LastIndexOf('.');
            if (lastIndex == -1)
            {
                relativePath += "/index.html";
            }

            var parts = relativePath.TrimStart('/').Split(new [] { "/" }, StringSplitOptions.None);

            var path = "";
            parts.Each(part => path = path.AppendPath(part));

            return new DownloadToken(url, parts, path);
        }
    }
}