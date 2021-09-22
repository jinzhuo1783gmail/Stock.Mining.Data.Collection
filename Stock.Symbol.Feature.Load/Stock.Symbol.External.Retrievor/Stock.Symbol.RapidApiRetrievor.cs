using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using Stock.Symbol.External.Retrievor.Manager;
using Stock.Symbol.Feature.EF.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Symbol.External.Retrievor
{
    public class RapidApiRetrievor : IExternalRetrievor
    {


        private static IRestClient _restClient;
        private static RestRequest _request;
        private ILogger<RapidApiRetrievor> _logger;
        private RapidApiTokenManager _rapidApiTokenManager;
        private RestSharpRepository _restSharpRepository;
        
        private string _key;

        public RapidApiRetrievor(ILogger<RapidApiRetrievor> logger, RapidApiTokenManager rapidApiTokenManager, RestSharpRepository restSharpRepository)
        {
            _logger = logger;
            _rapidApiTokenManager = rapidApiTokenManager;
            _restSharpRepository = restSharpRepository;
        }
        
        public async Task<bool> CreateRestAsync(string url, int timeout = 30)
        {

            if (string.IsNullOrEmpty(url))
            {
                _logger.LogError($"Url for request cannot be empty [{url ?? ""}]");
                return false;
            }

            _restClient = new RestClient(url);
            _restClient.Timeout = timeout * 1000;
            return true;
        }

        public async Task<TO> RetrieveAsync<TI, TO>(TI Input)
        {
            try { 
                
                var restKey = _rapidApiTokenManager.IncreaseAvailableTokenAsync(_key);
                if (restKey == null)
                     _logger.LogError($"Key {_key} failed to be update");


                IRestResponse response = await _restClient.ExecuteAsync(_request);

                if (response.IsSuccessful)
                    return JsonConvert.DeserializeObject<TO>(response.Content);
                else { 

                    _logger.LogError(response.ErrorMessage);
                    return default(TO);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(TO);
            }
        }

        public async Task<RestRequest> SetRequestAsync(Method method, RestRequest request = null )
        {

            try
            {
                _request = request ?? new RestRequest();
                _request.Method = method;

                _key = _rapidApiTokenManager.GetAvailableToken();

                if (string.IsNullOrEmpty(_key))
                {
                    _logger.LogError($"Failed to obtain access token");
                    return null;
                }

                var host = _restClient.BaseUrl.Host;

                _request.AddHeader("x-rapidapi-key", _key);
                _request.AddHeader("x-rapidapi-host", host);

                return _request;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            
        }
    }
}
