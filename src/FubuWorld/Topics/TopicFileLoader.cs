using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.View.Model;
using FubuMVC.Spark.SparkModel;

namespace FubuWorld.Topics
{
    // Only testing w/ integration tests.
    public class TopicFileLoader
    {
        private readonly ISparkTemplateRegistry _sparkTemplates;

        public TopicFileLoader(ISparkTemplateRegistry sparkTemplates)
        {
            _sparkTemplates = sparkTemplates;
        }

        public static bool IsTopic(ITemplate descriptor)
        {
            var path = descriptor.ViewPath.Replace("\\", "/");

            if (path.Contains("/Samples/") || path.Contains("/Examples/")) return false;
            if (path.Contains("/samples/") || path.Contains("/examples/")) return false;

            return true;
        }

        public IEnumerable<ITopicFile> FindFilesFromBottle(string bottleName)
        {
            return _sparkTemplates.Where(x => x.Origin == bottleName)
                                  .OfType<Template>()
                                  .Where(IsTopic)
                                  .Select(x => new SparkTopicFile(new ViewDescriptor<Template>(x)));
        }
    }
}