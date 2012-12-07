namespace FubuWorld
{
    public class FubuWorldRootTopicRegistry : FubuDocs.TopicRegistry
    {
        public FubuWorldRootTopicRegistry()
        {
            For<FubuWorld.FubuWorldRoot>().Append<FubuWorld.HowTo.StartingANewFubuworldDocumentationProject>();
            For<FubuWorld.FubuWorldRoot>().Append<FubuWorld.HowTo.Topics.TheTopicNavigationStructure>();
            For<FubuWorld.FubuWorldRoot>().Append<FubuWorld.HowTo.FubuDocsRunner.WorkingWithFubudocsrunner>();
            For<FubuWorld.FubuWorldRoot>().Append<FubuWorld.HowTo.ViewHelpers.DocumentationViewHelpers>();

            For<FubuWorld.HowTo.Topics.TheTopicNavigationStructure>().Append<FubuWorld.HowTo.Topics.TopicNavigation>();
            For<FubuWorld.HowTo.Topics.TheTopicNavigationStructure>().Append<FubuWorld.HowTo.Topics.AddingANewTopic>();
            For<FubuWorld.HowTo.Topics.TheTopicNavigationStructure>().Append<FubuWorld.HowTo.Topics.SparkLayoutForATopic>();

            For<FubuWorld.HowTo.FubuDocsRunner.WorkingWithFubudocsrunner>().Append<FubuWorld.HowTo.FubuDocsRunner.RunningADocumentationProject>();
            For<FubuWorld.HowTo.FubuDocsRunner.WorkingWithFubudocsrunner>().Append<FubuWorld.HowTo.FubuDocsRunner.AddASingleTopic>();
            For<FubuWorld.HowTo.FubuDocsRunner.WorkingWithFubudocsrunner>().Append<FubuWorld.HowTo.FubuDocsRunner.GeneratingTheEntireTopicTree>();
            For<FubuWorld.HowTo.FubuDocsRunner.WorkingWithFubudocsrunner>().Append<FubuWorld.HowTo.FubuDocsRunner.BottlingTheFubuworldProject>();

            For<FubuWorld.HowTo.ViewHelpers.DocumentationViewHelpers>().Append<FubuWorld.HowTo.ViewHelpers.CodeSnippets>();
            For<FubuWorld.HowTo.ViewHelpers.DocumentationViewHelpers>().Append<FubuWorld.HowTo.ViewHelpers.DiagnosticsVisualizations>();
            For<FubuWorld.HowTo.ViewHelpers.DocumentationViewHelpers>().Append<FubuWorld.HowTo.ViewHelpers.UnitTestResults>();
            For<FubuWorld.HowTo.ViewHelpers.DocumentationViewHelpers>().Append<FubuWorld.HowTo.ViewHelpers.CommandLineResults>();

        }
    }
}
