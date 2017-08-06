using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StockMarket.Startup))]
namespace StockMarket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
