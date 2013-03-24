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
        public Topic Root { get { return _root; } }

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


        /*
         * Extends what?
         * Keywords?
         * Nugets?
         * 
         * 
         * 
         * 
         */

        public void OrganizeFiles(IEnumerable<ITopicFile> files)
        {
            var folders = new Cache<string, TopicFolder>(raw => new TopicFolder(raw, this));
            files.GroupBy(x => (x.Folder ?? string.Empty)).Each(group => {
//                Debug.WriteLine("Folder:  " + group.Key);
//                Debug.WriteLine("--------------------------------");
//                group.Each(x => Debug.WriteLine(x.FilePath));
//                Debug.WriteLine("");
//                Debug.WriteLine("");
//                Debug.WriteLine("");

                folders[group.Key].AddFiles(group);

                var parentUrl = group.Key.ParentUrl();
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
                _root = topLevelSubjects.Single();
            }
        }



        ProjectRoot ITopicNode.Project { get { return this; } }
    }
}