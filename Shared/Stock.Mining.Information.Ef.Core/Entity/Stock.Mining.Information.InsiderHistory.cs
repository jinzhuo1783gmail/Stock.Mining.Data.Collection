using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stock.Mining.Information.Ef.Core.Entity
{
    public class InsiderHistory : IEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Symbol")]
        public long SymbolId { get; set; }
        public Symbol Symbol { get; set; }
        public DateTime TransactionDate { get; set; }
        public string HolderName { get; set; }
        public decimal Shares { get; set; }
        public decimal Value { get; set; }
        public string Side { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
    }
}
