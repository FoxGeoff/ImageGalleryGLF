using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.API.Entities;
using ImageGallery.API.Entitiies;
using ImageGallery.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageGallery.API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
           _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // register the DbContext on the container, getting the connection string from
            // appSettings (note: use this during development; in a production environment,
            // it's better to store the connection string in an environment variable)
            // note: It is more secure in development to use secrets.json through "Manage User Secrets"
            services.AddDbContext<GalleryContext>(cfg =>
            {
                cfg.UseSqlServer(_config.GetConnectionString("imageGalleryDBConnectionString"));
            });

            //TODO: services.AddTransient<IMailService, NullMailService>();

            services.AddTransient<ImageSeeder>();

            services.AddScoped<IGalleryRepository, GalleryRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //TODO: Error page
                app.UseExceptionHandler("/Error");
            }

            //===
            AutoMapper.Mapper.Initialize(cfg =>
            {
                // Map from Image (entity) to Image, and back
                cfg.CreateMap<Image, Model.Image>().ReverseMap();

                // Map from ImageForCreation to Image
                // Ignore properties that shouldn't be mapped
                cfg.CreateMap<Model.ImageForCreation, Image>()
                    .ForMember(m => m.FileName, options => options.Ignore())
                    .ForMember(m => m.Id, options => options.Ignore())
                    .ForMember(m => m.OwnerId, options => options.Ignore());

                // Map from ImageForUpdate to Image
                // ignore properties that shouldn't be mapped
                cfg.CreateMap<Model.ImageForUpdate, Image>()
                    .ForMember(m => m.FileName, options => options.Ignore())
                    .ForMember(m => m.Id, options => options.Ignore())
                    .ForMember(m => m.OwnerId, options => options.Ignore());
            });

            AutoMapper.Mapper.AssertConfigurationIsValid();
            //===

            //Not requiered => app.UseStaticFiles();

            app.UseMvc();

            if (env.IsDevelopment())
            {
                // Seed the database
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetService<ImageSeeder>();
                    seeder.Seed();
                }
            }
        }
    }
}
