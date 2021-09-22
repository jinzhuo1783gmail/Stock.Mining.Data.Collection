using Microsoft.Extensions.Logging;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Ef.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Persist.Manager
{
    public class InsiderHistoryManager
    {
        private InsiderHistoryRepository _insiderHistoryRepository;
        private ILogger<InsiderHistoryRepository> _logger;
        public InsiderHistoryManager(InsiderHistoryRepository insiderHistoryRepository, ILogger<InsiderHistoryRepository> logger)
        {
            _insiderHistoryRepository = insiderHistoryRepository;
            _logger = logger;
        }

        public async Task<IList<InsiderHistory>> GetAllHsitories (string ticker)
        {
            try
            {
                 return await _insiderHistoryRepository.GetHistories(ticker);
            }
            catch (Exception ex)
            { 
                _logger.LogError($"GetAllHsitories failed: {ex.InnerException}");
                return null;
            }
        }

        public async Task<bool> AddBatchHistories (string ticker,  IList<InsiderHistory> histories)
        {
            try
            {
                var result = await _insiderHistoryRepository.AddRangedInsiderHistoryAsync(histories);
                result &= await _insiderHistoryRepository.SaveContextAsync();

                return result;
            }
            catch (Exception ex)
            { 
                _logger.LogError($"AddBatchHistories failed: {ex.InnerException} for Symbol {ticker}");
                return false;
            }
        }

        public async Task<bool> UpdateBatchHistories (string ticker, IList<InsiderHistory> histories)
        {
            try
            {
                var result = await _insiderHistoryRepository.UpdateRangedInsiderHistoryAsync(histories);
                result &= await _insiderHistoryRepository.SaveContextAsync();
                return result;
            }
            catch (Exception ex)
            { 
                _logger.LogError($"Update Histories failed: {ex.InnerException} for Symbol {ticker}");
                return false;
            }
        }


        public async Task<bool> AddOrUpdateBatchHsitories(IList<InsiderHistory> histories)
        {

            try
            {
                var updateHsitoryList = _insiderHistoryRepository.GetHistories(histories.FirstOrDefault().SymbolId,  histories.Select(p => p.TransactionDate.Date).ToList()).GetAwaiter().GetResult();

                Dictionary<DateTime, long> updateHistories;
                if (updateHsitoryList != null && updateHsitoryList.Any())
                {
                    updateHistories = updateHsitoryList.ToDictionary(h => h.TransactionDate, p => p.Id);
                    foreach (  var trans in updateHsitoryList)
                    {
                        if (updateHistories.Keys.Contains(trans.TransactionDate))
                        {
                            trans.Id = updateHistories[trans.TransactionDate];
                        }

                    }
                }
                

                var success = await _insiderHistoryRepository.AddRangedInsiderHistoryAsync(histories.Where(i => i.Id == 0).ToList());
                success &= await _insiderHistoryRepository.UpdateRangedInsiderHistoryAsync(histories.Where(i => i.Id != 0).ToList());
                success &= await _insiderHistoryRepository.SaveContextAsync();

                return success;       

            }
            catch (Exception ex)
            {
                _logger.LogError($"Upsert UpdateBatchHsitories failed: {ex.InnerException}");
                return false;
            }
            
        }
    }
}
