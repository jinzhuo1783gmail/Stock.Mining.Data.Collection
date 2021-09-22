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
        private InstitutionManager _institutionManager;
        private ILogger<Append> _logger;
        public Append(IServiceProvider services)
        {
            IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            _appendManager = provider.GetRequiredService<AppendManager>();
            _institutionManager = provider.GetRequiredService<InstitutionManager>();
            _logger = provider.GetRequiredService<ILogger<Append>>();
        }

        public async Task Run()
        {
            var symbols = await _institutionManager.GetSymbolsAsync();

            foreach (var symbol in symbols.Where(s => s.ScanEnable && s.Initialized))
            {
                var result = true;

                _logger.LogInformation($"InsitutionHoldings Appending For Symbol [{symbol.Ticker.ToUpper()}] Start ........ ");
                result =  await _appendManager.AppendInsitutionHoldingHistories(symbol);

                if (!result)
                    _logger.LogError($"Fail To Append Institution Holding for Symbol [{symbol.Ticker.ToUpper()}]");
                else
                    _logger.LogInformation($"InsitutionHoldings Appending For Symbol [{symbol.Ticker.ToUpper()}] Successful ");


                _logger.LogInformation($"Market Price Appending For Symbol [{symbol.Ticker.ToUpper()}]  Start ........  ");
                var startDate = DateTime.Now.AddDays(-7);
                var endDate = DateTime.Now.Date.AddDays(1);
                
                result =  await _appendManager.AppendMarketPrice(symbol, startDate, endDate);

                if (!result)
                    _logger.LogError($"Fail To Append Institution Holding for Symbol [{symbol.Ticker.ToUpper()}]");
                else
                    _logger.LogInformation($"InsitutionHoldings Appending For Symbol [{symbol.Ticker.ToUpper()}] Successful ");
            }


        }
    }
}
