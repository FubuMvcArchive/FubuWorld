﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FubuCore;
using FubuMVC.Core.Behaviors.Chrome;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.View;

namespace FubuDocs.Topics
{
    public class Topic : OrderedTopic, ITopicNode
    {
        public static readonly string Index = "index";
        private Topic _firstChild;
        private Topic _next;
        private Topic _parent;
        private Topic _previous;

        public Topic(ITopicNode parent, ITopicFile file) : base(Path.GetFileNameWithoutExtension(file.FilePath))
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Project = parent.Project;

            File = file;

            IsIndex = Name.EqualsIgnoreCase(Index);
            Key = IsIndex ? parent.Url : parent.Url.AppendUrl(Name);

            TopicBuilder.BuildOut(this);
        }

        public string AbsoluteUrl
        {
            get { return "/" + Url; }
        }

        public bool IsIndex { get; private set; }

        public ITopicFile File { get; private set; }

        public string Key { get; private set; }

        public string Title { get; set; }


        public Topic NextSibling
        {
            get { return _next; }
        }

        public Topic PreviousSibling
        {
            get { return _previous; }
        }

        public Topic Parent
        {
            get { return _previous == null ? _parent : _previous.Parent; }
        }

        public IEnumerable<Topic> ChildNodes
        {
            get
            {
                Topic node = FirstChild;
                while (node != null)
                {
                    yield return node;

                    node = node.NextSibling;
                }
            }
        }

        public Topic FirstChild
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

        public Topic LastChild
        {
            get { return ChildNodes.LastOrDefault(); }
        }

        public BehaviorChain BuildChain()
        {
            var chain = new BehaviorChain();
            chain.Route = new RouteDefinition(Url);
            chain.UrlCategory.Category = Key;
            IViewToken viewToken = File.ToViewToken();

            if (viewToken.ViewModel != typeof(Topic))
            {
                throw new InvalidOperationException("The view model has to be Topic here.");
            }

            chain.AddToEnd(new ChromeNode(typeof (TopicChrome)) {Title = () => Title});

            chain.AddToEnd(new TopicBehaviorNode(this, new ViewNode(viewToken)));

            return chain;
        }

        public string Url { get; set; }

        public ProjectRoot Project { get; private set; }

        public string Import { get; set; }

        public void AppendChild(Topic node)
        {
            Topic last = LastChild;
            if (last == null)
            {
                FirstChild = node;
            }
            else
            {
                last.InsertAfter(node);
            }
        }

        public void PrependChild(Topic node)
        {
            if (_firstChild != null)
            {
                _firstChild._previous = node;
                node._next = _firstChild;
            }

            FirstChild = node;
        }

        public void InsertAfter(Topic node)
        {
            if (_next != null)
            {
                _next._previous = node;
                node._next = _next;
            }

            node._previous = this;
            _next = node;
        }

        public void InsertBefore(Topic node)
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
            Topic parent = Parent;
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }

        public void RemoveChild(Topic child)
        {
            List<Topic> children = ChildNodes.ToList();
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


        public override string ToString()
        {
            return string.Format("Topic: {0}", Title);
        }

        public Topic FindNext()
        {
            if (_firstChild != null) return _firstChild;

            return findNextTopicNotChild();
        }

        private Topic findNextTopicNotChild()
        {
            if (NextSibling != null) return NextSibling;

            if (Parent == null) return null;

            return Parent.findNextTopicNotChild();
        }

        public Topic FindPrevious()
        {
            if (PreviousSibling != null) return PreviousSibling;

            return Parent;
        }

        public Topic FindIndex()
        {
            if (Parent == null) return null;

            if (Parent != null && Parent.Parent == null) return Parent;

            return Parent.FindIndex();
        }

        public IEnumerable<Topic> Descendents()
        {
            foreach (Topic childNode in ChildNodes)
            {
                yield return childNode;

                foreach (Topic descendent in childNode.Descendents())
                {
                    yield return descendent;
                }
            }
        }

        public override IEnumerable<Topic> TopLevelTopics()
        {
            yield return this;
        }

        public void ReplaceWith(Topic other)
        {
            other._next = NextSibling;
            if (_next != null)
            {
                _next._previous = other;
            }

            if (PreviousSibling != null)
            {
                PreviousSibling._next = other;
                other._previous = PreviousSibling;
            }
            else
            {
                _parent._firstChild = other;
            }

            _previous = null;
            _next = null;
        }
    }
}