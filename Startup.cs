using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduwardPost.AzureAd.K8s.Auth.SideCar.Extensions;
using EduwardPost.AzureAd.K8s.Auth.SideCar.Middleware;
using EduwardPost.AzureAd.K8s.Auth.SideCar.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EduwardPost.AzureAd.K8s.Auth.SideCar {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthentication(sharedOptions => {
                    sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddAzureAdBearer(options => Configuration.Bind("AzureAd", options));
        }

        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

               app.UseAuthentication();

            app.Use(async (context, next) =>
            {
                await context.AuthenticateAsync();
                await next.Invoke();
            });

            app.UseAuthenticatedProxy(new AuthenticatedProxyOptions {});
        }
    }
}