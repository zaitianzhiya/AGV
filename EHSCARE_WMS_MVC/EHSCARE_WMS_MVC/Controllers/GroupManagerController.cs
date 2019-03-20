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
    public class GroupManagerController : BaseController
    {
        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess(MvcApplication.conStr);
        List<Group> lstData = new List<Group>();
       
        public ActionResult Index()
        {
            ViewBag.Form = "G";
            DataModel<Group> model = new DataModel<Group>();
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

                string sql = @"SELECT * FROM TB_GROUP_INFO AS tgi WHERE 1=1 ";
                if (!string.IsNullOrEmpty(txtNo))
                {
                    sql += string.Format(" AND tgi.GROUP_NO LIKE '%{0}%' ", txtNo);
                }
                if (!string.IsNullOrEmpty(txtName))
                {
                    sql += string.Format(" AND tgi.[GROUP_NAME] LIKE '%{0}%' ", txtName);
                }
              
                DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(sql, null);

                model.PageIndex = int.Parse(page);
                model.PageSize = base.PageSize;
                model.TotalNum = dtData.Rows.Count;

                Group group;
                foreach (DataRow dr in dtData.AsEnumerable().Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize))
                {
                    group = new Group();
                    group.GROUP_NO = dr["GROUP_NO"].ToString();
                    group.GROUP_NAME = dr["GROUP_NAME"].ToString();
                    lstData.Add(group);
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
            ViewBag.Form = "G";
            DataModel<Group> model = new DataModel<Group>();
            string page = Request.QueryString["page"];
            if (string.IsNullOrEmpty(page))
            {
                page = "1";
            }
            string txtNo = fc["txtNo"];
            string txtName = fc["txtName"];
            TempData["txtNo"] = ViewBag.UserNo = txtNo;
            TempData["txtName"] = ViewBag.UserName = txtName;

            string sql = @"SELECT * FROM TB_GROUP_INFO AS tgi WHERE 1=1 ";
            if (!string.IsNullOrEmpty(txtNo))
            {
                sql += string.Format(" AND tgi.GROUP_NO LIKE '%{0}%' ", txtNo);
            }
            if (!string.IsNullOrEmpty(txtName))
            {
                sql += string.Format(" AND tgi.[GROUP_NAME] LIKE '%{0}%' ", txtName);
            }
            DataTable dtData = dataAccess.ExecuteQueryDataTable_Text(sql, null);

            model.PageIndex = int.Parse(page);
            model.PageSize = base.PageSize;
            model.TotalNum = dtData.Rows.Count;

            Group group;
            foreach (DataRow dr in dtData.AsEnumerable().Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize))
            {
                group = new Group();
                group.GROUP_NO = dr["GROUP_NO"].ToString();
                group.GROUP_NAME = dr["GROUP_NAME"].ToString();
                lstData.Add(group);
            }
            model.Products = lstData;
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateGroup(Group group)
        {
            if (int.Parse(
                dataAccess.ExecuteScalar_Text("SELECT COUNT(1) FROM TB_GROUP_INFO AS tgi WHERE tgi.GROUP_NO=@GROUP_NO",
                    new SqlParameter[]
                    {
                        DataAccess.DataAccess.CreateParameter("GROUP_NO", SqlDbType.VarChar, group.GROUP_NO)
                    }).ToString()) == 0)
            {
                return Json("编号不存在", JsonRequestBehavior.AllowGet);
            }
            dataAccess.ExecuteNonQuery_Text(@"UPDATE TB_GROUP_INFO
                                                SET GROUP_NAME =@GROUP_NAME,
	                                                UPDATE_TIME = GETDATE(),
	                                                UPDATE_USERID = @UPDATE_USERID
                                                WHERE GROUP_NO=@GROUP_NO", new SqlParameter[]
            {
                DataAccess.DataAccess.CreateParameter("GROUP_NAME", SqlDbType.VarChar, group.GROUP_NAME),
                DataAccess.DataAccess.CreateParameter("UPDATE_USERID", SqlDbType.VarChar, Session["UID"]),
                DataAccess.DataAccess.CreateParameter("GROUP_NO", SqlDbType.VarChar, group.GROUP_NO)
            });

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddGroup(Group group)
        {
            if (int.Parse(
                dataAccess.ExecuteScalar_Text("SELECT COUNT(1) FROM TB_GROUP_INFO AS tgi WHERE tgi.GROUP_NO=@GROUP_NO",
                    new SqlParameter[]
                    {
                        DataAccess.DataAccess.CreateParameter("GROUP_NO", SqlDbType.VarChar, group.GROUP_NO)
                    }).ToString()) > 0)
            {
                return Json("编号已存在", JsonRequestBehavior.AllowGet);
            }
            dataAccess.ExecuteNonQuery_Text(@"INSERT INTO TB_GROUP_INFO
                                                (
	                                                GROUP_NO,
	                                                GROUP_NAME,
	                                                CREATE_TIME,
	                                                CREATE_USERID
                                                )
                                                VALUES
                                                (
	                                                @GROUP_NO,
	                                                @GROUP_NAME,
	                                                GETDATE(),
	                                                @CREATE_USERID
                                                )", new SqlParameter[]
            {
                DataAccess.DataAccess.CreateParameter("GROUP_NO", SqlDbType.VarChar, group.GROUP_NO),
                DataAccess.DataAccess.CreateParameter("GROUP_NAME", SqlDbType.VarChar, group.GROUP_NAME),
                DataAccess.DataAccess.CreateParameter("CREATE_USERID", SqlDbType.VarChar, Session["UID"])
            });

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DelGroup(string Nos)
        {
            string sql = string.Format("DELETE FROM TB_GROUP_INFO WHERE GROUP_NO IN ({0})", Nos.TrimEnd(','));
          
            int i = dataAccess.ExecuteNonQuery_Text(sql, null);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}
