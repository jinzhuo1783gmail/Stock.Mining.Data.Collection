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
    public class SymbolUpdateHistoryRepository :  MiningRespository, IMiningRespository
    {
        public ILogger<SymbolUpdateHistoryRepository> _logger;
        public SymbolUpdateHistoryRepository(InformationDBContext context, ILogger<SymbolUpdateHistoryRepository> logger) 
        {
            _context = context;
            _logger = logger;

        }

        public async Task<SymbolUpdateHistory> GetLastUpdate(string ticker)
        { 
            return _context.SymbolUpdateHistories.Where(h => h.Ticker == ticker && h.IsSuccess).OrderByDescending(h => h.NextUpdateTime).FirstOrDefault();
        }

        public async Task<IList<SymbolUpdateHistory>> GetAllUpdates(string ticker)
        { 
            return await _context.SymbolUpdateHistories.AsNoTracking().Where(h => h.Ticker == ticker).ToListAsync();
        }

        public async Task<bool> AddHistoryAsync(SymbolUpdateHistory history)
        {
            try
            {
                await _context.SymbolUpdateHistories.AddAsync(history);
                return true;

            }
            catch (Exception ex)
            { 
                 _logger.LogError($"insert history failed {ex.Message + ex.InnerException}");
                return false;
            }
        }

        public async Task<bool> UpdateHistoryAsync(SymbolUpdateHistory history)
        {
            try
            {
                _context.DetachLocal<SymbolUpdateHistory>(history, history.Id);
                var historyUpdate = (_context.SymbolUpdateHistories.Update(history)).Entity;

                return true; 

            }
            catch (Exception ex)
            { 
                 _logger.LogError($"update history failed {ex.Message + ex.InnerException}");
                return false;
            }
        }

    }
}
