using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EHSCARE_WMS.Controllers
{
    public class LoginController : Controller
    {
        DataAccess.DataAccess dataAccess;
        //
        // GET: /Login/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string uid = fc["txtUid"];
            string pwd = fc["txtPwd"];

            dataAccess = new DataAccess.DataAccess(MvcApplication.conStr);

            int count =
                int.Parse(
                    dataAccess.ExecuteScalar_Text(
                        "SELECT COUNT(1) FROM TB_USER_INFO AS tui WHERE USER_NO=@USER_NO AND USER_PWD=@USER_PWD",
                        new SqlParameter[]
                        {
                            DataAccess.DataAccess.CreateParameter("USER_NO", SqlDbType.VarChar, uid),
                            DataAccess.DataAccess.CreateParameter("USER_PWD", SqlDbType.VarChar, pwd)
                        }).ToString());
            if (count ==0)
            {
                ViewBag.js = "<script type='text/javascript'>alert('帐号或密码错误！');</script>";
                ViewBag.Uid = uid;
                ViewBag.Pwd = pwd;
                return View();
            }
            Session["UID"] = uid;
            return RedirectToAction("Index", "Base");
        }
    }
}
