using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EHSCARE_WMS.Models;

namespace EHSCARE_WMS.Controllers
{
    public class BaseController : Controller
    {
        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess(MvcApplication.conStr);

        internal int PageSize = 5;

        public ActionResult Main()
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

        /// 取得所有群组方法
        /// <summary>
        /// 取得所有群组方法
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Group> GetGroup() 
        {
            DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(@"SELECT [ID]
                                                                          ,[GROUP_NO]
                                                                          ,[GROUP_NAME]
                                                                          ,[GROUP_PRIVILEGE]
                                                                          ,[CREATE_TIME]
                                                                          ,[CREATE_USERID]
                                                                          ,[UPDATE_TIME]
                                                                          ,[UPDATE_USERID]
                                                                      FROM [dbo].[TB_GROUP_INFO]", null);
            List<Group> lstGroup = new List<Group>();
            Group group = new Group();
            group.GROUP_NO ="";
            group.GROUP_NAME ="";
            group.GROUP_PRIVILEGE ="";
            lstGroup.Add(group);
           
            foreach (DataRow dr in dtData.Rows)
            {
                group = new Group();
                group.GROUP_NO = dr["GROUP_NO"].ToString();
                group.GROUP_NAME = dr["GROUP_NAME"].ToString();
                group.GROUP_PRIVILEGE = dr["GROUP_PRIVILEGE"].ToString();
                lstGroup.Add(group);
            }
            return lstGroup;
        }

        /// 取得所有仓库方法
        /// <summary>
        /// 取得所有仓库方法
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Store> GetStore()
        {
            DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(@"SELECT * FROM TB_STORE AS ts", null);
            List<Store> lstStore = new List<Store>();
            Store store = new Store();
            store.STORE_NO = "";
            store.STORE_NAME = "";
            lstStore.Add(store);

            foreach (DataRow dr in dtData.Rows)
            {
                store = new Store();
                store.STORE_NO = dr["STORE_NO"].ToString();
                store.STORE_NAME = dr["STORE_NAME"].ToString();
                lstStore.Add(store);
            }
            return lstStore;
        }
    }
}
