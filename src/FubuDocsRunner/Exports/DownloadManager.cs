using System.IO;
using System.Net;

namespace FubuDocsRunner.Exports
{
    public interface IStreamProvider
    {
        Stream Open(string url);
    }

    public class StreamProvider : IStreamProvider
    {
        public Stream Open(string url)
        {
            using (var client = new WebClient())
            {
                return client.OpenRead(url);
            }
        }
    }

    public class DownloadManager
    {
        private static IStreamProvider _provider;

        static DownloadManager()
        {
            Live();
        }

        public static void Live()
        {
            Stub(new StreamProvider());
        }

        public static void Stub(IStreamProvider provider)
        {
            _provider = provider;
        }

        public static void Download(string url, string path)
        {
            using (var stream = _provider.Open(url))
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var writer = new StreamWriter(File.Open(path, FileMode.CreateNew)))
                    {
                        writer.Write(reader.ReadToEnd());
                    }
                }
            }
        }
    }
}