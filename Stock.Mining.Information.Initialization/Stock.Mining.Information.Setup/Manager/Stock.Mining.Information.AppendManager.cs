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
        
    public class AppendManager
    {
        private ILogger<AppendManager> _logger;
        private InstitutionManager _institutionManager;
        private MarketPriceManager _marketPriceManager;
        private InsiderTransactionManager _insiderTransactionManager;
        private SymbolManager _symbolManager;
        public AppendManager(ILogger<AppendManager> logger, SymbolManager symbolManager, InstitutionManager institutionManager, MarketPriceManager marketPriceManager, InsiderTransactionManager insiderTransactionManager)
        { 
             _logger = logger;
            _institutionManager = institutionManager;
            _marketPriceManager = marketPriceManager;
            _insiderTransactionManager = insiderTransactionManager;
            _symbolManager = symbolManager;
        }

        public async Task<bool> AppendInsitutionHoldingHistories(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol, DateTime? startDate = null, DateTime? endDate = null)
        {

            // get it from html webpage

            var holdings = await _institutionManager.GetInstitutionHoldingsAsync(symbol.Ticker);
            var histories = await _institutionManager.GetInstitutionHoldingHistoriesAsync(symbol.Ticker);

            startDate = startDate ?? DateTime.Now.Date;
            startDate = startDate.Value.AddDays(-1);
            endDate = endDate ?? startDate.Value.AddDays(1).AddSeconds(-1);

            startDate = startDate.Value.AddDays(-10);

            var filteredHistories = histories.Where(h => h.FileDate >= startDate && h.FileDate <= endDate).ToList();

            if (filteredHistories.Any())
            {
                var result = await _institutionManager.AppendInstitutionHoldingHsitories(filteredHistories, holdings, symbol);

                if (result)
                    _logger.LogInformation($"Append history successful for symbol [{symbol.Ticker}]");
                else
                    _logger.LogError($"Append hsitory failed for symbol [{symbol.Ticker}]");

                return result;
            }
                

            return true;
        
        }

        public async Task<bool> AppendMarketPrice(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol, DateTime? startDate = null, DateTime? endDate = null)
        {

            // get it from html webpage

            var priceList = await _marketPriceManager.GetMarketPriceAsync(symbol.Ticker, startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
            var priceEntity = priceList.Select(p => p.ToMarketPriceEntity(symbol.Id)).ToList();

            var result = await _marketPriceManager.UpsertMarketPriceAsync(priceEntity);
            

            if (!result)
            {
                _logger.LogError($"{symbol.Ticker} market price append fail total {JsonConvert.SerializeObject(priceList)}");
                return false;
            }

            return true;
        
        }

        public async Task<bool> AppendInsiderTransaction(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol,  DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate = startDate ?? DateTime.MinValue;
            endDate = endDate == null ? DateTime.MaxValue : endDate.Value.Date.AddDays(1).AddSeconds(-1);
            

            // get it from html webpage

            var transList = await _insiderTransactionManager.GetInsiderTransactionHistoriesAsync(symbol.Ticker);

            transList = transList.Where(tr => tr.TransactionDate >= startDate && tr.TransactionDate <= endDate).ToList();

            
            var transListEntity = transList.Select(t => t.ToInsiderHistoryEntity(symbol.Id)).ToList();

            var result = await _insiderTransactionManager.UpsertInsiderTransactionHistoriesAsync(transListEntity);

            if (!result)
            {
                _logger.LogError($"{symbol.Ticker} insider transaction append fail total {JsonConvert.SerializeObject(transListEntity)}");
                return false;
            }

            return true;
        
        }


        public async Task<bool> AddUpdateHistoryRecord(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol, bool isSuccess, string failReason, DateTime nextCutOffTime)
        { 
            return await _symbolManager.InsertUpdateHistoryAsync(symbol.Id, symbol.Ticker, isSuccess, failReason, nextCutOffTime);
        }

    }
}
