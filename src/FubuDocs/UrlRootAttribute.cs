using System;

namespace FubuDocs
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class UrlRootAttribute : Attribute
    {
        private readonly string _root;

        public UrlRootAttribute(string root)
        {
            _root = root;
        }

        public string Root
        {
            get { return _root; }
        }
    }
}