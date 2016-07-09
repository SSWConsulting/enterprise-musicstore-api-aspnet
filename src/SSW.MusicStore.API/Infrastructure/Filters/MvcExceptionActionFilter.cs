using System.Data.Common;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Mindscape.Raygun4Net;

namespace SSW.MusicStore.API.Filters
{
    public class MvcExceptionActionFilter : ActionFilterAttribute
    {
        private readonly IConfigurationRoot config;

        public MvcExceptionActionFilter(IConfigurationRoot config)
        {
            this.config = config;
        }
        
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

            var apiKey = this.config["RaygunSettings:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return;
            }

            //TODO: update this once Raygun supports ASPNET CORE RTM
            //var settings = new RaygunSettings { ApiKey = apiKey };

            //var client = new RaygunClient(settings);
            //client.Send(context.Exception);
        }
    }
}
