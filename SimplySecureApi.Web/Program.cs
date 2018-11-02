using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SimplySecureApi.Data.Models.Static;

namespace SimplySecureApi.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                    .UseUrls($"http://*:{ApplicationConfig.Port}")
                        .Build();
    }
}