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
    public class StockManagerController : BaseController
    {
        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess(MvcApplication.conStr);
        List<Stock> lstData = new List<Stock>();

        public ActionResult Index()
        {
            ViewBag.Form = "K";
            ViewBag.AllStore = base.GetStore();
            DataModel<Stock> model = new DataModel<Stock>();
            string page = Request.QueryString["page"];
            if (page == null)
            {
                ViewBag.IsUsed = "";
                ViewBag.Status = "";
                ViewBag.RadFid = "";
                model.PageIndex = 1;
                model.PageSize = base.PageSize;
                model.TotalNum = 0;
            }
            else
            {
                string dateStart = TempData["txtDateStart"].ToString();
                string dateEnd = TempData["txtDateEnd"].ToString();
                string txtStore = TempData["txtStore"].ToString();
                string txtBoxNo = TempData["txtBoxNo"].ToString();
                string txtDemoNo = TempData["txtDemoNo"].ToString();
                string txtWhNo = TempData["txtWhNo"].ToString();
                string txtStatus = TempData["txtStatus"].ToString();
                string chkIsUsed = TempData["chkIsUsed"].ToString();
                string radFobid = TempData["radFobid"].ToString();

                string sql = @"SELECT twi.WH_NO,
                                   twi.WH_NAME,
                                   twi.WH_IS_USED,
                                   twi.FORBBIDEN_REASON,
                                   ts.BOX_BARCODE,
                                   ts.[CREATE_TIME],
                                   ts.[CREATE_USERID],
                                   ts.[UPDATE_TIME],
                                   ts.[UPDATE_USERID],
                                   tui.[USER_NAME]         AS CREATE_USER_NAME,
                                   tui2.[USER_NAME]        AS UPDATE_USER_NAME,
                                   ts2.STORE_NO,
                                   ts2.STORE_NAME,
                                   tb.BOX_NO,
                                   ISNULL(A.DEMOSUM, 0)    AS DEMOSUM
                            FROM   TB_WH_INFO              AS twi
                                   LEFT JOIN TB_STOCK      AS ts
                                        ON  twi.ID = ts.PID
                                   LEFT JOIN TB_STORE      AS ts2
                                        ON  twi.PID = ts2.ID
                                   LEFT JOIN TB_USER_INFO  AS tui
                                        ON  ts.[CREATE_USERID] = tui.USER_NO
                                   LEFT JOIN TB_USER_INFO  AS tui2
                                        ON  ts.[UPDATE_USERID] = tui2.USER_NO
                                   LEFT JOIN TB_BOX        AS tb
                                        ON  ts.BOX_BARCODE = tb.BOX_BARCODE
                                   LEFT JOIN (
                                            SELECT tb2.BOX_BARCODE,
                                                   COUNT(1)           AS DEMOSUM
                                            FROM   TB_BOX             AS tb2
                                                   LEFT JOIN TB_BOXS  AS tb3
                                                        ON  tb2.BOX_BARCODE = tb3.BOX_BARCODE
                                            GROUP BY
                                                   tb2.BOX_BARCODE
                                        ) A
                                        ON  ts.BOX_BARCODE = A.BOX_BARCODE
                            WHERE  1 = 1 ";

                if (!string.IsNullOrEmpty(dateStart))
                {
                    sql += string.Format(" AND TS.[CREATE_TIME]>=CONVERT(DATETIME,'{0}') ", dateStart);
                }
                if (!string.IsNullOrEmpty(dateEnd))
                {
                    sql += string.Format(" AND TS.[CREATE_TIME]><=CONVERT(DATETIME,'{0}') ", dateEnd);
                }
                if (!string.IsNullOrEmpty(txtStore))
                {
                    sql += string.Format("AND TS2.STORE_NO='{0}' ", txtStore);
                }
                if (!string.IsNullOrEmpty(txtBoxNo))
                {
                    sql += string.Format(" AND tb.BOX_NO LIKE '%{0}%' ", txtBoxNo);
                }
                if (!string.IsNullOrEmpty(txtDemoNo))
                {
                    sql += string.Format(@" AND tb.BOX_BARCODE IN (SELECT BOX_BARCODE
                         FROM TB_BOXS WHERE DEMO_NO LIKE '%{0}%') ", txtDemoNo);
                }
                if (!string.IsNullOrEmpty(txtWhNo))
                {
                    sql += string.Format(" AND twi.WH_NO LIKE  '%{0}%' ", txtWhNo);
                }
                if (!string.IsNullOrEmpty(txtStatus))
                {
                    if (txtStatus == "1")
                    {
                        sql += @"AND tb.BOX_BARCODE IN (SELECT BOX_BARCODE
                         FROM TB_BOXS WHERE DATEDIFF(DAY, CHECK_DATE,GETDATE())>0 ";
                    }
                    else
                    {
                        sql += @"AND tb.BOX_BARCODE IN (SELECT BOX_BARCODE
                         FROM TB_BOXS WHERE DATEDIFF(DAY, SAVE_DATE,GETDATE())>0 ";
                    }
                }
                if (!string.IsNullOrEmpty(chkIsUsed))
                {
                    if (chkIsUsed == "1")
                    {
                        sql += "AND TS.ID IS NOT NULL";
                    }
                    else
                    {
                        sql += "AND TS.ID IS NULL";
                    }
                }
                if (!string.IsNullOrEmpty(radFobid))
                {
                    sql += string.Format("AND isnull(twi.WH_IS_USED,1)={0}", radFobid);
                }

                DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(sql, null);

                model.PageIndex = int.Parse(page);
                model.PageSize = base.PageSize;
                model.TotalNum = dtData.Rows.Count;

                Stock stock;
                foreach (DataRow dr in dtData.AsEnumerable().Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize))
                {
                    stock = new Stock();
                    stock.BOX_NO = dr["BOX_NO"].ToString();
                    stock.BOX_BARCODE = dr["BOX_BARCODE"].ToString();
                    stock.STORE_NO = dr["STORE_NO"].ToString();
                    stock.STORE_NAME = dr["STORE_NAME"].ToString();
                    stock.WH_NO = dr["WH_NO"].ToString();
                    stock.WH_NAME = dr["WH_NAME"].ToString();
                    stock.WH_IS_USED_NAME = "正常";
                    if (!string.IsNullOrEmpty(dr["WH_IS_USED"].ToString()))
                    {
                        stock.WH_IS_USED = int.Parse(dr["WH_IS_USED"].ToString());
                        if (stock.WH_IS_USED == 0)
                        {
                            stock.WH_IS_USED_NAME = "禁用";
                        }
                    }
                    stock.FORBBIDEN_REASON = dr["FORBBIDEN_REASON"].ToString();

                    if (!string.IsNullOrEmpty(dr["CREATE_TIME"].ToString()))
                    {
                        stock.CREATE_TIME = DateTime.Parse(dr["CREATE_TIME"].ToString());
                    }
                    stock.CREATE_USERID = dr["CREATE_USERID"].ToString();
                    stock.CREATE_USERNAME = dr["CREATE_USERNAME"].ToString();
                    if (!string.IsNullOrEmpty(dr["UPDATE_TIME"].ToString()))
                    {
                        stock.UPDATE_TIME = DateTime.Parse(dr["UPDATE_TIME"].ToString());
                    }
                    stock.UPDATE_USERID = dr["UPDATE_USERID"].ToString();
                    stock.UPDATE_USERNAME = dr["UPDATE_USERNAME"].ToString();
                    stock.DEMOSUM = int.Parse(dr["DEMOSUM"].ToString());
                    lstData.Add(stock);
                }
                TempData["txtDateStart"] =  dateStart;
                TempData["txtDateEnd"] =  dateEnd;
                TempData["txtStore"] =  txtStore;
                TempData["txtBoxNo"] =  txtBoxNo;
                TempData["txtDemoNo"] = txtDemoNo;
                TempData["txtWhNo"] =  txtWhNo;
                TempData["txtStatus"] =  txtStatus;
                TempData["chkIsUsed"] = chkIsUsed;
                TempData["radFobid"] = radFobid;
            }
            model.Products = lstData;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            ViewBag.Form = "K";
            ViewBag.AllStore = base.GetStore();
            DataModel<Stock> model = new DataModel<Stock>();
            string page = Request.QueryString["page"];
            if (string.IsNullOrEmpty(page))
            {
                page = "1";
            }
            string dateStart = Request.Form["txtDateStart"]??"";
            string dateEnd = Request.Form["txtDateEnd"]??"";
            string txtStore = Request.Form["txtStore"]??"";
            string txtBoxNo = Request.Form["txtBoxNo"]??"";
            string txtDemoNo = Request.Form["txtDemoNo"]??"";
            string txtWhNo = Request.Form["txtWhNo"]??"";
            string txtStatus = Request.Form["txtStatus"]??"";
            string chkIsUsed = Request.Form["chkIsUsed"] ?? "";
            string radFobid = Request.Form["radFobid"] ?? "";

            TempData["txtDateStart"] = ViewBag.DateStart = dateStart;
            TempData["txtDateEnd"] = ViewBag.DateEnd = dateEnd;
            TempData["txtStore"] = ViewBag.Store = txtStore;
            TempData["txtBoxNo"] = ViewBag.BoxNo = txtBoxNo;
            TempData["txtDemoNo"] = ViewBag.DemoNo = txtDemoNo;
            TempData["txtWhNo"] = ViewBag.WhNo = txtWhNo;
            TempData["txtStatus"] = ViewBag.Status = txtStatus;
            TempData["chkIsUsed"] = ViewBag.IsUsed = chkIsUsed;
            TempData["radFobid"] = ViewBag.RadFid = radFobid;

            string sql = @"SELECT twi.WH_NO,
                                   twi.WH_NAME,
                                   twi.WH_IS_USED,
                                   twi.FORBBIDEN_REASON,
                                   ts.BOX_BARCODE,
                                   ts.[CREATE_TIME],
                                   ts.[CREATE_USERID],
                                   ts.[UPDATE_TIME],
                                   ts.[UPDATE_USERID],
                                   tui.[USER_NAME]         AS CREATE_USER_NAME,
                                   tui2.[USER_NAME]        AS UPDATE_USER_NAME,
                                   ts2.STORE_NO,
                                   ts2.STORE_NAME,
                                   tb.BOX_NO,
                                   ISNULL(A.DEMOSUM, 0)    AS DEMOSUM
                            FROM   TB_WH_INFO              AS twi
                                   LEFT JOIN TB_STOCK      AS ts
                                        ON  twi.ID = ts.PID
                                   LEFT JOIN TB_STORE      AS ts2
                                        ON  twi.PID = ts2.ID
                                   LEFT JOIN TB_USER_INFO  AS tui
                                        ON  ts.[CREATE_USERID] = tui.USER_NO
                                   LEFT JOIN TB_USER_INFO  AS tui2
                                        ON  ts.[UPDATE_USERID] = tui2.USER_NO
                                   LEFT JOIN TB_BOX        AS tb
                                        ON  ts.BOX_BARCODE = tb.BOX_BARCODE
                                   LEFT JOIN (
                                            SELECT tb2.BOX_BARCODE,
                                                   COUNT(1)           AS DEMOSUM
                                            FROM   TB_BOX             AS tb2
                                                   LEFT JOIN TB_BOXS  AS tb3
                                                        ON  tb2.BOX_BARCODE = tb3.BOX_BARCODE
                                            GROUP BY
                                                   tb2.BOX_BARCODE
                                        ) A
                                        ON  ts.BOX_BARCODE = A.BOX_BARCODE
                            WHERE  1 = 1 ";

            if (!string.IsNullOrEmpty(dateStart))
            {
                sql += string.Format(" AND TS.[CREATE_TIME]>=CONVERT(DATETIME,'{0}') ", dateStart);
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                sql += string.Format(" AND TS.[CREATE_TIME]><=CONVERT(DATETIME,'{0}') ", dateEnd);
            }
            if (!string.IsNullOrEmpty(txtStore))
            {
                sql += string.Format("AND TS2.STORE_NO='{0}' ", txtStore);
            }
            if (!string.IsNullOrEmpty(txtBoxNo))
            {
                sql += string.Format(" AND tb.BOX_NO LIKE '%{0}%' ", txtBoxNo);
            }
            if (!string.IsNullOrEmpty(txtDemoNo))
            {
                sql += string.Format(@" AND tb.BOX_BARCODE IN (SELECT BOX_BARCODE
                         FROM TB_BOXS WHERE DEMO_NO LIKE '%{0}%') ", txtDemoNo);
            }
            if (!string.IsNullOrEmpty(txtWhNo))
            {
                sql += string.Format(" AND twi.WH_NO LIKE  '%{0}%' ", txtWhNo);
            }
            if (!string.IsNullOrEmpty(txtStatus))
            {
                if (txtStatus == "1")
                {
                    sql += @"AND tb.BOX_BARCODE IN (SELECT BOX_BARCODE
                         FROM TB_BOXS WHERE DATEDIFF(DAY, CHECK_DATE,GETDATE())>0 ";
                }
                else
                {
                    sql += @"AND tb.BOX_BARCODE IN (SELECT BOX_BARCODE
                         FROM TB_BOXS WHERE DATEDIFF(DAY, SAVE_DATE,GETDATE())>0 ";
                }
            }
            if (!string.IsNullOrEmpty(chkIsUsed))
            {
                if (chkIsUsed == "1")
                {
                    sql += "AND TS.ID IS NOT NULL";
                }
                else
                {
                    sql += "AND TS.ID IS NULL";
                }
            }
            if (!string.IsNullOrEmpty(radFobid))
            {
                sql += string.Format("AND isnull(twi.WH_IS_USED,1)={0}", radFobid);
            }
          
            DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(sql, null);

            model.PageIndex = int.Parse(page);
            model.PageSize = base.PageSize;
            model.TotalNum = dtData.Rows.Count;

            Stock stock;
            foreach (DataRow dr in dtData.AsEnumerable().Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize))
            {
                stock = new Stock();
                stock.BOX_NO = dr["BOX_NO"].ToString();
                stock.BOX_BARCODE = dr["BOX_BARCODE"].ToString();
                stock.STORE_NO = dr["STORE_NO"].ToString();
                stock.STORE_NAME = dr["STORE_NAME"].ToString();
                stock.WH_NO = dr["WH_NO"].ToString();
                stock.WH_NAME = dr["WH_NAME"].ToString();
                stock.WH_IS_USED_NAME = "正常";
                if (!string.IsNullOrEmpty(dr["WH_IS_USED"].ToString()))
                {
                    stock.WH_IS_USED = int.Parse(dr["WH_IS_USED"].ToString());
                    if (stock.WH_IS_USED == 0)
                    {
                        stock.WH_IS_USED_NAME = "禁用";
                    }
                }
                stock.FORBBIDEN_REASON = dr["FORBBIDEN_REASON"].ToString();
               
                if (!string.IsNullOrEmpty(dr["CREATE_TIME"].ToString()))
                {
                    stock.CREATE_TIME = DateTime.Parse(dr["CREATE_TIME"].ToString());
                }
                stock.CREATE_USERID = dr["CREATE_USERID"].ToString();
                stock.CREATE_USERNAME = dr["CREATE_USERNAME"].ToString();
                if (!string.IsNullOrEmpty(dr["UPDATE_TIME"].ToString()))
                {
                    stock.UPDATE_TIME = DateTime.Parse(dr["UPDATE_TIME"].ToString());
                }
                stock.UPDATE_USERID = dr["UPDATE_USERID"].ToString();
                stock.UPDATE_USERNAME = dr["UPDATE_USERNAME"].ToString();
                stock.DEMOSUM =int.Parse(dr["DEMOSUM"].ToString());
                lstData.Add(stock);
            }
            model.Products = lstData;
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateStock(Stock model)
        {
            if (int.Parse(
                 dataAccess.ExecuteScalar_Text(@"SELECT COUNT(1)
                                                    FROM   TB_STOCK AS ts
                                                    WHERE  ts.PID = (
                                                               SELECT ID
                                                               FROM   TB_WH_INFO AS twi
                                                               WHERE  twi.WH_NO = @WH_NO
                                                                      AND twi.PID = (
                                                                              SELECT ID
                                                                              FROM   TB_STORE AS ts2
                                                                              WHERE  ts2.STORE_NO = @STORE_NO
                                                                          )
                                                           ) ",
                    new SqlParameter[]
                    {
                        DataAccess.DataAccess.CreateParameter("WH_NO", SqlDbType.VarChar, model.WH_NO),
                        DataAccess.DataAccess.CreateParameter("STORE_NO", SqlDbType.VarChar, model.STORE_NO)
                    }).ToString()) == 0)
            {
                return Json("无入库记录", JsonRequestBehavior.AllowGet);
            }
            dataAccess.ExecuteNonQuery_Text(@"UPDATE TB_STOCK
                                                SET BOX_BARCODE =@BOX_BARCODE,
	                                                UPDATE_TIME = GETDATE(),
	                                                UPDATE_USERID =@UPDATE_USERID
                                                WHERE PID=(
                                                           SELECT ID
                                                           FROM   TB_WH_INFO AS twi
                                                           WHERE  twi.WH_NO = @WH_NO
                                                                  AND twi.PID = (
                                                                          SELECT ID
                                                                          FROM   TB_STORE AS ts2
                                                                          WHERE  ts2.STORE_NO = @STORE_NO
                                                                      )
                                                       )", new SqlParameter[]
            {
                DataAccess.DataAccess.CreateParameter("BOX_BARCODE", SqlDbType.VarChar, model.BOX_BARCODE),
                DataAccess.DataAccess.CreateParameter("UPDATE_USERID", SqlDbType.Int,Session["UID"]),
                DataAccess.DataAccess.CreateParameter("WH_NO", SqlDbType.VarChar, model.WH_NO),
                DataAccess.DataAccess.CreateParameter("STORE_NO", SqlDbType.VarChar, model.STORE_NO)
            });

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DelWareHouse(string Nos)
        {
            string sql = string.Format("DELETE FROM TB_STOCK WHERE BOX_BARCODE IN ({0})", Nos.TrimEnd(','));
            
            int i = dataAccess.ExecuteNonQuery_Text(sql, null);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}
