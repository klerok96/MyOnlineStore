using OnlineStore.Models;
using OnlineStore.Models.StoreDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Routing;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        OnlineStoreEntities db;
        User currentUser;

        protected override void Initialize(RequestContext requestContext)
        {
            db = new OnlineStoreEntities();

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string loginCookie = requestContext.HttpContext.User.Identity.Name;
                currentUser = db.Users.FirstOrDefault(u => u.Login == loginCookie);
            }

            base.Initialize(requestContext);
        }
        
        public ActionResult Index()
        {
            var basketM = new List<BasketViewModel>();
            var basket = db.Basket.Include(b => b.Goods).Where(b => b.UserId == currentUser.UserId);

            var totalAmount = new decimal();

            foreach (var i in basket)
            {
                basketM.Add(new BasketViewModel
                {
                    BasketId = i.BasketId,
                    BasketName = i.Goods.ProductName,
                    Price = i.Goods.Price,
                    Quantity = i.Quantity 
                });

                totalAmount += i.Quantity * i.Goods.Price; 
            }

            if (currentUser.Money < totalAmount)
                ViewBag.Message = "Не хватает денег!";

            ViewBag.TotalAmount = totalAmount + " руб";

            return View(basketM.OrderByDescending(b => b.BasketId));
        }

        [HttpPost]
        public ActionResult AddToBasket(int id, int quantity)
        {
            var findProduct = db.Basket.FirstOrDefault(f => f.ProductId == id && f.UserId == currentUser.UserId);
     
            if (findProduct != null)
                findProduct.Quantity += quantity;
            else
                db.Basket.Add(new Basket
                {
                    ProductId = id,
                    Quantity = quantity,
                    UserId = currentUser.UserId
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
            var basketOfUser = db.Basket
                .Where(b => b.UserId == currentUser.UserId)
                .ToList();

            if (basketOfUser.Count != 0)
            {
                Goods product;
                PurchaseStatistics statistics;
                decimal sumPurchase = 0;

                foreach (var i in basketOfUser)
                {
                    product = db.Goods.FirstOrDefault(p => p.ProductId == i.ProductId);
                    product.Quantity -= i.Quantity;

                    sumPurchase += product.Price * i.Quantity;

                    statistics = new PurchaseStatistics
                    {
                        ProductId = product.ProductId,
                        UserId = currentUser.UserId,
                        Quantity = i.Quantity,
                        Date = DateTime.Now,
                        Price = product.Price * i.Quantity
                    };

                    db.PurchaseStatistics.Add(statistics);
                }

                currentUser.Money -= sumPurchase;
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