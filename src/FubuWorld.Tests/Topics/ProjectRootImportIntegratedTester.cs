using FubuTestingSupport;
using NUnit.Framework;
using System.Linq;

namespace FubuWorld.Tests.Topics
{
    [TestFixture]
    public class ProjectRootImportIntegratedTester
    {
        [Test]
        public void the_imported_docs_project_is_imported_to_sample_docs()
        {
            ObjectMother.TopicGraph.ProjectFor("Imported")
                .Parent
                .ShouldBeTheSameAs(ObjectMother.TopicGraph.ProjectFor("FubuMVC")); // FubuMVC is the sample docs here
        }

        [Test]
        public void importdocs_is_attached_into_parent_project_in_the_right_place()
        {
            var importedProject = ObjectMother.TopicGraph.ProjectFor("Imported");
            ObjectMother.Topics["fubumvc/imported"].FirstChild.NextSibling
                                                               .ShouldBeTheSameAs(importedProject.Index);
        }

        [Test]
        public void the_imported_project_is_removed()
        {
            ObjectMother.TopicGraph.Projects.Any(x => x.BottleName == "Imported")
                .ShouldBeFalse();
        }
    }
}