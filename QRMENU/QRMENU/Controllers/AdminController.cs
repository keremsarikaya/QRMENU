using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using QRCoder;
using QRMENU.Models;

namespace QRMENU.Controllers
{
    public class AdminController : Controller
    {
        qrpandemenu_Entities c = new qrpandemenu_Entities();
        // GET: Admin
        [Route("kategoriler")]
        public ActionResult Index()
        {
            if (Session["login"] != null)
            {
                int usr = Convert.ToInt32(Session["login"]);
                var kat = c.kategoriler.Where(x => x.user_id == usr).ToList();
                return View(kat);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }
        [Route("subeler")]
        public ActionResult Branch()
        {
            if (Session["login"] != null)
            {
                int usr = Convert.ToInt32(Session["login"]);
                var res = c.restaurant.Where(x => x.user_id == usr).ToList();
                return View(res);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
        [Route("subeekle")]
        public ActionResult BranchAdd()
        {
            if (Session["login"] != null)
            {
                int usr = Convert.ToInt32(Session["login"]);
                var user = c.admin.Where(x => x.id == usr).FirstOrDefault();
                if (user.user_pay != false)
                {
                    if (user.user_branch == false)
                    {
                        return View();
                    }
                    else
                    {
                        TempData["brancerror"] = " ";
                        return RedirectToAction("Branch", "Admin");
                    }
                }
                else
                {
                    TempData["branchpay"] = " ";
                    return RedirectToAction("Branch", "Admin");
                }
                
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BranchAdd(restaurant r, HttpPostedFileBase branchimg)
        {

            if (Session["login"] != null)
            {

                // ŞUBE EKLEME
                int usr = Convert.ToInt32(Session["login"]);
                var user = c.admin.Where(x => x.id == usr).FirstOrDefault();
                user.user_branch = true;
                r.user_id = usr;
                c.restaurant.Add(r);
                c.SaveChanges();
                // ŞUBE EKLEME SON
                string brncimg = null;
                if (!ModelState.IsValid)
                {
                    return View("Branch");
                }
                if (branchimg != null)
                {
                    string img = Path.GetFileName(branchimg.FileName); // dosyanın ismini al
                    brncimg = Guid.NewGuid().ToString() + "-" + img;
                    string yol = Path.Combine(Server.MapPath("~/resimler/branch"), brncimg); // dosyanın kaydedileceği yer
                                                                                             // DOSYA YÜKLENDİ
                    branchimg.SaveAs(yol);
                }
                else
                {
                    ViewBag.uyari = "lütfen resim ekleyiniz";
                }
                ResimEkleme branc = new ResimEkleme();
                branc.branchimgAdd(brncimg, r.sube_id);
                TempData["branchsuccess"] = " ";
                return RedirectToAction("Branch", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
        [Route("kategoriekle")]
        public ActionResult CatAdd()
        {
            if (Session["login"] != null)
            {
                int usrid = Convert.ToInt32(Session["login"]);
                var user = c.admin.Where(x => x.id == usrid).FirstOrDefault();
                if (user.user_pay != false)
                {
                    List<SelectListItem> rest = (from i in c.restaurant.Where(x => x.user_id == usrid).ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = i.sube_adi,
                                                     Value = i.sube_id.ToString()
                                                 }).ToList();

                    ViewBag.restaurant = rest;
                    return View();
                }
                else
                {
                    TempData["catpay"] = " ";
                    return RedirectToAction("Index", "Admin");
                }
                

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CatAdd(kategoriler p2, HttpPostedFileBase catimg)
        {
            if (Session["login"] != null)
            {
                int usrid = Convert.ToInt32(Session["login"]);
                if (!ModelState.IsValid)
                {
                    return View("CatAdd");
                }

                // KATEGORİ EKLEME
                var restaurant = c.restaurant.Where(x => x.sube_id == p2.restaurant.sube_id).FirstOrDefault();
                var branc = c.restaurant.Where(x => x.sube_id == p2.restaurant.sube_id).FirstOrDefault();
                p2.restaurant = branc;
                p2.user_id = usrid;
                c.kategoriler.Add(p2);       
                c.SaveChanges();               
                // KATEGORİ EKLEME SON

                string catrsm = null;
                if (catimg != null)
                {
                    string img = Path.GetFileName(catimg.FileName); // dosyanın ismini al
                    catrsm = Guid.NewGuid().ToString() + "-" + img;
                    string yol = Path.Combine(Server.MapPath("~/resimler/kategori"), catrsm); // dosyanın kaydedileceği yer
                                                                                              // DOSYA YÜKLENDİ
                    catimg.SaveAs(yol);
                }
                else
                {
                    ViewBag.uyari = "lütfen resim ekleyiniz";
                }
                
                ResimEkleme categoryimage = new ResimEkleme();
                categoryimage.catresimEkle(catrsm, p2.katid);
                TempData["cataddsuccess"] = " ";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        
        [Route("kategoriguncelle/{id}")]
       
        public ActionResult CatUpdate(int id)
        {
            if (Session["login"] != null)
            {
                var cat = c.kategoriler.Find(id);
                return View(cat);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }
        [ValidateAntiForgeryToken]
        public ActionResult CUpdate(kategoriler p)
        {
            if (Session["login"] != null)
            {
                var cat = c.kategoriler.Find(p.katid);
                cat.katadi = p.katadi;
                c.SaveChanges();
                TempData["catupdate"] = " ";
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [Route("kategorisil/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (Session["login"] != null)
            {
                catUPI dlt = new catUPI();
                dlt.catDelete(id);             
                TempData["catdelete"] = " ";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [Route("urunler")]
        public ActionResult Product()
        {
            if (Session["login"] != null)
            {
                int usrid = Convert.ToInt32(Session["login"]);
                var urunler = c.urunler.Where(x => x.use_id == usrid).ToList();
                return View(urunler);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
        [Route("urunekle")]
        public ActionResult ProdAdd()
        {
            if (Session["login"] != null)
            {
                int usrid = Convert.ToInt32(Session["login"]);
                var user = c.admin.Where(x => x.id == usrid).FirstOrDefault();
                if (user.user_pay != false)
                {
                    List<SelectListItem> deger = (from i in c.kategoriler.Where(x => x.user_id == usrid).ToList()
                                                  select new SelectListItem
                                                  {
                                                      Text = i.katadi,
                                                      Value = i.katid.ToString()
                                                  }).ToList();
                    List<SelectListItem> branc = (from i in c.restaurant.Where(x => x.user_id == usrid).ToList()
                                                  select new SelectListItem
                                                  {
                                                      Text = i.sube_adi,
                                                      Value = i.sube_id.ToString()
                                                  }).ToList();
                    ViewBag.dgr = deger;
                    ViewBag.res = branc;
                    return View();
                }
                else
                {
                    TempData["prodpay"] = " ";
                    return RedirectToAction("Product", "Admin");
                }
                
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProdAdd(urunler p3, HttpPostedFileBase urunresim)
        {
            if (Session["login"] != null)
            {
                int usrid = Convert.ToInt32(Session["login"]);
                string resimad = null;
                var kt = c.kategoriler.Where(x => x.katid == p3.kategoriler.katid).FirstOrDefault();
                p3.kategoriler = kt;
                var branc = c.restaurant.Where(x => x.sube_id == p3.restaurant.sube_id).FirstOrDefault();
                p3.restaurant = branc;
                p3.use_id = usrid;
                if (urunresim != null)
                {
                    string pic = Path.GetFileName(urunresim.FileName); // dosyanın ismini al
                    resimad = Guid.NewGuid().ToString() + "-" + pic;
                    string path = Path.Combine(Server.MapPath("~/resimler"), resimad); // dosyanın kaydedileceği yer
                                                                                       // DOSYA YÜKLENDİ
                    urunresim.SaveAs(path);
                }
                else
                {
                    ViewBag.uyari = "lütfen resim ekleyiniz";
                }

                c.urunler.Add(p3);
                c.SaveChanges();
                ResimEkleme resim = new ResimEkleme();
                resim.resimekle(resimad, p3.urunid);
                TempData["productadd"] = " ";

                return RedirectToAction("Product");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [Route("urunguncelle/{id}")]
        public ActionResult PrUpdate(int id)
        {
            if (Session["login"] != null)
            {
                var urun = c.urunler.Find(id);
                List<SelectListItem> deger = (from i in c.kategoriler.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.katadi,
                                                  Value = i.katid.ToString()
                                              }
                                              ).ToList();
                ViewBag.degerler = deger;
                return View(urun);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult PUpdate(urunler p1)
        {
            if (Session["login"] != null)
            {
                var products = c.urunler.Find(p1.urunid);
                products.urunadi = p1.urunadi;
                products.urunfiyat = p1.urunfiyat;
                products.urunaciklama = p1.urunaciklama;
                var ctgr = c.kategoriler.Where(x => x.katid == p1.kategoriler.katid).FirstOrDefault();
                products.kategoriid = ctgr.katid;
                c.SaveChanges();
                TempData["productupdate"] = " ";
                return RedirectToAction("Product");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [Route("urunsil/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult PDelete(int id)
        {
            if (Session["login"] != null)
            {
                prodUDI del = new prodUDI();
                del.proDelete(id);
                TempData["prodelete"] = " ";
                return RedirectToAction("Product");

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
        public ActionResult QRCode(int id)
        {
            if (Session["login"] != null)
            {
                var res = c.restaurant.Find(id);
                ViewBag.url = "www.qrpandemenu.com/" + Seo.AdresDuzenle(res.sube_adi) + "/kategoriler" + res.sube_id;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QRCode(string qrcode)
        {
            if (Session["login"] != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qRCodeGenerator.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    using (Bitmap bitmap = qrCode.GetGraphic(20))
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());

                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

    }
}