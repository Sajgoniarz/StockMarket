using StockMarket.Models.Entities;
using System.Collections.Generic;

namespace StockMarket.Models.Abstract
{
    interface IWalletRepository
    {
        Wallet GetUserWallet(string username);
        Wallet GetMarketWallet();
        void RemovePrices(IEnumerable<Price> prices);
        void Save();
    }
}
