using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Payroll.DataAccess;
using Payroll.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Payroll
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            CurrentEnvironment = hostingEnvironment;
        }

        public IWebHostEnvironment CurrentEnvironment { set; get; }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionStrings = Configuration.GetConnectionString("PayrollTest");
            services.AddDbContext<PayrollDB>(options => options
                .UseMySQL(connectionStrings));

            services.Configure<ViewModels.PayrollConfiguration>(Configuration.GetSection("PayrollConfiguration"));

            services.AddControllersWithViews();

            services.AddSignalR();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = Configuration["Login:Path"];
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseStatusCodePagesWithRedirects("/Error/{0}");
            //app.UseStatusCodePages();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Auth}/{action=Index}/{id?}");
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });
        }
    }
}
