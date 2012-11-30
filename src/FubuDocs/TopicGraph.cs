using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore.Util;

namespace FubuDocs
{
    public class TopicGraph
    {
        private readonly Cache<Type, TopicNode> _topics = new Cache<Type, TopicNode>();

        public TopicGraph()
        {
            _topics.OnMissing = type => {
                var node = findInChildren(type);
                return node ?? new TopicNode(type);
            };
        }

        private TopicNode findInChildren(Type topicType)
        {
            return _topics.GetAll().SelectMany(x => x.ChildNodes).FirstOrDefault(x => x.TopicType == topicType);
        }

        /// <summary>
        /// Returns the TopicNode for a specified topic.  If the topic does not already exist,
        /// it will be added as a new top level topic
        /// </summary>
        /// <param name="topicType"></param>
        /// <returns></returns>
        public TopicNode For(Type topicType)
        {
            return _topics[topicType];
        }

        public TopicNode For<T>() where T : Topic, new()
        {
            return For(typeof (T));
        }

        public IEnumerable<TopicNode> TopLevelNodes()
        {
            return _topics.GetAll().Where(x => x.Parent == null);
        } 
    }
}