using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FubuCore;

namespace FubuDocsRunner.Exports
{
    public interface IAssetParsingStrategy
    {
        IEnumerable<string> AssetsFor(string source);
    }

    public abstract class AssetParsingStrategy : IAssetParsingStrategy
    {
        private readonly Regex _regex;
        private readonly string _attribute;

        protected AssetParsingStrategy(string expression, string attribute)
        {
            _regex = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            _attribute = attribute;
        }

        public IEnumerable<string> AssetsFor(string source)
        {
            var matches = _regex.Matches(source);
            foreach (Match match in matches)
            {
                var value = HtmlElement.GetAttributeValue(match.Value, _attribute);
                if (value.IsNotEmpty())
                {
                    yield return value;
                }
            }
        }
    }

    public class LinkStrategy : AssetParsingStrategy
    {
        public LinkStrategy()
            : base("<link\\b[^>]*>", "href")
        {
        }
    }

    public class ImgStrategy : AssetParsingStrategy
    {
        public ImgStrategy()
            : base("<img\\b[^>]*>", "src")
        {
        }
    }

    public class ScriptStrategy : AssetParsingStrategy
    {
        public ScriptStrategy()
            : base("<script\\b[^>]*>", "src")
        {
        }
    }

    public class AssetParser
    {
        private static readonly IList<IAssetParsingStrategy> _strategies = new List<IAssetParsingStrategy>();
 
        static AssetParser()
        {
            Reset();
        }

        public static void Reset()
        {
            Clear();

            AddStrategy<LinkStrategy>();
            AddStrategy<ImgStrategy>();
            AddStrategy<ScriptStrategy>();
        }

        public static void Clear()
        {
            _strategies.Clear();
        }

        public static void AddStrategy<T>() where T : IAssetParsingStrategy, new()
        {
            AddStrategy(new T());
        }

        public static void AddStrategy(IAssetParsingStrategy strategy)
        {
            _strategies.Add(strategy);
        }

        public static IEnumerable<string> AssetsFor(string source)
        {
            return _strategies.SelectMany(strategy => strategy.AssetsFor(source));
        }
    }
}