using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.Shared.Model
{
    public class InstitutionTransaction
    {
        public long Id { get; set; }
        public long InstitutionOwnerId { get; set; }
        public DateTime FileDate { get; set; }
        public string Source { get; set; }
        public string InstitutionName { get; set; }
        public string Option { get; set; }
        public decimal Price { get; set; }
        public decimal Position { get; set; }
        public decimal ShareChanged { get; set; }
        public decimal Value { get; set; }
        public decimal ValueChanged { get; set; }


    }
}
