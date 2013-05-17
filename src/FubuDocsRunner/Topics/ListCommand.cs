using System;
using System.Collections.Generic;
using FubuCore.Util.TextWriting;
using FubuDocs.Todos;
using FubuDocs.Topics;
using FubuCore;

namespace FubuDocsRunner.Topics
{
    public class ListCommand
    {
         
    }

    public class TodoTextReport : TextReport
    {
        public TodoTextReport(string folder, IEnumerable<Topic> topics)
        {
            AddDivider('-');
            StartColumns(new Column(ColumnJustification.left, 0, 5),
                new Column(ColumnJustification.right, 0, 5),
                new Column(ColumnJustification.left, 0, 0)
                );            
            AddColumnData("File", "Line", "Message");
            AddDivider('-');

            var todos = TodoTask.FindAllTodos(topics);

            todos.Each(todo => {
                AddColumnData(todo.File.PathRelativeTo(folder), todo.Line.ToString(), todo.Message);
            });
        }
    }

    public class TopicTextReport : TextReport
    {

        public TopicTextReport(IEnumerable<Topic> topics)
        {
            AddDivider('-');
            StartColumns(3);
            AddDivider('-');
            AddColumnData("Url", "Title", "Key");
            AddDivider('-');

            topics.Each(topic => AddColumnData(topic.Url, topic.Title, topic.Key));
        }

    }
}