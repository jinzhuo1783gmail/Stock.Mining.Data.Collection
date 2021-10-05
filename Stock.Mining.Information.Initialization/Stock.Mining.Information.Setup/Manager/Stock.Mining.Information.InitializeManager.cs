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
        private InsiderTransactionManager _insiderTransactionManager;
        public InitializeManager(ILogger<InitializeManager> logger, InstitutionManager institutionManager, MarketPriceManager marketPriceManager, InsiderTransactionManager insiderTransactionManager)
        { 
             _logger = logger;
            _institutionManager = institutionManager;
            _marketPriceManager = marketPriceManager;
            _insiderTransactionManager = insiderTransactionManager;
        }

        public async Task<bool> AnyInsitutionHoldings(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        { 
            var holdings = await _institutionManager.GetInstitutionHoldingsAsync(symbol.Ticker);

            return holdings != null && holdings.Any();
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

        public async Task<bool> AnyMarketPrices(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        { 
            var prices = await _marketPriceManager.GetPersistMarketPriceAsync(symbol.Ticker);

            return prices != null && prices.Any();
        }

        public async Task<bool> InitializeMarketPrices(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        {
            var priceList = await _marketPriceManager.GetMarketPriceAsync(symbol.Ticker);
            var priceEntity = priceList.Select(p => p.ToMarketPriceEntity(symbol.Id)).ToList();

            var result = await _marketPriceManager.UpsertMarketPriceAsync(priceEntity);
            

            if (!result)
            {
                _logger.LogError($"{symbol.Ticker} market price upload fail total {JsonConvert.SerializeObject(priceEntity)}");
                return false;
            }

            return true;
        }

        public async Task<bool> AnyInsiderHistories(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        { 
            var histories = await _insiderTransactionManager.GetInsiderHistories(symbol.Ticker);

            return histories != null && histories.Any();
        }

        public async Task<bool> InitializeInsiderTransaction(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        {
            var transactionList = await _insiderTransactionManager.GetInsiderTransactionHistoriesAsync(symbol.Ticker);
            var transactionEntity = transactionList.Select(i => i.ToInsiderHistoryEntity(symbol.Id)).ToList();

            var result = await _insiderTransactionManager.UpsertInsiderTransactionHistoriesAsync(transactionEntity);
            

            if (!result)
            {
                _logger.LogError($"{symbol.Ticker} insider histories upload fail total {JsonConvert.SerializeObject(transactionEntity)}");
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
