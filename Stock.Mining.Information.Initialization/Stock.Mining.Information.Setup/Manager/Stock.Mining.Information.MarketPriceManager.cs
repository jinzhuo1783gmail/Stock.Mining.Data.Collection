using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Proxy;
using Stock.Symbol.Feature.Shared.Model;
using Stock.Symbol.Feature.Shared.Model.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Initialization.Manager
{
    public class MarketPriceManager
    {


        
        private RestSharpUtil _restSharpUtil;
        private ILogger<MarketPriceManager> _logger;
        private IConfiguration _configuration;
        private CollectionProxy _collectionProxy;
        private LoadProxy _loadProxy;
        public MarketPriceManager(ILogger<MarketPriceManager> logger, IConfiguration configuration, RestSharpUtil restSharpUtil, CollectionProxy collectionProxy, LoadProxy loadProxy)
        {
            _logger = logger;
            _configuration = configuration;
            _restSharpUtil = restSharpUtil;
            _collectionProxy = collectionProxy;
            _loadProxy = loadProxy;
        }

        
        public async Task<IList<StockPriceViewModel>> GetMarketPriceAsync(string ticker)
        { 
            return await _loadProxy.GetMarketPriceAsync(ticker, DateTime.MinValue, DateTime.MaxValue);
        }

        public async Task<IList<StockPriceViewModel>> GetMarketPriceAsync(string ticker, DateTime startDate, DateTime endDate)
        { 
            return await _loadProxy.GetMarketPriceAsync(ticker, startDate, endDate);
        }

        public async Task<bool> UpsertMarketPriceAsync(IList<SymbolPrice> prices)
        { 
            return await _collectionProxy.UpsertMarketPrice(prices);
        }

       
        public async Task<IList<SymbolPrice>> GetPersistMarketPriceAsync(string ticker)
        { 
            return await _collectionProxy.GetMarketPrice(ticker);
        }

    }
}
