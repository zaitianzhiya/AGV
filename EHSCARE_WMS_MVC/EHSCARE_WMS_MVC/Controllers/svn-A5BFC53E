using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHSCARE_WMS.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Form = "";
            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UID"] == null)
            {
                filterContext.Result = new RedirectResult(Url.Action("Login", "Login"));
            }

            base.OnActionExecuting(filterContext);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
