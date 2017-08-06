using StockMarket.Models.Entities;

namespace StockMarket.Models.Abstract
{
    interface IPriceRepository
    {
        void Add(Price price);
        void Save();
    }
}
