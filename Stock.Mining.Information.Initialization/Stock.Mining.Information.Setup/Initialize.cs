using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Stock.Mining.Information.Initialization.Manager;
using Stock.Mining.Information.Proxy;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Symbol.Feature.Shared.Model;

namespace Stock.Mining.Information.Setup
{
    public class Initialize
    {

        private InitializeManager _initializeManager;
        private InstitutionManager _institutionManager;
        private ILogger<Initialize> _logger;
        public Initialize(IServiceProvider services)
        {
            IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            _initializeManager = provider.GetRequiredService<InitializeManager>();
            _institutionManager = provider.GetRequiredService<InstitutionManager>();
            _logger = provider.GetRequiredService<ILogger<Initialize>>();
        }

        public async Task Run()
        {
            var symbols = await _institutionManager.GetSymbolsAsync();

            foreach (var symbol in symbols.Where(s => s.ScanEnable && !s.Initialized))
            {
                bool result;

                _logger.LogInformation($"InsitutionHoldings Initialization For Symbol [{symbol.Ticker.ToUpper()}] Start ........ ");
                result = await _initializeManager.InitializeInsitutionHoldings(symbol);

                if (!result)
                    _logger.LogError($"Fail To Intitalize Institution Holding for Symbol [{symbol.Ticker.ToUpper()}]");
                else
                    _logger.LogInformation($"InsitutionHoldings Initialization For Symbol [{symbol.Ticker.ToUpper()}] Succeed");

                result =  await _initializeManager.InitializeMarketPrices(symbol);

                if (!result)
                    _logger.LogError($"Fail To Intitalize MarketPrice for Symbol [{symbol.Ticker.ToUpper()}]");
                else
                    _logger.LogInformation($"MarketPrice Initialization For Symbol [{symbol.Ticker.ToUpper()}] Succeed");

                _initializeManager.CompleteInitialization(symbol);
            }


        }
    }
}
