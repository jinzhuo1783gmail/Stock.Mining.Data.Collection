using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Ef.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Persist.Manager
{
    public class SymbolPriceManager
    {
        private SymbolPriceRepository _symbolRepository;
        private ILogger<SymbolPriceRepository> _logger;
        public SymbolPriceManager(SymbolPriceRepository symbolRepository, ILogger<SymbolPriceRepository> logger)
        {
            _symbolRepository = symbolRepository;
            _logger = logger;
        }

        public async Task<IList<SymbolPrice>> GetAllPrice (string ticker)
        {
            try
            {
                 return await _symbolRepository.GetPriceAsync(ticker, DateTime.MinValue, DateTime.MaxValue);
            }
            catch (Exception ex)
            { 
                _logger.LogError($"GetAllPrice failed: {ex.InnerException}");
                return null;
            }
        }

        public async Task<IList<SymbolPrice>> GetAllPrice (string ticker, DateTime startDate, DateTime endDate)
        {
            try
            {
                 return await _symbolRepository.GetPriceAsync(ticker, startDate, endDate);
            }
            catch (Exception ex)
            { 
                _logger.LogError($"GetAllPrice failed: {ex.InnerException}");
                return null;
            }
        }

        public async Task<bool> AddBatchPrices (string ticker,  IList<SymbolPrice> symbolPrices)
        {
            try
            {
                var result = await _symbolRepository.AddRangedPriceAsync(symbolPrices);
                result &= await _symbolRepository.SaveContextAsync();

                return result;
            }
            catch (Exception ex)
            { 
                _logger.LogError($"AddBatchPrices failed: {ex.InnerException} for Symbol {ticker}");
                return false;
            }
        }

        public async Task<bool> UpdateBatchPrices (string ticker,  IList<SymbolPrice> symbolPrices)
        {
            try
            {
                var result = await _symbolRepository.UpdateRangedPriceAsync(symbolPrices);
                result &= await _symbolRepository.SaveContextAsync();
                return result;
            }
            catch (Exception ex)
            { 
                _logger.LogError($"Update batchPrices failed: {ex.InnerException} for Symbol {ticker}");
                return false;
            }
        }


        public async Task<bool> AddOrUpdateBatchPrices(IList<SymbolPrice> symbolPrices)
        {

            try
            {
                var updatePriceList = _symbolRepository.GetPriceAsync(symbolPrices.FirstOrDefault().SymbolId,  symbolPrices.Select(p => p.PriceDate.Date).ToList()).GetAwaiter().GetResult();

                Dictionary<DateTime, long> updatePrice;
                if (updatePriceList != null && updatePriceList.Any())
                {
                    updatePrice = updatePriceList.ToDictionary(p => p.PriceDate, p => p.Id);
                    foreach (  var price in symbolPrices)
                    {
                        if (updatePrice.Keys.Contains(price.PriceDate))
                        {
                            price.Id = updatePrice[price.PriceDate];
                        }

                    }
                }
                

                var success = await _symbolRepository.AddRangedPriceAsync(symbolPrices.Where(p => p.Id == 0).ToList());
                success &= await _symbolRepository.UpdateRangedPriceAsync(symbolPrices.Where(p => p.Id != 0).ToList());
                success &= await _symbolRepository.SaveContextAsync();

                return success;       

            }
            catch (Exception ex)
            {
                _logger.LogError($"Upsert batchPrices failed: {ex.InnerException}");
                return false;
            }
            
        }


    }
}
