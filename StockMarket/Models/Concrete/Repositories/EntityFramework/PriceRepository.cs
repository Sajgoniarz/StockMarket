using StockMarket.Models.Entities;

namespace StockMarket.Models.Concrete.Repositories.EntityFramework
{
    public class PriceRepository
    {
        private ApplicationContext context;

        public PriceRepository()
        {
            context = ApplicationContext.Create();
        }

        public void Add(Price price)
        {
            context.Prices.Add(price);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}