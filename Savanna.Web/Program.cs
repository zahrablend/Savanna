using CodeLibrary.GameEngine;
using CodeLibrary;
using Common.Identity;
using Common.Interfaces;
using Common.ValueObjects;
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

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddSingleton<IGameFieldFactory, GameFieldFactory>();
            builder.Services.AddSingleton<AnimalFactoryLoader>();
            builder.Services.AddSingleton(sp =>
            {
                var gameRepository = sp.GetRequiredService<IGameRepository>();
                var gameFieldFactory = sp.GetRequiredService<IGameFieldFactory>();
                var animalFactoryLoader = sp.GetRequiredService<AnimalFactoryLoader>();
                return new GameService(gameRepository, gameFieldFactory, animalFactoryLoader);
            });

            builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
            builder.Services.AddScoped(sp =>
            {
                var gameRepository = sp.GetRequiredService<IGameRepository>();
                var animalRepository = sp.GetRequiredService<IAnimalRepository>();
                return new StatisticsService(gameRepository, animalRepository);
            });

            builder.Services.AddSingleton(sp =>
            {
                var gameField = sp.GetRequiredService<IGameField>();
                var animalDict = new AnimalDictionary();
                return new GameSetup(gameField, animalDict);
            });

            builder.Services.AddSingleton(sp => sp.GetRequiredService<GameService>().GameField);
            builder.Services.AddSingleton<AnimalService>();
            builder.Services.AddSingleton<UserManager<ApplicationUser>>();
            builder.Services.AddSingleton<Func<Task>>(provider => () => Task.CompletedTask);

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
