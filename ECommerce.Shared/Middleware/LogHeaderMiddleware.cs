using Microsoft.AspNetCore.Http;

namespace ECommerce.Shared.Middleware
{
    public class LogHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public LogHeaderMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var header = context.Request.Headers["CorrelationId"];
            string sessionId;

            if (header.Count > 0)
            {
                sessionId = header[0];
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
            }

            context.Items["CorrelationId"] = sessionId;
            await this._next(context);
        }
    }
}
