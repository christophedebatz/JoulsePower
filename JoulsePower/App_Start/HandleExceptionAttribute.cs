using System.Web.Mvc;
using IExceptionFilter = System.Web.Mvc.IExceptionFilter;

namespace JoulsePower
{
    public class HandleExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            filterContext.Result = new JsonResult
            {
                Data = new
                {
                    success = false,
                    source = filterContext.Exception.Source,
                    message = filterContext.Exception.Message
                }
            };
        }
    }
}