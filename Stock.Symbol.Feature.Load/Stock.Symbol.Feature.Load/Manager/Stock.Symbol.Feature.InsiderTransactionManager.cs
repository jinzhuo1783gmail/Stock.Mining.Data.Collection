using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Stock.Symbol.External.Retrievor;
using Stock.Symbol.Feature.Load.Mapper;
using Stock.Symbol.Feature.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stock.Symbol.Feature.Load.ConfigModel;

namespace Stock.Symbol.Feature.Load.Manager
{
    
    public class InsiderTransactionManager
    {

        private readonly RapidApiRetrievor _rapidApiRetrievor;
        ILogger<InsiderTransactionManager> _logger;
        IConfiguration _configuration;
        ApiEndpointList _apiEndpointList;
        public InsiderTransactionManager(RapidApiRetrievor rapidApiRetrievor, ILogger<InsiderTransactionManager> logger, IConfiguration configuration, ApiEndpointList apiEndpointList)
        {
            _rapidApiRetrievor = rapidApiRetrievor;
            _logger = logger;
            _configuration = configuration;
            _apiEndpointList = apiEndpointList;
        }

        public async Task<IList<InsiderTransaction>> GetInsiderTransaction(string symbol)
        {


            try
            {
                var url = _apiEndpointList.ApiRapidYahooInsiderTransaction;
                if (string.IsNullOrEmpty(url))
                {
                    _logger.LogError($"No Api configured for ApiRapidYahooInsiderTransaction");
                    return null;
                }


                url = url.Replace("{{symbol}}", symbol);

                if (_rapidApiRetrievor.CreateRestAsync(url).GetAwaiter().GetResult() == false)
                {
                    _logger.LogError($"Unable to create rest client for symbol {symbol}!");
                    return null;
                }

                if (_rapidApiRetrievor.SetRequestAsync(RestSharp.Method.GET).GetAwaiter().GetResult() == null)
                {
                    _logger.LogError($"Unable to create rest request for symbol {symbol}!");
                    return null;
                }

                var insiderTransactionList = new List<InsiderTransaction>();


                var insiderTrans = await _rapidApiRetrievor.RetrieveAsync<JObject, JObject>(null);


                if (insiderTrans != null)
                {
                    var transListRaw = insiderTrans["insiderTransactions"]["transactions"] as JArray;

                    if (transListRaw == null)
                    {
                        _logger.LogError($"insider transaction list is empty from api for symbol {symbol}!");
                        return null;
                    }



                    foreach (var transRaw in transListRaw)
                    {
                        var error = "";
                        var trans = InsiderTransactionMapping.RapidJObjToModel(transRaw, out error);
                        if (!string.IsNullOrEmpty(error))
                        {
                            _logger.LogWarning(error);
                        }

                        insiderTransactionList.Add(trans);
                    }

                }

                return insiderTransactionList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Fail to get insider transaction:" + ex.Message + ex.InnerException ?? "");
                return null;
            }
            
        }

    }
}
