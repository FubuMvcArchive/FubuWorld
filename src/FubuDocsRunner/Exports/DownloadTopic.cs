using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium;

namespace FubuDocsRunner.Exports
{
    public class DownloadTopic : IDownloadStep
    {
        private readonly DownloadToken _token;
        private readonly IPageSource _source;

        public DownloadTopic(DownloadToken token, IPageSource source)
        {
            _token = token;
            _source = source;
        }

        public void Execute(DownloadContext context)
        {
            var source = _source.SourceFor(_token);
            var path = _token.EnsureLocalPath(context.Plan.OutputDirectory);

            using (var writer = new StreamWriter(File.Open(path, FileMode.CreateNew)))
            {
                writer.Write(source);
            }

            context.Report.ItemDownloaded(new ItemDownloaded(_token, path));

            queueAssetDownloads(context, source);
        }

        private void queueAssetDownloads(DownloadContext context, string source)
        {
            var assets = AssetParser.AssetsFor(source);
            assets.Each(context.QueueAssetDowload);
        }
    }
}