using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using FubuCore;
using FubuDocs;
using System.Linq;

namespace FubuDocsRunner
{
    public class TopicParser
    {
        public TopicRequest Parse(string directory)
        {
            var file = directory.AppendPath("Topics.xml");
            if (!File.Exists(file))
            {
                Console.WriteLine("{0} does not exist, creating it for you now", file);
                var doc = new XmlDocument();
                doc.LoadXml("<Topic title=\"CHANGEME\" name=\"CHANGEME\" ></Topic>");
                doc.Save(file);

                return null;
            }

            var document = new XmlDocument();
            document.Load(file);

            return Parse(directory, document);
        }

        public TopicRequest Parse(string directory, XmlDocument document)
        {
            var element = document.DocumentElement;
            var stack = new TopicXmlStack(directory);

            var request = stack.AddTopic(element);
            stack.PushTopic(request);

            walkChildren(element, stack);

            return request;
        }

        // assume that topic has been pushed before you get here!
        private void walkChildren(XmlElement element, TopicXmlStack stack)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                var child = node as XmlElement;
                if (child == null) continue;

                if (child.Name == "Topic")
                {
                    walkTopicNode(stack, child);
                }
                else
                {
                    walkFolderNode(stack, child);
                }
            }
        }

        private void walkTopicNode(TopicXmlStack stack, XmlElement child)
        {
            var request = stack.AddTopic(child);
            if (child.HasChildNodes)
            {
                stack.PushTopic(request);

                walkChildren(child, stack);

                stack.PopTopic();
            }
        }

        private void walkFolderNode(TopicXmlStack stack, XmlElement child)
        {
            var folder = child.Name;
            stack.PushFolder(folder);

            walkChildren(child, stack);

            stack.PopFolder();
        }
    }

    public class TopicXmlStack
    {
        private readonly string _directory;
        private readonly Stack<string> _namespace = new Stack<string>(); 
        private readonly Stack<TopicRequest> _topics = new Stack<TopicRequest>(); 

        public TopicXmlStack(string directory)
        {
            _directory = directory;
        }

        public string CurrentNamespace
        {
            get
            {
                if (_namespace.Any())
                {
                    return _namespace.Reverse().Join(".");
                }

                return string.Empty;
            }
        }

        public void PushFolder(string name)
        {
            _namespace.Push(name);
        }

        public void PopFolder()
        {
            _namespace.Pop();
        }


        public TopicRequest AddTopic(XmlElement element)
        {
            var request = new TopicRequest
            {
                Title = element.GetAttribute("title"),
                Namespace = CurrentNamespace,
                RootDirectory = _directory,
                TopicName = element.GetAttribute("name")
            };

            if (_topics.Any())
            {
                _topics.Peek().Children.Add(request);
            }

            return request;
        }

        public void PushTopic(TopicRequest request)
        {
            _topics.Push(request);
        }

        public void PopTopic()
        {
            _topics.Pop();
        }
    }
}