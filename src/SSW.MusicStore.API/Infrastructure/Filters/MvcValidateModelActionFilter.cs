using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace SSW.MusicStore.API.Filters
{
    public class MvcValidateModelActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
