using System.Web.Mvc;
using StockMarket.Models.Abstract;
using StockMarket.Models.Concrete.Repositories.EntityFramework;

namespace StockMarket.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated) return View("Index");

            return View("Market");
        }
    }
}