using System.Xml.Serialization;
using FubuCore;

namespace FubuWorld.Topics
{
    public class ProjectRoot
    {
        public string Name { get; set; }
        public string GitHubPage { get; set; }
        public string UserGroupUrl { get; set; }
        public string BuildServerUrl { get; set; }
        public string BottleName { get; set; }

        [XmlIgnore]
        public TopicNode RootNode { get; set; }

        public static ProjectRoot LoadFrom(string file)
        {
            return new FileSystem().LoadFromFile<ProjectRoot>(file);
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
    }
}