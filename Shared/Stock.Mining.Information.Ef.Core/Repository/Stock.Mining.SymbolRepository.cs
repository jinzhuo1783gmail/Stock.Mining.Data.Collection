using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Ef.Core.Extension;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Ef.Core.Repository
{
    public class SymbolRepository : MiningRespository, IMiningRespository
    {
        //public InformationDBContext _context;
        public ILogger<SymbolRepository> _logger;

        public SymbolRepository(InformationDBContext context, ILogger<SymbolRepository> logger) 
        {
            _context = context;
            _logger = logger;

        }

        public async Task<IList<Symbol>> GetSymbolsAsync()
        {
            return await _context.Symbols.AsNoTracking().ToListAsync();
        }

        public async Task<Symbol> GetSymbolsAsync(string ticker)
        {
            return await _context.Symbols.AsNoTracking().FirstOrDefaultAsync(s => s.Ticker == ticker);
        }


        public async Task<Symbol> AddNewSymbolAsync(Symbol ticker)
        {
            var symbolAdded =  (await _context.Symbols.AddAsync(ticker)).Entity;
            return symbolAdded;
        }

        public async Task<Symbol> UpdateSymbolsAsync(Symbol symbol)
        {
             _context.DetachLocal<Symbol>(symbol, symbol.Id);
            var symbolUpdate = (_context.Symbols.Update(symbol)).Entity;

            return symbolUpdate; 
        }

        public async Task<bool> DisableSymbolsAsync(string ticker)
        {
            try
            {

                var symbolEnt = await _context.Symbols.AsNoTracking().FirstOrDefaultAsync(s => s.Ticker == ticker);

                if (symbolEnt == null)
                {
                    _logger.LogError($"Symbol {ticker} does not exist, disable failed");
                    return false;
                }

                symbolEnt.ScanEnable = false;

                var symbolUpdate = _context.Symbols.Update(symbolEnt);
                return true;

            }
            catch (Exception ex)
            { 
                _logger.LogError($"Symbol {ticker} disable scan failed");
                return false;
            }
        }

    }
}
