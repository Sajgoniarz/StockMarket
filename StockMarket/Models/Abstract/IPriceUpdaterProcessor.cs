using StockMarket.Models.Entities.StocksApiResponse;

namespace StockMarket.Models.Abstract
{
    interface IPriceUpdaterProcessor
    {
        void UpdatePrices(RootObject response);
        void NotifyUserAboutUpdateError();
        void RefreshUserStocks();
    }
}
