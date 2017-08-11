using StockMarket.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace StockMarket.Models.Entities
{
    public class Wallet
    {
        public int WalletId { get; set; }

        public string UserId { get; set; }
        
        public decimal Founds { get; set; }

        public virtual IList<UserStock> OwnedStocks { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public Wallet()
        {
            OwnedStocks = new List<UserStock>();
        }

        public TransactionErrorCode Transfer(Wallet buyerWallet, string code, int amount)
        {
            var sellerWallet = this;
            var sellerStock = sellerWallet.OwnedStocks.Where(s => s.Stock.Code == code).FirstOrDefault();
            var buyerStock = buyerWallet.OwnedStocks.Where(s => s.Stock.Code == code).FirstOrDefault();
            decimal stockTotalPrice = amount * (decimal)sellerStock?.Stock.Price;

            if (sellerStock == null)
                return TransactionErrorCode.StockNotFound;

            if (!sellerStock.Stock.Price.HasValue)
                return TransactionErrorCode.PriceUnavailable;

            if (sellerStock.Amount < amount)
                return TransactionErrorCode.UnsufficientAmount;

            if (buyerWallet.Founds - stockTotalPrice < 0)
                return TransactionErrorCode.UnsufficientFounds;

            sellerStock.Amount -= amount;

            if (buyerStock == null)
            {
                buyerWallet.OwnedStocks.Add(new UserStock
                {
                    Amount = amount,
                    StockId = sellerStock.StockId
                });
            }
            else
            {
                if (buyerStock.Amount.HasValue)
                {
                    buyerStock.Amount += amount;
                }
                else
                {
                    buyerStock.Amount = amount;
                }
            }

            sellerWallet.Founds += stockTotalPrice;
            buyerWallet.Founds -= stockTotalPrice;

            return TransactionErrorCode.Ok;
        }
    }
}