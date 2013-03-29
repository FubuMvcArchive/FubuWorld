using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bottles;
using FubuCore.Util;
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
            var folders = new Cache<string, TopicFolder>(raw => new TopicFolder(raw, project));
            files.GroupBy(x => (x.Folder ?? string.Empty)).Each(@group => {
                var topicFolder = folders[@group.Key];
                var folderTopics = @group.Select(file => new Topic(topicFolder, file)).ToArray();

                topicFolder.AddFiles(folderTopics);
                folderTopics.Each(TopicBuilder.BuildOut);

                var parentUrl = @group.Key.ParentUrl();
                while (parentUrl.IsNotEmpty())
                {
                    folders.FillDefault(parentUrl);
                    parentUrl = parentUrl.ParentUrl();
                }
            });

            folders.Each(x => {
                if (x.Raw == string.Empty) return;

                var rawParent = x.Raw.ParentUrl();
                

                folders.WithValue(rawParent, parent => parent.Add(x));
            });

            var masterFolder = folders[string.Empty];
            var topLevelSubjects = masterFolder.TopLevelTopics();
            if (topLevelSubjects.Count() > 1)
            {
                throw new NotImplementedException("Don't know what to do here");
            }
            else
            {
                project.Root = topLevelSubjects.Single();
            }

            return project;
        }
    }
}