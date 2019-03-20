using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EHSCARE_WMS.Models;

namespace EHSCARE_WMS.Controllers
{
    public class StockManagerController : Controller
    {
        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess(MvcApplication.conStr);
        List<WareHouse> lstData = new List<WareHouse>();

        public ActionResult Index()
        {
            ViewBag.Form = "K";
            return View();
        }

    }
}
