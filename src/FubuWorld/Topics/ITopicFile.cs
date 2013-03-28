using FubuMVC.Core.View;

namespace FubuWorld.Topics
{
    public interface ITopicFile
    {
        string FilePath { get; }
        string Name { get; }
        string Folder { get; }
        IViewToken ToViewToken();
    }
}