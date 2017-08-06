using StockMarket.Models.Entities;
using System;
using System.Collections.Generic;

namespace StockMarket.ViewModels
{
    public class WalletViewModel
    {
        public DateTime PublicationDate = new DateTime();
        public List<StockItem> StockList = new List<StockItem>();
        public decimal Founds;

        public class StockItem
        {
            public string Name;
            public string Code;
            public decimal? Price;
            public int? Amount;
            public int Unit;
        }

        static public explicit operator WalletViewModel(Wallet wallet)
        {
            var temp = new WalletViewModel();

            if (wallet.OwnedStocks.Count == 0) return temp;

            temp.Founds = wallet.Founds;
            temp.PublicationDate = wallet.OwnedStocks[0].Stock.PublicationDate;

            foreach (UserStock us in wallet.OwnedStocks)
            {
                if (us.Amount.HasValue && us.Amount > 0)
                {
                    temp.StockList.Add(new StockItem
                    {
                        Name = us.Stock.Name,
                        Code = us.Stock.Code,
                        Price = us.Stock.Price,
                        Amount = us.Amount,
                        Unit = us.Stock.Unit,
                    });
                }
            }
            return temp;
        }
    }
}