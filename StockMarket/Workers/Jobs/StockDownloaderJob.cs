using System;
using Quartz;
using System.Net;
using System.Configuration;
using System.Web.Script.Serialization;
using StockMarket.Models.Abstract;
using StockMarket.Models.Entities.StocksApiResponse;
using StockMarket.Models.Concrete.Repositories.EntityFramework;

namespace StockMarket.Workers.Jobs
{
    public class StocksDownloaderJob : IJob
    {
        private IPriceUpdaterProcessor updater;

        public StocksDownloaderJob()
        {
            updater = new PriceUpdater();
        }

        public void Execute(IJobExecutionContext context)
        {
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.Accept, "application/json");

            client.DownloadStringCompleted += (sender, args) => {
                if (!args.Cancelled && args.Error == null)
                {
                    var serializer = new JavaScriptSerializer();
                    var responseObject = new RootObject();

                    try
                    {
                        responseObject = serializer.Deserialize<RootObject>(args.Result);
                        updater.UpdatePrices(responseObject);
                        updater.RefreshUserStocks();
                    }
                    catch
                    {
                        updater.NotifyUserAboutUpdateError();
                    }
                }
                else
                {
                    updater.NotifyUserAboutUpdateError();
                }
            };

            client.DownloadStringAsync(new Uri(ConfigurationManager.AppSettings["StocksAPI"]));
        }
    }
}