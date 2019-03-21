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
    public class WareHouseManagerController : BaseController
    {
        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess(MvcApplication.conStr);
        List<WareHouse> lstData = new List<WareHouse>();

        public ActionResult Index()
        {
            ViewBag.Form = "W";
            ViewBag.AllStore = base.GetStore();
            DataModel<WareHouse> model = new DataModel<WareHouse>();
            string page = Request.QueryString["page"];
            if (page == null)
            {
                ViewBag.IsUsed = "";
                model.PageIndex = 1;
                model.PageSize = base.PageSize;
                model.TotalNum = 0;
            }
            else
            {
                string txtNo = TempData["txtNo"].ToString();
                string txtName = TempData["txtName"].ToString();
                string txtStore = TempData["txtStore"].ToString();
                string chkIsUsed = TempData["chkIsUsed"].ToString();
                string reason = TempData["txtReason"].ToString();
              
                string sql = @"SELECT twi.*,ts.STORE_NO,ts.STORE_NAME,tui.[USER_NAME] AS CREATE_USERNAME,tui2.[USER_NAME] AS UPDATE_USERNAME
                                FROM TB_WH_INFO AS twi
                                LEFT JOIN TB_STORE AS ts ON twi.PID=ts.ID
                                LEFT JOIN TB_USER_INFO AS tui ON twi.CREATE_USERID=tui.USER_NO
                                LEFT JOIN TB_USER_INFO AS tui2 ON twi.UPDATE_USERID=tui2.USER_NO
                                WHERE 1=1 ";
                if (!string.IsNullOrEmpty(txtNo))
                {
                    sql += string.Format(" AND twi.WH_NO  LIKE '%{0}%' ", txtNo);
                }
                if (!string.IsNullOrEmpty(txtName))
                {
                    sql += string.Format(" AND twi.WH_NAME LIKE '%{0}%' ", txtName);
                }
                if (!string.IsNullOrEmpty(reason))
                {
                    sql += string.Format(" AND twi.FORBBIDEN_REASON LIKE '%{0}%' ", reason);
                }
                if (!string.IsNullOrEmpty(txtStore))
                {
                    sql += string.Format(" AND ts.STORE_NO = '{0}' ", txtStore);
                }
                if (!string.IsNullOrEmpty(chkIsUsed) && chkIsUsed.IndexOf(',') == -1)
                {
                    sql += string.Format(" AND twi.WH_IS_USED = {0} ", chkIsUsed);
                }
                DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(sql, null);

                model.PageIndex = int.Parse(page);
                model.PageSize = base.PageSize;
                model.TotalNum = dtData.Rows.Count;

                WareHouse wareHouse;
                foreach (DataRow dr in dtData.AsEnumerable().Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize))
                {
                    wareHouse = new WareHouse();
                    wareHouse.WH_NO = dr["WH_NO"].ToString();
                    wareHouse.WH_NAME = dr["WH_NAME"].ToString();
                    wareHouse.STORE_NO = dr["STORE_NO"].ToString();
                    wareHouse.STORE_NAME = dr["STORE_NAME"].ToString();
                    wareHouse.FORBBIDEN_REASON = dr["FORBBIDEN_REASON"].ToString();
                    if (!string.IsNullOrEmpty(dr["PID"].ToString()))
                    {
                        wareHouse.PID = int.Parse(dr["PID"].ToString());
                    }
                    wareHouse.WH_IS_USED_NAME = "正常";
                    if (!string.IsNullOrEmpty(dr["WH_IS_USED"].ToString()))
                    {
                        wareHouse.WH_IS_USED = int.Parse(dr["WH_IS_USED"].ToString());
                        if (wareHouse.WH_IS_USED == 0)
                        {
                            wareHouse.WH_IS_USED_NAME = "禁用";
                        }
                    }
                    if (!string.IsNullOrEmpty(dr["CREATE_TIME"].ToString()))
                    {
                        wareHouse.CREATE_TIME = DateTime.Parse(dr["CREATE_TIME"].ToString());
                    }
                    wareHouse.CREATE_USERID = dr["CREATE_USERID"].ToString();
                    wareHouse.CREATE_USERNAME = dr["CREATE_USERNAME"].ToString();
                    if (!string.IsNullOrEmpty(dr["UPDATE_TIME"].ToString()))
                    {
                        wareHouse.UPDATE_TIME = DateTime.Parse(dr["UPDATE_TIME"].ToString());
                    }
                    wareHouse.UPDATE_USERID = dr["UPDATE_USERID"].ToString();
                    wareHouse.UPDATE_USERNAME = dr["UPDATE_USERNAME"].ToString();
                    lstData.Add(wareHouse);
                }
                TempData["txtNo"] = txtNo;
                TempData["txtName"] = txtName;
                TempData["txtStore"] = txtStore;
                TempData["chkIsUsed"] = chkIsUsed;
                TempData["txtReason"] = reason;
            }
            model.Products = lstData;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            ViewBag.Form = "W";
            ViewBag.AllStore = base.GetStore();
            DataModel<WareHouse> model = new DataModel<WareHouse>();
            string page = Request.QueryString["page"];
            if (string.IsNullOrEmpty(page))
            {
                page = "1";
            }
            string txtNo = Request.Form["txtNo"];
            string txtName = Request.Form["txtName"];
            string txtStore = Request.Form["txtStore"];
            string chkIsUsed = Request.Form["chkIsUsed"]??"";
            string reason = Request.Form["txtReason"];

            TempData["txtNo"]=ViewBag.No = txtNo;
            TempData["txtName"] = ViewBag.Name = txtName;
            TempData["txtStore"] = ViewBag.Store = txtStore;
            TempData["chkIsUsed"] = ViewBag.IsUsed = chkIsUsed;
            TempData["txtReason"] = ViewBag.Reason = reason;

            string sql = @"SELECT twi.*,ts.STORE_NO,ts.STORE_NAME,tui.[USER_NAME] AS CREATE_USERNAME,tui2.[USER_NAME] AS UPDATE_USERNAME
                                FROM TB_WH_INFO AS twi
                                LEFT JOIN TB_STORE AS ts ON twi.PID=ts.ID
                                LEFT JOIN TB_USER_INFO AS tui ON twi.CREATE_USERID=tui.USER_NO
                                LEFT JOIN TB_USER_INFO AS tui2 ON twi.UPDATE_USERID=tui2.USER_NO
                                WHERE 1=1 ";
            if (!string.IsNullOrEmpty(txtNo))
            {
                sql += string.Format(" AND twi.WH_NO  LIKE '%{0}%' ", txtNo);
            }
            if (!string.IsNullOrEmpty(txtName))
            {
                sql += string.Format(" AND twi.WH_NAME LIKE '%{0}%' ", txtName);
            }
            if (!string.IsNullOrEmpty(reason))
            {
                sql += string.Format(" AND twi.FORBBIDEN_REASON LIKE '%{0}%' ", reason);
            }
            if (!string.IsNullOrEmpty(txtStore))
            {
                sql += string.Format(" AND ts.STORE_NO = '{0}' ", txtStore);
            }
            if (!string.IsNullOrEmpty(chkIsUsed)&&chkIsUsed.IndexOf(',')==-1)
            {
                sql += string.Format(" AND twi.WH_IS_USED = {0} ", chkIsUsed);
            }
            DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(sql, null);

            model.PageIndex = int.Parse(page);
            model.PageSize = base.PageSize;
            model.TotalNum = dtData.Rows.Count;

            WareHouse wareHouse;
            foreach (DataRow dr in dtData.AsEnumerable().Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize))
            {
                wareHouse = new WareHouse();
                wareHouse.WH_NO = dr["WH_NO"].ToString();
                wareHouse.WH_NAME = dr["WH_NAME"].ToString();
                wareHouse.STORE_NO = dr["STORE_NO"].ToString();
                wareHouse.STORE_NAME = dr["STORE_NAME"].ToString();
                wareHouse.FORBBIDEN_REASON = dr["FORBBIDEN_REASON"].ToString();
                if (!string.IsNullOrEmpty(dr["PID"].ToString()))
                {
                    wareHouse.PID = int.Parse(dr["PID"].ToString());
                }
                wareHouse.WH_IS_USED_NAME = "正常";
                if (!string.IsNullOrEmpty(dr["WH_IS_USED"].ToString()))
                {
                    wareHouse.WH_IS_USED = int.Parse(dr["WH_IS_USED"].ToString());
                    if (wareHouse.WH_IS_USED == 0)
                    {
                        wareHouse.WH_IS_USED_NAME = "禁用";
                    }
                }
                if (!string.IsNullOrEmpty(dr["CREATE_TIME"].ToString()))
                {
                    wareHouse.CREATE_TIME = DateTime.Parse(dr["CREATE_TIME"].ToString());
                }
                wareHouse.CREATE_USERID = dr["CREATE_USERID"].ToString();
                wareHouse.CREATE_USERNAME = dr["CREATE_USERNAME"].ToString();
                if (!string.IsNullOrEmpty(dr["UPDATE_TIME"].ToString()))
                {
                    wareHouse.UPDATE_TIME = DateTime.Parse(dr["UPDATE_TIME"].ToString());
                }
                wareHouse.UPDATE_USERID = dr["UPDATE_USERID"].ToString();
                wareHouse.UPDATE_USERNAME = dr["UPDATE_USERNAME"].ToString();
                lstData.Add(wareHouse);
            }
            model.Products = lstData;
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateWareHouse(WareHouse model)
        {
            if (int.Parse(
                 dataAccess.ExecuteScalar_Text(@"SELECT COUNT(1) FROM TB_WH_INFO AS twi 
                                                WHERE twi.WH_NO=@WH_NO 
                                                AND twi.PID =(SELECT ID FROM TB_STORE AS ts WHERE ts.STORE_NO=@STORE_NO) ",
                    new SqlParameter[]
                    {
                        DataAccess.DataAccess.CreateParameter("WH_NO", SqlDbType.VarChar, model.WH_NO),
                        DataAccess.DataAccess.CreateParameter("STORE_NO", SqlDbType.VarChar, model.STORE_NO)
                    }).ToString()) == 0)
            {
                return Json("编号不存在", JsonRequestBehavior.AllowGet);
            }
            dataAccess.ExecuteNonQuery_Text(@"UPDATE TB_WH_INFO
                                                SET WH_NAME = @WH_NAME,
	                                                WH_IS_USED = @WH_IS_USED,
	                                                UPDATE_TIME = GETDATE(),
	                                                UPDATE_USERID = @UPDATE_USERID,
	                                                FORBBIDEN_REASON = @FORBBIDEN_REASON
                                                WHERE WH_NO=@WH_NO 
                                                AND PID =(SELECT ID FROM TB_STORE AS ts WHERE ts.STORE_NO=@STORE_NO)", new SqlParameter[]
            {
                DataAccess.DataAccess.CreateParameter("WH_NAME", SqlDbType.VarChar, model.WH_NAME??""),
                DataAccess.DataAccess.CreateParameter("WH_IS_USED", SqlDbType.VarChar, model.WH_IS_USED),
                DataAccess.DataAccess.CreateParameter("UPDATE_USERID", SqlDbType.Int,Session["UID"]),
                DataAccess.DataAccess.CreateParameter("FORBBIDEN_REASON", SqlDbType.VarChar, model.FORBBIDEN_REASON??""),
                DataAccess.DataAccess.CreateParameter("WH_NO", SqlDbType.VarChar, model.WH_NO),
                DataAccess.DataAccess.CreateParameter("STORE_NO", SqlDbType.VarChar, model.STORE_NO)
            });

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddWareHouse(WareHouse model)
        {
            if (int.Parse(
                dataAccess.ExecuteScalar_Text(@"SELECT COUNT(1) FROM TB_WH_INFO AS twi 
                                                WHERE twi.WH_NO=@WH_NO 
                                                AND twi.PID =(SELECT ID FROM TB_STORE AS ts WHERE ts.STORE_NO=@STORE_NO) ",
                    new SqlParameter[]
                    {
                        DataAccess.DataAccess.CreateParameter("WH_NO", SqlDbType.VarChar, model.WH_NO),
                        DataAccess.DataAccess.CreateParameter("STORE_NO", SqlDbType.VarChar, model.STORE_NO)
                    }).ToString()) > 0)
            {
                return Json("编号已存在", JsonRequestBehavior.AllowGet);
            }
            dataAccess.ExecuteNonQuery_Text(@"INSERT INTO TB_WH_INFO
                                            (
	                                            WH_NO,
	                                            PID,
	                                            WH_NAME,
	                                            WH_IS_USED,
	                                            CREATE_TIME,
	                                            CREATE_USERID,
	                                            FORBBIDEN_REASON
                                            )
                                            VALUES
                                            (
	                                            @WH_NO,
	                                            (SELECT ID FROM TB_STORE AS ts WHERE ts.STORE_NO=@STORE_NO),
	                                            @WH_NAME,
	                                            @WH_IS_USED,
	                                            GETDATE(),
	                                            @CREATE_USERID,
	                                            @FORBBIDEN_REASON
                                            )", new SqlParameter[]
            {
                DataAccess.DataAccess.CreateParameter("WH_NO", SqlDbType.VarChar, model.WH_NO),
                DataAccess.DataAccess.CreateParameter("STORE_NO", SqlDbType.VarChar, model.STORE_NO),
                DataAccess.DataAccess.CreateParameter("WH_NAME", SqlDbType.VarChar, model.WH_NAME??""),
                DataAccess.DataAccess.CreateParameter("WH_IS_USED", SqlDbType.Int, model.WH_IS_USED),
                DataAccess.DataAccess.CreateParameter("CREATE_USERID", SqlDbType.VarChar, Session["UID"]),
                DataAccess.DataAccess.CreateParameter("FORBBIDEN_REASON", SqlDbType.VarChar, model.FORBBIDEN_REASON??"")
            });

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DelWareHouse(string Nos)
        {
            string sql = string.Format("DELETE FROM TB_WH_INFO WHERE CAST(PID AS VARCHAR)+'|'+WH_NO IN ({0})", Nos.TrimEnd(','));

            int i = dataAccess.ExecuteNonQuery_Text(sql, null);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}
