using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHSCARE_WMS.Controllers
{
    public class StoreBorrowController : Controller
    {
        //
        // GET: /StoreBorrow/

        public ActionResult Index()
        {
            ViewBag.Form = "R";
            return View();
        }

    }
}
