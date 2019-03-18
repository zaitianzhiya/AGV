using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHSCARE_WMS.Controllers
{
    public class StoreManagerController : Controller
    {
        //
        // GET: /StoreManager/

        public ActionResult Index()
        {
            ViewBag.Form = "S";
            return View();
        }

    }
}
