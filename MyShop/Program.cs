using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyShop.Data;
using Models;

namespace MyShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<MyDbContext>();
                try
                {
                    context.Database.EnsureCreated();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }

                if(context.Users.Count() == 0)
                {
                    var user = new ApplicationUser() { UserName = "jianjiuss" };
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    userManager.CreateAsync(user, "123456").Wait();
                    userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "administrator")).Wait();
                    userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "customer")).Wait();
                    userManager.UpdateAsync(user).Wait();
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5000")
                .UseKestrel()
                .UseStartup<Startup>();
    }
}
