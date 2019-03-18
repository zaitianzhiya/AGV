using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHSCARE_WMS.Controllers
{
    public class WareHouseManagerController : Controller
    {
        //
        // GET: /WareHouseManager/

        public ActionResult Index()
        {
            ViewBag.Form = "W";
            return View();
        }

    }
}
