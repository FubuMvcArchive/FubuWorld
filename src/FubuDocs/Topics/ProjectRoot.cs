﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FubuCore;

namespace FubuDocs.Topics
{
    public class ProjectRoot : ITopicNode
    {
        public static readonly string File = "project.xml";
        private readonly IList<ProjectRoot> _plugins = new List<ProjectRoot>();

        public string Description { get; set; }
        public string Name { get; set; }
        public string GitHubPage { get; set; }
        public string UserGroupUrl { get; set; }
        public string BuildServerUrl { get; set; }
        public string BottleName { get; set; }

        public string PluginTo { get; set; }

        [XmlIgnore]
        public Topic Index { get; set; }

        [XmlIgnore]
        public Topic Splash { get; set; }

        [XmlIgnore]
        public string Filename { get; set; }

        public string Url { get; set; }

        ProjectRoot ITopicNode.Project
        {
            get { return this; }
        }

        public ProjectRoot Parent { get; set; }

        public IList<ProjectRoot> Plugins
        {
            get { return _plugins; }
        }

        public Topic Home
        {
            get { return Splash ?? Index; }
        }

        public static ProjectRoot LoadFrom(string file)
        {
            var project = new FileSystem().LoadFromFile<ProjectRoot>(file);
            if (project.Url.IsEmpty())
            {
                project.Url = project.Name.ToLower();
            }

            project.Filename = file;

            return project;
        }

        public void WriteTo(string file)
        {
            new FileSystem().WriteObjectToFile(file, this);
        }


        public Topic FindByKey(string key)
        {
            if (Index.Key.EqualsIgnoreCase(key))
            {
                return Index;
            }

            if (key.StartsWith(Name.ToLower()))
            {
                return Index.Descendents().FirstOrDefault(x => x.Key == key);
            }
            else
            {
                var searchKey = Name.ToLowerInvariant().AppendUrl(key);
                return Index.Descendents().FirstOrDefault(x => x.Key == searchKey);
            }
        }

        public IEnumerable<Topic> AllTopics()
        {
            yield return Index;

            foreach (Topic descendent in Index.Descendents())
            {
                yield return descendent;
            }
        }
    }
}