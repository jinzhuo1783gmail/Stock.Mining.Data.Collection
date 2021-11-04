using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Initialization.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Initialization
{
    public class Append
    {
        private AppendManager _appendManager;
        private InitializeManager _initializeManager;
        private InstitutionManager _institutionManager;
         private SymbolManager _symbolManager;
        private ILogger<Append> _logger;
        
        public Append(IServiceProvider services)
        {
            IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            _appendManager = provider.GetRequiredService<AppendManager>();
            _initializeManager = provider.GetRequiredService<InitializeManager>();
            _institutionManager = provider.GetRequiredService<InstitutionManager>();
            _symbolManager = provider.GetRequiredService<SymbolManager>();
            _logger = provider.GetRequiredService<ILogger<Append>>();
        }

        public async Task Run()
        {
            var symbols = await _symbolManager.GetSymbolsAsync();

            foreach (var symbol in symbols.Where(s => s.ScanEnable && s.Initialized))
            {
                var result = true;
                var todayCutOffTime = await _symbolManager.GetTodaySchedule();
                var nextRunTime = await _symbolManager.GetNextUpdateTimeBySymbolAsync(symbol);

                // not time yet ,just skip
                if (DateTime.Now <= nextRunTime) continue;
                    
                
                var failReason = new StringBuilder();

                _logger.LogInformation($"InsitutionHoldings Appending For Symbol [{symbol.Ticker.ToUpper()}] Start ........ ");

                if (!_initializeManager.AnyInsitutionHoldings(symbol).GetAwaiter().GetResult())
                {
                    _logger.LogInformation($"No InsitutionHoldings found in the DB switch to Intialization For Symbol [{symbol.Ticker.ToUpper()}] ");
                    result = await _initializeManager.InitializeInsitutionHoldings(symbol);
                }
                else 
                { 
                    result =  await _appendManager.AppendInsitutionHoldingHistories(symbol);
                }
                
                if (!result)
                {
                    var reason = $"Fail To Append Institution Holding for Symbol [{symbol.Ticker.ToUpper()}]";
                    failReason.Append(reason);
                    _logger.LogError(reason);
                }
                else
                    _logger.LogInformation($"InsitutionHoldings Appending For Symbol [{symbol.Ticker.ToUpper()}] Successful ");

                
                _logger.LogInformation($"Market Price Appending For Symbol [{symbol.Ticker.ToUpper()}]  Start ........  ");

                 var startDate = DateTime.Now.AddDays(-7);
                 var endDate = DateTime.Now.Date.AddDays(1);

                if (!_initializeManager.AnyMarketPrices(symbol).GetAwaiter().GetResult())
                {
                    _logger.LogInformation($"No MarketPrice found in the DB switch to Intialization For Symbol [{symbol.Ticker.ToUpper()}] ");
                    result = await _initializeManager.InitializeMarketPrices(symbol);
                }
                else 
                { 
                    result =  await _appendManager.AppendMarketPrice(symbol, startDate, endDate);
                }
                

                if (!result)
                {
                    var reason = $"Fail To Append MarketPrice Holding for Symbol [{symbol.Ticker.ToUpper()}]";
                    failReason.Append(reason);
                    _logger.LogError(reason);

                }
                else
                    _logger.LogInformation($"MarketPrice Appending For Symbol [{symbol.Ticker.ToUpper()}] Successful ");



                _logger.LogInformation($"Insider Transaction Appending For Symbol [{symbol.Ticker.ToUpper()}]  Start ........  ");


                if (!_initializeManager.AnyInsiderHistories(symbol).GetAwaiter().GetResult())
                {
                    _logger.LogInformation($"No InsiderHistories found in the DB switch to Intialization For Symbol [{symbol.Ticker.ToUpper()}] ");
                    result = await _initializeManager.InitializeInsiderTransaction(symbol);
                }
                else 
                { 
                    result =  await _appendManager.AppendInsiderTransaction(symbol, startDate, endDate);
                }
                

                if (!result)
                {
                    var reason = $"Fail To Append insider transaction for Symbol [{symbol.Ticker.ToUpper()}]";
                    failReason.Append(reason);
                    _logger.LogError(reason);
                }
                else
                    _logger.LogInformation($"insider transaction Appending For Symbol [{symbol.Ticker.ToUpper()}] Successful ");
            
                result = await _appendManager.AddUpdateHistoryRecord(symbol, failReason.Length != 0 ? false : true, failReason.ToString(), todayCutOffTime);

                if (!result)
                {
                    _logger.LogError($"Fail To add Update History for Symbol [{symbol.Ticker.ToUpper()}]");
                }
                
            }



        }
    }
}
