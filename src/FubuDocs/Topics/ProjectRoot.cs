using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FubuCore;

namespace FubuDocs.Topics
{
    public class ProjectRoot : ITopicNode
    {
        public static readonly string File = "project.xml";
        private Topic _root;

        public string Name { get; set; }
        public string GitHubPage { get; set; }
        public string UserGroupUrl { get; set; }
        public string BuildServerUrl { get; set; }
        public string BottleName { get; set; }

        [XmlIgnore]
        public Topic Root { get; set; }

        public string Url { get; set; }

        ProjectRoot ITopicNode.Project
        {
            get { return this; }
        }

        public static ProjectRoot LoadFrom(string file)
        {
            var project = new FileSystem().LoadFromFile<ProjectRoot>(file);
            if (project.Url.IsEmpty())
            {
                project.Url = project.Name.ToLower();
            }

            return project;
        }

        public void WriteTo(string file)
        {
            new FileSystem().WriteObjectToFile(file, this);
        }


        public Topic FindByKey(string key)
        {
            if (Root.Key.EqualsIgnoreCase(key))
            {
                return Root;
            }

            return Root.Descendents().FirstOrDefault(x => x.Key == key);
        }

        public IEnumerable<Topic> AllTopics()
        {
            yield return Root;

            foreach (Topic descendent in Root.Descendents())
            {
                yield return descendent;
            }
        }
    }
}