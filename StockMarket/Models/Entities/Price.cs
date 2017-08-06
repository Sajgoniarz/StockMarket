using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockMarket.Models.Entities
{
    public class Price
    {
        public int PriceId { get; set; }
        [ForeignKey("Stock")]
        public int StockId { get; set; }
        public DateTime PublicationDate { get; set; }
        public decimal Value { get; set; }
        public int Unit { get; set; }

        public virtual Stock Stock { get; set; }
    }
}