using OnlineStore.Models;
using OnlineStore.Models.StoreDB;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                var category = db.Categories.FirstOrDefault(c => c.CategoryName == genre);

                if (category != null)
                    goods = db.Goods.Where(g => g.CategoryId == category.CategoryId && g.Visibility == true).ToList();
                else
                    return HttpNotFound();
            }
            else
               goods = db.Goods.Where(g => g.Visibility == true).ToList();

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(goods.OrderByDescending(g => g.ProductId).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goods goods = db.Goods.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }

            return View(goods);
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