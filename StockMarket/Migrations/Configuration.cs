using StockMarket.Models.Concrete.Repositories.EntityFramework;
using StockMarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;

namespace StockMarket.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationContext context)
        {
            var now = new DateTime(2017, 7, 8, 20, 30, 25);

            var stockList = new List<Stock>
            {
                new Stock {
                    StockId = 1,
                    Name = "Future Processing",
                    Code = "FP",
                },
                new Stock
                {
                    StockId = 2,
                    Name = "FP Lab",
                    Code = "FPL",
                },
                new Stock
                {
                    StockId = 3,
                    Name = "Progress Bar",
                    Code = "PGB",
                },
                new Stock
                {
                    StockId = 4,
                    Name = "FP Coin",
                    Code = "FPC",
                },
                new Stock
                {
                    StockId = 5,
                    Name = "FP Adventure",
                    Code = "FPA",
                },
                new Stock
                {
                    StockId = 6,
                    Name = "Deadline 24",
                    Code = "DL24",
                },
            };

            var priceList = new List<Price>
            {
                new Price
                {
                    PriceId = 1,
                    StockId = 1,
                    PublicationDate = now,
                    Unit = 1,
                    Value = 5.0208M
                },
                new Price
                {
                    PriceId = 2,
                    StockId = 2,
                    PublicationDate = now,
                    Unit = 100,
                    Value = 3.5052M
                },
                new Price
                {
                    PriceId = 3,
                    StockId = 3,
                    PublicationDate = now,
                    Unit = 1,
                    Value = 4.0389M
                },
                new Price {
                    PriceId = 4,
                    StockId = 4,
                    PublicationDate = now,
                    Unit = 50,
                    Value = 16.2704M
                },
                new Price {
                    PriceId = 5,
                    StockId = 5,
                    PublicationDate = now,
                    Unit = 50,
                    Value = 11.5327M
                },
                new Price {
                    PriceId = 6,
                    StockId = 6,
                    PublicationDate = now,
                    Unit = 100,
                    Value = 5.4094M
                }
            };

            var userStockList = new List<UserStock>
            {
                new UserStock
                {
                    UserStockId = 1,
                    StockId = 1,
                    Amount = 1000
                },
                new UserStock
                {
                    UserStockId = 2,
                    StockId = 2,
                    Amount = 1000
                },
                new UserStock
                {
                    UserStockId = 3,
                    StockId = 3,
                    Amount = 1000
                },
                new UserStock
                {
                    UserStockId = 4,
                    StockId = 4,
                    Amount = 1000
                },
                new UserStock
                {
                    UserStockId = 5,
                    StockId = 5,
                    Amount = 1000
                },
                new UserStock
                {
                    UserStockId = 6,
                    StockId = 6,
                    Amount = 1000
                }
            };

            var wallet = new Wallet
            {
                Founds = 1000000000M,
                OwnedStocks = userStockList
            };

            var StockMarket = new ApplicationUser
            {
                UserName = ConfigurationManager.AppSettings["StockMarketUsername"],
                Wallet = wallet
            };

            stockList.ForEach(s => context.Stocks.Add(s));
            priceList.ForEach(p => context.Prices.Add(p));
            userStockList.ForEach(us => context.UserStocks.Add(us));
            context.Wallets.Add(wallet);
            context.Users.Add(StockMarket);
        }
    }
}