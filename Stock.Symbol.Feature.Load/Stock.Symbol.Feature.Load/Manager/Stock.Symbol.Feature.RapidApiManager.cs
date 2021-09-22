using Microsoft.Extensions.Logging;
using Stock.Symbol.Feature.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.Load.Manager
{
    public class RapidApiManager
    {

        RestSharpRepository _restSharpRepository;
        ILogger<RapidApiManager> _logger;

        public RapidApiManager(RestSharpRepository restSharpRepository, ILogger<RapidApiManager> logger)
        {
            _restSharpRepository = restSharpRepository;
            _logger = logger;
        }

        public async Task<IList<RestSharpAccessKey>> GetAllKeysAsync()
        {
            try
            {
                return await _restSharpRepository.GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            
        }

        public IList<RestSharpAccessKey> GetAllKeys()
        {
            try
            {
                return _restSharpRepository.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            
        }

        public async Task<RestSharpAccessKey> GetKeyByIdAsync(int Id)
        {

            try
            {
                return await _restSharpRepository.GetAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            
        }

        public async Task<RestSharpAccessKey> GetKeyByAccessKeyStringAsync(string Key)
        {

            try
            {
                return await _restSharpRepository.GetAsync(Key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            
        }


        public async Task<RestSharpAccessKey> IncreaseCountByIdAsync(int Id)
        {
            try
            {
                return await _restSharpRepository.IncreaseCountAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        
        }


        public async Task<RestSharpAccessKey> IncreaseCountByKeyAsync(string Key)
        {
            try
            {
                // set as default 0 , cound never flushed
                return await _restSharpRepository.IncreaseCountAsync(Key, default(int));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        
        }

    }
}
