using System;
using FubuDocsRunner.Topics;
using NUnit.Framework;
using FubuCore;

namespace FubuWorld.Tests.Commands
{
    [TestFixture]
    public class ListCommandSmokeTester
    {
        [Test]
        public void write_topics_for_FubuWorld_Docs()
        {
            Environment.CurrentDirectory = ".".ToFullPath().ParentDirectory().ParentDirectory().ParentDirectory()
                                              .AppendPath("FubuWorld.Docs");

            new ListCommand().Execute(new ListInput());
        }

        [Test]
        public void write_todos_for_FubuWorld_Docs()
        {
            Environment.CurrentDirectory = ".".ToFullPath().ParentDirectory().ParentDirectory().ParentDirectory()
                                              .AppendPath("FubuWorld.Docs");

            new ListCommand().Execute(new ListInput{Mode = ListMode.todo});
        }

        [Test]
        public void write_all_for_FubuWorld_Docs()
        {
            Environment.CurrentDirectory = ".".ToFullPath().ParentDirectory().ParentDirectory().ParentDirectory()
                                              .AppendPath("FubuWorld.Docs");

            new ListCommand().Execute(new ListInput { Mode = ListMode.all });
        }
    }
}