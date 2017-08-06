using Microsoft.AspNet.SignalR;
using StockMarket.Models.Abstract;
using StockMarket.Models.Concrete.Repositories.EntityFramework;
using StockMarket.Models.Entities;
using StockMarket.Models.Enums;
using StockMarket.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockMarket.Hubs
{
    public class StockMarketHub : Hub
    {
        IWalletRepository walletRepository;

        public static List<string> connectedUsers = new List<string>();

        public StockMarketHub()
        {
            walletRepository = new WalletRepository();
        }

        public void Sell(int amount, string code)
        {
            Wallet userWallet = walletRepository.GetUserWallet(Context.User.Identity.Name);
            Wallet marketWallet = walletRepository.GetMarketWallet();

            TransactionErrorCode result = userWallet.Transfer(marketWallet, code, amount);

            walletRepository.Save();
            Clients.User(Context.User.Identity.Name).completeTransaction(result, (WalletViewModel)userWallet);

        }

        public void Buy(int amount, string code)
        {
            var userWallet = new Wallet();
            TransactionErrorCode result = TransactionErrorCode.UnknownError;

            try
            {
                userWallet = walletRepository.GetUserWallet(Context.User.Identity.Name);
                Wallet marketWallet = walletRepository.GetMarketWallet();

                result = marketWallet.Transfer(userWallet, code, amount);

                walletRepository.Save();
            }
            catch { }

            Clients.User(Context.User.Identity.Name).completeTransaction(result, (WalletViewModel)userWallet);
        }

        public override Task OnConnected()
        {
            Wallet userWallet = walletRepository.GetUserWallet(Context.User.Identity.Name);
            Wallet marketWallet = walletRepository.GetMarketWallet();

            connectedUsers.Add(Context.User.Identity.Name);
            Clients.User(Context.User.Identity.Name).onConnected();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            connectedUsers.Remove(Context.User.Identity.Name);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            connectedUsers.Remove(Context.User.Identity.Name);
            return base.OnReconnected();
        }
    }
}