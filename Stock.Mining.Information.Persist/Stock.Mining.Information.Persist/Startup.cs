using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Repository;
using Stock.Mining.Information.Ef.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stock.Mining.Information.Collection.Manager;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Stock.Mining.Information.Persist.Manager;
using Serilog;
using Serilog.Formatting.Compact;

namespace Stock.Mining.Information.Collection
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
            
            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(Configuration)
                        .CreateLogger();

            Log.Logger.Information("Stock Mining Collection Service Start ........ ");


            services.AddControllers()
                //.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
                .AddNewtonsoftJson(options =>
                    {
                     // Return JSON responses in LowerCase?
                     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                     // Resolve Looping navigation properties
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 });


            services.AddDbContext<InformationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbString"), optBuilder => optBuilder.MigrationsAssembly("Stock.Mining.Information.Collection")));
            services.AddTransient<SymbolRepository>();
            services.AddTransient<InstitutionHoldingRespository>();
            services.AddTransient<InstitutionHoldingsHistoryRespository>();
            services.AddTransient<SymbolPriceRepository>();
            services.AddTransient<InsiderHistoryRepository>();
            

            services.AddTransient<SymbolManager>();
            services.AddTransient<InstitutionHoldingManager>();
            services.AddTransient<InstitutionHoldingHistoryManager>();
            services.AddTransient<SymbolPriceManager>();
            services.AddTransient<InsiderHistoryManager>();
            
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
