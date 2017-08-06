using StockMarket.Models.Entities;

namespace StockMarket.Models.Abstract
{
    interface IUserStockRepository
    {
        void Add(UserStock price);
        void Save();
    }
}
