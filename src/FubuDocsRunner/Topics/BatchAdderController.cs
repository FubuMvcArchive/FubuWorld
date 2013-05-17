using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore;

namespace FubuDocsRunner.Topics
{
    public class BatchAdderController
    {
        private readonly ITopicFileSystem _fileSystem;
        private readonly IList<TopicToken> _topics = new List<TopicToken>(); 

        public BatchAdderController(ITopicFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void Start()
        {
            _topics.Clear();
            _topics.AddRange(_fileSystem.ReadTopics().OrderBy(x => x.Order));

            for (int i = 0; i < _topics.Count; i++)
            {
                TopicToken topicToken = (TopicToken) _topics[i];
                var order = i + 1;
                if (topicToken.Order != order)
                {
                    _fileSystem.Reorder(topicToken, order);
                }
            }
        }

        public WhatNext ReadText(string text)
        {
            if (text == null) return WhatNext.Stop;

            text = text.Trim();

            if (text.IsEmpty()) return WhatNext.Stop;
            
            if (text.EqualsIgnoreCase("q") || text.EqualsIgnoreCase("quit")) return WhatNext.Stop;

            var token = TopicToken.Read(text);
            token.Order = _topics.Count + 1;

            _fileSystem.AddTopic(token);

            _topics.Add(token);

            return WhatNext.ReadMore;
        }
    }
    
    public enum WhatNext
    {
        ReadMore,
        Stop
    }

    public interface ITopicFileSystem
    {
        IEnumerable<TopicToken> ReadTopics();
        void AddTopic(TopicToken token);
        void Reorder(TopicToken topicToken, int order);
    }

    public enum TopicTokenType
    {
        File,
        Folder
    }

    public class TopicToken
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string RelativePath { get; set; }
        public TopicTokenType Type { get; set; }

        public TopicToken()
        {
            Type = TopicTokenType.File;
        }

        public override string ToString()
        {
            return string.Format("Key: {0}, Title: {1}, Order: {2}, RelativePath: {3}", Key, Title, Order, RelativePath);
        }

        protected bool Equals(TopicToken other)
        {
            return string.Equals(Key, other.Key) && string.Equals(Title, other.Title) && Order == other.Order && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TopicToken) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Key != null ? Key.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Order;
                hashCode = (hashCode*397) ^ (int) Type;
                return hashCode;
            }
        }

        public static TopicToken Read(string text)
        {
            var topic = new TopicToken();

            var parts = text.Split('=');
            var key = parts.First();
            if (key.StartsWith("/"))
            {
                topic.Type = TopicTokenType.Folder;
                topic.Key = key.TrimStart('/');
            }
            else
            {
                topic.Key = key;
            }

            topic.Title = parts.Length > 1
                              ? parts.ElementAt(1)
                              : topic.Key.Capitalize();

            return topic;
        }
    }
}