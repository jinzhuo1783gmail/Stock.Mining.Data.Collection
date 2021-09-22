using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Ef.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Ef.Core.Repository
{
    public class InstitutionHoldingsHistoryRespository : MiningRespository, IMiningRespository
    {
        //public InformationDBContext _context;
        public ILogger<InstitutionHoldingsHistoryRespository> _logger;

        public InstitutionHoldingsHistoryRespository(InformationDBContext context, ILogger<InstitutionHoldingsHistoryRespository> logger) 
        {
            _context = context;
            _logger = logger;

        }

        public async Task<IList<InstitutionHoldingsHistory>> GetInstitutionHoldingsHistoriesByInstitutionIdAsync(long InstitutionId)
        { 
            return await _context.InstitutionHoldingsHistory.Where(ih => ih.InstitutionId == InstitutionId).ToListAsync();
        
        }

        public async Task<InstitutionHoldingsHistory> GetInstitutionHoldingsHistoryByIdAsync(long InstitutionHoldingsHistoryId)
        { 
            return await _context.InstitutionHoldingsHistory.FirstOrDefaultAsync(ih => ih.Id == InstitutionHoldingsHistoryId);
        }

        public async Task<bool> AddRangedInstitutionHoldingsHistoryAsync(IList<InstitutionHoldingsHistory> InstitutionHoldingsHistories)
        {

            try
            {
                await _context.InstitutionHoldingsHistory.AddRangeAsync(InstitutionHoldingsHistories);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Institution History add failed Reason " + ex.InnerException);
                return false;
            }
        }

        public async Task<IList<InstitutionHoldingsHistory>> UpdateInstitutionHoldingHistoriesAsync(IList<InstitutionHoldingsHistory> InstitutionHistories)
        {

            InstitutionHistories.ToList().ForEach(ih =>
                {
                    _context.DetachLocal<InstitutionHoldingsHistory>(ih, ih.Id);
                    _context.Entry(ih).State = EntityState.Modified;
                }
            );

            return InstitutionHistories;
        }
    }
}
