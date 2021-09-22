using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stock.Mining.Information.Ef.Core.Extension;

namespace Stock.Mining.Information.Ef.Core.Repository
{
    public class InstitutionHoldingRespository : MiningRespository, IMiningRespository
    {
        //public InformationDBContext _context;
        public ILogger<InstitutionHoldingRespository> _logger;

        public InstitutionHoldingRespository(InformationDBContext context, ILogger<InstitutionHoldingRespository> logger) 
        {
            _context = context;
            _logger = logger;

        }

        public async Task<IList<InstitutionHolding>> GetInstitutionHoldingsAsync()
        { 
            return await _context.InstitutionHoldings.ToListAsync();
        
        }

         public async Task<IList<InstitutionHolding>> GetInstitutionHoldingsAsync(string Ticker, bool IncludeAllSymbolAndHistory = false)
        {
            var symbolId = _context.Symbols.AsNoTracking().Where(s => s.Ticker == Ticker).Select(s => s.Id).FirstOrDefault();

            if (symbolId == default(int))
            {
                _logger.LogError($"No Tikcer name {Ticker} exit in the db");
                return null;
            }

            if (IncludeAllSymbolAndHistory == false)
                return await _context.InstitutionHoldings.Where(ih => ih.SymbolId == symbolId).Include(ih => ih.Symbol).ToListAsync();
            else
            { 
                return await _context.InstitutionHoldings.Where(ih => ih.SymbolId == symbolId).Include(ih => ih.Symbol).Include(ih => ih.InstitutionHoldingsHistories).ToListAsync();
            }

                
        
        }

        public async Task<InstitutionHolding> GetInstitutionHoldingAsync(long InstitutionId, bool IncludeAllSymbolAndHistory = false)
        { 
             if (IncludeAllSymbolAndHistory == false)
                return await _context.InstitutionHoldings.FirstOrDefaultAsync(ih => ih.Id == InstitutionId);

             else
                return await _context.InstitutionHoldings.Where(ih => ih.Id == InstitutionId).Include(ih => ih.Symbol).Include(ih => ih.InstitutionHoldingsHistories).FirstOrDefaultAsync();
        }

        public async Task<InstitutionHolding> GetInstitutionHoldingAsync(string InstitutionName,  bool IncludeAllSymbolAndHistory = false)
        { 
            
            if (IncludeAllSymbolAndHistory == false)
                return await _context.InstitutionHoldings.FirstOrDefaultAsync(ih => ih.InstitutionName == InstitutionName);
            else
                return await _context.InstitutionHoldings.Where(ih => ih.InstitutionName == InstitutionName).Include(ih => ih.Symbol).Include(ih => ih.InstitutionHoldingsHistories).FirstOrDefaultAsync();
        }

        public async Task<IList<InstitutionHolding>> MatchInstitutionHoldingAsync(string InstitutionName)
        {
            // very low efficent , only match when cannot find directly from the name or Id
            return await _context.InstitutionHoldings.Where(ih => InstitutionName.Contains(ih.MatchWord)).ToListAsync();

        }

        public async Task<InstitutionHolding> AddNewInstitutionHoldingAsync(InstitutionHolding InstitutionHolding)
        {
            //_context.DetachLocal<InstitutionHolding>(InstitutionHolding, InstitutionHolding.Id);
            var added = (await _context.InstitutionHoldings.AddAsync(InstitutionHolding)).Entity;
            return added;
        }

        public async Task<bool> AddRangedInstitutionHoldingAsync(IList<InstitutionHolding> InstitutionHoldings)
        {
            try
            {
                await _context.InstitutionHoldings.AddRangeAsync(InstitutionHoldings);
                return true;
            }
            catch (Exception ex)
            { 
                 _logger.LogError($"Batch insert InstitutionHolding failed {ex.InnerException}");
                return false;
            }

            
            
        }

        public async Task<InstitutionHolding> UpdateInstitutionHoldingAsync(InstitutionHolding Institution)
        {

            _context.DetachLocal<InstitutionHolding>(Institution, Institution.Id);

            _context.Entry(Institution).State = EntityState.Modified;

            return Institution;

        }

        public async Task<bool> UpdateRangedInstitutionHoldingAsync(IList<InstitutionHolding> InstitutionHoldings)
        {
            try
            {
                InstitutionHoldings.ToList().ForEach(ih => _context.DetachLocal<InstitutionHolding>(ih, ih.Id));
                _context.InstitutionHoldings.UpdateRange(InstitutionHoldings);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Batch update InstitutionHolding failed {ex.InnerException}");
                return false;
            }
        }

        
    }
}
