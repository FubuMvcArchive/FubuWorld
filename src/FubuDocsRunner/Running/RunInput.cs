using System.ComponentModel;
using Fubu.Running;

namespace FubuDocsRunner.Running
{
    public class RunInput : ApplicationRequest
    {
        [Description("Disables the bottle and code snippet scanning while this command runs")]
        public bool NoBottlingFlag { get; set; }
    }
}