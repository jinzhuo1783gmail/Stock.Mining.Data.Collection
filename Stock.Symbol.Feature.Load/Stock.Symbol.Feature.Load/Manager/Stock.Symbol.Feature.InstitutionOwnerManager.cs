using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Stock.Symbol.External.Retrievor;
using Stock.Symbol.Feature.Load.Mapper;
using Stock.Symbol.Feature.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.Load.Manager
{
    public class InstitutionOwnerManager
    {
        private readonly RapidApiRetrievor _rapidApiRetrievor;
        ILogger<InstitutionOwnerManager> _logger;
        IConfiguration _configuration;
        HtmlTableRetrievor _htmlTableRetrievor;

        public InstitutionOwnerManager(RapidApiRetrievor rapidApiRetrievor, HtmlTableRetrievor htmlTableRetrievor, ILogger<InstitutionOwnerManager> logger, IConfiguration configuration)
        {
            _rapidApiRetrievor = rapidApiRetrievor;
            _logger = logger;
            _configuration = configuration;
            _htmlTableRetrievor = htmlTableRetrievor;
        }


        public async Task<IList<InstitutionOwner>> GetInstitutionOwnersFromTransaction(IList<InstitutionTransaction> transactions)
        {
            var error = "";
            var institutionOwnerList = new List<InstitutionOwner>();

            foreach (var tran in transactions.OrderBy(t => t.FileDate))
            {
                // ingore option transaction
                if (!string.IsNullOrEmpty(tran.Option))  continue;

                if (tran.ShareChanged == -100 && !institutionOwnerList.Any())
                    continue;

                // transaction start , later postion overwrite previous one
                // company name as key
                if (!institutionOwnerList.Any(IServiceProvider => IServiceProvider.InstitutionName == tran.InstitutionName))
                {
                   institutionOwnerList.Add(InstitutionOwnerMapping.TransactionsToModel(tran, out error));     
                }


                else {
                    // overwrite
                    var institution = institutionOwnerList.FirstOrDefault(i => i.InstitutionName == tran.InstitutionName);

                    institution.InstitutionName = tran.InstitutionName;
                    institution.ReportDate = tran.FileDate;
                    institution.Position = tran.Position;
                    institution.Value = tran.Value;
                    institution.Percentage = 0;

                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                _logger.LogError(error);
                return null;
            }

            return institutionOwnerList;
        }


         

        public async Task<IList<InstitutionOwner>> GetInstitutionOwnersFromRapidAsync(string symbol)
        {

            var url = _configuration["ApiEndpointList:ApiRapidYahooInstitutionalHolders"];
            if (string.IsNullOrEmpty(url))
            { 
                _logger.LogError($"No Api configured for ApiRapidYahooInstitutionalHolders");
                return null;
            }


            url = url.Replace("{{symbol}}", symbol);

            if (_rapidApiRetrievor.CreateRestAsync(url).GetAwaiter().GetResult() == false)
            {
                _logger.LogError($"Unable to create rest client for symbol {symbol}!");
                return null;
            }

            if (_rapidApiRetrievor.SetRequestAsync(RestSharp.Method.GET).GetAwaiter().GetResult() == null)
            {
                _logger.LogError($"Unable to create rest request for symbol {symbol}!");
                return null;
            }

            var ownerList = new List<InstitutionOwner>();

             
            var isSummary = await _rapidApiRetrievor.RetrieveAsync<JObject, JObject>(null);

            if (isSummary != null)
            { 
                var ownershipList = isSummary["institutionOwnership"]["ownershipList"] as JArray;

                if (ownershipList == null)
                {
                    _logger.LogError($"ownership list is empty from api for symbol {symbol}!");
                    return null;
                }

            

                foreach (var ownership in ownershipList)
                {
                    var error = "";
                    var owner = InstitutionOwnerMapping.RapidJObjToModel(ownership, out error);
                    if (!string.IsNullOrEmpty(error))
                    {
                        _logger.LogWarning(error);
                    }

                    ownerList.Add(owner);
                }
            
            }

            return ownerList;
        }

        public async Task<IList<InstitutionTransaction>> GetInstitutionOwnersTransactionFromHTMLFintelAsync(string symbol)
        {
            var host = _configuration["ApiEndpointList:ApiHTMLExtract"];
            if (string.IsNullOrEmpty(host))
            { 
                _logger.LogError($"No Api configured for HTMLExtract");
                return null;
            }


            var url = _configuration["ApiEndpointList:ApiHtmlHoldingsFintel"];
            if (string.IsNullOrEmpty(url))
            { 
                _logger.LogError($"No Api configured for ApiHtmlHoldingsFintel");
                return null;
            }

            url = url.Replace("{{symbol}}", symbol);

            if (_htmlTableRetrievor.CreateRestAsync(host, 60).GetAwaiter().GetResult() == null)
            {
                _logger.LogError($"Unable to create rest client for symbol {symbol}!");
                return null;
            }

            if (_htmlTableRetrievor.SetRequestAsync(RestSharp.Method.POST).GetAwaiter().GetResult() == null)
            {
                _logger.LogError($"Unable to create rest request for symbol {symbol}!");
                return null;
            }

            JObject jObjectbody = new JObject();
            jObjectbody.Add("url", url);

            var isSummary = await _htmlTableRetrievor.RetrieveAsync<JObject, JObject>(jObjectbody);
            var tableName = _configuration["HtmlTableExtract:Fintel:InstitutionHolding"];


            var isOwnerShipTransaction = isSummary[tableName] as JArray;

            if (isOwnerShipTransaction == null)
            { 
                _logger.LogError($"no transaction founded from api ApiHtmlFintel!");
                return null;
            }


            var tranList = new List<InstitutionTransaction>();

            foreach (var tran in isOwnerShipTransaction)
            {
                var error = "";
                var transaction = InstitutionTransactionMapping.HtmlJObjToModel(tran, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    _logger.LogWarning(error);
                }

                tranList.Add(transaction);
            }
                    
             return tranList;
            
        }


        public async Task<IList<InstitutionOwner>> GetInstitutionOwnersFromHoldingsChannelAsync(string symbol)
        {

            var host = _configuration["ApiEndpointList:ApiHTMLExtract"];
            if (string.IsNullOrEmpty(host))
            { 
                _logger.LogError($"No Api configured for HTMLExtract");
                return null;
            }


            var url = _configuration["ApiEndpointList:ApiHtmlHoldingsHoldingsChannel"];
            if (string.IsNullOrEmpty(url))
            { 
                _logger.LogError($"No Api configured for ApiHtmlHoldingsHoldingsChannel");
                return null;
            }

            

            url = url.Replace("{{symbol}}", symbol);

            if (_htmlTableRetrievor.CreateRestAsync(host, 60).GetAwaiter().GetResult() == null)
            {
                _logger.LogError($"Unable to create rest client for symbol {symbol}!");
                return null;
            }

            if (_htmlTableRetrievor.SetRequestAsync(RestSharp.Method.POST).GetAwaiter().GetResult() == null)
            {
                _logger.LogError($"Unable to create rest request for symbol {symbol}!");
                return null;
            }

            JObject jObjectbody = new JObject();
            jObjectbody.Add("url", url);

            var ownerList = new List<InstitutionOwner>();


            var isSummary = await _htmlTableRetrievor.RetrieveAsync<JObject, JObject>(jObjectbody);
            var exclusiveList = new List<string>() { "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Put", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Call",  $"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{symbol}"};
            var tableName = _configuration["HtmlTableExtract:HoldingsChannel:InstitutionHolding"];

            if (isSummary != null)
            { 
                var isOwnerShipTransaction = isSummary[tableName] as JArray;

                if (isOwnerShipTransaction == null)
                { 
                    _logger.LogError($"no transaction founded from api HoldingsChannel!");
                    return null;
                }

                foreach (var trans in isOwnerShipTransaction)
                {

                    var error = "";
                    var owner = InstitutionOwnerMapping.HoldingsChannelJObjToModel(trans, out error);
                    if (!string.IsNullOrEmpty(error))
                    {
                        _logger.LogWarning(error);
                    }

                    ownerList.Add(owner);
                }

            }

            var ownerListFilter = ownerList.Where(o => !exclusiveList.Contains(o.InstitutionName)).ToList();

            return ownerListFilter;
        }
    }
}
