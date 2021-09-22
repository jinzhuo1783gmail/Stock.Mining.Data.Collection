using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Ef.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Collection.Manager
{
    public class SymbolManager
    {
        SymbolRepository _symbolRepository;
        ILogger<SymbolManager> _logger;
        public SymbolManager(SymbolRepository symbolRepository, ILogger<SymbolManager> logger)
        {
            _symbolRepository = symbolRepository;
            _logger = logger;
        }

        public async Task<IList<Symbol>> GetAllSymbols()
        {
            try
            { 
                return await _symbolRepository.GetSymbolsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllSymbols failed: {ex.Message}");
                return null;
            }
        }

        public async Task<Symbol> GetSymbolByName(string Name)
        {
            try
            { 
                return await _symbolRepository.GetSymbolsAsync(Name);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetSymbolByName failed: {ex.Message}");
                return null;
            }
            
            
        }

        public async Task<Symbol> AddOrUpdateSymbol(Symbol Ticker)
        {
            
            try
            { 
                var currSymbol = _symbolRepository.GetSymbolsAsync(Ticker.Ticker).GetAwaiter().GetResult();
                Symbol updatedSymbol; 

                if (currSymbol == null)
                    updatedSymbol =  await _symbolRepository.AddNewSymbolAsync(Ticker);
                else
                {
                    if (Ticker.Id == 0)
                        Ticker.Id = currSymbol.Id;
                    updatedSymbol =  await _symbolRepository.UpdateSymbolsAsync(Ticker);
                }

                if (!_symbolRepository.SaveContextAsync().GetAwaiter().GetResult())
                { 
                    _logger.LogError($"AddOrUpdateSymbol failed Symbol [{Ticker}]");
                }

                return updatedSymbol;

            }
            catch (Exception ex)
            {
                _logger.LogError($"AddOrUpdateSymbol failed: {ex.Message}");
                return null;
            }
            
            
            
            
        }

        public async Task<bool> DisableSymbolScanning(string Ticker)
        {
            try
            { 
                var result = await _symbolRepository.DisableSymbolsAsync(Ticker);

                if (!_symbolRepository.SaveContextAsync().GetAwaiter().GetResult())
                { 
                    _logger.LogError($"DisableSymbolScanning failed Symbol [{Ticker}]");
                    return false;    
                }

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError($"DisableSymbolScanning failed: {ex.Message}");
                return false;
            }
            
            
        }

    }
}
