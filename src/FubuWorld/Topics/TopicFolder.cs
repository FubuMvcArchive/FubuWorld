using System;
using System.Collections.Generic;
using System.Linq;

namespace FubuWorld.Topics
{
    public class TopicFolder : ITopicNode
    {
        private readonly string _rawName;
        private readonly ProjectRoot _project;
        private readonly OrderedString _order;
        private string _url;

        public TopicFolder(string rawName, ProjectRoot project)
        {
            _rawName = rawName;
            _project = project;
            var last = rawName.Split('/').Last();

            _order = new OrderedString(last);

            _url = rawName.Split('/').Select(x => new OrderedString(x).Value).Join("/");
            _url = project.Url.AppendUrl(_url);
        }

        public string Name
        {
            get { return _order.Value; }
        }

        public OrderedString Order
        {
            get { return _order; }
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