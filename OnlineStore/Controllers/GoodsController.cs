using OnlineStore.Models;
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
            var goodsM = new List<GoodsModel>();

            if (genre != null)
            {
                var category = db.Categories.FirstOrDefault(c => c.CategoryName == genre);

                if (category != null)
                {
                    goods = db.Goods.Where(g => g.CategoryId == category.CategoryId).ToList();

                    foreach (var i in goods)
                        goodsM.Add(new GoodsModel
                        {
                            Price = i.Price,
                            ProductId = i.ProductId,
                            ProductName = i.ProductName,
                        });
                }

                else
                    return HttpNotFound();
            }
            else
            {
                goods = db.Goods.ToList();

                foreach (var i in goods)
                    goodsM.Add(new GoodsModel
                    {
                        Price = i.Price,
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                    });
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(goodsM.ToPagedList(pageNumber, pageSize));
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