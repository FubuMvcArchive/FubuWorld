using FubuCore.Descriptions;
using FubuDocs;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuCore.Reflection;
using FubuMVC.Core.Registration.Routes;

namespace FubuWorld.Infrastructure
{
    [ConfigurationType(ConfigurationType.Conneg)]
    public class TopicUrlPolicy : Policy
    {
        public TopicUrlPolicy()
        {
            Where.InputTypeImplements<Topic>();
            ModifyBy(chain => {
                chain.IsPartialOnly = false;
                if (chain.Route == null && !chain.InputType().HasAttribute<UrlPatternAttribute>())
                {
                    chain.Route = new RouteDefinition(Topic.UrlPatternFor(chain.InputType()));
                }
            });
        }

        protected override void describe(Description description)
        {
            description.Title = "TopicUrlPolicy";
            description.ShortDescription = "Sets the default Url pattern for chains where the input type is a Topic";
        }
    }
}