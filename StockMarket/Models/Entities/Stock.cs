using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockMarket.Models.Entities
{
    public class Stock
    {
        public int StockId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual List<Price> PriceList { get; set; }
        public virtual List<UserStock> UserStockList { get; set; }
        public decimal? Price
        {
            get
            {
                return PriceList.OrderByDescending(p => p.PublicationDate).FirstOrDefault()?.Value;
            }
        }

        public DateTime PublicationDate
        {
            get
            {
                return PriceList.OrderByDescending(p => p.PublicationDate).FirstOrDefault().PublicationDate;
            }
        }

        public int Unit
        {
            get
            {
                return PriceList.OrderByDescending(p => p.PublicationDate).FirstOrDefault().Unit;
            }
        }
    }
}