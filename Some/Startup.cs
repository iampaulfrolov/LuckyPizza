using System;
using CourseProject.Data.Repositories;
using CourseProject.Identity;
using CourseProject.Identity.Models;
using CourseProject.Models;
using CourseProject.Models.DataModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Some;

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
        services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); });

        services.Configure<Settings>(Configuration.GetSection("ConnectionStrings"));

        services.AddIdentity<User, Role>(options =>
                {
                    options.Password.RequiredLength = 1;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                }
            )
            .AddDefaultTokenProviders()
            .AddDefaultUI();

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddGoogle(options =>
            {
                const string clientId = "372741696784-joitl276u7i76gqiltni09rpcc34nar7.apps.googleusercontent.com";
                const string clientSecret = "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";

                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            });

        services.AddTransient<IUserStore<User>, UserStore>();
        services.AddTransient<IRoleStore<Role>, RoleStore>();

        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient(typeof(IRepository<>), typeof(AdoNetRepository<>));
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
            app.UseBrowserLink();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStaticFiles();
        app.UseSession();
        app.UseHttpsRedirection();


        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Product}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }
}