using System.Data.Common;
using System.Web.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace SSW.MusicStore.API.Filters
{
    public class MvcExceptionActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (context.Exception == null)
            {
                return;
            }

            var dbException = context.Exception as DbException;
            if (dbException != null)
            {
                context.Result = new BadRequestObjectResult(dbException);
            }
            else
            {
                context.Result = new InternalServerErrorResult();
            }
        }
    }
}
