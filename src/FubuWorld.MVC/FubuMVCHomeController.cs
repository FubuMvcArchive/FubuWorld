namespace FubuWorld.MVC
{
    public class FubuMVCHomeController
    {
         public FubuMVCHomeModel get_mvc(FubuMVCHomeInput input)
         {
             return new FubuMVCHomeModel();
         }
    }

    public class FubuMVCHomeInput { }
    public class FubuMVCHomeModel { }
}