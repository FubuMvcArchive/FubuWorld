using System.IO;
using FubuCore.Util;
using FubuDocsRunner.Exports;

namespace FubuWorld.Tests.Exports
{
    public class StubStreamProvider : IStreamProvider
    {
        private readonly Cache<string, string> _cache = new Cache<string, string>();

        public void Add(string url, string contents)
        {
            _cache.Fill(url, contents);
        }

        public string ContentsFor(string url)
        {
            return _cache[url];
        }

        public Stream Open(string url)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(ContentsFor(url));
            writer.Flush();

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }
    }
}