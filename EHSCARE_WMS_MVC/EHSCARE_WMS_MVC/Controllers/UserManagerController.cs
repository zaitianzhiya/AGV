using System;
using System.Collections.Generic;
using System.Data;
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
        //
        // GET: /UserManager/

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
                string txtNo = Request.Form["txtNo"];
                string txtName = Request.Form["txtName"];
                string txtGroup = Request.Form["txtGroup"];
                ViewBag.Group = txtGroup;
                ViewBag.UserNo = txtNo;
                ViewBag.UserName = txtName;

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
            ViewBag.Group = txtGroup;
            ViewBag.UserNo = txtNo;
            ViewBag.UserName = txtName;

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
    }
}
