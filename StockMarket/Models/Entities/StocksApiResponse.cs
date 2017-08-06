using System;
using System.Collections.Generic;

namespace StockMarket.Models.Entities.StocksApiResponse
{
    public class Item
    {
        public string name { get; set; }
        public string code { get; set; }
        public int unit { get; set; }
        public decimal price { get; set; }
    }

    public class RootObject
    {
        public DateTime publicationDate { get; set; }
        public List<Item> items { get; set; }
    }
}