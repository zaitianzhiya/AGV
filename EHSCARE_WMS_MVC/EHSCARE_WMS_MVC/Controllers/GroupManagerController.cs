using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHSCARE_WMS.Controllers
{
    public class GroupManagerController : Controller
    {
        //
        // GET: /GroupManager/

        public ActionResult Index()
        {
            ViewBag.Form = "G";
            return View();
        }

    }
}
