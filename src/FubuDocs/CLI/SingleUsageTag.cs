﻿using System.Collections.Generic;
using System.Linq;
using FubuCore.CommandLine;
using HtmlTags;

namespace FubuDocs.CLI
{
    public class SingleUsageTag : HtmlTag
    {
        public SingleUsageTag(UsageReport report) : base("div")
        {
            AddClass("single-command-usage");

            var parts = report.Usage.Split(' ');
            var command = parts.Take(2).Join(" ");
            var argsAndFlags = parts.Skip(2).Join(" ");

            Add("em").Text(command);
            Append(new LiteralTag(argsAndFlags));
        }
    }
}