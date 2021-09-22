using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model
{
    public class StockPriceViewModel
    {
        public long Id { get; set; }
        public long SymbolId { get; set; }
        public string  Symbol { get; set; }
        public DateTime PriceDate { get; set; }
        public Decimal Open { get; set; }
        public Decimal High { get; set; }
        public Decimal Low { get; set; }
        public Decimal Close { get; set; }

        public Decimal AdjustClose { get; set; }
        public Decimal Volume { get; set; }
        public Decimal DividendAmount { get; set; }
        public Decimal SplitCoefficient { get; set; }
    }
}
