using FubuCore;
using FubuDocs.Topics;
using NUnit.Framework;
using FubuTestingSupport;

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
    }
}