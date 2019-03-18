using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHSCARE_WMS.Controllers
{
    public class StoreInController : Controller
    {
        //
        // GET: /StoreIn/

        public ActionResult Index()
        {
            ViewBag.Form = "I";
            return View();
        }

    }
}
