using FubuDocs;
using FubuWorld.HowTo;
using FubuWorld.HowTo.FubuDocsRunner;
using FubuWorld.HowTo.Topics;
using FubuWorld.HowTo.ViewHelpers;

namespace FubuWorld
{
    public class FubuWorldRootTopicRegistry : TopicRegistry
    {
        public FubuWorldRootTopicRegistry()
        {
            For<FubuWorldRoot>().Append<StartingANewFubuworldDocumentationProject>();
            For<FubuWorldRoot>().Append<TheTopicNavigationStructure>();
            For<FubuWorldRoot>().Append<WorkingWithFubudocsrunner>();
            For<FubuWorldRoot>().Append<DocumentationViewHelpers>();

            For<TheTopicNavigationStructure>().Append<TopicNavigation>();
            For<TheTopicNavigationStructure>().Append<AddingANewTopic>();
            For<TheTopicNavigationStructure>().Append<SparkLayoutForATopic>();

            For<WorkingWithFubudocsrunner>().Append<RunningADocumentationProject>();
            For<WorkingWithFubudocsrunner>().Append<AddASingleTopic>();
            For<WorkingWithFubudocsrunner>().Append<GeneratingTheEntireTopicTree>();
            For<WorkingWithFubudocsrunner>().Append<BottlingTheFubuworldProject>();

            For<DocumentationViewHelpers>().Append<EmbeddingCodeSnippets>();
            For<DocumentationViewHelpers>().Append<EmbeddingDiagnosticsVisualizations>();
            For<DocumentationViewHelpers>().Append<EmbeddingUnitTestResults>();
            For<DocumentationViewHelpers>().Append<EmbeddingCommandLineResults>();
        }
    }
}