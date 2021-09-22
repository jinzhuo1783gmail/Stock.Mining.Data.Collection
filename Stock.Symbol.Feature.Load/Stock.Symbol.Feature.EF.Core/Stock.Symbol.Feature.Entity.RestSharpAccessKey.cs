using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.Symbol.Feature.EF.Core
{
    public class RestSharpAccessKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string AccessKey { get; set; }

        public string Account { get; set; }
        public int MonthlyCount { get; set; }
        public int NextRefreshMonth { get; set; }
    }
}
