using Microsoft.AspNetCore.Hosting;

namespace WebApplication1;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                // Decommentare in caso di upload di grandi dimensioni
                //webBuilder.UseIISIntegration();
                //webBuilder.ConfigureKestrel(option =>
                //{
                //    option.Limits.MaxRequestBodySize = null;
                //});
            });
}