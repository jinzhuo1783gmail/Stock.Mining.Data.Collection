using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Symbol.Feature.Shared.Model;
using Stock.Symbol.Feature.Shared.Model.Util;

namespace Stock.Mining.Information.Proxy
{
    public class CollectionProxy
    {
        private RestSharpUtil _restSharpUtil;
        private ILogger<CollectionProxy> _logger;
        private IConfiguration _configuration;
        private readonly string _host;
        //private ApiSymbol _apiSymbol;

        public CollectionProxy(ILogger<CollectionProxy> logger, IConfiguration configuration, RestSharpUtil restSharpUtil )
        {
            _logger = logger;
            _configuration = configuration;
            _restSharpUtil = restSharpUtil;
            //_apiSymbol = apiSymbol.Value;

            _host = _configuration.GetSection("ApiCollection:Host").Value;

        }

        public async Task<IList<Stock.Mining.Information.Ef.Core.Entity.Symbol>> GetSymbols()
        {
            return _restSharpUtil.GetList<Stock.Mining.Information.Ef.Core.Entity.Symbol>(_host + _configuration.GetSection("ApiCollection:Symbol:GetAllSymbol").Value);
        }

        public async Task<Stock.Mining.Information.Ef.Core.Entity.Symbol> UpdateSymbol(Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        {
            return _restSharpUtil.Post<Stock.Mining.Information.Ef.Core.Entity.Symbol, Stock.Mining.Information.Ef.Core.Entity.Symbol>(_host + _configuration.GetSection("ApiCollection:Symbol:UpsertSymbol").Value, symbol);
        }


        public async Task<IList<InstitutionHolding>> GetInsititutionalHoldings(string ticker)
        {
            return _restSharpUtil.GetList<InstitutionHolding>(_host + _configuration.GetSection("ApiCollection:InstitutionHolding:GetInstitutionHolding").Value, new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("TickerName", ticker) });
        }


        public async Task<IList<InstitutionHolding>> UpsertInsititutionalHoldings(IList<InstitutionHolding> institutionHoldings)
        {
            return _restSharpUtil.Post<InstitutionHolding, InstitutionHolding>(_host + _configuration.GetSection("ApiCollection:InstitutionHolding:UpsertInstitutionHolding").Value, institutionHoldings);
        }

        public async Task<bool> InsertInsititutionalHoldingHistories(IList<InstitutionHoldingsHistory> institutionHoldingsHistory)
        {
            return _restSharpUtil.PostReturnNoResult<InstitutionHoldingsHistory, bool>(_host + _configuration.GetSection("ApiCollection:InstitutionHolding:AddInstitutionHoldingHistory").Value, institutionHoldingsHistory);
        }

        public async Task<bool> UpsertMarketPrice(IList<SymbolPrice> prices)
        {
            return _restSharpUtil.PostReturnNoResult<SymbolPrice, bool>(_host + _configuration.GetSection("ApiCollection:MarketPrice:UpertMarketPrice").Value, prices);
        }
    }
}
