using System;
using System.Text;
using System.Data;
using System.Globalization;

namespace IssueTracker.SystemServices
{
    public class SessionExpireMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionExpireMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sessionStarted = context.Session.GetString("SessionStarted");

            if (!string.IsNullOrEmpty(sessionStarted))
            {
                if (DateTime.TryParse(sessionStarted, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var lastActivityTime))
                {
                    if ((DateTime.UtcNow - lastActivityTime).TotalMinutes > 2)
                    {
                        context.Session.Clear();
                        context.Response.Redirect("/Account/Login");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            context.Session.SetString("SessionStarted", DateTime.Now.ToString());
            await _next(context);
        }
    }

    public static class SessionExpireMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionExpireMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionExpireMiddleware>();
        }
    }

}
