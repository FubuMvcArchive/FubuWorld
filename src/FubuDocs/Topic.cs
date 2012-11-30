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
    }
}