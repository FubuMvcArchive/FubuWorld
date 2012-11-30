using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuCore.Util;

namespace FubuDocs
{
    public abstract class Topic
    {
        private readonly string _title;

        protected Topic(string title)
        {
            _title = title;
        }

        public string Title
        {
            get { return _title; }
        }
    }

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

    public interface ITopicRegistration
    {
        void Modify(TopicGraph graph);
    }

    public class TopicNode
    {
        public static TopicNode For<T>() where T : Topic, new()
        {
            return new TopicNode(typeof(T));
        }

        private readonly Type _topicType;

        public static bool IsTopicType(Type type)
        {
            return type.IsConcreteWithDefaultCtor() && type.IsConcreteTypeOf<Topic>();
        }

        private readonly Lazy<Topic> _topic; 

        public TopicNode(Type topicType)
        {
            if (!IsTopicType(topicType))
            {
                throw new ArgumentOutOfRangeException("topicType", "topicType must be a subclass of Topic with a default ctor");
            }

            _topic = new Lazy<Topic>(() => (Topic) Activator.CreateInstance(topicType));
            _topicType = topicType;
        }

        public Type TopicType
        {
            get { return _topicType; }
        }

        public string Title
        {
            get { return _topic.Value.Title; }
        }

        private TopicNode _next;
        private TopicNode _parent;
        private TopicNode _previous;
        private TopicNode _firstChild;


        public TopicNode NextSibling
        {
            get { return _next; }
        }

        public TopicNode PreviousSibling
        {
            get { return _previous; }
        }

        public TopicNode Parent
        {
            get
            {
                return _previous == null ? _parent : _previous.Parent;
            }
        }

        public IEnumerable<TopicNode> ChildNodes
        {
            get
            {
                var node = FirstChild;
                while (node != null)
                {
                    yield return node;

                    node = node.NextSibling;
                }
            }
        } 

        public TopicNode FirstChild 
        {
            get { return _firstChild; }
            private set
            {
                if (value != null)
                {
                    value._parent = this;
                }
                _firstChild = value;
            }
        }

        public TopicNode LastChild
        {
            get { return ChildNodes.LastOrDefault(); }
        }

        public void AppendChild(TopicNode node)
        {
            var last = LastChild;
            if (last == null)
            {
                FirstChild = node;
            }
            else
            {
               last.InsertAfter(node);
            }
        }

        public void PrependChild(TopicNode node)
        {
            if (_firstChild != null)
            {
                _firstChild._previous = node;
                node._next = _firstChild;
            }
            
            FirstChild = node;
        }

        public void InsertAfter(TopicNode node)
        {
            if (_next != null)
            {
                _next._previous = node;
                node._next = _next;
            }

            node._previous = this;
            _next = node;
        }

        public void InsertBefore(TopicNode node)
        {
            if (_previous == null)
            {
                if (_parent != null)
                {
                    _parent.PrependChild(node);
                }
                else
                {
                    node._next = this;
                    _previous = node;
                }
            }
            else
            {
                _previous._next = node;
                node._previous = _previous;

                node._next = this;
                _previous = node;
            }

        }

        public void Remove()
        {
            var parent = Parent;
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }

        public void RemoveChild(TopicNode child)
        {
            var children = ChildNodes.ToList();
            children.Remove(child);

            child._parent = null;
            child._previous = null;
            child._next = null;


            if (!children.Any())
            {
                _firstChild = null;
                return;
            }

            _firstChild = children.First();
            _firstChild._parent = this;
            _firstChild._previous = null;

            children.Last()._next = null;

            for (int i = 1; i < children.Count; i++)
            {
                children[i]._previous = children[i - 1];
            }

            for (int i = 0; i < children.Count - 1; i++)
            {
                children[i]._next = children[i + 1];
            }
        }

        /// <summary>
        /// Adds a child topic and returns itself
        /// </summary>
        /// <typeparam name="T">The child topic</typeparam>
        /// <param name="configuration">Optional configuration of the child topic</param>
        /// <returns></returns>
        public TopicNode Append<T>(Action<TopicNode> configuration = null) where T : Topic, new()
        {
            AppendChild(For<T>());

            return this;
        }

        public override string ToString()
        {
            return string.Format("Topic: {0}", Title);
        }

        protected bool Equals(TopicNode other)
        {
            return Equals(_topicType, other._topicType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TopicNode) obj);
        }

        public override int GetHashCode()
        {
            return (_topicType != null ? _topicType.GetHashCode() : 0);
        }
    }
}