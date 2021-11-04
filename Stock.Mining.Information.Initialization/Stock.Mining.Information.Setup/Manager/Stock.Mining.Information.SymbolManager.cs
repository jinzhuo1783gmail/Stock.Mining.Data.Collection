using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Proxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Initialization.Manager
{
    public class SymbolManager
    {
        private ILogger<SymbolManager> _logger;
        private IConfiguration _configuration;
        private CollectionProxy _collectionProxy;
        private LoadProxy _loadProxy;

        public SymbolManager(ILogger<SymbolManager> logger, IConfiguration configuration, CollectionProxy collectionProxy, LoadProxy loadProxy)
        {
            _logger = logger;
            _configuration = configuration;
            _collectionProxy = collectionProxy;
            _loadProxy = loadProxy;
        }

         public async Task<IList<Stock.Mining.Information.Ef.Core.Entity.Symbol>> GetSymbolsAsync()
        { 
            return await _collectionProxy.GetSymbols();
        }

        public async Task<Stock.Mining.Information.Ef.Core.Entity.Symbol> UpdateSymbolAsync(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        { 
            return await _collectionProxy.UpdateSymbol(symbol);
        }

        public async Task<DateTime> GetNextUpdateTimeBySymbolAsync(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        { 
            return await _collectionProxy.GetNextUpdateTime(symbol.Ticker);
        }

        public async Task<bool> InsertUpdateHistoryAsync(long symbolId, string ticker, bool isSuccess,  string failReason , DateTime nextCutOffDatetime)
        {
            nextCutOffDatetime = nextCutOffDatetime.AddDays(1);
            while (nextCutOffDatetime.DayOfWeek == DayOfWeek.Saturday || nextCutOffDatetime.DayOfWeek == DayOfWeek.Sunday)
            {
                nextCutOffDatetime = nextCutOffDatetime.AddDays(1);

            }


            var history = new SymbolUpdateHistory()
            {
                SymbolId = symbolId,
                Ticker = ticker,
                UpdateTime = DateTime.Now,
                IsSuccess = isSuccess,
                FailReason = failReason,
                NextUpdateTime = nextCutOffDatetime
            };
            
            return await _collectionProxy.InsertUpdateHistories(new List<SymbolUpdateHistory>() { history });
        }

        public async Task<DateTime> GetTodaySchedule()
        {
            return await _loadProxy.GetUpdateScheduleAsync();

        }
        
    }
}
