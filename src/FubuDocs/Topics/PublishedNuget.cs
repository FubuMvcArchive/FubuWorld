using FubuCore;

namespace FubuDocs.Topics
{
    public class PublishedNuget
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public static PublishedNuget ReadFrom(string file)
        {
            var text = new FileSystem().ReadStringFromFile(file);

            var nuget = new PublishedNuget();

            findName(text, nuget);
            findDescription(text, nuget);

            return nuget;
        }

        private static void findDescription(string text, PublishedNuget nuget)
        {
            string openingTag = "<description>";
            string closingTag = "</description>";

            var start = text.IndexOf(openingTag);

            var end = text.IndexOf(closingTag);

            if (start > -1 && end > -1)
            {
                nuget.Description = text.Substring(start + openingTag.Length, end - start - openingTag.Length);
            }
        }

        private static void findName(string text, PublishedNuget nuget)
        {
            string openingTag = "<id>";
            string closingTag = "</id>";

            var start = text.IndexOf(openingTag);
            
            var end = text.IndexOf(closingTag);

            if (start > -1 && end > -1)
            {
                nuget.Name = text.Substring(start + openingTag.Length, end - start - openingTag.Length);
            }
        }
    }
}