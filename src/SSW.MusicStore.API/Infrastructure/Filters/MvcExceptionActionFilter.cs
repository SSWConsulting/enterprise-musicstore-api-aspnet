using System;
using System.Data.Common;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.AspNetCore;

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

            var apiKey = this.config.Get<string>("RaygunSettings:ApiKey");
            if (string.IsNullOrEmpty(apiKey))
            {
                return;
            }

            var settings = new RaygunSettings { ApiKey = apiKey };
            var client = new RaygunAspNetCoreClient(settings);
            client.Send(context.Exception).Wait();
        }
    }
}
