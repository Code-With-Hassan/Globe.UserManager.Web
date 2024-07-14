using Globe.Shared.Entities;
using Globe.Shared.RestCallManager.Services.HttpClientService;
using Globe.Shared.RestCallManager.Services.HttpClientService.Impl;
using Globe.Shared.RestCallManager.Services.RestClientManager;
using Globe.Shared.RestCallManager.Services.RestClientManager.Impl;
using Globe.Shared.RestCallManager.Services.TokenManagerService;
using Globe.Shared.RestCallManager.Services.TokenManagerService.Impl;
using Globe.UserManager.Web.Services.Services.AccountService;
using Globe.UserManager.Web.Services.Services.AccountService.Impl;
using Globe.UserManager.Web.Services.Services.UserService;
using Globe.UserManager.Web.Services.Services.UserService.Impl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using System.Text;

namespace Globe.UserManager.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

            builder.Host.UseSerilog();

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);


            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ITokenManager, TokenManager>();
            builder.Services.AddScoped<IHttpClient, HttpClientWrapper>();
            builder.Services.AddScoped<IRestClientManager, RestClientManager>();

            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };

            app.UseCookiePolicy(cookiePolicyOptions);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
