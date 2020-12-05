using System;
using Billboards.Models;
using DataBaseAccess.DbContext;
using DataBaseAccess.Repsitories;
using DataBaseAccess.Repsitory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelServices;
using ModelServices.AdvertisementStatisticsServicing;
using ModelServices.AdvertisingServicing;
using ModelServices.DeviceGroupServicing;
using ModelServices.DeviceServicing;
using ModelServices.UserLogServicing;
using ModelServices.UserServicing;
using Server;
using Service;

namespace Billboards
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

            string connection = Configuration.GetConnectionString("DefaultConnection");


            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddTransient<IDeviceService, DeviceService>();

            services.AddTransient<IDeviceService, DeviceService>();
            services.AddTransient<IDeviceGroupService, DeviceGroupService>();
            services.AddTransient<IAdvertisingService, AdvertisingService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserLogService, UserLogService>();
            services.AddTransient<IAdvertisementStatisticsService, AdvertisementStatisticsService>();

            services.AddTransient<IRepository<Advertisement>, AdvertisementRepository>();
            services.AddTransient<IRepository<AdvertisementStatistics>, AdvertisementStatisticsRepository>();
            services.AddTransient<IRepository<DeviceGroup>, DeviceGroupRepository>();
            services.AddTransient<IRepository<Device>, DeviceRepository>();
            services.AddTransient<IRepository<UserLog>, UserLogRepository>();
            services.AddTransient<IRepository<User>, UserRepository>();

            var provider = services.BuildServiceProvider();
           
            var server = new ServerEmulation(provider.GetService<IAdvertisingService>(),
                provider.GetService<IDeviceService>(), provider.GetService<IAdvertisementStatisticsService>(),provider.GetService<IDeviceGroupService>());
            services.AddSingleton<IServerEmulation>(server);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
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
