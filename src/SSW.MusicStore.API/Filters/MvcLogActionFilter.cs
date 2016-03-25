using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Serilog;

namespace SSW.MusicStore.API.Filters
{
    public class MvcLogActionFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger = Log.ForContext<MvcLogActionFilter>();

        private readonly Stopwatch _sw = new Stopwatch();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var logger = _logger;
            var requestHeaders = context.HttpContext.Request.Headers;
            foreach (var key in requestHeaders.Keys)
            {
                if (key == "Authorization") continue;
                var value = requestHeaders[key];
                logger = logger.ForContext(key, value);
            }

            _logger.Debug("HTTP {HttpMethod} to {RawUrl} ({@RequestHeaders}) {RequestState}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.GetDisplayUrl(),
                requestHeaders,
                "started");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            _sw.Stop();

            var exception = context.Exception;
            if (exception == null)
            {
                _logger.Information("HTTP {HttpMethod} ({RequestDuration}) to {RawUrl} {RequestState}",
                    context.HttpContext.Request.Method,
                    _sw.Elapsed,
                    context.HttpContext.Request.GetDisplayUrl(),
                    "completed");
            }
            else
            {
                var logContext = _logger;
                foreach (var key in exception.Data.Keys.OfType<string>())
                {
                    logContext = logContext.ForContext(key, exception.Data[key]);
                }

                logContext.Error(exception, "HTTP {HttpMethod} ({RequestDuration}) to {RawUrl} {RequestState} ({Message})",
                    context.HttpContext.Request.Method,
                    _sw.Elapsed,
                    context.HttpContext.Request.GetDisplayUrl(),
                    "failed",
                    exception.Message);

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
}
