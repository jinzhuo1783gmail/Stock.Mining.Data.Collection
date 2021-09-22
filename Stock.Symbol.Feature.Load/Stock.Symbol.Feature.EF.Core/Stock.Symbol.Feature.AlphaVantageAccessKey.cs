using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Stock.Symbol.Feature.EF.Core
{
    public class AlphaVantageAccessKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string AccessKey { get; set; }

        public string Account { get; set; }

    }
}
