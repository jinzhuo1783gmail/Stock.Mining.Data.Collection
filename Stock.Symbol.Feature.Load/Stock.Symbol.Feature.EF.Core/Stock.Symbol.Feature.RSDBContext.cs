using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Symbol.Feature.EF.Core
{
    public class RSDBContext 
        : DbContext
    {
        public DbSet<RestSharpAccessKey> RestSharpAccessKeys { get; set; }
        public DbSet<AlphaVantageAccessKey> AlphaVantageAccessKeys { get; set; }
        
        public RSDBContext(DbContextOptions<RSDBContext> Option) : base(Option)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
