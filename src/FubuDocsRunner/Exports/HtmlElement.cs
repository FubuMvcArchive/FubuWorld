namespace FubuDocsRunner.Exports
{
    public class HtmlElement
    {
        public static string GetAttributeValue(string input, string attribute)
        {
            var index = input.IndexOf(attribute);
            if (index == -1)
            {
                return "";
            }

            var length = attribute.Length + 2; // attribute="
            var value = "";

            index += length;

            while (true)
            {
                var next = input.Substring(index, 1);
                if (next == "\"")
                {
                    break;
                }

                value += next;
                ++index;
            }

            return value;
        }
    }
}