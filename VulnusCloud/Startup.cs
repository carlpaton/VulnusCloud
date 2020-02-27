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
using System;
using Data.Implementation.MsSQL;

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

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(15); 
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<EntityFrameWorkMagicContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EntityFrameWorkMagicContext")));

            // DI
            var connectionString = GetConnectionString(Configuration.GetConnectionString("ConnMsSQL"));

            var projectRepository = new ProjectRepository(connectionString);
            var componentRepository = new ComponentRepository(connectionString);
            var ossIndexRepository = new OssIndexRepository(connectionString);
            var ossIndexVulnerabilitiesRepository = new OssIndexVulnerabilitiesRepository(connectionString);
            var reportRepository = new ReportRepository(connectionString);
            var reportLinesRepository = new ReportLinesRepository(connectionString);
            var packageTypeRepository = new PackageTypeRepository();

            var httpWebRequestFactory = new HttpWebRequestFactory();
            
            var coordinatesService = new CoordinatesService();
            var jsonConvertService = new JsonConvertService();
            var ossReportService = new OssReportService(
                reportRepository,
                componentRepository,
                ossIndexRepository,
                reportLinesRepository,
                coordinatesService,
                httpWebRequestFactory,
                jsonConvertService,
                ossIndexVulnerabilitiesRepository);

            services.AddTransient<IProjectRepository>(s => projectRepository);
            services.AddTransient<IComponentRepository>(s => componentRepository);
            services.AddTransient<IOssIndexRepository>(s => ossIndexRepository);
            services.AddTransient<IOssIndexVulnerabilitiesRepository>(s => ossIndexVulnerabilitiesRepository);
            services.AddTransient<IReportRepository>(s => reportRepository);
            services.AddTransient<IReportLinesRepository>(s => reportLinesRepository);
            services.AddTransient<IPackageTypeRepository>(s => packageTypeRepository);

            services.AddSingleton<IJsonConvertService>(jsonConvertService);
            services.AddSingleton<ICoordinatesService>(coordinatesService);
            services.AddSingleton<ISelectListItemService>(new SelectListItemService(projectRepository, packageTypeRepository));
            services.AddSingleton<IScoreService>(new ScoreService(reportRepository, reportLinesRepository, ossIndexRepository, ossIndexVulnerabilitiesRepository));
            services.AddSingleton<IScoreClassService>(new ScoreClassService());
            services.AddSingleton<IBreadcrumbReportService>(new BreadcrumbReportService());
            services.AddSingleton<IOssReportService>(ossReportService);
            services.AddSingleton<IOssIndexStatusService>(new OssIndexStatusService(
                reportRepository,
                reportLinesRepository,
                ossIndexRepository));
            services.AddSingleton<IApiCallerService>(new ApiCallerService(
                ossIndexRepository,
                ossReportService));

            services.AddSingleton<ICoordinatePartsFactory>(new CoordinatePartsFactory());
            services.AddSingleton<IHttpWebRequestFactory>(httpWebRequestFactory);
        }

        /// <summary>
        /// Todo - this can be a connection string service, leaving it here for now. for reasons of its a hack anyway :D
        /// </summary>
        /// <param name="appSettingsConnectionString"></param>
        /// <returns></returns>
        private string GetConnectionString(string appSettingsConnectionString)
        {
            var environmentVariableConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION");
            if (environmentVariableConnectionString != null) 
            {
                var findAndReplace = "@@MACHINE_NAME@@";

                if (environmentVariableConnectionString.Contains(findAndReplace)) 
                { 
                    return environmentVariableConnectionString.Replace(findAndReplace, new IpAddressService().GetLocalIPv4());
                }

                return environmentVariableConnectionString;
            }

            return appSettingsConnectionString;
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
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
