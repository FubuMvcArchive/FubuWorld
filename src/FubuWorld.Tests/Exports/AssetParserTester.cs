using System.Collections.Generic;
using FubuDocsRunner.Exports;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuWorld.Tests.Exports
{
    [TestFixture]
    public class AssetParserTester
    {
        [Test]
        public void aggregates_the_strategies()
        {
            var s1 = new StubStrategy("a1", "a2");
            var s2 = new StubStrategy("a3", "a4");

            AssetParser.Clear();
            AssetParser.AddStrategy(s1);
            AssetParser.AddStrategy(s2);

            AssetParser.AssetsFor("test").ShouldHaveTheSameElementsAs("a1", "a2", "a3", "a4");

            AssetParser.Reset();
        }



        public class StubStrategy : IAssetParsingStrategy
        {
            private readonly IEnumerable<string> _assets;

            public StubStrategy(params string[] assets)
            {
                _assets = assets;
            }

            public IEnumerable<string> AssetsFor(string source)
            {
                return _assets;
            }
        }
    }
}