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
        private SymbolManager _symbolManager;
        private ILogger<Initialize> _logger;
        public Initialize(IServiceProvider services)
        {
            IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            
            _initializeManager = provider.GetRequiredService<InitializeManager>();
            _symbolManager = provider.GetRequiredService<SymbolManager>();
            _logger = provider.GetRequiredService<ILogger<Initialize>>();
        }

        public async Task Run()
        {
            var symbols = await _symbolManager.GetSymbolsAsync();
            var todayCutOffTime = await _symbolManager.GetTodaySchedule();
            foreach (var symbol in symbols.Where(s => s.ScanEnable && !s.Initialized))
            {
                bool result;

                _logger.LogInformation($"InsitutionHoldings Initialization For Symbol [{symbol.Ticker.ToUpper()}] Start ........ ");
                result = await _initializeManager.InitializeInsitutionHoldings(symbol);
                var failReason = new StringBuilder();

                if (!result)
                {
                    var reason = $"Fail To Intitalize Institution Holding for Symbol [{symbol.Ticker.ToUpper()}]";
                    failReason.Append(reason);
                    _logger.LogError(reason);

                }
                else
                    _logger.LogInformation($"InsitutionHoldings Initialization For Symbol [{symbol.Ticker.ToUpper()}] Succeed");

                result = await _initializeManager.InitializeMarketPrices(symbol);

                if (!result)
                { 
                    var reason = $"Fail To Intitalize MarketPrice for Symbol [{symbol.Ticker.ToUpper()}]";
                    failReason.Append(reason);
                    _logger.LogError(reason);
                }
                    
                else
                    _logger.LogInformation($"MarketPrice Initialization For Symbol [{symbol.Ticker.ToUpper()}] Succeed");


                result =  await _initializeManager.InitializeInsiderTransaction(symbol);

                if (!result)
                {
                    var reason = $"Fail To Intitalize InsiderTransaction for Symbol [{symbol.Ticker.ToUpper()}]";
                    failReason.Append(reason);
                    _logger.LogError(reason);
                }

                else
                    _logger.LogInformation($"InsiderTransaction Initialization For Symbol [{symbol.Ticker.ToUpper()}] Succeed");

                result = await _initializeManager.AddUpdateHistoryRecord(symbol, failReason.Length != 0 ? false : true, failReason.ToString(), todayCutOffTime);

                if (!result)
                {
                    _logger.LogError($"Fail To add Update History for Symbol [{symbol.Ticker.ToUpper()}]");
                }
                else { 
                    await _initializeManager.CompleteInitialization(symbol);
                }
                
            }


        }
    }
}
