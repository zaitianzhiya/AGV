using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EHSCARE_WMS.Models;

namespace EHSCARE_WMS.Controllers
{
    public class UserManagerController : BaseController
    {
        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess(MvcApplication.conStr);
        List<User> lstUsers = new List<User>();
       
        public ActionResult Index()
        {
            ViewBag.Form = "U";
            ViewBag.AllGroup = base.GetGroup();
            DataModel<User> model = new DataModel<User>();
            string page = Request.QueryString["page"];
            if (page == null)
            {
                model.PageIndex = 1;
                model.PageSize = base.PageSize;
                model.TotalNum = 0;
            }
            else
            {
                string txtNo = TempData["txtNo"].ToString();
                string txtName = TempData["txtName"].ToString();
                string txtGroup = TempData["txtGroup"].ToString();

                string sql = @"SELECT tui.*,
                                   tgi.GROUP_NAME
                            FROM   TB_USER_INFO             AS tui
                                   LEFT JOIN TB_GROUP_INFO  AS tgi
                                        ON  tui.USER_GROUPID = tgi.ID 
                            WHERE 1=1 ";
                if (!string.IsNullOrEmpty(txtNo))
                {
                    sql += string.Format(" AND tui.USER_NO LIKE '%{0}%' ", txtNo);
                }
                if (!string.IsNullOrEmpty(txtName))
                {
                    sql += string.Format(" AND tui.USER_NAME LIKE '%{0}%' ", txtName);
                }
                if (!string.IsNullOrEmpty(txtGroup))
                {
                    sql += string.Format(" AND tgi.GROUP_NO = '{0}' ", txtGroup);
                }
                DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(sql, null);

                model.PageIndex = int.Parse(page);
                model.PageSize = base.PageSize;
                model.TotalNum = dtData.Rows.Count;

                User user;
                foreach (DataRow dr in dtData.AsEnumerable().Skip((model.PageIndex-1) * model.PageSize).Take(model.PageSize))
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
                TempData["txtNo"] = txtNo;
                TempData["txtName"] = txtName;
                TempData["txtGroup"] = txtGroup;

            }
            model.Products = lstUsers;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            ViewBag.Form = "U";
            ViewBag.AllGroup = base.GetGroup();
            DataModel<User> model = new DataModel<User>();
            string page = Request.QueryString["page"];
            if (string.IsNullOrEmpty(page))
            {
                page = "1";
            }
            string txtNo = fc["txtNo"];
            string txtName = fc["txtName"];
            string txtGroup = fc["txtGroup"];
           
            TempData["txtNo"] = ViewBag.UserNo = txtNo;
            TempData["txtName"] = ViewBag.UserName = txtName;
            TempData["txtGroup"] = ViewBag.Group = txtGroup;

            string sql = @"SELECT tui.*,
                                   tgi.GROUP_NAME
                            FROM   TB_USER_INFO             AS tui
                                   LEFT JOIN TB_GROUP_INFO  AS tgi
                                        ON  tui.USER_GROUPID = tgi.ID 
                            WHERE 1=1 ";
            if (!string.IsNullOrEmpty(txtNo))
            {
                sql += string.Format(" AND tui.USER_NO LIKE '%{0}%' ", txtNo);
            }
            if (!string.IsNullOrEmpty(txtName))
            {
                sql += string.Format(" AND tui.USER_NAME LIKE '%{0}%' ", txtName);
            }
            if (!string.IsNullOrEmpty(txtGroup))
            {
                sql += string.Format(" AND tgi.GROUP_NO = '{0}' ", txtGroup);
            }
            DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(sql, null);

            model.PageIndex = int.Parse(page);
            model.PageSize = base.PageSize;
            model.TotalNum = dtData.Rows.Count;

            User user;
            foreach (DataRow dr in dtData.AsEnumerable().Skip((model.PageIndex-1) * model.PageSize).Take(model.PageSize))
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
            model.Products = lstUsers;
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateUser(User user)
        {
            if (int.Parse(
                dataAccess.ExecuteScalar_Text("SELECT COUNT(1) FROM TB_USER_INFO AS tui WHERE tui.USER_NO=@USER_NO",
                    new SqlParameter[]
                    {
                        DataAccess.DataAccess.CreateParameter("USER_NO", SqlDbType.VarChar, user.USER_NO)
                    }).ToString()) == 0)
            {
                return Json("编号不存在", JsonRequestBehavior.AllowGet);
            }
            dataAccess.ExecuteNonQuery_Text(@"UPDATE TB_USER_INFO
                                                SET	[USER_NAME] =@USER_NAME,
	                                                USER_PWD = @USER_PWD,
	                                                UPDATE_TIME = GETDATE(),
	                                                UPDATE_USERID = @UPDATE_USERID,
	                                                USER_GROUPID = @USER_GROUPID
                                                WHERE USER_NO=@USER_NO", new SqlParameter[]
            {
                DataAccess.DataAccess.CreateParameter("USER_NAME", SqlDbType.VarChar, user.USER_NAME),
                DataAccess.DataAccess.CreateParameter("USER_PWD", SqlDbType.VarChar, user.USER_PWD),
                DataAccess.DataAccess.CreateParameter("UPDATE_USERID", SqlDbType.VarChar, Session["UID"]),
                DataAccess.DataAccess.CreateParameter("USER_GROUPID", SqlDbType.Int, user.USER_GROUPID),
                DataAccess.DataAccess.CreateParameter("USER_NO", SqlDbType.VarChar, user.USER_NO)
            });

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUser(User user)
        {
            if (int.Parse(
                dataAccess.ExecuteScalar_Text("SELECT COUNT(1) FROM TB_USER_INFO AS tui WHERE tui.USER_NO=@USER_NO",
                    new SqlParameter[]
                    {
                        DataAccess.DataAccess.CreateParameter("USER_NO", SqlDbType.VarChar, user.USER_NO)
                    }).ToString()) > 0)
            {
                return Json("编号已存在", JsonRequestBehavior.AllowGet);
            }
            dataAccess.ExecuteNonQuery_Text(@"INSERT INTO TB_USER_INFO
                                                (
	                                                USER_NO,
	                                                [USER_NAME],
	                                                USER_PWD,
	                                                CREATE_TIME,
	                                                CREATE_USERID,
	                                                USER_GROUPID,
	                                                USER_IS_USED
                                                )
                                                VALUES
                                                (
	                                                @USER_NO,
	                                                @USER_NAME,
	                                                @USER_PWD,
	                                                GETDATE(),
	                                                @CREATE_USERID,
	                                                @USER_GROUPID,
	                                                1
                                                )", new SqlParameter[]
            {
                DataAccess.DataAccess.CreateParameter("USER_NO", SqlDbType.VarChar, user.USER_NO),
                DataAccess.DataAccess.CreateParameter("USER_NAME", SqlDbType.VarChar, user.USER_NAME),
                DataAccess.DataAccess.CreateParameter("USER_PWD", SqlDbType.VarChar, user.USER_PWD),
                DataAccess.DataAccess.CreateParameter("CREATE_USERID", SqlDbType.VarChar, Session["UID"]),
                DataAccess.DataAccess.CreateParameter("USER_GROUPID", SqlDbType.Int, user.USER_GROUPID)
            });

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DelUser(string Nos)
        {
            string sql = string.Format("DELETE FROM TB_USER_INFO WHERE USER_NO IN ({0})", Nos.TrimEnd(','));
          
            int i = dataAccess.ExecuteNonQuery_Text(sql,null);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}
