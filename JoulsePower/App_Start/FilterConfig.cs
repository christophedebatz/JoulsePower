using System.Web.Mvc;

namespace JoulsePower
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExceptionAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}