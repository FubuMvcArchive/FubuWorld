using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore.Util;
using FubuCore;

namespace FubuDocs
{
    public class TopicGraph
    {
        public static readonly TopicGraph AllTopics = new TopicGraph();

        private readonly Cache<Type, TopicNode> _topics = new Cache<Type, TopicNode>();

        public TopicGraph()
        {
            _topics.OnMissing = type => {
                var node = findInChildren(type);
                return node ?? new TopicNode(type);
            };

            _typeByName.OnMissing = name => {
                return All().FirstOrDefault(x => x.TopicType.Name.EqualsIgnoreCase(name));
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
        public TopicNode Find(Type topicType)
        {
            return _topics[topicType];
        }

        public TopicNode Find<T>() where T : Topic, new()
        {
            return Find(typeof (T));
        }

        public void Append<TParent, TChild>() where TParent : Topic, new() where TChild : Topic, new()
        {
            Find<TParent>().AppendChild(Find<TChild>());
        }

        public IEnumerable<TopicNode> TopLevelNodes()
        {
            return _topics.GetAll().Where(x => x.Parent == null);
        } 

        public TopicExpression For<T>() where T : Topic, new()
        {
            return new TopicExpression(Find<T>(), this);
        }

        public class TopicExpression
        {
            private readonly TopicNode _parent;
            private readonly TopicGraph _graph;

            public TopicExpression(TopicNode parent, TopicGraph graph)
            {
                _parent = parent;
                _graph = graph;
            }

            public TopicExpression Append<T>(Action<TopicExpression> childConfiguration = null) where T : Topic, new()
            {
                var child = _graph.Find<T>();
                _parent.AppendChild(child);

                if (childConfiguration != null)
                {
                    childConfiguration(new TopicExpression(child, _graph));
                }

                return this;
            }
        }

        public IEnumerable<TopicNode> All()
        {
            foreach (var topic in TopLevelNodes())
            {
                yield return topic;

                foreach (var descendant in topic.Descendents())
                {
                    yield return descendant;
                }
            }
        } 

        private readonly Cache<string, TopicNode> _typeByName = new Cache<string, TopicNode>(); 

        public TopicNode FindByName(string name)
        {
            return _typeByName[name];
        }
    }
}