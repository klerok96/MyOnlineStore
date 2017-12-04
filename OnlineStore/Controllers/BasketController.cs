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
            var basket = db.Basket.Include(b => b.Goods).Where(b => b.UserId == user.UserId);

            var totalAmount = new decimal();

            foreach (var i in basket)
            {
                basketM.Add(new BasketModel
                {
                    BasketId = i.BasketId,
                    BasketName = i.Goods.ProductName,
                    Price = i.Goods.Price,
                    Quantity = i.Quantity 
                });

                totalAmount += i.Quantity * i.Goods.Price; 
            }

            ViewBag.TotalAmount = totalAmount.ToString()+ " руб";

            return View(basketM);
        }

        [HttpPost]
        public ActionResult AddToBasket(int id, int quantity)
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

            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var productOfB = db.Basket.Find(id);
            db.Basket.Remove(productOfB);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Purchase()
        {
            var loginCookie = User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.Login == loginCookie);

            var basketOfUser = db.Basket
                .Where(b => b.UserId == user.UserId)
                .ToList();

            if (basketOfUser.Count != 0)
            {
                Goods product;
                foreach (var i in basketOfUser)
                {
                    product = db.Goods.FirstOrDefault(p => p.ProductId == i.ProductId);

                    product.Quantity -= i.Quantity;
                }

                db.Basket.RemoveRange(basketOfUser);

                db.SaveChanges();

                ViewBag.Message = "Покупка завершена";
            }
            else
                ViewBag.Message = "Корзина пуста";

            return View();
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