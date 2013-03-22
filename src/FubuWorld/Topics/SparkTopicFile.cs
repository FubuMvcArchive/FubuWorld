using System.IO;
using FubuMVC.Core.View;
using FubuMVC.Core.View.Model;
using FubuMVC.Spark;
using FubuMVC.Spark.SparkModel;
using System.Linq;
using System.Collections.Generic;

namespace FubuWorld.Topics
{
    public class SparkTopicFile : ITopicFile
    {
        private readonly ViewDescriptor<Template> _viewDescriptor;

        public SparkTopicFile(ViewDescriptor<Template> viewDescriptor)
        {
            _viewDescriptor = viewDescriptor;
        }

        public string FilePath { get { return _viewDescriptor.Template.FilePath; } }
        public string Name { get { return _viewDescriptor.Name(); } }
        public string RelativePath()
        {
            string relativeFile = _viewDescriptor.RelativePath().Replace('\\', '/');
            return relativeFile.Split('.').Reverse().Skip(1).Reverse().Join(".");
        }

        public IViewToken ToViewToken()
        {
            return new SparkViewToken(new SparkDescriptor(_viewDescriptor.Template));
        }
    }
}