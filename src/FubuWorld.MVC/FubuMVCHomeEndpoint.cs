namespace FubuWorld.MVC
{
    public class FubuMVCHomeEndpoint
    {
         public FubuMVCHomeModel get_fubumvc(FubuMVCHomeInput input)
         {
             return new FubuMVCHomeModel();
         }
    }

    public class FubuMVCHomeInput { }
    public class FubuMVCHomeModel { }
}