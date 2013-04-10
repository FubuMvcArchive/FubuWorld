using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bottles;
using Bottles.Diagnostics;
using Bottles.Manifest;
using FubuCore;

namespace FubuDocsRunner.Running
{
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