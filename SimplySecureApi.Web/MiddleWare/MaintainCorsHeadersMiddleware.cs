using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.MiddleWare
{
    public class MaintainCorsHeadersMiddleware
    {
        public MaintainCorsHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public async Task Invoke(HttpContext httpContext)
        {
            var corsHeaders = new HeaderDictionary();

            foreach (var pair in httpContext.Response.Headers)
            {
                if (!pair.Key.ToLower().StartsWith("access-control-"))
                {
                    continue;
                }

                corsHeaders[pair.Key] = pair.Value;
            }

            httpContext.Response.OnStarting(o =>
            {
                var context = (HttpContext)o;

                var headers = context.Response.Headers;

                foreach (var pair in corsHeaders)
                {
                    if (headers.ContainsKey(pair.Key))
                    {
                        continue;
                    }

                    headers.Add(pair.Key, pair.Value);
                }

                return Task.CompletedTask;
            }, httpContext);

            await _next(httpContext);
        }
    }
}