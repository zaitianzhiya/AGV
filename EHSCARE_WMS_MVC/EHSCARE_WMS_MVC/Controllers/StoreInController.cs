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
    public class StoreInController : BaseController
    {
        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess(MvcApplication.conStr);
        List<StockRecord> lstData = new List<StockRecord>();

        public ActionResult Index()
        {
            ViewBag.Form = "I";
            ViewBag.AllStore = base.GetStore();
            DataModel<StockRecord> model = new DataModel<StockRecord>();
            string page = Request.QueryString["page"];
            if (page == null)
            {
                ViewBag.IsComplete = "";
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

                model.PageIndex = int.Parse(page);
                model.PageSize = base.PageSize;
                int total;
                lstData = GetProducts(model, dateStart, dateEnd, txtStore, txtBoxNo, txtDemoNo, txtWhNo, txtStatus,
                    chkIsUsed, radFobid, out total);
                model.TotalNum = total;

                TempData["txtDateStart"] = dateStart;
                TempData["txtDateEnd"] = dateEnd;
                TempData["txtStore"] = txtStore;
                TempData["txtBoxNo"] = txtBoxNo;
                TempData["txtDemoNo"] = txtDemoNo;
                TempData["txtWhNo"] = txtWhNo;
                TempData["txtStatus"] = txtStatus;
                TempData["chkIsUsed"] = chkIsUsed;
                TempData["radFobid"] = radFobid;
            }
            model.Products = lstData;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            ViewBag.Form = "I";
            ViewBag.AllStore = base.GetStore();
            DataModel<StockRecord> model = new DataModel<StockRecord>();
            string page = Request.QueryString["page"];
            if (string.IsNullOrEmpty(page))
            {
                page = "1";
            }

            string txtStore = Request.Form["txtStore"] ?? "";
            string txtBoxBar = Request.Form["txtBoxBar"] ?? "";
            string txtDemoBar = Request.Form["txtDemoBar"] ?? "";
            string txtBoxRemark = Request.Form["txtBoxRemark"] ?? "";
            string txtDemoRemark = Request.Form["txtDemoRemark"] ?? "";
            string txtStatus = Request.Form["chkIsComplete"] ?? "";

            TempData["txtStore"] = ViewBag.Store = txtStore;
            TempData["txtBoxBar"] = ViewBag.BoxBar = txtBoxBar;
            TempData["txtDemoBar"] = ViewBag.DemoBar = txtDemoBar;
            TempData["txtBoxRemark"] = ViewBag.BoxRemark = txtBoxRemark;
            TempData["txtDemoRemark"] = ViewBag.DemoRemark = txtDemoRemark;
            TempData["chkIsComplete"] = ViewBag.IsComplete = txtStatus;

            model.PageIndex = int.Parse(page);
            model.PageSize = base.PageSize;
            int total;
            lstData = GetProducts(model, txtStore, txtBoxBar, txtDemoBar, txtBoxRemark, txtDemoRemark, txtStatus,out total);
            model.TotalNum = total;
            model.Products = lstData;
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateStockRecord(StockRecord model)
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
        public JsonResult DelStockRecord(string Nos)
        {
            string sql = string.Format("DELETE FROM TB_STOCK WHERE BOX_BARCODE IN ({0})", Nos.TrimEnd(','));

            int i = dataAccess.ExecuteNonQuery_Text(sql, null);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        /// 根据条件取得数据源方法
        /// <summary>
        /// 根据条件取得数据源方法
        /// </summary>
        /// <returns></returns>
        private List<StockRecord> GetProducts(DataModel<StockRecord> model, string txtStore, string txtBoxBar, string txtDemoBar, string txtBoxRemark, string txtDemoRemark, string txtStatus, out int total)
        {
            lstData.Clear();
            string sql = @"SELECT tsr.*,
                            twi.WH_NO,ts.STORE_NO,ts.STORE_NAME,
                            tui.[USER_NAME]
                            FROM TB_STOCK_RECORD AS tsr
                            LEFT JOIN TB_WH_INFO AS twi ON tsr.WH_NEW=twi.ID
                            LEFT JOIN TB_STORE AS ts ON tsr.WH_IN=ts.STORE_NO
                            LEFT JOIN TB_USER_INFO AS tui ON tsr.CREATE_USERID=tui.USER_NO
                            LEFT JOIN TB_BOX AS tb ON tsr.[BOX_BARCODE]=tb.BOX_BARCODE
                            WHERE 1=1 ";

            if (!string.IsNullOrEmpty(txtStore))
            {
                sql += string.Format(" AND ts.STORE_NO='{0}' ", txtStore);
            }
            if (!string.IsNullOrEmpty(txtBoxBar))
            {
                sql += string.Format(" AND tsr.[BOX_BARCODE] LIKE '%{0}%' ", txtBoxBar);
            }
            if (!string.IsNullOrEmpty(txtDemoBar))
            {
                sql += string.Format(@" AND tsr.[BOX_BARCODE] IN (
                                        SELECT tb2.BOX_BARCODE
                                        FROM TB_BOXS AS tb2
                                        WHERE tb2.BARCODE LIKE 	'%{0}%'
                                        ) ", txtDemoBar);
            }
            if (!string.IsNullOrEmpty(txtBoxRemark))
            {
                sql += string.Format(" AND tb.[REMARK] LIKE '%{0}%' ", txtBoxRemark);
            }
            if (!string.IsNullOrEmpty(txtDemoRemark))
            {
                sql += string.Format(@" AND tsr.[BOX_BARCODE] IN (
                                        SELECT tb2.BOX_BARCODE
                                        FROM TB_BOXS AS tb2
                                        WHERE tb2.REMARK LIKE 	'%{0}%'
                                        ) ", txtDemoRemark);
            }
            if (!string.IsNullOrEmpty(txtStatus))
            {
                sql += string.Format(" AND tsr.STA={0} ", txtStatus);
            }

            DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(sql, null);

            total = dtData.Rows.Count;

            StockRecord stockRecord;
            foreach (DataRow dr in dtData.AsEnumerable().Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize))
            {
                stockRecord = new StockRecord();
                stockRecord.ID = int.Parse(dr["ID"].ToString());
                stockRecord.BOX_BARCODE = dr["BOX_BARCODE"].ToString();
                stockRecord.ACT_TYPE = int.Parse(dr["STORE_NO"].ToString());
                stockRecord.STORE_NAME = dr["STORE_NAME"].ToString();
                stockRecord.WH_NO = dr["WH_NO"].ToString();
                stockRecord.WH_NAME = dr["WH_NAME"].ToString();
                stockRecord.WH_IS_USED_NAME = "正常";
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
            return lstData;
        }
    }
}
