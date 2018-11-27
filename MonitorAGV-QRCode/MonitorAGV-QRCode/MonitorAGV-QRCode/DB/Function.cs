using System.Collections;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Data;

namespace MonitorAGV_QRCode
{
    public class Function
    {
        public Function()
        {
        }

        public static void getConn(string connstring)
        {
            SqlDBControl._defultConnectionString = connstring;//获取数据库连接参数
        }

        public static DataTable GetDataInfo(string proName)
        {
            SqlCommand comm = new SqlCommand(proName);
            comm.CommandType = CommandType.StoredProcedure;
            return SqlDBControl.ExecuteQuery(comm).Tables[0];
        }

        //TJA2017102416:24
        public static DataTable ReadDB_tb_AGV_Info()
        {
            SqlCommand comm = new SqlCommand();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "SELECT * FROM tb_AGV_Info";
            DataTable Select = null;
            try
            {
                Select = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return Select;
        }
        //TJZ2017102416:24

        public static DataTable PR_Read_Map_FQ(int map_used)
        {
            SqlCommand comm = new SqlCommand("PR_FQ_Map_Used");
            comm.CommandType = CommandType.StoredProcedure;

            DataTable Select = null;
            //SqlCommand comm = new SqlCommand("PR_Select_AGV_CallBoxNo");
            //comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_Used", SqlDbType.Int, 10).Value = map_used;
            try
            {
                Select = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return Select;
        }

        public static DataTable PR_Read_Map_FQ()
        {
            SqlCommand comm = new SqlCommand("PR_FQ_Map_Used_All");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable Select = null;
            try
            {
                Select = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return Select;
        }

        public static int PR_Write_Map_FQ(string map_no, int map_used)
        {

            SqlCommand comm = new SqlCommand("PR_UPDATE_Map_Info_FQ");
            comm.CommandType = CommandType.StoredProcedure;
            //DataTable Select = null;
            //SqlCommand comm = new SqlCommand("PR_Select_AGV_CallBoxNo");
            //comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_Used", SqlDbType.Int, 10).Value = map_used;
            comm.Parameters.Add("@Map_No", SqlDbType.VarChar, 50).Value = map_no;
            int i = -1;
            try
            {
                i = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                i = -1;
            }
            return i;
        }


        public static int PR_UPDATE_Map_Info_Real(int map_no, int realNo)
        {

            SqlCommand comm = new SqlCommand("PR_UPDATE_Map_Info_Real");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_No", SqlDbType.Int).Value = map_no;
            comm.Parameters.Add("@RealNo", SqlDbType.Int).Value = realNo;
            int i = -1;
            try
            {
                i = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                i = -1;
            }
            return i;
        }

        public static int PR_Insert_Task(string from, string to)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_TASK");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@TASK_NO", SqlDbType.VarChar, 50).Value = from + "-" + to;
            comm.Parameters.Add("@FROM_POINT", SqlDbType.VarChar, 50).Value = from;
            comm.Parameters.Add("@TO_POINT", SqlDbType.VarChar, 50).Value = to;
            int i = -1;
            try
            {
                i = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                i = -1;
            }
            return i;
        }

        public static int PR_Update_Map_Info(string mapNo, string mapx, string mapy, int mapUsed)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_Map_Info");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_No", SqlDbType.VarChar, 50).Value = mapNo;
            comm.Parameters.Add("@Map_X", SqlDbType.VarChar, 50).Value = mapx;
            comm.Parameters.Add("@Map_Y", SqlDbType.VarChar, 50).Value = mapy;
            comm.Parameters.Add("@Map_Used", SqlDbType.VarChar, 50).Value = mapUsed;
            int i = -1;
            try
            {
                i = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                i = -1;
            }
            return i;
        }

        public static int PR_Update_Map_Info(int maxx, int maxy, int mapUsed)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_Map_Info2");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@MAX_X", SqlDbType.Int).Value = maxx;
            comm.Parameters.Add("@MAX_Y", SqlDbType.Int).Value = maxy;
            comm.Parameters.Add("@MAP_USED", SqlDbType.Int).Value = mapUsed;
            int i = -1;
            try
            {
                i = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                i = -1;
            }
            return i;
        }

        public static bool IsExist_Map_Info()
        {
            SqlCommand comm = new SqlCommand("[PR_ISEXISTS_MAP_INFO]");
            comm.CommandType = CommandType.StoredProcedure;
            SqlParameter para1 = new SqlParameter("@RESULT", SqlDbType.VarChar, 1);
            para1.Direction = ParameterDirection.Output;
            comm.Parameters.Add(para1);
            SqlDBControl.ExecuteNonQuery(comm);
            return para1.Value.ToString() == "1";
        }

        /// 根据x,y取得mapno方法
        /// <summary>
        /// 根据x,y取得mapno方法
        /// </summary>
        /// <returns></returns>
        public static string GetMapNoByXY(int mapx, int mapy)
        {
            SqlCommand comm = new SqlCommand("[PR_GET_MAPNO_BYXY]");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@MAPX", SqlDbType.VarChar, 50).Value = mapx;
            comm.Parameters.Add("@MAPY", SqlDbType.VarChar, 50).Value = mapy;
            SqlParameter para1 = new SqlParameter("@MAPNO", SqlDbType.VarChar, 50);
            para1.Direction = ParameterDirection.Output;
            comm.Parameters.Add(para1);
            SqlDBControl.ExecuteQuery(comm);
            return para1.Value.ToString();
        }
    }
}
