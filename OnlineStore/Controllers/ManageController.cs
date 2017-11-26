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

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var aloneUser = db.Users.FirstOrDefault(u => u.Login == user.Login && u.UserId != user.UserId || u.Email == user.Email && u.UserId != user.UserId);

                if (aloneUser == null)
                {
                    var currentUser = db.Users.FirstOrDefault(u => u.UserId == user.UserId);
                    if (currentUser == null)
                        return HttpNotFound();

                    if (user.Password != currentUser.Password)
                    {
                        string salt = Hashing.Hashing.CreateSalt();
                        string hashPass = Hashing.Hashing.GenerateHash(user.Password + salt);
                        currentUser.Password = hashPass;
                        currentUser.Salt = salt;
                    }

                    currentUser.Login = user.Login;
                    currentUser.Email = user.Email ?? "";
                    currentUser.FullName = user.FullName ?? "";
                    currentUser.Money = user.Money;

                    db.SaveChanges();

                    ViewBag.Suc = true;
                }
            }
            else
                ViewBag.Suc = false;

            return View(user);
        }
    }
}