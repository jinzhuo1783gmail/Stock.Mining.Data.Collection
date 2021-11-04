using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stock.Mining.Information.Ef.Core.Entity;
using Stock.Mining.Information.Proxy;
using Stock.Symbol.Feature.Shared.Model;
using Stock.Symbol.Feature.Shared.Model.Extension.Mapper;
using Stock.Symbol.Feature.Shared.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Mining.Information.Initialization.Manager
{
    public class InstitutionManager
    {
        private RestSharpUtil _restSharpUtil;
        private ILogger<InstitutionManager> _logger;
        private IConfiguration _configuration;
        private CollectionProxy _collectionProxy;
        private LoadProxy _loadProxy;
        public InstitutionManager(ILogger<InstitutionManager> logger, IConfiguration configuration, RestSharpUtil restSharpUtil, CollectionProxy collectionProxy, LoadProxy loadProxy)
        {
            _logger = logger;
            _configuration = configuration;
            _restSharpUtil = restSharpUtil;
            _collectionProxy = collectionProxy;
            _loadProxy = loadProxy;
        }



        public async Task<IList<InstitutionOwner>> GetInstitutionOwnersAsync(string ticker)
        { 
            return await _loadProxy.GetInstitutionHoldingsAsync(ticker);
        }

        public async Task<IList<InstitutionHolding>> GetInstitutionHoldingsAsync(string ticker)
        { 
            return await _collectionProxy.GetInsititutionalHoldings(ticker);
        }

        public async Task<IList<InstitutionHolding>> UpsertInstitutionHoldingsAsync(IList<InstitutionHolding> institutionHoldings)
        { 
            return await _collectionProxy.UpsertInsititutionalHoldings(institutionHoldings);
        }

        public async Task<bool> InsertInsititutionalHoldingHistories(IList<InstitutionHoldingsHistory> institutionHoldingsHistory)
        { 
            return await _collectionProxy.InsertInsititutionalHoldingHistories(institutionHoldingsHistory);
        }

        public async Task<IList<InstitutionTransaction>> GetInstitutionHoldingHistoriesAsync(string ticker)
        { 
            return await _loadProxy.GetInstitutionHoldingHistoriesAsync(ticker);
        }



        public async Task<bool> AppendInstitutionHoldingHsitories(IList<InstitutionTransaction> histories, IList<InstitutionHolding> holdings, Stock.Mining.Information.Ef.Core.Entity.Symbol symbol)
        { 
            var addInsititionList = new List<InstitutionHolding>();
            var updateInsititionList = new List<InstitutionHolding>();
            var linkedHistory = new List<InstitutionHoldingsHistory>();
            var unlinkedtrans = new List<InstitutionTransaction>();
            var result = true;

            foreach (var history in histories)
            {
                var matchholdinginst = holdings.FirstOrDefault(h => h.InstitutionName == history.InstitutionName);

                if (matchholdinginst != null)
                {

                    var historyEntity = history.ToInstitutionHoldingHistoryEntity(matchholdinginst.Id);
                    if (history.Position == 0)
                        historyEntity.Action = InsitutionHoldingTrend.Liquidated.ToString(); // temp write this way
                    else if (history.Position > matchholdinginst.Postion)
                    {
                        historyEntity.Action = InsitutionHoldingTrend.Increase.ToString();
                    }
                    else if (history.Position < matchholdinginst.Postion)
                    {
                        historyEntity.Action = InsitutionHoldingTrend.Decrease.ToString();
                    }
                    else
                    {
                        historyEntity.Action = InsitutionHoldingTrend.Hold.ToString();
                    }

                    linkedHistory.Add(historyEntity);

                    // found exiting institition , need update the postion of exsiting insitution

                    if (historyEntity.Action != InsitutionHoldingTrend.Hold.ToString())
                    { 
                        matchholdinginst.PrevPostion = matchholdinginst.Postion;
                        matchholdinginst.Postion = history.Position;
                        matchholdinginst.Value = history.Value;
                        updateInsititionList.Add(matchholdinginst);
                    }
                }
                else 
                {
                    // a brand new transaction, create new insititution holding
                    addInsititionList.Add(history.ToInstitutionHoldingEntity(symbol.Id));
                    unlinkedtrans.Add(history); // this is model not entity yet
                }
            }

            if (updateInsititionList.Any())
            {
                result &= (await UpsertInstitutionHoldingsAsync(updateInsititionList))?.Any() ?? false;

                if (result)
                    _logger.LogInformation($"{symbol.Ticker} update institution holding upload successfully total {updateInsititionList.Count()}");
                else
                {
                    _logger.LogError($"{symbol.Ticker} update institution holding upload fail total {JsonConvert.SerializeObject(updateInsititionList)}, History has not been load");
                    return false;
                }
            }


            if (addInsititionList.Any())
            { 
                // add those insitution in 
                var holdingsReturn= await UpsertInstitutionHoldingsAsync(addInsititionList);
                
                if (holdingsReturn?.Any() ?? false)
                    _logger.LogInformation($"{symbol.Ticker} append institution holding upload successfully total {addInsititionList.Count()}");
                else
                {
                    _logger.LogError($"{symbol.Ticker} append institution holding upload fail total {JsonConvert.SerializeObject(addInsititionList)}, History has not been load");
                    return false;
                }

                holdings = await GetInstitutionHoldingsAsync(symbol.Ticker);

                foreach (var unlink in unlinkedtrans) {

                    var holdingsId = holdingsReturn.FirstOrDefault(h => h.InstitutionName == unlink.InstitutionName);

                    if (holdingsId != null && holdingsId.Id != default(long))
                        linkedHistory.Add(unlink.ToInstitutionHoldingHistoryEntity(holdingsId.Id, InsitutionHoldingTrend.New));
                    else
                        _logger.LogWarning($"unlinked institution history {unlink.InstitutionName} for ticker {symbol.Ticker}");
                }
            }

            //var history
            if (!InsertInsititutionalHoldingHistories(linkedHistory).GetAwaiter().GetResult())
            {
                _logger.LogError($"Fail to update history total count of history {linkedHistory.Count}, ticker {symbol.Ticker}");
                return false;
            }

            return result;
        }
        
        
    }
}
