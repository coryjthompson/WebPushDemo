using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using WebPushDemo.Models;

namespace WebPushDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages()
                .AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<WebPushDemoContext>(options =>
                options.UseSqlite("Data Source=Data/WebPushDb.db"));
            builder.Services.AddSingleton(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            // Check if Vapid keys are configured
            var config = app.Services.GetRequiredService<IConfiguration>();
            var publicKey = config.GetSection("VapidKeys")["PublicKey"] ?? "";
            var privateKey = config.GetSection("VapidKeys")["PrivateKey"] ?? "";

            if (string.IsNullOrEmpty(publicKey) || string.IsNullOrEmpty(privateKey))
            {
                // Route to generate keys if not configured
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=WebPush}/{action=GenerateKeys}/{id?}");
            }
            else
            {
                // Normal routing to devices
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Devices}/{action=Index}/{id?}");
            }

            app.Run();
        }
    }
}
