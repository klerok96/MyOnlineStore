using OnlineStore.Models.StoreDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class StatisticsController : Controller
    {
        OnlineStoreEntities db = new OnlineStoreEntities();
        
        // GET: Statistics
        public ActionResult Index()
        {
            ViewBag.TotalAmount = db.PurchaseStatistics.Sum(t => t.Quantity);

            return View(db.PurchaseStatistics.OrderByDescending(p => p.PSId).ToList());
        }
    }
}