using FubuCore;
using FubuWorld.Topics;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuWorld.Tests.Topics
{
    [TestFixture]
    public class ProjectRootPersistenceTester
    {
        [Test]
        public void write_and_load()
        {
            var file = "project.json";
            new FileSystem().DeleteFile(file);

            var project = new ProjectRoot
            {
                Name = "FubuMVC",
                GitHubPage = "https://github.com/DarthFubuMVC/fubumvc",
                UserGroupUrl = "https://groups.google.com/forum/?fromgroups#!forum/fubumvc-devel",
                BuildServerUrl = "http://build.fubu-project.org/project.html?projectId=project3&tab=projectOverview",
                BottleName = "FubuMVC.Docs"
            };

            project.WriteTo(file);

            var project2 = ProjectRoot.LoadFrom(file);

            project2.Name.ShouldEqual(project.Name);
            project2.GitHubPage.ShouldEqual(project.GitHubPage);
            project2.UserGroupUrl.ShouldEqual(project.UserGroupUrl);
            project2.BuildServerUrl.ShouldEqual(project.BuildServerUrl);
            project2.BottleName.ShouldEqual(project.BottleName);
        }

        [Test]
        public void url_is_lower_case_name_if_no_url_is_set()
        {
            Assert.Fail("Do.");
        }

        [Test]
        public void use_the_persisted_url_if_it_exists()
        {
            Assert.Fail("Do.");
        }
    }
}