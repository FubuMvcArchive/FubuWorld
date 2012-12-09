using System;

namespace FubuDocs
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class FubuDocModuleAttribute : Attribute
    {
        private readonly string _root;

        public FubuDocModuleAttribute(string root)
        {
            _root = root;
        }

        public string Root
        {
            get { return _root; }
        }
    }
}