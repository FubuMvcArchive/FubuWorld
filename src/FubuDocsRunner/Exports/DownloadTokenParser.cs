using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FubuCore;

namespace FubuDocsRunner.Exports
{
    public interface IDownloadTokenStrategy
    {
        IEnumerable<DownloadToken> TokensFor(string baseUrl, string source);
    }

    public abstract class DownloadTokenStrategy : IDownloadTokenStrategy
    {
        private readonly Regex _regex;
        private readonly string _attribute;

        protected DownloadTokenStrategy(string expression, string attribute)
        {
            _regex = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
            _attribute = attribute;
        }

        public IEnumerable<DownloadToken> TokensFor(string baseUrl, string source)
        {
            var matches = _regex.Matches(source);
            foreach (Match match in matches)
            {
                var value = HtmlElement.GetAttributeValue(match.Value, _attribute);
                if (value.IsNotEmpty())
                {
                    yield return DownloadToken.For(baseUrl, value);
                }
            }
        }
    }

    public class LinkTagStrategy : DownloadTokenStrategy
    {
        public LinkTagStrategy()
            : base("<link\\b[^>]*>", "href")
        {
        }
    }

    public class ImgTagStrategy : DownloadTokenStrategy
    {
        public ImgTagStrategy()
            : base("<img\\b[^>]*>", "src")
        {
        }
    }

    public class ScriptTagStrategy : DownloadTokenStrategy
    {
        public ScriptTagStrategy()
            : base("<script\\b[^>]*>", "src")
        {
        }
    }

    public class AnchorTagStrategy : DownloadTokenStrategy
    {
        public AnchorTagStrategy()
            : base("<a\\b[^>]*>", "href")
        {
        }
    }

    // TODO -- Let's call this DownloadTokenParser and return....DownloadTokens
    public class DownloadTokenParser
    {
        private static readonly IList<IDownloadTokenStrategy> Strategies = new List<IDownloadTokenStrategy>();
 
        static DownloadTokenParser()
        {
            Reset();
        }

        public static void Reset()
        {
            Clear();

            AddStrategy<LinkTagStrategy>();
            AddStrategy<ImgTagStrategy>();
            AddStrategy<ScriptTagStrategy>();
            AddStrategy<AnchorTagStrategy>();
        }

        public static void Clear()
        {
            Strategies.Clear();
        }

        public static void AddStrategy<T>() where T : IDownloadTokenStrategy, new()
        {
            AddStrategy(new T());
        }

        public static void AddStrategy(IDownloadTokenStrategy strategy)
        {
            Strategies.Add(strategy);
        }

        public static IEnumerable<DownloadToken> TokensFor(string baseUrl, string source)
        {
            return Strategies.SelectMany(strategy => strategy.TokensFor(baseUrl, source));
        }
    }
}