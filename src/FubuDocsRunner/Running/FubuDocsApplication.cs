using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bottles;
using Bottles.Diagnostics;
using Bottles.Manifest;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuWorld.Infrastructure;
using StructureMap;

namespace FubuDocsRunner.Running
{
    public class FubuDocsApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            return FubuApplication.For<RunFubuWorldRegistry>()
                                  .StructureMap(new Container())
                                  .Packages(x => {
                                      string[] directories =
                                          AppDomain.CurrentDomain.SetupInformation.AppDomainInitializerArguments;
                                      directories.Each(directory
                                                       => x.Loader(new DocumentPackageLoader(directory)));

                                      x.Loader(new FubuDocsPackageLoader());
                                  });
        }
    }


    public class DocumentPackageLoader : IPackageLoader
    {
        private readonly string _directory;

        public static IEnumerable<string> FindDocumentDirectories(string directory)
        {
            return Directory.GetDirectories(directory.AppendPath("src"), "*.Docs", SearchOption.TopDirectoryOnly);
        } 

        public DocumentPackageLoader(string directory)
        {
            _directory = directory;
        }

        public IEnumerable<IPackageInfo> Load(IPackageLog log)
        {
            var reader = new PackageManifestReader(new FileSystem(), folder => folder);
            var docDirs = FindDocumentDirectories(_directory);

            return docDirs.Select(reader.LoadFromFolder);
        }
    }
}