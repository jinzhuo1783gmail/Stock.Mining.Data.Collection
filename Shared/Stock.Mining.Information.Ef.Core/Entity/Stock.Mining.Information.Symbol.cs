using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stock.Mining.Information.Ef.Core.Entity
{
    public class Symbol :IEntityBase
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Ticker { get; set; }
        public string CompanyName { get; set; }
        public int TotalIssuedShares { get; set; }
        public bool ScanEnable { get; set; }
        public bool Initialized { get; set; }
    }
}
