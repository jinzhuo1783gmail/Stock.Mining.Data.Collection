using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.Shared.Model.Util
{
    public class RestSharpUtil
    {
        private static IRestClient _restClient ;
        private ILogger<RestSharpUtil> _logger;
        private IConfiguration _configuration;

        public RestSharpUtil(ILogger<RestSharpUtil> logger, IConfiguration configuration, IRestClient restClient)
        {
            _logger = logger;
            _configuration = configuration;
            _restClient = restClient;
        }

        public IList<TO> GetList<TO>(string url, List<KeyValuePair<string, string>> param = null)
        {
            try
            {
                _restClient.BaseUrl = new Uri(url);

                var request = new RestRequest();

                if (param != null)
                {
                    param.ForEach(p => request.AddParameter(p.Key.ToString(), p.Value.ToString()));
                }

                request.Method = Method.GET;

                IRestResponse response = _restClient.Execute(request);

                if (response.IsSuccessful)
                {
                    var returnObj = JsonConvert.DeserializeObject<IList<TO>>(response.Content);

                    if (returnObj == default(IList<TO>))
                        _logger.LogError($"StatusCode: {response.StatusCode} [{url}] Response: {response.Content.ToString()}");

                    return returnObj;
                }
                    
                else
                {
                    _logger.LogError($"StatusCode: {response.StatusCode} [{url}] Response: {response.ErrorMessage}");
                    return default(IList<TO>);
                }
            }
            catch (Exception ex)
            { 
                _logger.LogError($"Api Fail url {url} message {ex.Message}");
                return default(IList<TO>);
            }
        }

        public IList<TO> Post<TI,TO>(string url, IList<TI> body)
        {
                
            try
            {
                _restClient = new RestClient(url);
            
                var _request = new RestRequest();
                _request.Method = Method.POST;

                _request.AddHeader("Content-Type", "application/json");

                _request.AddParameter("application/json", JArray.FromObject(body), ParameterType.RequestBody);

                var response = _restClient.Execute<IList<TO>>(_request);

                if (response.IsSuccessful)
                { 
                    var returnObj = JsonConvert.DeserializeObject<IList<TO>>(response.Content);

                    if (returnObj == default(IList<TO>))
                        _logger.LogError($"StatusCode: {response.StatusCode} [{url}] Response: {response.Content}");

                    return returnObj;
                
                
                }
                    
                else
                {
                     _logger.LogError($"StatusCode: {response.StatusCode} [{url}] Response: {response.ErrorMessage}");
                    return default(IList<TO>);
                }
                    

            }
            catch (Exception ex)
            {
                _logger.LogError($"Api Fail url {url} message {ex.Message}");
                return default(IList<TO>);
            }
        }

        public TO PostReturnNoResult<TI,TO>(string url, IList<TI> body)
        {
            try
            {
                _restClient = new RestClient(url);
            
                var _request = new RestRequest();
                _request.Method = Method.POST;

                _request.AddHeader("Content-Type", "application/json");

                _request.AddParameter("application/json", JArray.FromObject(body), ParameterType.RequestBody);

                var response = _restClient.Execute<TO>(_request);

                if (response.IsSuccessful)
                { 
                    var returnObj = JsonConvert.DeserializeObject<TO>(response.Content);

                    if (returnObj == null)
                        _logger.LogError($"StatusCode: {response.StatusCode} [{url}] Response: {response.Content}");

                    return returnObj;
                
                }
                else
                {
                     _logger.LogError($"StatusCode: {response.StatusCode} [{url}] Response: {response.ErrorMessage}");
                    return default(TO);
                }
                    

            }
            catch (Exception ex)
            {
                _logger.LogError($"Api Fail url {url} message {ex.Message}");
                return default(TO);
            }
        }



        public TO Post<TI,TO>(string url, TI body)
        {
            try
            {   
                _restClient = new RestClient(url);
            
                var _request = new RestRequest();
                _request.Method = Method.POST;

                _request.AddHeader("Content-Type", "application/json");

                _request.AddParameter("application/json", JObject.FromObject(body), ParameterType.RequestBody);

            
                var response = _restClient.Execute<TO>(_request);

                if (response.IsSuccessful)
                {
                    var returnObj = JsonConvert.DeserializeObject<TO>(response.Content);

                    if (returnObj == null)
                        _logger.LogError($"StatusCode: {response.StatusCode} [{url}] Response: {response.Content}");

                    return returnObj;
                }
                    
                else
                {
                     _logger.LogError($"StatusCode: {response.StatusCode} [{url}] Response: {response.ErrorMessage}");
                    return default(TO);
                }
                    

            }
            catch (Exception ex)
            {
                _logger.LogError($"Api Fail url {url} message {ex.Message}");
                return default(TO);
            }
        }
    }


}
