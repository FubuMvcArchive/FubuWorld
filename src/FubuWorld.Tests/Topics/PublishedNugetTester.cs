﻿using System.Diagnostics;
using FubuCore;
using FubuDocs.Topics;
using NUnit.Framework;
using FubuTestingSupport;
using System.Collections.Generic;
using System.Linq;

namespace FubuWorld.Tests.Topics
{
    [TestFixture]
    public class PublishedNugetTester
    {
        [Test]
        public void read_from_file()
        {
            new FileSystem().WriteStringToFile("test.nuspec", @"
<?xml version='1.0'?>
<package xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
  <metadata xmlns='http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd'>
    <id>FubuWorld.Docs</id>
    <version>0.0.0</version>
    <authors>Jeremy D. Miller</authors>
    <owners>Jeremy D. Miller</owners>
    <licenseUrl>https://github.com/DarthFubuMVC/fubucore/raw/master/license.txt</licenseUrl>
    <projectUrl>http://localization.fubu-project.org</projectUrl>
    <iconUrl>https://github.com/DarthFubuMVC/fubu-collateral/raw/master/Icons/FubuLocalization_256.png</iconUrl>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>Documentation for FubuWorld.Docs</description>
    <tags>fubumvc documentation</tags>
    <dependencies>
    </dependencies>
  </metadata>
  <files>
    <file src='..\..\src\FubuWorld.Docs\bin\Release\FubuWorld.Docs.*' target='lib\net40' />
  </files>
</package>
".Trim().Replace("'", "\""));



            var nuget = PublishedNuget.ReadFrom("test.nuspec");

            nuget.Name.ShouldEqual("FubuWorld.Docs");
            nuget.Description.ShouldEqual("Documentation for FubuWorld.Docs");
        }

        [Test]
        public void read_into_a_project_with_no_whitelist()
        {
            var directory = ".".ToFullPath().ParentDirectory().ParentDirectory().ParentDirectory().ParentDirectory();
            var project = new ProjectRoot();

            PublishedNuget.GatherNugetsIntoProject(project,directory);

            // There's a second nuspec for testing that throws this off
            project.Nugets.Select(x => x.ToString()).ShouldHaveTheSameElementsAs(
"Name: FubuWorld, Description: The core content bottle and infrastructure for the FubuWorld website"
                );

        }

        [Test]
        public void read_into_a_project_with_a_whitelist()
        {
            var directory = ".".ToFullPath().ParentDirectory().ParentDirectory().ParentDirectory().ParentDirectory();
            var project = new ProjectRoot();
            project.NugetWhitelist = "Sample.Docs, Imported.Docs";

            PublishedNuget.GatherNugetsIntoProject(project, directory);

            project.Nugets.Select(x => x.ToString()).ShouldHaveTheSameElementsAs(
"Name: Imported.Docs, Description: Documentation for Imported.Docs",
"Name: Sample.Docs, Description: Documentation for Sample.Docs"
                );
        }
    }
}