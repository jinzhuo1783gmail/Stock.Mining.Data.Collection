using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Ef.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Ef.Core.Repository
{
    public class InsiderHistoryRepository : MiningRespository, IMiningRespository
    {
         //public InformationDBContext _context;
        public ILogger<InsiderHistoryRepository> _logger;

        public InsiderHistoryRepository(InformationDBContext context, ILogger<InsiderHistoryRepository> logger) 
        {
            _context = context;
            _logger = logger;

        }

        public async Task<IList<InsiderHistory>> GetHistories(string ticker)
        { 
            var symbol = _context.Symbols.Where(s => s.Ticker == ticker).FirstOrDefault();
            if (symbol == null)
            {
                _logger.LogError($"GetHistories: Invalid ticker name {ticker}");
                return null;
            }
            
            return await _context.InsiderHistories.AsNoTracking().Where(p => p.SymbolId == symbol.Id).ToListAsync();
        }

        public async Task<IList<InsiderHistory>> GetHistories(long symbolId, IList<DateTime> listOfDateContains)
        {

            if (_context.InsiderHistories.AnyAsync().GetAwaiter().GetResult())
            {
                return await _context.InsiderHistories.AsNoTracking().Where(p => p.SymbolId == symbolId && listOfDateContains.Contains(p.TransactionDate.Date)).ToListAsync();
            }
            else 
            {
                return new List<InsiderHistory>();
            }

            
        }
        

        public async Task<bool> AddRangedInsiderHistoryAsync(IList<InsiderHistory> insiderHistoryList)
        {
            try
            {
                await _context.InsiderHistories.AddRangeAsync(insiderHistoryList);
                return true;
            }
            catch (Exception ex)
            { 
                 _logger.LogError($"Batch insert insiderHistoryList failed {ex.InnerException}");
                return false;
            }

            
            
        }

        public async Task<InsiderHistory> UpdateInsiderHistoryAsync(InsiderHistory insiderHistory)
        {

            _context.DetachLocal<InsiderHistory>(insiderHistory, insiderHistory.Id);

            _context.Entry(insiderHistory).State = EntityState.Modified;

            return insiderHistory;

        }

        public async Task<bool> UpdateRangedInsiderHistoryAsync(IList<InsiderHistory> insiderHistoryList)
        {
            try
            {
                insiderHistoryList.ToList().ForEach(ih => _context.DetachLocal<InsiderHistory>(ih, ih.Id));
                _context.InsiderHistories.UpdateRange(insiderHistoryList);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Batch update insiderHistoryList failed {ex.InnerException}");
                return false;
            }
        }

    }
}
