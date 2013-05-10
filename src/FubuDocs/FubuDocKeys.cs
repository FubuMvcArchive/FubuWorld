using FubuLocalization;

namespace FubuDocs
{
    public class FubuDocKeys : StringToken
    {
        public static readonly FubuDocKeys Fubu = new FubuDocKeys("Fubu");

        private FubuDocKeys(string defaultValue)
            : base(null, defaultValue, namespaceByType: true)
        {
        }
    }
}