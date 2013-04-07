using System.ComponentModel;

namespace FubuDocsRunner.Running
{
    public class RunInput : DocActionInput
    {
        [Description("Disables the bottle and code snippet scanning while this command runs")]
        public bool NoBottlingFlag { get; set; }
    }
}