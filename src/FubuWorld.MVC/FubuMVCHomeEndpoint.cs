namespace FubuWorld.MVC
{
    public class FubuMVCHomeEndpoint
    {
         public FubuMVCHomeModel get_home(FubuMVCHomeInput input)
         {
             return new FubuMVCHomeModel();
         }
    }

    public class FubuMVCHomeInput { }
    public class FubuMVCHomeModel { }
}