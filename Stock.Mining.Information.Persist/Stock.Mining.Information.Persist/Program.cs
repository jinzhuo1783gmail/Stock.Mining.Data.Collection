using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Collection
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();

                    config.AddJsonFile("appsettings.json");

                    JToken jAppSettings = JToken.Parse(
                      File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "appsettings.json"))
                    );

                    string env = jAppSettings["Env"].ToString();

                    if (File.Exists($"appsettings.{env}.json"))
                        config.AddJsonFile($"appsettings.{env}.json");

                    config.AddEnvironmentVariables();

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
