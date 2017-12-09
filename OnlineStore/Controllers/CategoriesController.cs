using OnlineStore.Models.StoreDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class CategoriesController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.Where(c => c.Visibility == true).OrderByDescending(c => c.CategoryId).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}