using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Symbol.Feature.Shared.Model;
using Stock.Symbol.Feature.Shared.Model.Util;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Proxy
{
    public class LoadProxy
    {
        private RestSharpUtil _restSharpUtil;
        private ILogger<LoadProxy> _logger;
        private IConfiguration _configuration;
        private readonly string _host;
        public LoadProxy(ILogger<LoadProxy> logger, IConfiguration configuration, RestSharpUtil restSharpUtil )
        {
            _logger = logger;
            _configuration = configuration;
            _restSharpUtil = restSharpUtil;
            _host = _configuration.GetSection("ApiLoad:Host").Value;
        }

        public async Task<IList<InstitutionOwner>> GetInstitutionHoldingsAsync(string ticker)
        {
            return _restSharpUtil.GetList<InstitutionOwner>(_host + _configuration.GetSection("ApiLoad:Fintel:InstitutionsHoldings").Value, new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("symbol", ticker) });
        }


        public async Task<IList<InstitutionTransaction>> GetInstitutionHoldingHistoriesAsync(string ticker)
        {
            return _restSharpUtil.GetList<InstitutionTransaction>(_host + _configuration.GetSection("ApiLoad:Fintel:InstitutionsHoldingsHistory").Value, new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("symbol", ticker) });
        }

        public async Task<IList<InsiderTransaction>> GetInsiderTransactionHistoriesAsync(string ticker)
        {
            return _restSharpUtil.GetList<InsiderTransaction>(_host + _configuration.GetSection("ApiLoad:Rapid:InsiderTransaction").Value, new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("symbol", ticker) });
        }

        public async Task<DateTime> GetUpdateScheduleAsync()
        {
            var updateList = _restSharpUtil.GetList<DateTime>(_host + _configuration.GetSection("ApiLoad:Schedule:UpdateSchedule").Value);

            return updateList.FirstOrDefault();
        }

        public async Task<IList<StockPriceViewModel>> GetMarketPriceAsync(string ticker, DateTime startDate, DateTime endDate)
        {
            var requestParameter = new List<KeyValuePair<string, string>>();
            requestParameter.Add(new KeyValuePair<string, string>("symbol", ticker));
            requestParameter.Add(new KeyValuePair<string, string>("startDate", startDate.ToString("yyyy-MM-dd")));
            requestParameter.Add(new KeyValuePair<string, string>("endDate", endDate.ToString("yyyy-MM-dd")));

            return _restSharpUtil.GetList<StockPriceViewModel>(_host + _configuration.GetSection("ApiLoad:AlphaVantage:MarketPriceAdjustedDaily").Value, requestParameter);
        }
    }
}
