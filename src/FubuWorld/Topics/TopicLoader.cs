using System.Collections.Generic;
using System.Linq;
using Bottles;
using FubuMVC.Core.View.Model;
using FubuMVC.Spark.SparkModel;
using FubuCore;

namespace FubuWorld.Topics
{
    // Only testing w/ integration tests.
    public class TopicLoader
    {
        private readonly ISparkTemplateRegistry _sparkTemplates;

        public TopicLoader(ISparkTemplateRegistry sparkTemplates)
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

        public ProjectRoot LoadProject(string bottleName)
        {
            var pak = PackageRegistry.Packages.FirstOrDefault(x => x.Name == bottleName);
            string folder = null;
            pak.ForFolder(BottleFiles.WebContentFolder, x => folder = x);

            var project = ProjectRoot.LoadFrom(folder.AppendPath(ProjectRoot.File));
            var files = FindFilesFromBottle(bottleName);
            project.OrganizeFiles(files);

            return project;
        }
    }
}