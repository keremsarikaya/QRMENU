using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRMENU.Models;

namespace QRMENU.Controllers
{
    public class HomeController : Controller
    {
        qrpandemenu_Entities c = new qrpandemenu_Entities();

        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("iletisim")]
        public ActionResult Offer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Offer(teklif p)
        {
            p.ifread = false;
            c.teklif.Add(p);
            c.SaveChanges();
            TempData["teklif"] = " ";
            return RedirectToAction("Index", "Home");
        }

        [Route("{deger}/kategoriler{id}")]
        public ActionResult Category(int id)
        {
            var cat = c.kategoriler.Where(x => x.rest_id == id).ToList();
            return View(cat);
        }


        /// <summary>
        /// List<urunler> urunler = c.urunler.Include("resim").Where(x => x.kategoriid == x.kategoriler.katid).ToList();
        /// var urunler = c.urunler.Where(x => x.rest_id == id && x.kategoriid == x.kategoriler.katid).ToList();
        /// ViewBag.kat = cat;
        /// </summary>
        

        [Route("{deger}/urunler{id}")]
        public ActionResult CatProd(int id)
        {
            var catproducts = c.urunler.Where(x => x.kategoriid == id).ToList();
            var kat = c.kategoriler.ToList();
            ViewBag.kat = kat;
            return View(catproducts);
        }
    }
}