using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHSCARE_WMS.Controllers
{
    public class StoreBackController : Controller
    {
        //
        // GET: /StoreBack/

        public ActionResult Index()
        {
            ViewBag.Form = "B";
            return View();
        }

    }
}
