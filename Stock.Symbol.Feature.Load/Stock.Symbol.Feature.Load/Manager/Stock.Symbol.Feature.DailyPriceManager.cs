using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stock.Symbol.External.Retrievor;
using Stock.Symbol.Feature.EF.Core;
using Stock.Symbol.Feature.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.Load.Manager
{
    public class DailyPriceManager
    {
        private readonly AlphaVantageApiRetrievor _alphaVantageApiRetrievor;
        private ILogger<AlphaVantageApiRetrievor> _logger;
        private IConfiguration _configuration;
        private AlphaVantageRepository _alphaVantageRepository;
        public DailyPriceManager(AlphaVantageApiRetrievor alphaVantageApiRetrievor, HtmlTableRetrievor htmlTableRetrievor, ILogger<AlphaVantageApiRetrievor> logger, IConfiguration configuration, AlphaVantageRepository alphaVantageRepository)
        {
            _alphaVantageApiRetrievor = alphaVantageApiRetrievor;
            _logger = logger;
            _configuration = configuration;
            _alphaVantageRepository = alphaVantageRepository;
        }

        public async Task<IList<StockPrice>> GetStockDailyPrice(string ticker, DateTime startDate, DateTime endDate)
        {

            try 
            { 
                var accessKey = _alphaVantageRepository.Get().FirstOrDefault();

                var url = _configuration["ApiEndpointList:ApiAlphaVantageHistoricalPrice"];
                if (string.IsNullOrEmpty(url))
                {
                    _logger.LogError($"No Api configured for ApiRapidYahooInstitutionalHolders");
                    return null;
                }

                if (string.IsNullOrEmpty(accessKey?.AccessKey))
                {
                    _logger.LogError($"No Alpha Vantage Api access key set up in the DB");
                    return null;
                }

                url = url.Replace("{{symbol}}", ticker).Replace("{{apikey}}", accessKey.AccessKey);


                if (_alphaVantageApiRetrievor.CreateRestAsync(url).GetAwaiter().GetResult() == false)
                {
                    _logger.LogError($"Unable to create alpha vantage rest client for symbol {ticker}!");
                    return null;
                }

                if (_alphaVantageApiRetrievor.SetRequestAsync(RestSharp.Method.GET).GetAwaiter().GetResult() == null)
                {
                    _logger.LogError($"Unable to create alpha vantage  rest request for symbol {ticker}!");
                    return null;
                }

             
                var allPrices = await _alphaVantageApiRetrievor.RetrieveAsync<JObject, JObject>(null);

                if (allPrices == null) 
                {
                    _logger.LogError($"fail to get market price for symbol {ticker}. please check VPN");
                    return null;
                }

                var priceListDict = JsonConvert.DeserializeObject<Dictionary<DateTime, StockPrice>>(allPrices["Time Series (Daily)"].ToString());

                var priceList = priceListDict.Select(p =>
                        { 
                            p.Value.PriceDate = p.Key;
                            p.Value.Symbol = ticker;
                
                            return p.Value; });


                return priceList.Where(p => p.PriceDate >= startDate && p.PriceDate <= endDate).ToList();
            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fail to get daily price from alphavantange symbol {ticker}");

                return null;
            }
            
            

        }


    }
}
