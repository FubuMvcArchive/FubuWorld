using FubuLocalization;

namespace FubuWorld.MVC
{
    public class FubuMVCWorldKeys : StringToken
    {
        public static readonly FubuMVCWorldKeys MvcHome = new FubuMVCWorldKeys("MVC");
        public static readonly FubuMVCWorldKeys GettingStarted = new FubuMVCWorldKeys("Getting Started");

        public FubuMVCWorldKeys(string defaultValue)
            : base(null, defaultValue, namespaceByType: true)
        {
        }

        public bool Equals(FubuMVCWorldKeys other)
        {
            return other.Key.Equals(Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as FubuMVCWorldKeys);
        }

        public override int GetHashCode()
        {
            return ("FubuMVCWorldKeys:" + Key).GetHashCode();
        }
    }
}