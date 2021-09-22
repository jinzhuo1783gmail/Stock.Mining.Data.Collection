using System;
using System.Collections.Generic;

namespace Stock.Symbol.Feature.Shared.Model
{
    public class InstitutionOwner
    {

        public long Id { get; set; }
        public long SymbolId { get; set; }
        public string InstitutionName { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal Position { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
        public string MatchWord { get; set; }

        public IList<InstitutionTransaction> InstitutionHoldingsHistories { get; set; }
    }
}
