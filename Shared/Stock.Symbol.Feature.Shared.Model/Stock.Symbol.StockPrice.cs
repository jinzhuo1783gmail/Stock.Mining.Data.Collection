using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model
{
    public class StockPrice {
        public long Id { get; set; }
        public long SymbolId { get; set; }

        
        public string  Symbol { get; set; }
        public DateTime PriceDate { get; set; }

        [JsonProperty("1. open")]
        public Decimal Open { get; set; }

        [JsonProperty("2. high")]
        public Decimal High { get; set; }

        [JsonProperty("3. low")]
        public Decimal Low { get; set; }

        [JsonProperty("4. close")]
        public Decimal Close { get; set; }

        [JsonProperty("5. adjusted close")]
        public Decimal AdjustClose { get; set; }

        [JsonProperty("6. volume")]
        public Decimal Volume { get; set; }

        [JsonProperty("7. dividend amount")]
        public Decimal DividendAmount { get; set; }

        [JsonProperty("8. split coefficient")]
        public Decimal SplitCoefficient { get; set; }


    }
}
