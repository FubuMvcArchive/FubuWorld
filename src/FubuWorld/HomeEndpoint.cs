namespace FubuWorld
{
    public class HomeEndpoint
    {
         public HomeViewModel Index()
         {
             return new HomeViewModel();
         }
    }

    public class HomeViewModel { }
}