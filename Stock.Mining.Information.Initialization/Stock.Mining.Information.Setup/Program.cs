using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;
using Serilog;
using Stock.Mining.Information.Initialization.Manager;
using Stock.Mining.Information.Proxy;
using Stock.Symbol.Feature.Shared.Model;
using Stock.Symbol.Feature.Shared.Model.Util;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Stock.Mining.Information.Initialization;

namespace Stock.Mining.Information.Setup
{
    class Program
    {
        public async static Task Main(string[] args)
        {


            var isAppend = true;
            var isProd = false;

            if (args.Contains("--init")) isAppend = false;
            if (args.Contains("--prod")) isProd = true;


            if (args.Contains("--prod")) isProd = true;
            if (args.Contains("--init")) isAppend = false;
                


            var builder = new ConfigurationBuilder();
            BuildConfig(builder, isProd);

            var builderRoot = builder.Build();


            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builderRoot)
                        .CreateLogger();

            //Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builderRoot)
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //    .WriteTo.File(@"\\192.168.1.108\public\log\Stock.Mining.Information.Setup.Log-.txt", rollingInterval: RollingInterval.Day) 
                
            //    .CreateLogger();

            

            Log.Logger.Information("Setup Program Start ........ ");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    
                    //services.Configure<ApiSymbol>(options => builderRoot.GetSection("ApiCollection:Symbol").Bind(options));
                    //services.Configure<ApiSymbol>(builderRoot.GetSection("ApiCollection:Symbol"));

                    services.AddTransient<CollectionProxy>();
                    services.AddTransient<RestSharpUtil>();
                    services.AddTransient<Initialize>();
                    services.AddTransient<LoadProxy>();

                    services.AddTransient<InstitutionManager>();
                    services.AddTransient<MarketPriceManager>();
                    services.AddTransient<InsiderTransactionManager>();
                    services.AddTransient<SymbolManager>();

                    services.AddTransient<InitializeManager>();
                    services.AddTransient<AppendManager>();
                    services.AddSingleton<IRestClient, RestClient>();
                    
                    
                })
                .UseSerilog()
                .Build();


            if (isAppend)
            {
                var append = new Append(host.Services);
                append.Run();
            }
            else
            {
                var init = new Initialize(host.Services);
                init.Run();
            }

        }

        public static void BuildConfig(IConfigurationBuilder builder, bool isProd  = false)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.josn", optional: true, reloadOnChange: true);
            
            if (isProd) 
                builder.AddJsonFile("appsettings.Production.josn", optional: true, reloadOnChange: true);

            builder.AddEnvironmentVariables().Build();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
                    Host.CreateDefaultBuilder(args)
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.AddConsole();
                            logging.SetMinimumLevel(LogLevel.Debug);
                        })
                        .ConfigureServices((_, services) =>
                                                                    services
                                                                    .AddTransient<CollectionProxy>()
                                                                    .AddLogging()
                                                              );
    }
}
