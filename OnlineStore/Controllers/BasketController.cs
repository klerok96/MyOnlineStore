using OnlineStore.Models;
using OnlineStore.Models.StoreDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        OnlineStoreEntities db = new OnlineStoreEntities();
        
        public ActionResult Index()
        {
            var loginCookie = User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.Login == loginCookie);

            var basketM = new List<BasketModel>();

            var basket = db.Basket.Include(b => b.Goods);

            foreach (var i in basket)
            {
                basketM.Add(new BasketModel
                {
                    BasketId = i.BasketId,
                    BasketName = i.Goods.ProductName,
                    Price = i.Goods.Price * i.Quantity,
                    Quantity = i.Quantity
                });
            }

            return View(basketM);
        }

        [HttpPost]
        public string AddToBasket(int id, int quantity)
        {
            if (quantity != 0)
            {
                var loginCookie = User.Identity.Name;
                var user = db.Users.FirstOrDefault(u => u.Login == loginCookie);

                var findProduct = db.Basket.FirstOrDefault(f => f.ProductId == id && f.UserId == user.UserId);

                if (findProduct != null)
                    findProduct.Quantity += quantity;
                else
                    db.Basket.Add(new Basket
                    {
                        ProductId = id,
                        Quantity = quantity,
                        UserId = user.UserId
                    });

                db.SaveChanges();

                return "Добавлено в корзину";
            }

            return "Выберите количество";
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