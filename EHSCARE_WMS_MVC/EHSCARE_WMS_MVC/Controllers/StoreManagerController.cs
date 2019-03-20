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
    public class StoreManagerController : BaseController
    {
        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess(MvcApplication.conStr);
        List<Store> lstData = new List<Store>();

        public ActionResult Index()
        {
            ViewBag.Form = "S";
            DataModel<Store> model = new DataModel<Store>();
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

                string sql = @"SELECT * FROM TB_STORE AS ts WHERE 1=1 ";
                if (!string.IsNullOrEmpty(txtNo))
                {
                    sql += string.Format(" AND ts.STORE_NO LIKE '%{0}%' ", txtNo);
                }
                if (!string.IsNullOrEmpty(txtName))
                {
                    sql += string.Format(" AND ts.STORE_NAME LIKE '%{0}%' ", txtName);
                }

                DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(sql, null);

                model.PageIndex = int.Parse(page);
                model.PageSize = base.PageSize;
                model.TotalNum = dtData.Rows.Count;

                Store store;
                foreach (DataRow dr in dtData.AsEnumerable().Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize))
                {
                    store = new Store();
                    store.STORE_NO = dr["STORE_NO"].ToString();
                    store.STORE_NAME = dr["STORE_NAME"].ToString();
                    lstData.Add(store);
                }
                TempData["txtNo"] = txtNo;
                TempData["txtName"] = txtName;
            }
            model.Products = lstData;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            ViewBag.Form = "S";
            DataModel<Store> model = new DataModel<Store>();
            string page = Request.QueryString["page"];
            if (string.IsNullOrEmpty(page))
            {
                page = "1";
            }
            string txtNo = fc["txtNo"];
            string txtName = fc["txtName"];
            TempData["txtNo"] = ViewBag.UserNo = txtNo;
            TempData["txtName"] = ViewBag.UserName = txtName;

            string sql = @"SELECT * FROM TB_STORE AS ts WHERE 1=1 ";
            if (!string.IsNullOrEmpty(txtNo))
            {
                sql += string.Format(" AND ts.STORE_NO LIKE '%{0}%' ", txtNo);
            }
            if (!string.IsNullOrEmpty(txtName))
            {
                sql += string.Format(" AND ts.STORE_NAME LIKE '%{0}%' ", txtName);
            }

            DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(sql, null);

            model.PageIndex = int.Parse(page);
            model.PageSize = base.PageSize;
            model.TotalNum = dtData.Rows.Count;

            Store store;
            foreach (DataRow dr in dtData.AsEnumerable().Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize))
            {
                store = new Store();
                store.STORE_NO = dr["STORE_NO"].ToString();
                store.STORE_NAME = dr["STORE_NAME"].ToString();
                lstData.Add(store);
            }
            model.Products = lstData;
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateStore(Store store)
        {
            if (int.Parse(
                dataAccess.ExecuteScalar_Text("SELECT COUNT(1) FROM TB_STORE AS ts WHERE ts.STORE_NO=@STORE_NO",
                    new SqlParameter[]
                    {
                        DataAccess.DataAccess.CreateParameter("STORE_NO", SqlDbType.VarChar, store.STORE_NO)
                    }).ToString()) == 0)
            {
                return Json("编号不存在", JsonRequestBehavior.AllowGet);
            }
            dataAccess.ExecuteNonQuery_Text(@"UPDATE TB_STORE
                                                SET STORE_NAME = @STORE_NAME,
	                                                UPDATE_TIME = GETDATE(),
	                                                UPDATE_USERID =@UPDATE_USERID
                                                WHERE STORE_NO=@STORE_NO", new SqlParameter[]
            {
                DataAccess.DataAccess.CreateParameter("STORE_NAME", SqlDbType.VarChar, store.STORE_NAME),
                DataAccess.DataAccess.CreateParameter("UPDATE_USERID", SqlDbType.VarChar, Session["UID"]),
                DataAccess.DataAccess.CreateParameter("STORE_NO", SqlDbType.VarChar, store.STORE_NO)
            });

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddStore(Store store)
        {
            if (int.Parse(
                dataAccess.ExecuteScalar_Text("SELECT COUNT(1) FROM TB_STORE AS ts WHERE ts.STORE_NO=@STORE_NO",
                    new SqlParameter[]
                    {
                        DataAccess.DataAccess.CreateParameter("STORE_NO", SqlDbType.VarChar, store.STORE_NO)
                    }).ToString()) > 0)
            {
                return Json("编号已存在", JsonRequestBehavior.AllowGet);
            }
            dataAccess.ExecuteNonQuery_Text(@"INSERT INTO TB_STORE
                                                (
	                                                STORE_NO,
	                                                STORE_NAME,
	                                                CREATE_TIME,
	                                                CREATE_USERID
                                                )
                                                VALUES
                                                (
	                                                @STORE_NO,
	                                                @STORE_NAME,
	                                                GETDATE(),
	                                                @CREATE_USERID
                                                )", new SqlParameter[]
            {
                DataAccess.DataAccess.CreateParameter("STORE_NO", SqlDbType.VarChar, store.STORE_NO),
                DataAccess.DataAccess.CreateParameter("STORE_NAME", SqlDbType.VarChar, store.STORE_NAME),
                DataAccess.DataAccess.CreateParameter("CREATE_USERID", SqlDbType.VarChar, Session["UID"])
            });

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DelStore(string Nos)
        {
            string sql = string.Format("DELETE FROM TB_STORE WHERE STORE_NO IN ({0})", Nos.TrimEnd(','));

            int i = dataAccess.ExecuteNonQuery_Text(sql, null);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}
