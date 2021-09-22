using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Ef.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Collection.Manager
{
    public class InstitutionHoldingManager
    {
        InstitutionHoldingRespository _institutionHoldingRespository;
        ILogger<InstitutionHoldingRespository> _logger;
        public InstitutionHoldingManager(InstitutionHoldingRespository institutionHoldingRespository, ILogger<InstitutionHoldingRespository> logger)
        {
            _institutionHoldingRespository = institutionHoldingRespository;
            _logger = logger;
        }

        public async Task<IList<InstitutionHolding>> GetInstitutionHoldings(string Ticker, bool IncludeAllSymbolAndHistory = false)
        {
            try
            {
                 return await _institutionHoldingRespository.GetInstitutionHoldingsAsync(Ticker, IncludeAllSymbolAndHistory);
            }
            catch (Exception ex)
            { 
                _logger.LogError($"GetInstitutionHoldings failed: {ex.InnerException}");
                return null;
            }
        }

        public async Task<InstitutionHolding> AddOrUpdateInstitutionHolding(InstitutionHolding InstitutionHolding)
        {
            try { 
                if (InstitutionHolding.Id == default(int) || _institutionHoldingRespository.GetInstitutionHoldingAsync(InstitutionHolding.Id).GetAwaiter().GetResult() == null)
                {
                    return await _institutionHoldingRespository.AddNewInstitutionHoldingAsync(InstitutionHolding);
                }
                else { 
                    return await _institutionHoldingRespository.UpdateInstitutionHoldingAsync(InstitutionHolding);
                }
            }
            catch (Exception ex)
            { 
                _logger.LogError($"AddOrUpdateInstitutionHolding failed: {ex.InnerException}");
                return null;
            }
        }

        public async Task<IList<InstitutionHolding>> AddOrUpdateInstitutionHoldings(IList<InstitutionHolding> InstitutionHoldings)
        {
            try {
                var returnList = new List<InstitutionHolding>();

                
                foreach (var InstitutionHolding in InstitutionHoldings)
                {
                    if (InstitutionHolding.Id == default(long))
                    {
                        returnList.Add(await _institutionHoldingRespository.AddNewInstitutionHoldingAsync(InstitutionHolding));
                    }
                    else 
                    {
                        returnList.Add(await _institutionHoldingRespository.UpdateInstitutionHoldingAsync(InstitutionHolding));
                    }
                }

                if (!await _institutionHoldingRespository.SaveContextAsync())
                { 
                    return null;
                }

                return returnList;
            }
            catch (Exception ex)
            { 
                _logger.LogError($"AddOrUpdateInstitutionHolding failed: {ex.InnerException}");
                return null;
            }
        }


    }
}
