using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DigitalWizardry.MazeGenerator
{
    public class Program
    {
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
    }
}