using OnlineStore.Models.StoreDB;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class GoodsController : Controller
    {
        OnlineStoreEntities db = new OnlineStoreEntities();

        public ActionResult Index(string genre, int? page)
        {
            ViewBag.Genre = genre;

            List<Goods> goods;

            if (genre != null)
            {
                Category category = db.Categories.FirstOrDefault(c => c.CategoryName == genre);

                if (category != null)
                    goods = db.Goods
                        .Where(g => g.CategoryId == category.CategoryId)
                        .ToList();
                else
                    return HttpNotFound();
            }
            else
                goods = db.Goods.ToList();

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(goods.ToPagedList(pageNumber, pageSize));
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