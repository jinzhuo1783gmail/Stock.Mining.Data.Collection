using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Symbol.External.Retrievor
{
    public class AlphaVantageApiRetrievor　: IExternalRetrievor
    {
        private static IRestClient _restClient;
        private static RestRequest _request;
        private ILogger<AlphaVantageApiRetrievor> _logger;

        public AlphaVantageApiRetrievor(ILogger<AlphaVantageApiRetrievor> logger)
        {
            _logger = logger;
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

                var host = _restClient.BaseUrl.Host;

 

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
