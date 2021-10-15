using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRMENU.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult PageError()
        {
            Response.TrySkipIisCustomErrors = true;
            ViewBag.Kaynak = "Hatalı adres";
            return View(ViewBag.Kaynak);
        }
        public ActionResult Page404()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}