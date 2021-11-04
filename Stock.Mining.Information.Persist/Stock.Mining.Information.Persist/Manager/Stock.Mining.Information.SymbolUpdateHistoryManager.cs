using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Ef.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Persist.Manager
{
    public class SymbolUpdateHistoryManager
    {
        private SymbolUpdateHistoryRepository _symbolUpdateHistoryRepository;
        private ILogger<SymbolUpdateHistoryManager> _logger;
        public SymbolUpdateHistoryManager(SymbolUpdateHistoryRepository symbolUpdateHistoryRepository, ILogger<SymbolUpdateHistoryManager> logger)
        {
            _symbolUpdateHistoryRepository = symbolUpdateHistoryRepository;
            _logger = logger;
        }

        public async Task<IList<SymbolUpdateHistory>> GetAllUpdates (string ticker)
        {
            try
            {
                 return await _symbolUpdateHistoryRepository.GetAllUpdates(ticker);
            }
            catch (Exception ex)
            { 
                _logger.LogError($"GetAllUpdates failed: {ex.InnerException}");
                return null;
            }
        }

        public async Task<SymbolUpdateHistory> GetLastUpdate (string ticker)
        {
            try
            {
                 return await _symbolUpdateHistoryRepository.GetLastUpdate(ticker);
            }
            catch (Exception ex)
            { 
                _logger.LogError($"GetLastUpdate failed: {ex.InnerException}");
                return null;
            }
        }

        public async Task<bool> AddUpdateHistories (IList<SymbolUpdateHistory> updateHistories)
        {
            try
            {
                var result = true;
                foreach (var history in updateHistories)
                { 
                    result &= await _symbolUpdateHistoryRepository.AddHistoryAsync(history);
                }
                
                return await _symbolUpdateHistoryRepository.SaveContextAsync();
            }
            catch (Exception ex)
            { 
                _logger.LogError($"AddUpdateHistory failed: {ex.InnerException}");
                return false;
            }
        }

        public async Task<bool> UpdateUpdateHistories (IList<SymbolUpdateHistory> updateHistories)
        {
            try
            {
                var result = true;
                foreach (var history in updateHistories)
                { 
                    result &= await _symbolUpdateHistoryRepository.UpdateHistoryAsync(history);
                }
                return await _symbolUpdateHistoryRepository.SaveContextAsync();

            }
            catch (Exception ex)
            { 
                _logger.LogError($"UpdateUpdateHistory failed: {ex.InnerException}");
                return false;
            }
        }
    }
}
