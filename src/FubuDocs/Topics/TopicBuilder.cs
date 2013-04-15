using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FubuCore;

namespace FubuDocs.Topics
{
    public static class TopicBuilder
    {
        private static FileSystem FileSystem = new FileSystem();

        public static void BuildOut(Topic topic)
        {
            topic.Url = topic.Key;

            if (FileSystem.FileExists(topic.File.FilePath))
            {
                Func<string, bool> filter = x => x.StartsWith("Title:", StringComparison.OrdinalIgnoreCase);
                IEnumerable<string> comments = findComments(topic.File.FilePath).ToArray();
                var rawTitle = comments.FirstOrDefault(filter);
                if (rawTitle != null)
                {
                    topic.Title = rawTitle.Split(':').Last().Trim();
                }

                if (!topic.IsIndex)
                {
                    var rawUrl = comments.FirstOrDefault(x => x.StartsWith("Url:", StringComparison.OrdinalIgnoreCase));
                    if (rawUrl.IsNotEmpty())
                    {
                        var segment = rawUrl.Split(':').Last().Trim();
                        topic.Url = topic.Url.ParentUrl().AppendUrl(segment);
                    }
                }
            }

            if (topic.Title.IsEmpty())
            {
                topic.Title = topic.Name.Capitalize().SplitPascalCase();
            }
        }

        private static IEnumerable<string> findComments(string file)
        {
            var regex = @"<!--(.*?)-->";
            var matches = Regex.Matches(FileSystem.ReadStringFromFile(file), regex);
            foreach (Match match in matches)
            {
                yield return match.Groups[1].Value.Trim();
            }
        }
    }
}