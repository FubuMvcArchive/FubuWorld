using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using FubuCore;
using FubuCore.Util;
using System.Linq;

namespace FubuWorld.Topics
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
        public string Url { get; set; }

        [XmlIgnore]
        public Topic Root { get; set; }

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


        ProjectRoot ITopicNode.Project { get { return this; } }

        public Topic FindByKey(string key)
        {
            if (key.EqualsIgnoreCase(key))
            {
                return Root;
            }

            return Root.Descendents().FirstOrDefault(x => x.Key == key);
        }
    }
}