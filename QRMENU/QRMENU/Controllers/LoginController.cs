using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRMENU.Models;

namespace QRMENU.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        qrpandemenu_Entities c = new qrpandemenu_Entities();

        [HttpGet]
        [Route("giris")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            var user = c.admin.Where(x => x.username == username && x.password == password).FirstOrDefault();
            
            if (user != null)
            {
                Session["login"] = user.id;
                TempData["loginsuccess"] = " ";
                return RedirectToAction("Branch", "Admin");
            }
            else
            {
                TempData["uyari"] = " ";
                ViewBag.uyari = "Kullanıcı adı veya şifre hatalı";
                return View();
            }
            
        }
        [HttpGet]
        [Route("kayitol")]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(admin user)
        {
            user.user_branch = false;
            user.user_pay = true;
            c.admin.Add(user);
            c.SaveChanges();
            TempData["register"] = " ";
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("ceogiris")]
        public ActionResult CeoLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CeoLogin(string username, string password)
        {
            var ceo = c.ceolar.Where(x => x.username == username && x.password == password).FirstOrDefault();
            if (ceo != null)
            {
                Session["ceo"] = ceo.id;
                return RedirectToAction("Index", "Ceo");
            }
            else
            {
                TempData["ceohata"] = " ";
                return View();
            }
            
        }

        public ActionResult Logout()
        {
            Session["login"] = null;
            return RedirectToAction("Login", "Login");
        }
    }
}