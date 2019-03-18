using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHSCARE_WMS.Controllers
{
    public class StoreOutController : Controller
    {
        //
        // GET: /StoreOut/

        public ActionResult Index()
        {
            ViewBag.Form = "O";
            return View();
        }

    }
}
