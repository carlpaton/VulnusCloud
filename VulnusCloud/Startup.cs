using Microsoft.AspNetCore.Builder;
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
using VulnusCloud.Domain.Interface;
using VulnusCloud.Domain;

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
            var connectionString = Configuration.GetConnectionString("ConnMsSQL");
            var projectRepository = new ProjectRepository(connectionString);
            var componentRepository = new ComponentRepository(connectionString);
            var ossIndexRepository = new OssIndexRepository(connectionString);
            var ossIndexVulnerabilitiesRepository = new OssIndexVulnerabilitiesRepository(connectionString);
            var reportRepository = new ReportRepository(connectionString);
            var reportLinesRepository = new ReportLinesRepository(connectionString);

            services.AddSingleton<IProjectRepository>(projectRepository);
            services.AddSingleton<IComponentRepository>(componentRepository);
            services.AddSingleton<IOssIndexRepository>(ossIndexRepository);
            services.AddSingleton<IOssIndexVulnerabilitiesRepository>(ossIndexVulnerabilitiesRepository);
            services.AddSingleton<IReportRepository>(reportRepository);
            services.AddSingleton<IReportLinesRepository>(reportLinesRepository);

            services.AddSingleton<IJsonConvertService>(new JsonConvertService());
            services.AddSingleton<IOssIndexService>(new OssIndexService());
            services.AddSingleton<ISelectListItemService>(new SelectListItemService(projectRepository));
            services.AddSingleton<IScoreService>(new ScoreService(reportRepository, reportLinesRepository, ossIndexRepository, ossIndexVulnerabilitiesRepository));
            services.AddSingleton<IScoreClassService>(new ScoreClassService());

            services.AddSingleton<IHttpWebRequestFactory>(new HttpWebRequestFactory());
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
