using System;
using FubuCore.Reflection;

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

        public static string UrlPatternFor(Type type)
        {
            var fullname = type.FullName;
            var assemblyName = type.Assembly.GetName().Name;

            var name = fullname.Substring(assemblyName.Length);

            var url = name.TrimStart('.').Replace('.', '/').ToLower();


            type.Assembly.ForAttribute<FubuDocModuleAttribute>(att => {
                url = att.Root + "/" + url;
            });


            return url;
        }
    }
}