namespace FubuWorld.Topics
{
    public static class StringExtensions
    {
         public static string AppendUrl(this string url, string part)
         {
             return (url + "/" + part).Replace("//", "/");
         }
    }
}