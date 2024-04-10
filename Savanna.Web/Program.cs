using CodeLibrary;
using CodeLibrary.GameEngine;
using Common.Identity;
using Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Savanna.Infrastructure;
using Savanna.Web.Services;

namespace Savanna.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddAreaPageRoute("Identity", "Account/Login", "");
            });
            builder.Services.AddSignalR();
            builder.Services.AddDbContext<GameContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<GameContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, GameContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, GameContext, Guid>>();

            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();
            });

            builder.Services.ConfigureExternalCookie(options =>
            {
                options.LoginPath = "/Account/login";
            });

            builder.Services.AddScoped<IGameFieldFactory, AnimalGameFieldFactoryService>();
            builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<IGameRunner, WebGameRunnerService>();
            builder.Services.AddScoped<IGameUI, WebGameUIService>();
            builder.Services.AddScoped<IGameEventService, WebGameUIService>();
            builder.Services.AddScoped<UserManager<ApplicationUser>>();
            builder.Services.AddScoped<Func<Task>>(provider => () => Task.CompletedTask);
            builder.Services.AddScoped<IGameRunningCallback>(provider =>
            {
                var func = provider.GetRequiredService<Func<Task>>();
                return new GameRunningCallbackService(func);
            });
            builder.Services.AddScoped<IGameRepository, GameRepository>();
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

            app.UseRouting();
            app.UseAuthentication();
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
