using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Payroll.DataAccess;
using System.Security.Claims;
using System.Threading.Tasks;
using Payroll.WebSockets;
using Microsoft.AspNetCore.Http;

namespace Payroll
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
            var connectionString = Configuration.GetConnectionString("PayrollDB");
            services.AddDbContext<PayrollDB>(options => options
                .UseMySQL(connectionString));

            services.Configure<ViewModels.PayrollConfiguration>(Configuration.GetSection("PayrollConfiguration"));

            services.AddControllersWithViews();

            services.AddSignalR();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = Configuration["Login:Path"];
            }).AddGoogle(options =>
            {
                options.ClientId = Configuration["Login:ClientId"];
                options.ClientSecret = Configuration["Login:ClientSecret"];
                options.Scope.Add("profile");
                options.Events.OnCreatingTicket = (context) =>
                {
                    var picture = context.User.GetProperty("picture").GetString();
                    context.Identity.AddClaim(new Claim("picture", picture));
                    var name = context.User.GetProperty("name").GetString();
                    context.Identity.AddClaim(new Claim("name", name));
                    var email = context.User.GetProperty("email").GetString();
                    context.Identity.AddClaim(new Claim("email", email));
                    return Task.CompletedTask;
                };
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });
        }
    }
}
