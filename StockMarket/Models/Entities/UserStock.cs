using System.ComponentModel.DataAnnotations.Schema;

namespace StockMarket.Models.Entities
{
    public class UserStock
    {
        public int UserStockId { get; set; }
        [ForeignKey("Stock")]
        public int StockId { get; set; }
        public int? Amount { get; set; }

        public virtual Wallet Wallet { get; set; }
        public virtual Stock Stock { get; set; }
    }
}