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
    public class SymbolPriceRepository : MiningRespository, IMiningRespository
    {

        //public InformationDBContext _context;
        public ILogger<SymbolPriceRepository> _logger;

        public SymbolPriceRepository(InformationDBContext context, ILogger<SymbolPriceRepository> logger) 
        {
            _context = context;
            _logger = logger;

        }

        public async Task<IList<SymbolPrice>> GetPriceAsync(string ticker, DateTime startTime, DateTime endDate)
        {
            var symbol = _context.Symbols.Where(s => s.Ticker == ticker).FirstOrDefault();
            if (symbol == null)
            {
                _logger.LogError($"GetPriceAsync: Invalid ticker name {ticker}");
                return null;
            }
            
            return await _context.SymbolPrice.AsNoTracking().Where(p => p.SymbolId == symbol.Id && p.PriceDate >= startTime && p.PriceDate <= endDate).ToListAsync();
        }

        public async Task<IList<SymbolPrice>> GetPriceAsync(long symbolId, IList<DateTime> listOfDateContains)
        {

            if (_context.SymbolPrice.AnyAsync().GetAwaiter().GetResult())
            {
                return await _context.SymbolPrice.AsNoTracking().Where(p => p.SymbolId == symbolId && listOfDateContains.Contains(p.PriceDate.Date)).ToListAsync();
            }
            else 
            {
                return new List<SymbolPrice>();
            }

            
        }
        

        public async Task<bool> AddRangedPriceAsync(IList<SymbolPrice> symbolPriceList)
        {
            try
            {
                await _context.SymbolPrice.AddRangeAsync(symbolPriceList);
                return true;
            }
            catch (Exception ex)
            { 
                 _logger.LogError($"Batch insert price failed {ex.InnerException}");
                return false;
            }
        }

        public async Task<SymbolPrice> UpdatePriceAsync(SymbolPrice symbolPrice)
        {

            _context.DetachLocal<SymbolPrice>(symbolPrice, symbolPrice.Id);

            _context.Entry(symbolPrice).State = EntityState.Modified;

            return symbolPrice;

        }

        public async Task<bool> UpdateRangedPriceAsync(IList<SymbolPrice> symbolPriceList)
        {
            try
            {
                symbolPriceList.ToList().ForEach(p => _context.DetachLocal<SymbolPrice>(p, p.Id));
                _context.SymbolPrice.UpdateRange(symbolPriceList);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Batch update price failed {ex.InnerException}");
                return false;
            }
        }
    }
}
