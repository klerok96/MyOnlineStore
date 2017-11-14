using OnlineStore.Models;
using OnlineStore.Models.StoreDB;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineStore.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            User user;
            Role userRole;

            using (OnlineStoreEntities db = new OnlineStoreEntities())
            {
                user = db.Users.FirstOrDefault(u => u.Login == model.Login);
                userRole = db.Roles.Find(user.RoleId);
            }

            string hashPass = Hashing.Hashing.GenerateHash(model.Password + user.Salt);

            if (hashPass == user.Password)
            {
                FormsAuthentication.SetAuthCookie(model.Login, true);

                if (userRole.RoleName == "admin")
                    return RedirectToAction("Index", "CategoriesAdmin");

                return RedirectToAction("Index", "Categories");
            }
            else
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
            }

            return View(model);
        }

        // GET: Authentication
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;

                using (OnlineStoreEntities db = new OnlineStoreEntities())
                {
                    user = db.Users.FirstOrDefault(u => u.Login == model.Login);
                }
                if (user == null)
                {
                    string salt = Hashing.Hashing.CreateSalt();
                    string hashPass = Hashing.Hashing.GenerateHash(model.Password + salt);

                    using (OnlineStoreEntities db = new OnlineStoreEntities())
                    {
                        db.Users.Add(new User {
                            Login = model.Login,
                            Password = hashPass,
                            RoleId = 2,
                            Salt = salt,
                            Email = "",
                            FullName = "",
                            Money = 1000000
                        });
                        db.SaveChanges();

                        user = db.Users.Where(u => u.Login == model.Login && u.Password == hashPass).FirstOrDefault();
                    }
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);

                        return RedirectToAction("Index", "Test");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с такими логином уже существует");
                }
            }

            return View(model);
        }
    }
}