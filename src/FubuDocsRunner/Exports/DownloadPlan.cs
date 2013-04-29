using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace FubuDocsRunner.Exports
{
    // contains steps
    public class DownloadPlan
    {
        private readonly ConcurrentQueue<IDownloadStep> _steps = new ConcurrentQueue<IDownloadStep>();
        private readonly string _outputDirectory;
        private readonly string _baseUrl;

        public DownloadPlan(string outputDirectory, string baseUrl)
        {
            _outputDirectory = outputDirectory;
            _baseUrl = baseUrl;
        }

        public string OutputDirectory
        {
            get { return _outputDirectory; }
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
        }

        public IEnumerable<IDownloadStep> Steps { get { return _steps; } }

        public void Add(IDownloadStep step)
        {
            _steps.Enqueue(step);
        }

        public DownloadReport Execute()
        {
            var context = new DownloadContext(this);

            while (true)
            {
                IDownloadStep step;
                if (_steps.TryDequeue(out step))
                {
                    step.Execute(context);
                }
                else
                {
                    break;
                }
            }

            return context.Report;
        }
    }
}