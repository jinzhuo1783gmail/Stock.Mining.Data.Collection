using Microsoft.Extensions.Logging;
using Stock.Symbol.Feature.EF.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Stock.Symbol.External.Retrievor.Manager
{
    public class RapidApiTokenManager
    {
        public RestSharpRepository _restSharpRepository;
        public ILogger<RapidApiTokenManager> _logger;
        public IConfiguration _configuration;
        private int _threshold;
        private int _resetDay = 1;

        public RapidApiTokenManager(RestSharpRepository restSharpRepository, ILogger<RapidApiTokenManager> logger, IConfiguration configuration)
        { 
            _restSharpRepository = restSharpRepository;
            _logger = logger;
            _configuration = configuration;
            if (!int.TryParse(_configuration["RestSharp:Threshold"], out _threshold))
            {
                _logger.LogWarning("Config section of [RestSharp:Threshold] muset been int, threshold reset to be 500 as default");
                _threshold = 500;
            }

        }

        public string GetAvailableToken()
        {
            try
            {
                var allKeys = _restSharpRepository.Get();
                var avalibaleKey = allKeys.Where(k => k.MonthlyCount < _threshold).OrderBy(k => k.MonthlyCount).FirstOrDefault();

                if (avalibaleKey == null)
                { 
                    _logger.LogError($"No Access Token to use in the DB , all of them reach the threshold of {_threshold}");
                    return "";
                }

                return avalibaleKey.AccessKey;

            }
            catch (Exception ex)
            {
                return "";
            }
        }



        public async Task<RestSharpAccessKey> IncreaseAvailableTokenAsync(string key)
        {
            if (!int.TryParse(_configuration["RestSharp:RefreshDate"], out _resetDay))
            {
                _logger.LogWarning("Config section of [RestSharp:RefreshDate] muset been int, RefreshDate reset to 0 by default");
                _threshold = 500;
            }
            return await _restSharpRepository.IncreaseCountAsync(key, _resetDay);
        }


    }
}
