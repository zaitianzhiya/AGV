using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHSCARE_WMS.Controllers
{
    public class PrivilegeController : Controller
    {
        //
        // GET: /Privilege/

        public ActionResult Index()
        {
            ViewBag.Form = "P";
            return View();
        }

    }
}
