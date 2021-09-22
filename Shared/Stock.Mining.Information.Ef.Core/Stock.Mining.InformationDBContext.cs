using Microsoft.EntityFrameworkCore;
using Stock.Mining.Information.Ef.Core.Entity;
using System;

namespace Stock.Mining.Information.Ef.Core
{
     public class InformationDBContext : DbContext
    {
        public DbSet<InstitutionHolding> InstitutionHoldings { get; set; }
        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<InstitutionHoldingsHistory> InstitutionHoldingsHistory { get; set; }

        public DbSet<SymbolPrice> SymbolPrice { get; set; }

        public DbSet<InsiderHistory> InsiderHistories { get; set; }

        public InformationDBContext(DbContextOptions<InformationDBContext> Option) : base(Option)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstitutionHoldingsHistory>().HasOne(a => a.InstitutionHolding);
                                            
        }

    }
}
