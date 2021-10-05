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
    public class InsiderTransactionManager
    {
        private RestSharpUtil _restSharpUtil;
        private ILogger<InsiderTransactionManager> _logger;
        private IConfiguration _configuration;
        private CollectionProxy _collectionProxy;
        private LoadProxy _loadProxy;
        public InsiderTransactionManager(ILogger<InsiderTransactionManager> logger, IConfiguration configuration, RestSharpUtil restSharpUtil, CollectionProxy collectionProxy, LoadProxy loadProxy)
        {
            _logger = logger;
            _configuration = configuration;
            _restSharpUtil = restSharpUtil;
            _collectionProxy = collectionProxy;
            _loadProxy = loadProxy;
        }

        public async Task<IList<InsiderTransaction>> GetInsiderTransactionHistoriesAsync(string ticker)
        { 
            return await _loadProxy.GetInsiderTransactionHistoriesAsync(ticker);
        }

        public async Task<bool> UpsertInsiderTransactionHistoriesAsync(IList<InsiderHistory> histories)
        { 
            return await _collectionProxy.UpsertTransactionHistory(histories);
        }

        public async Task<IList<InsiderHistory>> GetInsiderHistories(string ticker)
        { 
            return await _collectionProxy.GetInsiderHistory(ticker);
        }

    }
}
