using System.IO;
using FubuCore;

namespace CodeSnippetHarness
{
    public class SampleCollector
    {
        private readonly string _name;
        private readonly StringWriter _writer = new StringWriter();

        public SampleCollector(string name)
        {
            _name = name;
        }

        private bool _latched = false;

        // SAMPLE: AddLine
        public void AddLine(string line)
        {
            if (_latched) return;

            string startingString = line.TrimStart();
            if (startingString.StartsWith("// END:") && line.EndsWith(_name))
            {
                _latched = true;
                return;
            }

            if (startingString.StartsWith("// END:") || startingString.StartsWith("// SAMPLE:")) return;

            _writer.WriteLine(substituted(line));
        }
        // ENDSAMPLE

        private static string substituted(string line)
        {
            return line.Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public void WriteFile(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string file = Path.Combine(folder, _name + ".txt");
            new FileSystem().WriteStringToFile(file, _writer.GetStringBuilder().ToString().TrimEnd());
        }
    }
}