using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore.Util;
using FubuMVC.Core.Registration;
using FubuCore;
using FubuMVC.Core.View;
using FubuMVC.Core.View.Model;
using FubuMVC.Spark.SparkModel;

namespace FubuDocs.Topics
{
    // Only testing w/ integration tests.
    public class TopicLoader
    {
        private readonly ISparkTemplateRegistry _sparkTemplates;

        public TopicLoader(ISparkTemplateRegistry sparkTemplates)
        {
            _sparkTemplates = sparkTemplates;
        }

        public TopicLoader(BehaviorGraph graph)
        {
            // Just need to force the views to be executed and found
            var views = graph.Settings.Get<ViewEngines>().Views;

            // Not super wild about this
            _sparkTemplates = (ISparkTemplateRegistry) graph.Services.DefaultServiceFor<ISparkTemplateRegistry>().Value;
        }

        public static bool IsTopic(ITemplate descriptor)
        {
            string path = descriptor.ViewPath.Replace("\\", "/");

            if (path.Contains("/Samples/") || path.Contains("/Examples/")) return false;
            if (path.Contains("/samples/") || path.Contains("/examples/")) return false;

            if (descriptor.RelativePath().StartsWith("snippets")) return false;

            return true;
        }

        public IEnumerable<ITopicFile> FindFilesFromBottle(string bottleName)
        {
            return _sparkTemplates.Where(x => x.Origin == bottleName)
                                  .OfType<Template>()
                                  .Where(IsTopic)
                                  .Select(x => new SparkTopicFile(new ViewDescriptor<Template>(x)));
        }

        public ProjectRoot LoadProject(string bottleName, string folder)
        {
            ProjectRoot project = ProjectRoot.LoadFrom(folder.AppendPath(ProjectRoot.File));
            IEnumerable<ITopicFile> files = FindFilesFromBottle(bottleName);
            var folders = new Cache<string, TopicFolder>(raw => new TopicFolder(raw, project));
            files.GroupBy(x => (x.Folder ?? string.Empty)).Each(@group => {
                TopicFolder topicFolder = folders[@group.Key];
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

                string rawParent = x.Raw.ParentUrl();


                folders.WithValue(rawParent, parent => parent.Add(x));
            });

            TopicFolder masterFolder = folders[string.Empty];
            IEnumerable<Topic> topLevelSubjects = masterFolder.TopLevelTopics();
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