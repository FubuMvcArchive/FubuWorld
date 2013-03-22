using System;
using System.IO;
using FubuCore;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.View;
using FubuMVC.Spark;
using FubuWorld.Topics;

namespace FubuWorld.Tests.Topics
{
    public class StubTopicFile : ITopicFile
    {
        public StubTopicFile()
        {
            Name = Guid.NewGuid().ToString() + ".spark";
            FilePath = Name;
            RelativePath = Path.GetFileNameWithoutExtension(Name);
        }

        public string FilePath { get; set; }
        public string Name { get; set; }

        public string RelativePath { get; set; }

        public void WriteContents(string contents)
        {
            new FileSystem().WriteStringToFile(FilePath, contents);
        }

        public IViewToken ToViewToken()
        {
            return new StubViewToken(this);
        }
    }

    public class StubViewToken : IViewToken
    {
        private readonly StubTopicFile _file;

        public StubViewToken(StubTopicFile file)
        {
            _file = file;
        }

        protected bool Equals(StubViewToken other)
        {
            return Equals(_file, other._file);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StubViewToken) obj);
        }

        public override int GetHashCode()
        {
            return (_file != null ? _file.GetHashCode() : 0);
        }

        public string Name()
        {
            return _file.Name;
        }

        public ObjectDef ToViewFactoryObjectDef()
        {
            throw new NotImplementedException();
        }

        public Type ViewType { get; private set; }
        public Type ViewModel { get; private set; }
        public string Namespace { get; private set; }
        public string ProfileName { get; set; }
    }
}