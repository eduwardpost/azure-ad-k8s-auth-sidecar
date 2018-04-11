using System.Net;
using System.Threading.Tasks;
using EduwardPost.AzureAd.K8s.Auth.SideCar.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace EduwardPost.AzureAd.K8s.Auth.SideCar.Middleware
{
    public static class AuthenticatedProxyMiddlewareBuilderExtensions
    {
        public static IApplicationBuilder UseAuthenticatedProxy(this IApplicationBuilder app, AuthenticatedProxyOptions options)
        {
            return app.UseMiddleware<AuthenticatedProxyOptions>(options);
        }
    }


    public class AuthenticatedProxyMiddleware
    {
        readonly RequestDelegate _next;
        readonly AuthenticatedProxyOptions _settings;


        public AuthenticatedProxyMiddleware(RequestDelegate next, AuthenticatedProxyOptions settings)
        {
            _next = next;
            _settings = settings;
        }

        public async Task Invoke(HttpContext context)
        {
            await ExecuteAsync(context);

        }

        async Task ExecuteAsync(HttpContext context)
        {
            if (await CheckAuthentication(context))
            {
                //do proxy
            }
        }

        async Task<bool> CheckAuthentication(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
                return true;

            if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("You are unauthorized, please provide authorization");
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Access forbidden, please provide correct authorization");
            }

            return false;
        }
    }

}