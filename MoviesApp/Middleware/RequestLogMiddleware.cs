using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MoviesApp.Middleware
{
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<RequestLogMiddleware> logger)
        {
            if (httpContext.Request.Path.StartsWithSegments("/Actors"))
            {
                logger.LogTrace($"Request: {httpContext.Request.Path}  Method: {httpContext.Request.Method}");
                if (httpContext.Request.Method == HttpMethod.Post.Method && httpContext.Request.Form != null)
                {
                    foreach (var item in httpContext.Request.Form)
                    {
                        logger.LogTrace($"Key: {item.Key}  Value: {item.Value}");
                    }
                }
            }
            await _next(httpContext);
        }
    }
}