using OnlineStore.Models;
using OnlineStore.Models.StoreDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private OnlineStoreEntities db = new OnlineStoreEntities();

        // GET: Manage
        public ActionResult Edit()
        {
            var loginCookie = User.Identity.Name;
            User user = db.Users.FirstOrDefault(u => u.Login == loginCookie);

            ManageModel userM = new ManageModel
            {
                Login = user.Login,
                Email = user.Email,
                FullName = user.FullName,
                Money = user.Money,
                Password = user.Password,
                UserId = user.UserId
            };

            return View(userM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManageModel model)
        {
            if (ModelState.IsValid)
            {
                var aloneUser = db.Users.FirstOrDefault(u => u.Login == model.Login && u.UserId != model.UserId || u.Email == model.Email && u.UserId != model.UserId);

                if (aloneUser == null)
                {
                    var currentUser = db.Users.FirstOrDefault(u => u.UserId == model.UserId);
                    if (currentUser == null)
                        return HttpNotFound();

                    if (model.Password != currentUser.Password)
                    {
                        string salt = Hashing.Hashing.CreateSalt();
                        string hashPass = Hashing.Hashing.GenerateHash(model.Password + salt);
                        currentUser.Password = hashPass;
                        currentUser.Salt = salt;
                    }

                    currentUser.Login = model.Login;
                    currentUser.Email = model.Email ?? "";
                    currentUser.FullName = model.FullName ?? "";
                    currentUser.Money = model.Money;

                    db.SaveChanges();

                    ViewBag.Suc = true;
                }
                else
                    ModelState.AddModelError("", "Пользователь с такими логином или email уже существует");
            }
            else
                ViewBag.Suc = false;

            return View(model);
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