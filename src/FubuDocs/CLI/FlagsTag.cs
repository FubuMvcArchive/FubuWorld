﻿using System.Collections.Generic;
using FubuCore.CommandLine;
using HtmlTags;

namespace FubuDocs.CLI
{
    public class FlagsTag : TableTag
    {
        public FlagsTag(CommandReport report)
        {
            AddClass("table");
            AddHeaderRow(tr => tr.Header("Flags").Attr("colspan", 2));

            report.Flags.Each(x =>
            {
                AddBodyRow(tr =>
                {
                    tr.Cell(x.UsageDescription).AddClass("command-flag-usage");
                    tr.Cell(x.Description).AddClass("command-flag-description");
                });
            });
        }
    }
}