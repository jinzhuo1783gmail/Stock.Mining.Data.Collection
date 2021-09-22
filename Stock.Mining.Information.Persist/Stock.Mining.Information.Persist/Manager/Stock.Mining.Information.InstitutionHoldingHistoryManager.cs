using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Ef.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Collection.Manager
{
    public class InstitutionHoldingHistoryManager
    {

        InstitutionHoldingsHistoryRespository _institutionHoldingsHistoryRespository;
        ILogger<InstitutionHoldingHistoryManager> _logger;
        public InstitutionHoldingHistoryManager(InstitutionHoldingsHistoryRespository institutionHoldingsHistoryRespository, ILogger<InstitutionHoldingHistoryManager> logger)
        {
            _institutionHoldingsHistoryRespository = institutionHoldingsHistoryRespository;
            _logger = logger;
        }

        public async Task<IList<InstitutionHoldingsHistory>> GetInstitutionHoldingsHistoriesByInstitutionIdAsync(int InstitutionId)
        {

            try
            {
                return await _institutionHoldingsHistoryRespository.GetInstitutionHoldingsHistoriesByInstitutionIdAsync(InstitutionId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetInstitutionHoldingsHistoriesByInstitutionIdAsync failed: {ex.InnerException}");
                return null;
            }
            
            
        
        }

        public async Task<bool> AddInstitutionHoldingHistories(IList<InstitutionHoldingsHistory> InstitutionHoldingsHistories)
        {
            try
            {
                var result =  await _institutionHoldingsHistoryRespository.AddRangedInstitutionHoldingsHistoryAsync(InstitutionHoldingsHistories);
                if (!result || !_institutionHoldingsHistoryRespository.SaveContextAsync().GetAwaiter().GetResult())
                {
                    _logger.LogError($"Failed to insert histories total [{InstitutionHoldingsHistories.Count}]");
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AddInstitutionHoldingHistories failed: {ex.InnerException}");
                return false;
            }
            
            
        }

    }
}
