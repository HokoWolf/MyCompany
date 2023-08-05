using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCompany.Domain;
using MyCompany.Domain.Repositories.Abstract;
using MyCompany.Domain.Repositories.EntityFramework;
using MyCompany.Service;

namespace MyCompany
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // consequence is impossibly important
            // configuring
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.Bind("Project", new Config());

            builder.Services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            builder.Services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
            builder.Services.AddTransient<DataManager>();

            builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(opts =>
            {
                opts.Cookie.Name = "myCompanyAuth";
                opts.Cookie.HttpOnly = true;
                opts.LoginPath = "/account/login";
                opts.AccessDeniedPath = "/account/accessdenied";
                opts.SlidingExpiration = true;
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
            });

            builder.Services.AddControllersWithViews(configure =>
            {
                configure.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
            }).AddSessionStateTempDataProvider();


            // running
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // connecting static file (like css, js)
            app.UseStaticFiles();

            // connecting routing
            app.UseRouting();

            // must connect after routing and before endpoints (routes for routing)
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("admin", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}