using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Bottles;
using FubuCore;
using FubuCore.CommandLine;
using FubuCore.Util.TextWriting;
using FubuMVC.CodeSnippets;
using FubuMVC.Core;
using FubuMVC.Core.Packaging;
using FubuMVC.Core.Runtime.Files;
using System.Linq;
using FubuMVC.StructureMap;
using Container = StructureMap.Container;

namespace FubuDocsRunner
{
    public class SnippetsInput : DocActionInput
    {
        [Description("Scans and lists codesnippets, but does not perform the import")]
        public bool ScanFlag { get; set; }
    }

    [CommandDescription("Scrapes the entire solution for files with code snippets and places those files under the '/snippets' directory of the documentation project")]
    public class SnippetsCommand : FubuCommand<SnippetsInput>
    {
        public override bool Execute(SnippetsInput input)
        {
            var cache = buildCache(input);

            if (input.ScanFlag)
            {
                writePreview(cache);

                return true;
            }

            var directory = input.DetermineDocumentsFolder();
            var snippets = directory.AppendPath("snippets");

            var fileSystem = new FileSystem();
            fileSystem.DeleteDirectory(snippets);

            var srcDirectory = ".".ToFullPath().AppendPath("src");


            Console.WriteLine("Moving snippet files to " + snippets);
            var writer = new TextReport();
            writer.StartColumns(2);
            writer.AddColumnData("Source", "Destination");
            writer.AddDivider('-');

            cache.All().Each(snippet => {
                var relative = snippet.File.PathRelativeTo(srcDirectory).ParentDirectory();
                var newPath = snippets.AppendPath(relative);

                writer.AddColumnData(snippet.File, newPath);

                fileSystem.CopyToDirectory(snippet.File, newPath);
            });

            writer.WriteToConsole();

            return true;
        }

        private static void writePreview(ISnippetCache cache)
        {
            var writer = new TextReport();
            writer.StartColumns(2);

            writer.AddColumnData("Name", "File");
            writer.AddDivider('-');

            cache.All().Each(snippet => { writer.AddColumnData(snippet.Name, snippet.File); });

            writer.WriteToConsole();
        }

        private static ISnippetCache buildCache(SnippetsInput input)
        {
            var files = new SnippetApplicationFiles(".".ToFullPath().AppendPath("src"), input.DetermineDocumentsFolder());

            var cache = new SnippetCache();

            var scanners = new ISnippetScanner[]
            {
                new CLangSnippetScanner("cs"),
                new CLangSnippetScanner("js"),
                new BlockCommentScanner("<!--", "-->", "spark", "lang-html"),
                new BlockCommentScanner("<!--", "-->", "htm", "lang-html"),
                new BlockCommentScanner("<!--", "-->", "html", "lang-html"),
                new BlockCommentScanner("<!--", "-->", "xml", "lang-xml"),
                new BlockCommentScanner("/*", "*/", "css", "lang-css"),
                new RazorSnippetScanner()
            };
            scanners.Each(finder =>
            {
                files.FindFiles(finder.MatchingFileSet).Each(file =>
                {
                    var scanner = new SnippetReader(file, finder, snippet => {
                        snippet.File = file.Path;
                        cache.Add(snippet);
                    });

                    scanner.Start();
                });
            });
            

            return cache;
        }
    }

    public class SnippetApplicationFiles : IFubuApplicationFiles
    {
        private List<string> _directories;

        public SnippetApplicationFiles(string sourceDirectory, string documentationDirectory)
        {
            var name = Path.GetFileName(documentationDirectory);

            _directories =
                Directory.GetDirectories(sourceDirectory)
                         .Where(x => !Path.GetFileName(x).EqualsIgnoreCase(name))
                         .ToList();
        }

        public string GetApplicationPath()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IFubuFile> FindFiles(FileSet fileSet)
        {
            var fileSystem = new FileSystem();

            // I hate the duplication, but it's tooling code on a beautiful Saturday afternoon
            fileSet.AppendExclude(FubuMvcPackageFacility.FubuContentFolder + "/*.*");
            fileSet.AppendExclude(FubuMvcPackageFacility.FubuPackagesFolder + "/*.*");

            return
                _directories.SelectMany(
                    x => fileSystem.FindFiles(x, fileSet).Select(file => new FubuFile(file, "Unknown")));
        }

        public IFubuFile Find(string relativeName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ContentFolder> AllFolders { get; private set; }
        public IEnumerable<ContentFolder> Folders { get; private set; }
    }
}