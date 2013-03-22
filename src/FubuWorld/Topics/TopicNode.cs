using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FubuCore;
using FubuMVC.Core.Registration;

namespace FubuWorld.Topics
{
    public class TopicNode
    {
        private static FileSystem FileSystem = new FileSystem();


        private readonly Lazy<string> _title; 
        private readonly ITopicFile _file;
        private readonly ProjectRoot _projectRoot;
        private TopicNode _firstChild;
        private TopicNode _next;
        private TopicNode _parent;
        private TopicNode _previous;
        private string _key;

        public TopicNode(ProjectRoot projectRoot, ITopicFile file)
        {
            _projectRoot = projectRoot;
            _file = file;

            var filename = Path.GetFileNameWithoutExtension(file.FilePath).Split('.').Last();
            _key = _file.Folder.TrimEnd('/') + "/" + filename;

            _title = new Lazy<string>(() => {
                if (FileSystem.FileExists(_file.FilePath))
                {
                    Func<string, bool> filter = x => x.StartsWith("Title:", StringComparison.OrdinalIgnoreCase);
                    var rawTitle = findComments().FirstOrDefault(filter);

                    if (rawTitle != null)
                    {
                        return rawTitle.Split(':').Last().Trim();
                    }
                }

                return _file.Name;
            });
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
            get { throw new NotImplementedException(); }
        }

        public string Title
        {
            get { return _title.Value; }
        }


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
            get { return _previous == null ? _parent : _previous.Parent; }
        }

        public IEnumerable<TopicNode> ChildNodes
        {
            get
            {
                TopicNode node = FirstChild;
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

        public void BuildChain(BehaviorGraph graph)
        {
            throw new NotImplementedException();
        }

        public void AppendChild(TopicNode node)
        {
            TopicNode last = LastChild;
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
            TopicNode parent = Parent;
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }

        public void RemoveChild(TopicNode child)
        {
            List<TopicNode> children = ChildNodes.ToList();
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

        public TopicNode FindNext()
        {
            if (_firstChild != null) return _firstChild;

            return findNextTopicNotChild();
        }

        private TopicNode findNextTopicNotChild()
        {
            if (NextSibling != null) return NextSibling;

            if (Parent == null) return null;

            return Parent.findNextTopicNotChild();
        }

        public TopicNode FindPrevious()
        {
            if (PreviousSibling != null) return PreviousSibling;

            return Parent;
        }

        public TopicNode FindIndex()
        {
            if (Parent == null) return null;

            if (Parent != null && Parent.Parent == null) return Parent;

            return Parent.FindIndex();
        }

        public IEnumerable<TopicNode> Descendents()
        {
            foreach (TopicNode childNode in ChildNodes)
            {
                yield return childNode;

                foreach (TopicNode descendent in childNode.Descendents())
                {
                    yield return descendent;
                }
            }
        }
    }
}