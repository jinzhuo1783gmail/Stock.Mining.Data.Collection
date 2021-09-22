using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stock.Mining.Information.Ef.Core.Entity
{
    public class InstitutionHolding :IEntityBase
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Symbol")]
        public long SymbolId { get; set; }
        public Symbol Symbol { get; set; }
        public string InstitutionName { get; set; }
        public string  MatchWord { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal Postion { get; set; }
        public decimal PrevPostion { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }

        public ICollection<InstitutionHoldingsHistory> InstitutionHoldingsHistories { get; set; }
    }
}
