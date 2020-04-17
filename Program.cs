using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DigitalWizardry.MazeGenerator
{
    public class Program
    {
        // *** HEROKU ***

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseKestrel(options =>
            {
                options.ListenAnyIP(Int32.Parse(System.Environment.GetEnvironmentVariable("PORT")));
            });
        
        // *** TRADITIONAL ***
        /*
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
				.UseUrls("http://0.0.0.0:5000")  // Docker port forwarding
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
        */
    }
}