using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Symbol.Feature.Load.ConfigModel
{
    public class ApiEndpointList
    {
        public string ApiHtmlInnerAnalysis { get; set; }
        public string ApiHtmlHoldingsFintel { get; set; }
        public string ApiHtmlHoldingsHoldingsChannel { get; set; }
        public string ApiRapidYahooInstitutionalHolders { get; set; }
        public string ApiRapidStockTwistStreamSymbol { get; set; }
        public string ApiHTMLExtract { get; set; }
        public string ApiAlphaVantageHistoricalPrice { get; set; }
        public string ApiRapidYahooInsiderTransaction { get; set; }

    }
}
