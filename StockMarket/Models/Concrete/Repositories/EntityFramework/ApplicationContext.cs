using Microsoft.AspNet.Identity.EntityFramework;
using StockMarket.Migrations;
using StockMarket.Models.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace StockMarket.Models.Concrete.Repositories.EntityFramework
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("DefaultConnection", throwIfV1Schema: false){
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<UserStock> UserStocks { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(20, 4));

            modelBuilder.Entity<Wallet>()
                .HasKey(w => w.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasRequired(u => u.Wallet)
                .WithRequiredPrincipal(u => u.ApplicationUser);
        }
    }
}