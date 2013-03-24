using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FubuCore;
using FubuMVC.Core.Registration;

namespace FubuWorld.Topics
{
    public class Topic : OrderedTopic, ITopicNode
    {
        private static FileSystem FileSystem = new FileSystem();


        private readonly string _title; 
        private readonly ITopicFile _file;
        private Topic _firstChild;
        private Topic _next;
        private Topic _parent;
        private Topic _previous;
        private readonly string _key;
        private readonly string _url;
        public static readonly string Index = "index";

        public Topic(ITopicNode parent, ITopicFile file) : base(Path.GetFileNameWithoutExtension(file.FilePath))
        {
            _file = file;

            var isIndex = Name.EqualsIgnoreCase(Index);

            _url = _key = isIndex ? parent.Url : parent.Url.AppendUrl(Name);

            if (FileSystem.FileExists(_file.FilePath))
            {
                Func<string, bool> filter = x => x.StartsWith("Title:", StringComparison.OrdinalIgnoreCase);
                IEnumerable<string> comments = findComments();
                var rawTitle = comments.FirstOrDefault(filter);
                if (rawTitle != null)
                {
                    _title = rawTitle.Split(':').Last().Trim();
                }

                if (!isIndex)
                {
                    var rawUrl = comments.FirstOrDefault(x => x.StartsWith("Url:", StringComparison.OrdinalIgnoreCase));
                    if (rawUrl.IsNotEmpty())
                    {
                        var segment = rawUrl.Split(':').Last().Trim();
                        _url = _url.ParentUrl().AppendUrl(segment);
                    }
                }
            }

            if (_title.IsEmpty())
            {
                _title = Name.Capitalize().SplitPascalCase();
            }
        }

        private IEnumerable<string> findComments()
        {
            var regex = @"<!--(.*?)-->";
            var matches = Regex.Matches(FileSystem.ReadStringFromFile(_file.FilePath), regex);
            foreach (Match match in matches)
            {
                yield return match.Groups[1].Value.Trim();
            }
        } 


        public ITopicFile File
        {
            get { return _file; }
        }

        public string Key
        {
            get { return _key; }
        }

        public string Url
        {
            get { return _url; }
        }

        public ProjectRoot Project
        {
            get { return _parent.Project; }
        }

        public string Title
        {
            get { return _title; }
        }


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

        public void BuildChain(BehaviorGraph graph)
        {
            throw new NotImplementedException();
        }

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
    }
}