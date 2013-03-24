using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore;

namespace FubuWorld.Topics
{
    public abstract class OrderedTopic : IComparable<OrderedTopic>
    {
        public OrderedTopic(string text)
        {
            Raw = text;
            Name = FindValue(text);
        }

        public string Raw { get; private set; }
        public string Name { get; private set; }

        public int CompareTo(OrderedTopic other)
        {
            return Raw.CompareTo(other.Raw);
        }

        public static string FindValue(string text)
        {
            string[] values = text.Split('.');
            return values.Last();
        }

        public abstract IEnumerable<Topic> TopLevelTopics();
    }

    public class TopicFolder : OrderedTopic, ITopicNode
    {
        private readonly IList<OrderedTopic> _children = new List<OrderedTopic>();
        private readonly ProjectRoot _project;
        private readonly string _rawName;
        private readonly string _url;
        private Topic _root;

        public TopicFolder(string rawName, ProjectRoot project) : base(rawName.Split('/').Last())
        {
            _rawName = rawName;
            _project = project;

            _url = rawName.Split('/').Select(FindValue).Join("/");
            _url = project.Url.AppendUrl(_url);
        }

        public string RawName
        {
            get { return _rawName; }
        }

        public string Url
        {
            get { return _url; }
        }

        public ProjectRoot Project
        {
            get { return _project; }
        }

        public void AddFiles(IEnumerable<ITopicFile> topicFiles)
        {
            var topics = topicFiles.Select(x => new Topic(this, x)).ToArray();
            _root = topics.FirstOrDefault(x => x.Name.EqualsIgnoreCase(Topic.Index));

            var others = topics.Where(x => !x.Name.EqualsIgnoreCase(Topic.Index));

            _children.AddRange(others);
        }

        public override IEnumerable<Topic> TopLevelTopics()
        {
            var orderedTopics = _children.SelectMany(x => x.TopLevelTopics()).ToList();
            orderedTopics.Sort();
            
            if (_root == null)
            {
                return orderedTopics;
            }

            orderedTopics.Each(x => _root.AppendChild(x));

            return new[] {_root};
        }
    }
}