using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Client.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ImageGallery.Client
{
    public class Startup
    {
        private readonly IConfiguration _conf;

        public Startup(IConfiguration conf)
        {
            _conf = conf;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // register an IHttpContextAccessor so we can access the current
            // HttpContext in services by injecting it
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // register an IImageGalleryHttpClient
            services.AddScoped<IImageGalleryHttpClient, ImageGalleryHttpClient>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = "https://localhost:44326";
                options.ClientId = "imagegalleryclient";
                options.ResponseType = "code id_token";
                //options.CallbackPath = new PathString("...")
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.SaveTokens = true;
                options.ClientSecret = "secret";
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(_conf.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Gallery}/{action=Index}/{id?}");
            });
        }
    }
}
