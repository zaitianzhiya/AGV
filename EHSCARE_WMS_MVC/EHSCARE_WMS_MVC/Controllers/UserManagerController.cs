using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EHSCARE_WMS.Models;

namespace EHSCARE_WMS.Controllers
{
    public class UserManagerController : Controller
    {
        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess(MvcApplication.conStr);
        List<User> lstUsers = new List<User>();
        //
        // GET: /UserManager/

        public ActionResult Index()
        {
            ViewBag.Form = "U";
            ViewData["DisplayData"] = lstUsers;
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            dataAccess = new DataAccess.DataAccess(MvcApplication.conStr);
            ViewBag.Form = "U";
            DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(@"SELECT tui.*,tgi.GROUP_NAME FROM TB_USER_INFO AS tui 
                                                                    LEFT JOIN TB_GROUP_INFO AS tgi ON tui.USER_GROUPID=tgi.ID", null);
            User user;
            foreach (DataRow dr in dtData.Rows)
            {
                user = new User();
                user.USER_NO = dr["USER_NO"].ToString();
                user.USER_NAME = dr["USER_NAME"].ToString();
                user.USER_PWD = dr["USER_PWD"].ToString();
                if (!string.IsNullOrEmpty(dr["USER_GROUPID"].ToString()))
                {
                    user.USER_GROUPID = int.Parse(dr["USER_GROUPID"].ToString());
                }
                if (!string.IsNullOrEmpty(dr["USER_IS_USED"].ToString()))
                {
                    user.USER_IS_USED = int.Parse(dr["USER_IS_USED"].ToString());
                }
                user.GROUP_NAME = dr["GROUP_NAME"].ToString();
                lstUsers.Add(user);
            }
            ViewData["DisplayData"] = lstUsers;
            return View();
        }
    }
}
