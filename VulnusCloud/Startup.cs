﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using VulnusCloud.Models;
using Common.Serialization.Interface;
using Common.Serialization;
using Business;
using Business.Interface;
using Common.Http;
using Common.Http.Interface;
using Data.Interface;
using Data.MsSQL;

namespace VulnusCloud
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<EntityFrameWorkMagicContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EntityFrameWorkMagicContext")));
            
            // DI
            services.AddSingleton<IJsonConvertService>(new JsonConvertService());
            services.AddSingleton<IOssIndexService>(new OssIndexService());
            services.AddSingleton<IHttpWebRequestFactory>(new HttpWebRequestFactory());

            var connectionString = Configuration.GetConnectionString("ConnMsSQL");
            services.AddSingleton<IProjectRepository>(new ProjectRepository(connectionString));
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
