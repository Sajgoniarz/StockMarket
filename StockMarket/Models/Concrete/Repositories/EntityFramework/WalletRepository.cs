using StockMarket.Models.Abstract;
using StockMarket.Models.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace StockMarket.Models.Concrete.Repositories.EntityFramework
{
    public class WalletRepository : IWalletRepository
    {
        private ApplicationContext context;

        public WalletRepository()
            => context = ApplicationContext.Create();

        public Wallet GetUserWallet(string username)
        {
            return context.Wallets.Where(w => w.ApplicationUser.UserName == username).FirstOrDefault();
        }

        public Wallet GetMarketWallet()
        {
            string stockMarketUserName = ConfigurationManager.AppSettings["StockMarketUsername"];
            return context.Wallets.Where(w => w.ApplicationUser.UserName == stockMarketUserName).FirstOrDefault();
        }

        public void RemovePrices(IEnumerable<Price> prices)
        {
            foreach (Price price in prices)
            {
                context.Entry(price).State = System.Data.Entity.EntityState.Deleted;

            }

            context.SaveChanges();
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}