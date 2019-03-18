using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHSCARE_WMS.Controllers
{
    public class StoreExchangeController : Controller
    {
        //
        // GET: /StoreExchange/

        public ActionResult Index()
        {
            ViewBag.Form = "E";
            return View();
        }

    }
}
