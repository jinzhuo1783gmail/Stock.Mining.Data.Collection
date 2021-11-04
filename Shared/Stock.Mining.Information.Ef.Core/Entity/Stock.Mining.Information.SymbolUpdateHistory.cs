using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stock.Mining.Information.Ef.Core.Entity
{
    public class SymbolUpdateHistory : IEntityBase
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey("Symbol")]
        public long SymbolId { get; set; }
        public Symbol Symbol { get; set; }
        public string  Ticker { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsSuccess { get; set; }
        public string  FailReason { get; set; }
        public DateTime NextUpdateTime { get; set; }

    }
}
