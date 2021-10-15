using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRMENU.Models;

namespace QRMENU.Controllers
{
    public class CeoController : Controller
    {
        qrpandemenu_Entities c = new qrpandemenu_Entities();
        // GET: Ceo

        [Route("userlist")]
        public ActionResult Index()
        {
            if (Session["ceo"] != null)
            {
                var users = c.admin.ToList();
                return View(users);
            }
            else
            {
                return RedirectToAction("CeoLogin", "Login");
            }
            
        }

        [Route("offerlist")]
        public ActionResult OfferList()
        {
            if (Session["ceo"] != null)
            {
                var offers = c.teklif.ToList();
                return View(offers);
            }
            else
            {
                return RedirectToAction("CeoLogin", "Login");
            }
        }

        [Route("offerread/{id}")]
        public ActionResult OfferRead(int id)
        {
            if (Session["ceo"] != null)
            {
                var offer = c.teklif.Find(id);
                return View(offer);
            }
            else
            {
                return RedirectToAction("CeoLogin", "Login");
            }
        }

        [Route("readed/{id}")]
        public ActionResult Readed(int id)
        {
            if (Session["ceo"] != null)
            {
                var ofer = c.teklif.Find(id);
                ofer.ifread = true;
                c.SaveChanges();
                return RedirectToAction("OfferList", "Ceo");
            }
            else
            {
                return RedirectToAction("CeoLogin", "Login");
            }
            
        }

        [Route("useradd")]
        public ActionResult UserAdd()
        {
            if (Session["ceo"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("CeoLogin", "Login");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserAdd(admin a)
        {
            if (Session["ceo"] != null)
            {
                a.user_branch = false;
                a.user_pay = true;
                c.admin.Add(a);
                c.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("CeoLogin", "Login");
            }

        }

        [Route("branches")]
        public ActionResult Branches()
        {
            if (Session["ceo"] != null)
            {
                var branches = c.restaurant.ToList();
                return View(branches);
            }
            else
            {
                return RedirectToAction("CeoLogin", "Login");
            }

        }

        [Route("branchdel/{id}")]
        public ActionResult BranchDel(int id)
        {
            var del = c.restaurant.Find(id);
            del.admin.user_branch = false;
            c.restaurant.Remove(del);
            c.SaveChanges();
            TempData["branchdel"] = " ";
            return RedirectToAction("Branches", "Ceo");
        }

        [Route("userupdate/{id}")]
        public ActionResult UserUpdate(int id)
        {
            if (Session["ceo"] != null)
            {
                var user = c.admin.Find(id);
                return View(user);
            }
            else
            {
                return RedirectToAction("CeoLogin", "Login");
            }
        }

        public ActionResult UUpdate(admin p1)
        {
            if (Session["ceo"] != null)
            {
                var userr = c.admin.Find(p1.id);
                userr.username = p1.username;
                userr.password = p1.password;
                userr.user_tel = p1.user_tel;
                userr.user_mail = p1.user_mail;
                userr.isim = p1.isim;
                userr.soyisim = p1.soyisim;
                userr.user_branch = p1.user_branch;
                userr.user_pay = p1.user_pay;
                c.SaveChanges();
                return RedirectToAction("Index", "Ceo");
            }
            else
            {
                return RedirectToAction("CeoLogin", "Login");
            }
        }
    }
}