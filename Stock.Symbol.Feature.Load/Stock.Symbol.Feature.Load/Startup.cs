using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Stock.Symbol.Feature.EF.Core;
using Stock.Symbol.Feature.Load.Manager;
using Stock.Symbol.External.Retrievor.Manager;
using Stock.Symbol.External.Retrievor;
using Stock.Symbol.Feature.Load.Mapper;
using Serilog;

namespace Stock.Symbol.Feature.Load
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // allow from any source atm
                  // allow from any source atm
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(3600)));

            });

            services.AddMvc()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            
            services.AddDbContext<RSDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbString"), optBuilder => optBuilder.MigrationsAssembly("Stock.Symbol.Feature.Load")));
            

            services.AddTransient<RestSharpRepository>();
            services.AddTransient<AlphaVantageRepository>();

            services.AddTransient<RapidApiManager>();
            services.AddTransient<RapidApiTokenManager>();
            services.AddTransient<InstitutionOwnerManager>();
            services.AddTransient<DailyPriceManager>();
            services.AddTransient<InsiderTransactionManager>();


            services.AddTransient<RapidApiRetrievor>();
            services.AddTransient<HtmlTableRetrievor>();
            services.AddTransient<AlphaVantageApiRetrievor>();
            
            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(Configuration)
                        .CreateLogger();

            Log.Logger.Information("Stock Symbol Feature Load Service Start ........ ");

            


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
