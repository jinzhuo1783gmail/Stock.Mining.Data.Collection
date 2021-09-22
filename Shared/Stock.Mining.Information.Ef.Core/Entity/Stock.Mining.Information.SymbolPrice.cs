using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stock.Mining.Information.Ef.Core.Entity
{
    public class SymbolPrice : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Symbol")]
        public long SymbolId { get; set; }
        public Symbol Symbol { get; set; }
        public DateTime PriceDate { get; set; }
        public Decimal Open { get; set; }
        public Decimal High { get; set; }
        public Decimal Low { get; set; }
        public Decimal Close { get; set; }
        public Decimal AdjustClose { get; set; }
        public Decimal Volume { get; set; }
        public Decimal SplitCoefficient { get; set; }
    }
}
