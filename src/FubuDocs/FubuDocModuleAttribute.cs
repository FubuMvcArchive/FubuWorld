using System;

namespace FubuDocs
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class FubuDocModuleAttribute : Attribute
    {
        private readonly string _root;

        /// <summary>
        /// Marks an assembly as a FubuWorld documentation Bottle
        /// </summary>
        /// <param name="root">The url root for all the content in this assembly, like 'http://fubu-projects/root/'</param>
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