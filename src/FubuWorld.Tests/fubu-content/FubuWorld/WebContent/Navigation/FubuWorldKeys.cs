using FubuLocalization;

namespace FubuWorld.Navigation
{
    public class FubuWorldKeys : StringToken
    {
        public static readonly FubuWorldKeys Main = new FubuWorldKeys("Main");
        public static readonly FubuWorldKeys Home = new FubuWorldKeys("Home");

        public FubuWorldKeys(string defaultValue)
            : base(null, defaultValue, namespaceByType: true)
        {
        }

        public bool Equals(FubuWorldKeys other)
        {
            return other.Key.Equals(Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as FubuWorldKeys);
        }

        public override int GetHashCode()
        {
            return ("FubuWorldKeys:" + Key).GetHashCode();
        }
    }
}