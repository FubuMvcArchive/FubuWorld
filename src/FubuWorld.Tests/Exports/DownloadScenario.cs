using System;
using FubuCore;
using FubuDocsRunner.Exports;

namespace FubuWorld.Tests.Exports
{
    public class DownloadScenario
    {
        private static StubStreamProvider _provider;

        public static void Create(Action<ScenarioDefinition> configure)
        {
            var scenario = new ScenarioDefinition();
            configure(scenario);

            _provider = scenario.As<IScenarioDefinition>().BuildProvider();
            DownloadManager.Stub(_provider);
        }

        public static string ContentsFor(string url)
        {
            return _provider.ContentsFor(url);
        }

        public interface IScenarioDefinition
        {
            StubStreamProvider BuildProvider();
        }

        public class ScenarioDefinition : IScenarioDefinition
        {
            private readonly StubStreamProvider _provider;

            public ScenarioDefinition()
            {
                _provider = new StubStreamProvider();
            }

            public ScenarioDefinition Url(string url, string contents)
            {
                _provider.Add(url, contents);
                return this;
            }

            StubStreamProvider IScenarioDefinition.BuildProvider()
            {
                return _provider;
            }
        }
    }
}