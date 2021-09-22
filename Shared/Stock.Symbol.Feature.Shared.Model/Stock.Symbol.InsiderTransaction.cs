using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model
{
    public class InsiderTransaction
    {
        public DateTime TransactionDate { get; set; }
        public string HolderName { get; set; }
        public decimal Shares { get; set; }
        public decimal Value { get; set; }
        public string Side { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
    }
}
