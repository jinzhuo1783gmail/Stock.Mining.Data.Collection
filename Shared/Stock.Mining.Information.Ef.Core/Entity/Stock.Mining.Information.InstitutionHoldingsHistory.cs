using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stock.Mining.Information.Ef.Core.Entity
{
    public class InstitutionHoldingsHistory: IEntityBase
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("InstitutionHolding")]
        public long InstitutionId { get; set; }

        public InstitutionHolding InstitutionHolding { get; set; }
	    public DateTime FileDate { get; set; }
	    public decimal Postion { get; set; }
	    public decimal Value { get; set; }
	    public string Action { get; set; }

    }
}
