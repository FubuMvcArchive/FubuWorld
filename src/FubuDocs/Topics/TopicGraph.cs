using System.Collections.Generic;
using System.Linq;
using FubuCore.Util;
using FubuMVC.Core.Registration;

namespace FubuDocs.Topics
{
    [ApplicationLevel]
    public class TopicGraph
    {
        public static readonly TopicGraph AllTopics = new TopicGraph();
        private readonly Cache<string, ProjectRoot> _projects = new Cache<string, ProjectRoot>(); 
        private readonly Cache<string, Topic> _topicCache = new Cache<string, Topic>(); 

        public TopicGraph()
        {
            _topicCache.OnMissing = key => {
                var projectName = key.Split('/').First();
                return _projects[projectName].FindByKey(key);
            };
        }

        public void AddProject(ProjectRoot project)
        {
            _projects[project.Name.ToLowerInvariant()] = project;
        }

        public IEnumerable<ProjectRoot> Projects
        {
            get { return _projects; }
        }

        public ProjectRoot ProjectFor(string name)
        {
            return _projects[name.ToLower()];
        }

        /// <summary>
        /// Returns the Topic for a specified key.  
        /// </summary>
        /// <returns></returns>
        public Topic Find(string key)
        {
            return _topicCache[key];
        }

    }
}