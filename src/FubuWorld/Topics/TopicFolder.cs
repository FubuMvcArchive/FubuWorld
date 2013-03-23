using System;
using System.Collections.Generic;
using System.Linq;

namespace FubuWorld.Topics
{
    public class OrderedTopic : IComparable<OrderedTopic>
    {
        public OrderedTopic(string text)
        {
            Raw = text;
            var values = text.Split('.');
            Name = values.Last();
        }

        public string Raw { get; private set; }
        public string Name { get; private set; }
        public int CompareTo(OrderedTopic other)
        {
            return Raw.CompareTo(other.Raw);
        }
    }

    public class TopicFolder : OrderedTopic, ITopicNode
    {
        private readonly string _rawName;
        private readonly ProjectRoot _project;
        private readonly string _url;

        public TopicFolder(string rawName, ProjectRoot project) : base(rawName.Split('/').Last())
        {
            _rawName = rawName;
            _project = project;

            _url = rawName.Split('/').Select(x => new OrderedTopic(x).Name).Join("/");
            _url = project.Url.AppendUrl(_url);
        }


        public Topic RootTopic()
        {
            throw new System.NotImplementedException();
        }

        public string Url
        {
            get { return _url; }
        }

        public ProjectRoot Project
        {
            get { return _project; }
        }

        public string RawName
        {
            get { return _rawName; }
        }

        public void OrganizeFiles(IEnumerable<ITopicFile> topicFiles)
        {
            var topics = topicFiles.Select(x => new Topic(this, x)).ToArray();
            // if one is "index", that's the root.  If not, order it.
        }
    }
}