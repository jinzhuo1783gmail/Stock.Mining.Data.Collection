using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Proxy;
using Stock.Symbol.Feature.Shared.Model;
using Stock.Symbol.Feature.Shared.Model.Extension.Mapper;
using Stock.Symbol.Feature.Shared.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Initialization.Manager
{
    public class InitializeManager
    {

        private ILogger<InitializeManager> _logger;
        private InstitutionManager _institutionManager;
        private MarketPriceManager _marketPriceManager;
        public InitializeManager(ILogger<InitializeManager> logger, InstitutionManager institutionManager, MarketPriceManager marketPriceManager)
        { 
             _logger = logger;
            _institutionManager = institutionManager;
            _marketPriceManager = marketPriceManager;
        }

        public async Task<bool> InitializeInsitutionHoldings(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        { 
            // holdings 
            var owners = await _institutionManager.GetInstitutionOwnersAsync(symbol.Ticker);
            var holdingsEntity = owners.Select(h => h.ToInstitutionHoldingEntity(symbol.Id)).ToList();
            var result = await _institutionManager.UpsertInstitutionHoldingsAsync(holdingsEntity);

            if (result?.Any() ?? false)
                _logger.LogInformation($"{symbol.Ticker} institution holding upload successfully total {holdingsEntity.Count()}");
            else
            {
                _logger.LogError($"{symbol.Ticker} institution holding upload fail total {JsonConvert.SerializeObject(owners)}");
                return false;
            }
            var holdings = await _institutionManager.GetInstitutionHoldingsAsync(symbol.Ticker);        
            var histories = await _institutionManager.GetInstitutionHoldingHistoriesAsync(symbol.Ticker);

            // must map institution so loop through 
            if (!_institutionManager.AppendInstitutionHoldingHsitories(histories, holdings, symbol).GetAwaiter().GetResult()) 
            {
                _logger.LogError($"failed to add insitution histories for symbol [{symbol.Ticker}]");
                return false;
            }

           

            return true;



        }

        public async Task<bool> InitializeMarketPrices(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        {
            var priceList = await _marketPriceManager.GetMarketPriceAsync(symbol.Ticker);
            var priceEntity = priceList.Select(p => p.ToMarketPriceEntity(symbol.Id)).ToList();

            var result = await _marketPriceManager.UpsertMarketPriceAsync(priceEntity);
            

            if (!result)
            {
                _logger.LogError($"{symbol.Ticker} market price upload fail total {JsonConvert.SerializeObject(priceList)}");
                return false;
            }

            return true;
        }

        public async Task<bool> CompleteInitialization(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        { 
             symbol.Initialized = true;

            var finalizedSymbol = await _institutionManager.UpdateSymbolAsync(symbol);
            if (finalizedSymbol == default(Stock.Mining.Information.Ef.Core.Entity.Symbol))
            { 
                _logger.LogError($"failed to finalize symbol [{symbol.Ticker}]");
                return false;
            }

            return true;
        }
    }
}
