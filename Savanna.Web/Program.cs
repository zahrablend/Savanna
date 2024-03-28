using CodeLibrary;
using Common.IdentityEntities;
using Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Savanna.Infrastructure;
using Savanna.Web.Services;

namespace Savanna.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorPagesOptions(options =>
            {
                //options.Conventions.AddAreaPageRoute("Identity", "Account/Login", "");
            });
            builder.Services.AddSignalR();
            builder.Services.AddDbContext<GameContext>(options =>
            {
                options.UseSqlServer();
            });

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<GameContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, GameContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, GameContext, Guid>>();

            builder.Services.AddSingleton<IGameUI, WebGameUI>();
            builder.Services.AddSingleton<IGameEventService, WebGameUI>();
            builder.Services.AddSingleton<Game>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            // Map controllers and SignalR hubs
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");
            app.MapHub<GameHub>("/gameHub");

            app.Run();
        }
    }
}
