using Microsoft.AspNet.SignalR;
using StockMarket.Hubs;
using StockMarket.Models.Abstract;
using StockMarket.Models.Entities;
using StockMarket.Models.Entities.StocksApiResponse;
using StockMarket.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockMarket.Models.Concrete.Repositories.EntityFramework
{
    public class PriceUpdater : IPriceUpdaterProcessor
    {
        private IWalletRepository walletRepository;
        private IHubContext context;

        public PriceUpdater()
        {
            walletRepository = new WalletRepository();
            context = GlobalHost.ConnectionManager.GetHubContext<StockMarketHub>();
        }

        public void UpdatePrices(RootObject response)
        {
            Wallet StockMarketWallet = walletRepository.GetMarketWallet();

            foreach (Item stock in response.items)
            {
                Stock stockToUpdate = StockMarketWallet.OwnedStocks
                                       .Where(s => s.Stock.Code == stock.code).FirstOrDefault()
                                       ?.Stock;

                if (stockToUpdate != null)
                {
                    if (Math.Abs((response.publicationDate - stockToUpdate.PublicationDate).TotalSeconds) > 1)
                    {
                        var newPrice = new Price
                        {
                            PublicationDate = response.publicationDate,
                            Value = stock.price,
                            Unit = stock.unit,
                            StockId = stockToUpdate.StockId,
                        };


                        stockToUpdate.PriceList.Add(newPrice);

                        walletRepository.RemovePrices(stockToUpdate.PriceList.OrderByDescending(p => p.PublicationDate).Skip(20).ToList());
                        walletRepository.Save();
                    }
                }
                else
                {
                    var newStock = new Stock
                    {
                        Code = stock.code,
                        Name = stock.name,
                    };

                    var newPrice = new Price
                    {
                        PublicationDate = response.publicationDate,
                        Value = stock.price,
                        Stock = newStock,
                    };

                    var newUserStock = new UserStock
                    {
                        Stock = newStock
                    };

                    StockMarketWallet.OwnedStocks.Add(newUserStock);
                    walletRepository.Save();
                }
            }
        }

        public void NotifyUserAboutUpdateError()
        {
            context.Clients.All.errorOnStocksUpdate();
        }

        public void RefreshUserStocks()
        {
            Wallet marketWallet = walletRepository.GetMarketWallet();
            List<string> clients = StockMarketHub.connectedUsers;

            foreach (string clientId in clients)
            {
                Wallet userWallet = walletRepository.GetUserWallet(clientId);
                context.Clients.User(clientId).UpdateStocks((WalletViewModel)marketWallet, (WalletViewModel)userWallet);
            }
        }
    }
}