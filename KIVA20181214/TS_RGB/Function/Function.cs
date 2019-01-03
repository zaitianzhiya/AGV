using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBControl;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using GetPath;

namespace Fuction
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


        #region ---------------------------------- KIVA ----------------------------------------------
        /// <summary>
        /// 更新（添加）AGV信息
        /// </summary>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_INFO(string AGV_Ip, int AGV_Ac, string AGV_Now_X, string AGV_Now_Y, string AGV_Skip_No,
            string AGV_Voltage, string AGV_Electricity, string AGV_L_Speed, string AGV_R_Speed, string AGV_ErrorCord, string AGV_WarningCord,
             string AGV_Now_Ord, int AGV_Now_Ord_Count, string AGV_Remaining_Trip, string AGV_Angle, string AGV_Skip_Angle, string AGV_Lifting_Speed,
            string AGV_Rotating_Speed, string AGV_OrderNo, int AGV_AtErWeiMa,string NowStats,int AtStas)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            //comm.Parameters.Add("@AGV_No", SqlDbType.VarChar, 50).Value = AGV_No;
            comm.Parameters.Add("@AGV_Ac", SqlDbType.Int, 10).Value = AGV_Ac;//1在线，0离线
            comm.Parameters.Add("@AGV_Now_X", SqlDbType.VarChar, 50).Value = AGV_Now_X;
            comm.Parameters.Add("@AGV_Now_Y", SqlDbType.VarChar, 50).Value = AGV_Now_Y;
            comm.Parameters.Add("@AGV_Skip_No", SqlDbType.VarChar, 50).Value = AGV_Skip_No;
            //comm.Parameters.Add("@AGV_From", SqlDbType.VarChar, 50).Value = AGV_From;//0
            //comm.Parameters.Add("@AGV_To", SqlDbType.VarChar, 50).Value = AGV_To;//0
            comm.Parameters.Add("@AGV_Voltage", SqlDbType.VarChar, 50).Value = AGV_Voltage;
            comm.Parameters.Add("@AGV_Electricity", SqlDbType.VarChar, 50).Value = AGV_Electricity;
            comm.Parameters.Add("@AGV_L_Speed", SqlDbType.VarChar, 50).Value = AGV_L_Speed;
            comm.Parameters.Add("@AGV_R_Speed", SqlDbType.VarChar, 50).Value = AGV_R_Speed;
            //comm.Parameters.Add("@AGV_LineNo", SqlDbType.VarChar, 50).Value = AGV_LineNo;//0
            //comm.Parameters.Add("@AGV_LineString", SqlDbType.VarChar, 50).Value = AGV_LineString;//0
            comm.Parameters.Add("@AGV_ErrorCord", SqlDbType.VarChar, 50).Value = AGV_ErrorCord;
            comm.Parameters.Add("@AGV_WarningCord", SqlDbType.VarChar, 50).Value = AGV_WarningCord;
            comm.Parameters.Add("@AGV_Now_Ord", SqlDbType.VarChar, 50).Value = AGV_Now_Ord;
            comm.Parameters.Add("@AGV_Now_Ord_Count", SqlDbType.Int, 10).Value = AGV_Now_Ord_Count;
            comm.Parameters.Add("@AGV_Remaining_Trip", SqlDbType.VarChar, 50).Value = AGV_Remaining_Trip;
            comm.Parameters.Add("@AGV_Angle", SqlDbType.VarChar, 50).Value = AGV_Angle;
            comm.Parameters.Add("@AGV_Skip_Angle", SqlDbType.VarChar, 50).Value = AGV_Skip_Angle;
            comm.Parameters.Add("@AGV_Lifting_Speed", SqlDbType.VarChar, 50).Value = AGV_Lifting_Speed;
            comm.Parameters.Add("@AGV_Rotating_Speed", SqlDbType.VarChar, 50).Value = AGV_Rotating_Speed;
            comm.Parameters.Add("@AGV_Order_No", SqlDbType.VarChar, 50).Value = AGV_OrderNo;
            comm.Parameters.Add("@AGV_AtErWeiMa", SqlDbType.Int, 10).Value = AGV_AtErWeiMa;//@
            comm.Parameters.Add("@AGV_Stats", SqlDbType.VarChar, 50).Value = NowStats;//AtStas
            comm.Parameters.Add("@AGV_AtStats", SqlDbType.Int).Value = AtStas;

            
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

        /// <summary>
        /// 更新Map表的MAP_Used字段 0：可用；1：不可用；2：已禁用
        /// </summary>
        /// <param name="Power_IP"></param>
        /// <param name="OutIN"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_MAP_Used(string Map_No, int Map_Used)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_MAP_Used");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_No", SqlDbType.VarChar, 50).Value = Map_No;
            comm.Parameters.Add("@Map_Used", SqlDbType.Int, 10).Value = Map_Used;
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

        /// <summary>
        /// 更新点状态
        /// </summary>
        /// <param name="Map_X"></param>
        /// <param name="Map_Y"></param>
        /// <param name="Map_State"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_Map_Info_State(string Map_X, string Map_Y, int Map_State)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_Map_Info_State");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_X", SqlDbType.VarChar, 50).Value = Map_X;
            comm.Parameters.Add("@Map_Y", SqlDbType.VarChar, 50).Value = Map_Y;
            comm.Parameters.Add("@Map_State", SqlDbType.Int, 10).Value = Map_State;
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
        //
        public static int PR_INSERT_Map_Info(int Map_State,string Map_X, string Map_Y,int Map_A,int Map_B,int Map_C,int Map_D,int Map_Used)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_Map_Info");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_No", SqlDbType.VarChar, 50).Value = Map_State;
            comm.Parameters.Add("@Map_X", SqlDbType.VarChar, 50).Value = Map_X;
            comm.Parameters.Add("@Map_Y", SqlDbType.VarChar, 50).Value = Map_Y;
            comm.Parameters.Add("@Map_A", SqlDbType.Int, 10).Value = Map_A;
            comm.Parameters.Add("@Map_B", SqlDbType.Int, 10).Value = Map_B;
            comm.Parameters.Add("@Map_C", SqlDbType.Int, 10).Value = Map_C;
            comm.Parameters.Add("@Map_D", SqlDbType.Int, 10).Value = Map_D;
            comm.Parameters.Add("@Map_Used", SqlDbType.Int, 10).Value = Map_Used;
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
        public static int KIVA_UPDATE_AGV_Order_OPachAgain(string OAGV, int OPachAgain)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_Order_OPachAgain");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@OAGV", SqlDbType.VarChar, 50).Value = OAGV;
            comm.Parameters.Add("@OPachAgain", SqlDbType.Int, 10).Value = OPachAgain;
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

        public static int KIVA_UPDATE_AGV_State(string AGV_Ip, string TuoPan_Low, string TuoPan_High, string ZhangAi_Stop, string ZhangAi_Slow, string ZhangAi_SmallSlow, string TuoPan_Zero, string SkipSacn_Beffor, string HasSkip)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_State");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            comm.Parameters.Add("@TuoPan_Low", SqlDbType.VarChar, 50).Value = TuoPan_Low;
            comm.Parameters.Add("@TuoPan_High", SqlDbType.VarChar, 50).Value = TuoPan_High;
            comm.Parameters.Add("@ZhangAi_Stop", SqlDbType.VarChar, 50).Value = ZhangAi_Stop;
            comm.Parameters.Add("@ZhangAi_Slow", SqlDbType.VarChar, 50).Value = ZhangAi_Slow;
            comm.Parameters.Add("@ZhangAi_SmallSlow", SqlDbType.VarChar, 50).Value = ZhangAi_SmallSlow;
            comm.Parameters.Add("@TuoPan_Zero", SqlDbType.VarChar, 50).Value = TuoPan_Zero;
            comm.Parameters.Add("@SkipSacn_Beffor", SqlDbType.VarChar, 50).Value = SkipSacn_Beffor;
            comm.Parameters.Add("@HasSkip", SqlDbType.VarChar, 50).Value = HasSkip;

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

        /// <summary>
        /// 解析，更新PR_UPDATE_AGV_INFO_AND_STATE_Map_Info_State
        /// </summary>
        /// <param name="AGV_Ip"></param>
        /// <param name="AGV_Ac"></param>
        /// <param name="AGV_Now_X"></param>
        /// <param name="AGV_Now_Y"></param>
        /// <param name="AGV_Skip_No"></param>
        /// <param name="AGV_Voltage"></param>
        /// <param name="AGV_Electricity"></param>
        /// <param name="AGV_L_Speed"></param>
        /// <param name="AGV_R_Speed"></param>
        /// <param name="AGV_ErrorCord"></param>
        /// <param name="AGV_WarningCord"></param>
        /// <param name="AGV_Now_Ord"></param>
        /// <param name="AGV_Now_Ord_Count"></param>
        /// <param name="AGV_Remaining_Trip"></param>
        /// <param name="AGV_Angle"></param>
        /// <param name="AGV_Skip_Angle"></param>
        /// <param name="AGV_Lifting_Speed"></param>
        /// <param name="AGV_Rotating_Speed"></param>
        /// <param name="AGV_OrderNo"></param>
        /// <param name="AGV_AtErWeiMa"></param>
        /// <param name="NowStats"></param>
        /// <param name="AtStas"></param>
        /// <param name="TuoPan_Low"></param>
        /// <param name="TuoPan_High"></param>
        /// <param name="ZhangAi_Stop"></param>
        /// <param name="ZhangAi_Slow"></param>
        /// <param name="ZhangAi_SmallSlow"></param>
        /// <param name="TuoPan_Zero"></param>
        /// <param name="SkipSacn_Beffor"></param>
        /// <param name="HasSkip"></param>
        /// <param name="Map_X"></param>
        /// <param name="Map_Y"></param>
        /// <param name="Map_State"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_INFO_AND_STATE_Map_Info_State(string AGV_Ip, int AGV_Ac, string AGV_Now_X, string AGV_Now_Y, string AGV_Skip_No,
            string AGV_Voltage, string AGV_Electricity, string AGV_L_Speed, string AGV_R_Speed, string AGV_ErrorCord, string AGV_WarningCord,
             string AGV_Now_Ord, int AGV_Now_Ord_Count, string AGV_Remaining_Trip, string AGV_Angle, string AGV_Skip_Angle, string AGV_Lifting_Speed,
            string AGV_Rotating_Speed, string AGV_OrderNo, int AGV_AtErWeiMa, string NowStats, int AtStas, string TuoPan_Low, string TuoPan_High, string ZhangAi_Stop, string ZhangAi_Slow, string ZhangAi_SmallSlow, string TuoPan_Zero, string SkipSacn_Beffor, string HasSkip
            , string Map_X, string Map_Y, int Map_State)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_INFO_AND_STATE_Map_Info_State");
            comm.CommandType = CommandType.StoredProcedure;
            //1
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            //comm.Parameters.Add("@AGV_No", SqlDbType.VarChar, 50).Value = AGV_No;
            comm.Parameters.Add("@AGV_Ac", SqlDbType.Int, 10).Value = AGV_Ac;//1在线，0离线
            comm.Parameters.Add("@AGV_Now_X", SqlDbType.VarChar, 50).Value = AGV_Now_X;
            comm.Parameters.Add("@AGV_Now_Y", SqlDbType.VarChar, 50).Value = AGV_Now_Y;
            comm.Parameters.Add("@AGV_Skip_No", SqlDbType.VarChar, 50).Value = AGV_Skip_No;
            //comm.Parameters.Add("@AGV_From", SqlDbType.VarChar, 50).Value = AGV_From;//0
            //comm.Parameters.Add("@AGV_To", SqlDbType.VarChar, 50).Value = AGV_To;//0
            comm.Parameters.Add("@AGV_Voltage", SqlDbType.VarChar, 50).Value = AGV_Voltage;
            comm.Parameters.Add("@AGV_Electricity", SqlDbType.VarChar, 50).Value = AGV_Electricity;
            comm.Parameters.Add("@AGV_L_Speed", SqlDbType.VarChar, 50).Value = AGV_L_Speed;
            comm.Parameters.Add("@AGV_R_Speed", SqlDbType.VarChar, 50).Value = AGV_R_Speed;
            //comm.Parameters.Add("@AGV_LineNo", SqlDbType.VarChar, 50).Value = AGV_LineNo;//0
            //comm.Parameters.Add("@AGV_LineString", SqlDbType.VarChar, 50).Value = AGV_LineString;//0
            comm.Parameters.Add("@AGV_ErrorCord", SqlDbType.VarChar, 50).Value = AGV_ErrorCord;
            comm.Parameters.Add("@AGV_WarningCord", SqlDbType.VarChar, 50).Value = AGV_WarningCord;
            comm.Parameters.Add("@AGV_Now_Ord", SqlDbType.VarChar, 50).Value = AGV_Now_Ord;
            comm.Parameters.Add("@AGV_Now_Ord_Count", SqlDbType.Int, 10).Value = AGV_Now_Ord_Count;
            comm.Parameters.Add("@AGV_Remaining_Trip", SqlDbType.VarChar, 50).Value = AGV_Remaining_Trip;
            comm.Parameters.Add("@AGV_Angle", SqlDbType.VarChar, 50).Value = AGV_Angle;
            comm.Parameters.Add("@AGV_Skip_Angle", SqlDbType.VarChar, 50).Value = AGV_Skip_Angle;
            comm.Parameters.Add("@AGV_Lifting_Speed", SqlDbType.VarChar, 50).Value = AGV_Lifting_Speed;
            comm.Parameters.Add("@AGV_Rotating_Speed", SqlDbType.VarChar, 50).Value = AGV_Rotating_Speed;
            comm.Parameters.Add("@AGV_Order_No", SqlDbType.VarChar, 50).Value = AGV_OrderNo;
            comm.Parameters.Add("@AGV_AtErWeiMa", SqlDbType.Int, 10).Value = AGV_AtErWeiMa;//@
            comm.Parameters.Add("@AGV_Stats", SqlDbType.VarChar, 50).Value = NowStats;//AtStas
            comm.Parameters.Add("@AGV_AtStats", SqlDbType.Int).Value = AtStas;
            //2
            //comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            comm.Parameters.Add("@TuoPan_Low", SqlDbType.VarChar, 50).Value = TuoPan_Low;
            comm.Parameters.Add("@TuoPan_High", SqlDbType.VarChar, 50).Value = TuoPan_High;
            comm.Parameters.Add("@ZhangAi_Stop", SqlDbType.VarChar, 50).Value = ZhangAi_Stop;
            comm.Parameters.Add("@ZhangAi_Slow", SqlDbType.VarChar, 50).Value = ZhangAi_Slow;
            comm.Parameters.Add("@ZhangAi_SmallSlow", SqlDbType.VarChar, 50).Value = ZhangAi_SmallSlow;
            comm.Parameters.Add("@TuoPan_Zero", SqlDbType.VarChar, 50).Value = TuoPan_Zero;
            comm.Parameters.Add("@SkipSacn_Beffor", SqlDbType.VarChar, 50).Value = SkipSacn_Beffor;
            comm.Parameters.Add("@HasSkip", SqlDbType.VarChar, 50).Value = HasSkip;
            //3
            comm.Parameters.Add("@Map_X", SqlDbType.VarChar, 50).Value = Map_X;
            comm.Parameters.Add("@Map_Y", SqlDbType.VarChar, 50).Value = Map_Y;
            comm.Parameters.Add("@Map_State", SqlDbType.Int, 10).Value = Map_State;


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

        /// <summary>
        /// 更新/插入Map_Info
        /// </summary>
        /// <param name="Map_No"></param>
        /// <param name="Map_X"></param>
        /// <param name="Map_Y"></param>
        /// <param name="Map_Used"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_Map_Info(string Map_No, string Map_X, string Map_Y, int Map_Used)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_Map_Info");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_No", SqlDbType.VarChar, 50).Value = Map_No;
            comm.Parameters.Add("@Map_X", SqlDbType.VarChar, 50).Value = Map_X;
            comm.Parameters.Add("@Map_Y", SqlDbType.VarChar, 50).Value = Map_Y;
            comm.Parameters.Add("@Map_Used", SqlDbType.Int, 10).Value = Map_Used;
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

        /// <summary>
        /// 更新AGV起点
        /// </summary>
        /// <param name="Map_No"></param>
        /// <param name="Map_Used"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_FROM(string AGV_Ip, string AGV_From)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_FROM");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            comm.Parameters.Add("@AGV_From", SqlDbType.VarChar, 50).Value = AGV_From;
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

        /// <summary>
        /// 更新AGV终点
        /// </summary>
        /// <param name="AGV_Ip"></param>
        /// <param name="AGV_To"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_TO(string AGV_Ip, string AGV_To)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_TO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            comm.Parameters.Add("@AGV_To", SqlDbType.VarChar, 50).Value = AGV_To;
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

        /// <summary>
        /// 更新AGV路线（AGV_LineNo and AGV_LineString）
        /// </summary>
        /// <param name="AGV_Ip"></param>
        /// <param name="AGV_To"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_Line(string AGV_Ip, string AGV_LineNo, string AGV_LineString)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_Line");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            comm.Parameters.Add("@AGV_LineNo", SqlDbType.VarChar, 50).Value = AGV_LineNo;
            comm.Parameters.Add("@AGV_LineString", SqlDbType.VarChar, 50).Value = AGV_LineString;

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

        /// <summary>
        /// 拆分任务
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public static string  splitOrders(string orders)
        {
            //坐标,角度;
            string finalResult = "";
            string X = "";
            string Y = "";
            string A = "";

            string[] order = orders.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (order.Length > 0)
            {
                for (int i = 0; i < order.Length; i++)
                {
                    DataTable res = KIVA_SELECT_MAPXY_BY_NO(order[i].Split(',')[0]);
                    if (res != null && res.Rows.Count > 0)
                    {
                        X = res.Rows[0][0].ToString().Trim();
                        Y = res.Rows[0][1].ToString().Trim();
                    }
                    A = order[i].Split(',')[1];

                    finalResult += X + "," + Y + "," + A + ",";
                }
                finalResult += "." + order.Length;//XYA.指令数量
                return finalResult;
            }
            else
                return null;
        }

        /// <summary>
        /// 更新AGV任务
        /// </summary>
        /// <param name="OAGV"></param>
        /// <param name="OType"></param>
        /// <param name="OString"></param>
        /// <param name="ONow"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_Order(string OAGV, int OType, string OString, string ONow,string FromX,string ToX)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_Order");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@OAGV", SqlDbType.VarChar, 50).Value = OAGV;
            comm.Parameters.Add("@OType", SqlDbType.Int, 10).Value = OType;
            comm.Parameters.Add("@OString", SqlDbType.VarChar, 50).Value = OString;
            comm.Parameters.Add("@ONow", SqlDbType.VarChar, 50).Value = ONow;
            comm.Parameters.Add("@FROMX", SqlDbType.VarChar, 50).Value = FromX;
            comm.Parameters.Add("@TOX", SqlDbType.VarChar, 50).Value = ToX;

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

        public static int KIVA_UPDATE_Order_At_Wac_Arivest_To(string AGV_IP, int OType, string OString, string ONow, string AGV_FROM, string AGV_To
           , string skipNo, string at, int wac, string wfloatno, int warivest, string fullpath)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_Order_At_Wac_Arivest_To");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_IP", SqlDbType.VarChar, 50).Value = AGV_IP;
            comm.Parameters.Add("@OType", SqlDbType.Int, 10).Value = OType;
            comm.Parameters.Add("@OString", SqlDbType.VarChar, 50).Value = OString;
            comm.Parameters.Add("@ONow", SqlDbType.VarChar, 50).Value = ONow;
            comm.Parameters.Add("@AGV_FROM", SqlDbType.VarChar, 50).Value = AGV_FROM;
            comm.Parameters.Add("@AGV_To", SqlDbType.VarChar, 50).Value = AGV_To;
            comm.Parameters.Add("@skipNo", SqlDbType.VarChar, 50).Value = skipNo;
            comm.Parameters.Add("@at", SqlDbType.VarChar, 50).Value = at;
            comm.Parameters.Add("@wac", SqlDbType.Int, 10).Value = wac;
            comm.Parameters.Add("@wfloatno", SqlDbType.VarChar, 50).Value = wfloatno;
            comm.Parameters.Add("@warivest", SqlDbType.Int, 10).Value = warivest;
            comm.Parameters.Add("@fullpath", SqlDbType.VarChar, 1000).Value = fullpath;
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

        /// <summary>
        /// 更新帧编码，命令数量
        /// </summary>
        /// <param name="AGV_Ip"></param>
        /// <param name="AGV_Command"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_ORDER_COMMAND(string AGV_Ip, int AGV_Command, int Order_Count)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_ORDER_COMMAND");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            comm.Parameters.Add("@AGV_Command", SqlDbType.Int).Value = AGV_Command;
            comm.Parameters.Add("@Order_Count", SqlDbType.Int).Value = Order_Count;

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

        /// <summary>
        /// 依据料车号更新wac
        /// </summary>
        /// <param name="skipNo"></param>
        /// <param name="wac"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_WCS_To_KIVA_wac(string skipNo, int wac)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_WCS_To_KIVA_wac");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@skipNo", SqlDbType.VarChar, 50).Value = skipNo;
            comm.Parameters.Add("@wac", SqlDbType.Int, 10).Value = wac;

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

        /// <summary>
        /// 依据AGV IP更新AGV_Order_OType
        /// </summary>
        /// <param name="OAGV"></param>
        /// <param name="OType"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_Order_OType(string OAGV, int OType)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_Order_OType");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@OAGV", SqlDbType.VarChar, 50).Value = OAGV;
            comm.Parameters.Add("@OType", SqlDbType.Int, 10).Value = OType;

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

        /// <summary>
        /// SET wagvat=@at  WHERE wfloatno=@skipNo
        /// </summary>
        /// <param name="skipNo"></param>
        /// <param name="at"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_WCS_To_KIVA_at(string skipNo, string at)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_WCS_To_KIVA_at");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@skipNo", SqlDbType.VarChar, 50).Value = skipNo;
            comm.Parameters.Add("@at", SqlDbType.VarChar, 50).Value = at;

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

        /// <summary>
        /// 更新料架角度
        /// </summary>
        /// <param name="skipNo"></param>
        /// <param name="at"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_WCS_To_KIVA_RecSkipAng(string skipNo, string RecSkipAng)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_WCS_To_KIVA_RecSkipAng");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@skipNo", SqlDbType.VarChar, 50).Value = skipNo;
            comm.Parameters.Add("@RecSkipAng", SqlDbType.VarChar, 50).Value = RecSkipAng;

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

        /// <summary>
        /// 依据AGV IP更新AGV_Order_ONow
        /// </summary>
        /// <param name="OAGV"></param>
        /// <param name="ONow"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_Order_ONow(string OAGV, int ONow,int Otype)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_Order_ONow");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@OAGV", SqlDbType.VarChar, 50).Value = OAGV;
            comm.Parameters.Add("@ONow", SqlDbType.VarChar, 50).Value = ONow;//@OFType
            comm.Parameters.Add("@OFType", SqlDbType.Int).Value = Otype;
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

        /// <summary>
        /// 更新AGV_ORDER的OLOCK字段
        /// </summary>
        /// <param name="OAGV"></param>
        /// <param name="ONow"></param>
        /// <param name="OLock"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_ORDER_OLOCK(string OAGV, int OLock)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_ORDER_OLOCK");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@OAGV", SqlDbType.VarChar, 50).Value = OAGV;
            comm.Parameters.Add("@OLock", SqlDbType.Int).Value = OLock;
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

        public static int PR_UPDATE_AGV_INFO_PACH(string OAGV, int Pach)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_INFO_PACH");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_IP", SqlDbType.VarChar, 50).Value = OAGV;
            comm.Parameters.Add("@PACH", SqlDbType.Int).Value = Pach;
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

        /// <summary>
        /// update ARIVEST
        /// </summary>
        /// <param name="wfloatno"></param>
        /// <param name="Pach"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_ARIVEST(string wfloatno, int warivest)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_ARIVEST");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@wfloatno", SqlDbType.VarChar, 50).Value = wfloatno;
            comm.Parameters.Add("@warivest", SqlDbType.Int).Value = warivest;

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

        public static int KIVA_UPDATE_AGV_Info_Lock(string AGV_Ip, int Lock)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_Info_Lock");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            comm.Parameters.Add("@Lock", SqlDbType.Int).Value = Lock;
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

        public static int KIVA_UPDATE_WCS_WSTRING_WAC(string wfloatno, string wstring, int wac,string NowRFID)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_WCS_WSTRING_WAC_S");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@wfloatno", SqlDbType.VarChar, 50).Value = wfloatno;
            comm.Parameters.Add("@wstring", SqlDbType.VarChar, 1000).Value = wstring;
            comm.Parameters.Add("@wac", SqlDbType.Int).Value = wac;
            comm.Parameters.Add("@wNowRFID", SqlDbType.VarChar, 50).Value = NowRFID;
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

        public static int KIVA_UPDATE_WCS_WAC(string wfloatno, int wac)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_WCS_WAC");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@wfloatno", SqlDbType.VarChar, 50).Value = wfloatno;
            comm.Parameters.Add("@wac", SqlDbType.Int).Value = wac;
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

        //
        public static int PR_UPDATE_Map_Info_UN_used(int Map_No)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_Map_Info_UN_used");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_No", SqlDbType.Int).Value = Map_No;
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

        public static DataTable PR_SELECT_Map_Info()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Map_Info");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        //
        public static DataTable KIVA_SELECT_AGV_Order_all()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_Order_all");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static DataTable PR_SELECT_Charge_at()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Charge_at");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        /// <summary>
        /// all
        /// </summary>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_AGV_Info()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_Info");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable PR_SELECT_Charge_wac(string skip_no, string agv_ip)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Charge_wac");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@skip_no", SqlDbType.VarChar, 50).Value = skip_no;
            comm.Parameters.Add("@agv_ip", SqlDbType.VarChar, 50).Value = agv_ip;


            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }


        public static DataTable KIVA_SELECT_AGV_Info_FORCHARGE()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_Info_FORCHARGE");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        /// <summary>
        /// agv and wcs_arivst
        /// </summary>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_AGV_ARIVEST()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_ARIVEST");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable KIVA_SELECT_AGV_Order_show()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_Order_show");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 查找已知点四周占用点
        /// </summary>
        /// <param name="AGV_X"></param>
        /// <param name="AGV_Y"></param>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_MAP_Around(string AGV_X, string AGV_Y)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_MAP_Around");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_X", SqlDbType.VarChar, 50).Value = AGV_X;
            comm.Parameters.Add("@AGV_Y", SqlDbType.VarChar, 50).Value = AGV_Y;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable KIVA_SELECT_AGV_FinalCount(string Final_Location)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_FinalCount");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Final_Location", SqlDbType.VarChar, 50).Value = Final_Location;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 由编号查坐标
        /// </summary>
        /// <param name="Map_No"></param>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_MAPXY_BY_NO(string Map_No)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_MAPXY_BY_NO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_No", SqlDbType.VarChar, 50).Value = Map_No;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }


        public static DataTable KIVA_SELECT_Map_Info_State(int Map_State)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Map_Info_State");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_State", SqlDbType.VarChar, 50).Value = Map_State;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 查找某行空位
        /// </summary>
        /// <param name="Map_X"></param>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_MAP_Null(string Map_X)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_MAP_Null");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_X", SqlDbType.VarChar, 50).Value = Map_X;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        //
        public static DataTable PR_SELECT_AGV_ST(string AGV_IP,int ST_Type)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_ST");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_IP", SqlDbType.VarChar, 50).Value = AGV_IP;
            comm.Parameters.Add("@ST_TYPE", SqlDbType.Int).Value = ST_Type;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }


        /// <summary>
        /// 查找路径选择规则
        /// </summary>
        /// <param name="r_from_X"></param>
        /// <param name="r_from_Y"></param>
        /// <param name="r_to_X"></param>
        /// <param name="r_to_Y"></param>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_Ruler(int r_from_X, int r_from_Y, int r_to_X, int r_to_Y, int r_direction)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Ruler");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@r_from_X", SqlDbType.Int, 10).Value = r_from_X;
            comm.Parameters.Add("@r_from_Y", SqlDbType.Int, 10).Value = r_from_Y;
            comm.Parameters.Add("@r_to_X", SqlDbType.Int, 10).Value = r_to_X;
            comm.Parameters.Add("@r_to_Y", SqlDbType.Int, 10).Value = r_to_Y;
            comm.Parameters.Add("@r_direction", SqlDbType.Int, 10).Value = r_direction;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 查询全部AGV_Order
        /// </summary>
        /// <param name="Map_No"></param>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_AGV_Order()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_Order");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 查询全部AGV_Order
        /// </summary>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_AGV_Order_S()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_Order2");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// SELECT_AGV_Info_ByIP
        /// </summary>
        /// <param name="AGV_Ip"></param>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_AGV_Info_ByIP(string AGV_Ip)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_Info_ByIP");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// wac=1
        /// </summary>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_WCS_To_KIVA()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_WCS_To_KIVA");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 查询全部wcs
        /// </summary>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_WCS_To_KIVA_all()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_WCS_To_KIVA_all");
            comm.CommandType = CommandType.StoredProcedure;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable KIVA_SELECT_WCS_To_KIVA_for_auto_updateWac()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_WCS_To_KIVA_for_auto_updateWac");
            comm.CommandType = CommandType.StoredProcedure;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 依据料车号查找AGV IP
        /// </summary>
        /// <param name="OAGV"></param>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_AGV_Info_BySkip_No(string AGV_Skip_No)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_Info_BySkip_No");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_Skip_No", SqlDbType.VarChar, 50).Value = AGV_Skip_No;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 依据编号查找点状态
        /// </summary>
        /// <param name="Map_No"></param>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_MAP_BYNO(string Map_No)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_MAP_BYNO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_No", SqlDbType.VarChar, 50).Value = Map_No;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 依据AGV IP查询AGV Order
        /// </summary>
        /// <param name="OAGV"></param>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_AGV_Order_By_OAGV(string OAGV)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_Order_By_OAGV");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@OAGV", SqlDbType.VarChar, 50).Value = OAGV;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// SELECT Map_No FROM tb_Map_Info WHERE Map_X=@Map_X AND Map_Y=@Map_Y
        /// </summary>
        /// <param name="Map_X"></param>
        /// <param name="Map_Y"></param>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_MAPNO_BYXY(string Map_X, string Map_Y)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_MAPNO_BYXY");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Map_X", SqlDbType.VarChar, 50).Value = Map_X;
            comm.Parameters.Add("@Map_Y", SqlDbType.VarChar, 50).Value = Map_Y;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 查询全部AGV信息
        /// </summary>
        /// <returns></returns>
        public static DataTable KIVA_SELETE_AGV_INFO()
        {
            SqlCommand comm = new SqlCommand("PR_SELETE_AGV_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable KIVA_SELECT_MAPNo_used()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_MAPNo_used");
            comm.CommandType = CommandType.StoredProcedure;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception ex)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 查询全部Map信息
        /// </summary>
        /// <returns></returns>
        public static DataTable KIVA_SELETE_Map_Info()
        {
            SqlCommand comm = new SqlCommand("PR_SELETE_Map_Info");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        /// <summary>
        /// 更新充电站状态1
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_Charge1(string Charge_IP, string AGV_From, int Charge_state, string AGV_Ip)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_Charge1");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Charge_IP", SqlDbType.VarChar, 50).Value = Charge_IP;
            comm.Parameters.Add("@AGV_From", SqlDbType.VarChar, 50).Value = AGV_From;
            comm.Parameters.Add("@Charge_state", SqlDbType.Int, 10).Value = Charge_state;
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;

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

        /// <summary>
        /// 查找可用充电站
        /// </summary>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_Charge_canuse()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Charge_canuse");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable KIVA_SELECT_Turn_Angle()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Turn_Angle");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static int KIVA_UPDATE_Charge(string IP, int state)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_Charge");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@IP", SqlDbType.VarChar, 50).Value = IP;
            comm.Parameters.Add("@state", SqlDbType.Int, 10).Value = state;
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

        /// <summary>
        /// 查询已经到充电站位的AGV
        /// </summary>
        /// <returns></returns>
        public static DataTable KIVA_SELECT_Charge_Ready(int Lent)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Charge_Ready");
            comm.CommandType = CommandType.StoredProcedure;//
            comm.Parameters.Add("@Len", SqlDbType.Int).Value = Lent;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable KIVA_SELECT_Charge_end(int Lent)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Charge_end");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Len", SqlDbType.Int).Value = Lent;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        #endregion
        //====================================================================================
        public static int PR_UPDATE_Charge_Info_datetime(string Power_IP, string AGV_IP)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_Charge_Info_datetime");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@IP", SqlDbType.VarChar, 50).Value = Power_IP;
            comm.Parameters.Add("@Charge_AGV", SqlDbType.VarChar, 50).Value = AGV_IP;
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

        /// <summary>
        /// 添加RGB任务信息
        /// </summary>
        /// <param name="RGB_ORD_TYPE">任务类型,1呼车,2发车</param>
        /// <param name="RGB_CAR_NUMBER">小车编号"xxx"</param>
        /// <param name="RGB_FROM">出发站编号"xxx"</param>
        /// <param name="RGB_TO">到达站编号"xxx"</param>
        /// <param name="RGB_AC">命令状态0,已完成,1已接受,2执行中</param>
        /// <returns></returns>
        public static int Add_OrdMessage(string RGB_ORD_TYPE, string RGB_CAR_NUMBER, string RGB_FROM, string RGB_TO, int RGB_AC)
        {
            SqlCommand comm = new SqlCommand("ADD_RGB_ORDMESSAGE");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RGB_ORD_TYPE", SqlDbType.NChar, 10).Value = RGB_ORD_TYPE;
            comm.Parameters.Add("@RGB_CAR_NUMBER", SqlDbType.VarChar, 10).Value = RGB_CAR_NUMBER;
            comm.Parameters.Add("@RGB_FROM", SqlDbType.NChar, 10).Value = RGB_FROM;
            comm.Parameters.Add("@RGB_TO", SqlDbType.NChar, 10).Value = RGB_TO;
            comm.Parameters.Add("@RGB_AC", SqlDbType.Int, 2).Value = RGB_AC;
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
        /// <summary>
        /// 增加小车
        /// </summary>
        /// <param name="RGB_NUMBER">小车编号"xxx"</param>
        /// <param name="RGB_STA_NOW">小车当前位置"xxx"</param>
        /// <param name="RGB_AC">小车状态0，禁用；1，待命中；2，运行中</param>
        /// <returns></returns>
        public static int Add_RGB_CARMESSAGE(string RGB_NUMBER, string RGB_STA_NOW, int RGB_AC)
        {
            SqlCommand comm = new SqlCommand("ADD_RGB_CARMESSAGE");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RGB_NUMBER", SqlDbType.VarChar, 10).Value = RGB_NUMBER;
            comm.Parameters.Add("@RGB_STA_NOW", SqlDbType.NChar, 10).Value = RGB_STA_NOW;
            comm.Parameters.Add("@RGB_AC", SqlDbType.Int, 2).Value = RGB_AC;
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
        /// <summary>
        /// 更新、添加小车
        /// </summary>
        /// <param name="AGV_IP"></param>
        /// <param name="AGV_AC"></param>
        /// <param name="AGV_RFID_NOW"></param>
        /// <param name="AGV_FROM"></param>
        /// <param name="AGV_TO"></param>
        /// <param name="AGV_LineNo"></param>
        /// <returns></returns>
        public static int INSERET_AGV_INFO(string AGV_IP, int AGV_AC, string AGV_RFID_NOW, string AGV_FROM, string AGV_TO, string AGV_LineNo, string AGV_NUM, string ErroeCode, string Power, string Speed, int FW)
        {
            SqlCommand comm = new SqlCommand("PR_INSERET_AGV_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_IP", SqlDbType.VarChar, 50).Value = AGV_IP;
            comm.Parameters.Add("@AGV_AC", SqlDbType.Int, 10).Value = AGV_AC;
            comm.Parameters.Add("@AGV_RFID_Now", SqlDbType.VarChar, 50).Value = AGV_RFID_NOW;
            comm.Parameters.Add("@AGV_FROM", SqlDbType.VarChar, 50).Value = AGV_FROM;
            comm.Parameters.Add("@AGV_TO", SqlDbType.VarChar, 50).Value = AGV_TO;
            comm.Parameters.Add("@AGV_LINE", SqlDbType.VarChar, 50).Value = AGV_LineNo;
            comm.Parameters.Add("@AGV_REMARK", SqlDbType.VarChar, 50).Value = AGV_NUM;
            comm.Parameters.Add("@AGV_ErrorCord", SqlDbType.VarChar, 50).Value = ErroeCode;
            comm.Parameters.Add("@AGV_Power", SqlDbType.VarChar, 50).Value = Power;
            comm.Parameters.Add("@AGV_Speed", SqlDbType.VarChar, 50).Value = Speed;
            comm.Parameters.Add("@AGV_FW", SqlDbType.Int).Value = FW;
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

        //
        public static int UPDATE_POWER(string Power_IP, int OutIN)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_POWER");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@P_IP", SqlDbType.VarChar, 50).Value = Power_IP;
            comm.Parameters.Add("@P_INOUT", SqlDbType.Int, 10).Value = OutIN;
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

        public static int UPDATE_Cross_cac(int cid)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_Cross_cac");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Cid", SqlDbType.Int, 10).Value = cid;
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
        public static int UPDATE_Cross_cac_agv(int cid)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_Cross_cac_agv");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@cac", SqlDbType.Int, 10).Value = cid;
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
        public static int UPDATE_ELC_InAGVNo(int agvNo)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_ELC_InAGVNo");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@SagvNo", SqlDbType.Int, 10).Value = agvNo;
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

        //
        public static int UPDATE_AutoDoor(string Auto_IP, int OutIN)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AutoDoor");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@D_ip", SqlDbType.VarChar, 50).Value = Auto_IP;
            comm.Parameters.Add("@OutIn", SqlDbType.Int, 10).Value = OutIN;
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
        //       
        //
        public static int UPDATE_ELC(string ELEC_IP, int LINK_AC, int LOUCENG, int CANIN, int Sac)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_ELC");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@SIP", SqlDbType.VarChar, 50).Value = ELEC_IP;
            comm.Parameters.Add("@LINKAC", SqlDbType.Int, 10).Value = LINK_AC;
            comm.Parameters.Add("@LOUCENG", SqlDbType.Int, 10).Value = LOUCENG;
            comm.Parameters.Add("@CANIN", SqlDbType.Int, 10).Value = CANIN;
            comm.Parameters.Add("@AC", SqlDbType.Int, 10).Value = Sac;
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
        //yws
        public static int UPDATE_CallBoxLogic(string LCallBoxNo, string LAnJianNo, string LIsDown)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_CallBoxLogic");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@LCallBoxNo", SqlDbType.VarChar, 50).Value = LCallBoxNo;
            comm.Parameters.Add("@LAnJianNo", SqlDbType.VarChar, 50).Value = LAnJianNo;
            comm.Parameters.Add("@LIsDown", SqlDbType.VarChar, 50).Value = LIsDown;


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

        public static int UPDATE_CallBox(string LCallBoxNo, string SAc)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_CallBox");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@SCallBoxNo", SqlDbType.VarChar, 50).Value = LCallBoxNo;
            comm.Parameters.Add("@SAc", SqlDbType.Int, 10).Value = SAc;


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

        public static int INSERT_ELC(string EL_No, string EL_IP, int EL_X, int EL_Y, string EL_RFID, int EL_OUT)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_ELC");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Snumber", SqlDbType.VarChar, 50).Value = EL_No;
            comm.Parameters.Add("@Sip", SqlDbType.VarChar, 50).Value = EL_IP;
            comm.Parameters.Add("@Sx", SqlDbType.Int, 10).Value = EL_X;
            comm.Parameters.Add("@Sy", SqlDbType.Int, 10).Value = EL_Y;
            comm.Parameters.Add("@Srfid", SqlDbType.VarChar, 50).Value = EL_RFID;
            comm.Parameters.Add("@Sout", SqlDbType.Int, 10).Value = EL_OUT;
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
        ///
        public static int INSERT_CALLBOX(string SCallBoxNo, string Sip, int SCount, int Sx, int Sy, string SRFID)
        {
            SqlCommand comm = new SqlCommand("[PR_INSERT_CallBox]");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@SCallBoxNo", SqlDbType.VarChar, 50).Value = SCallBoxNo;
            comm.Parameters.Add("@Sip", SqlDbType.VarChar, 50).Value = Sip;
            comm.Parameters.Add("@SCount", SqlDbType.Int, 10).Value = SCount;
            comm.Parameters.Add("@Sx", SqlDbType.Int, 10).Value = Sx;
            comm.Parameters.Add("@Sy", SqlDbType.Int, 10).Value = Sy;
            comm.Parameters.Add("@SRFID", SqlDbType.Int, 10).Value = SRFID;


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
        ///
        public static int INSERT_CallBoxLogic(string LCallBoxNo, string LAnJianNo, string LToAGV_IP, string LIsDown, string LTask)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_CallBoxLogic");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@LCallBoxNo", SqlDbType.VarChar, 50).Value = LCallBoxNo;
            comm.Parameters.Add("@LAnJianNo", SqlDbType.VarChar, 50).Value = LAnJianNo;
            comm.Parameters.Add("@LToAGV_IP", SqlDbType.VarChar, 50).Value = LToAGV_IP;
            comm.Parameters.Add("@LIsDown", SqlDbType.VarChar, 50).Value = LIsDown;
            comm.Parameters.Add("@LTask", SqlDbType.VarChar, 50).Value = LTask;
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
        ///
        public static DataTable SELECT_CallBox_IP(string C_No)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_CallBox_IP");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@SCallBoxNo", SqlDbType.VarChar, 50).Value = C_No;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        ///
        public static DataTable SELECT_CallBoxLogic_IP(string C_No)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_CallBoxLogic_IP");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@LCallBoxNo", SqlDbType.VarChar, 50).Value = C_No;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable KIVA_SELECT_charge_Info_byagvIP(string agvip)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_charge_Info_byagvIP");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@agvip", SqlDbType.VarChar, 50).Value = agvip;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }


        public static int INSERT_Elec(string EL_IP, string EL_Name, string LineNo, string InRFID, int InLouceng, int Infw, int Outlouceng, string OutRFID)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_Elec");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Sip", SqlDbType.VarChar, 50).Value = EL_IP;
            comm.Parameters.Add("@Lname", SqlDbType.VarChar, 50).Value = EL_Name;
            comm.Parameters.Add("@LmainLineno", SqlDbType.VarChar, 50).Value = LineNo;
            comm.Parameters.Add("@Linrfid", SqlDbType.VarChar, 50).Value = InRFID;
            comm.Parameters.Add("@Linlouceng", SqlDbType.Int, 10).Value = InLouceng;
            comm.Parameters.Add("@Linfow", SqlDbType.Int, 10).Value = Infw;
            comm.Parameters.Add("@Loutlouceng", SqlDbType.Int, 10).Value = Outlouceng;
            comm.Parameters.Add("@Loutrfid", SqlDbType.VarChar, 50).Value = OutRFID;
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


        //
        public static int UPDATE_tb_AGV_INFO()
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_tb_AGV_INFO");
            comm.CommandType = CommandType.StoredProcedure;
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int PR_FORMART_tb_AGV_INFO()
        {
            SqlCommand comm = new SqlCommand("PR_FORMART_tb_AGV_INFO");
            comm.CommandType = CommandType.StoredProcedure;
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

        public static int UPDATE_tb_AGV_INFO(string MACH_IP)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_tb_AGV_INFO_AC");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_IP", SqlDbType.VarChar, 50).Value = MACH_IP;
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


        public static int DELETE_AGV_INFO(int AGV_ID)
        {
            SqlCommand comm = new SqlCommand("PR_DELETE_AGV_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_ID", SqlDbType.Int, 10).Value = AGV_ID;
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

        /// <summary>
        /// 更新命令
        /// </summary>
        /// <param name="RGB_CAR_ID"></param>
        /// <param name="RGB_CAR_NUMBER"></param>
        /// <returns></returns>
        public static int UPDATE_ORD_MESSAGE(int RGB_CAR_ID, string RGB_CAR_NUMBER, int RGB_AC)
        {
            SqlCommand comm = new SqlCommand("UPDATE_ORDMESSAGE");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RGB_CAR_ID", SqlDbType.Int, 10).Value = RGB_CAR_ID;
            comm.Parameters.Add("@RGB_CAR_NUMBER", SqlDbType.VarChar, 10).Value = RGB_CAR_NUMBER;
            comm.Parameters.Add("@RGB_AC", SqlDbType.Int, 2).Value = RGB_AC;
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

        /// <summary>
        /// 查找RFID MAP
        /// </summary>
        /// <returns></returns>
        public static DataTable Select_RFID_MAP()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_RFID_MAP");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        /// <summary>
        /// 查询全部AGV_Info
        /// </summary>
        /// <returns></returns>
        public static DataTable SELETE_AGV_INFO()
        {
            SqlCommand comm = new SqlCommand("PR_SELETE_AGV_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable KIVA_SELECT_Charge()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Charge");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable PR_SELECT_Charge_Small(string agv_IP)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Charge_Small");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@agv_IP", SqlDbType.VarChar, 50).Value = agv_IP;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable PR_SELECT_Charge_Info(int Arrver_X,int Arrver_Y)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Charge_Info");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Charge_X", SqlDbType.Int).Value = Arrver_X;
            comm.Parameters.Add("@Charge_Y", SqlDbType.Int).Value = Arrver_Y;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable SELECT_AGV_AC()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_AC");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        //
        public static DataTable SELECT_AGV_ORDER(string AGV_No)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_ORDER");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@OAGV", SqlDbType.VarChar, 50).Value = AGV_No;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        //
        public static DataTable SELECT_ELC_MAINLINENO(string LineNo, string AGV_IP)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_ELC_MAINLINENO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@MAINLINENO", SqlDbType.VarChar, 50).Value = LineNo;
            comm.Parameters.Add("@AGV_IP", SqlDbType.VarChar, 50).Value = AGV_IP;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        //
        public static DataTable SELECT_CROSS_INFO(int AGV_NO, string AGV_RFID, int AGV_FW, string OrderNo, string AGV_IP)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_CROSS_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGV_NO", SqlDbType.Int, 10).Value = AGV_NO;
            comm.Parameters.Add("@RFID", SqlDbType.VarChar, 50).Value = AGV_RFID;
            comm.Parameters.Add("@FW", SqlDbType.Int, 10).Value = AGV_FW;
            comm.Parameters.Add("@Cno", SqlDbType.VarChar, 50).Value = OrderNo;
            comm.Parameters.Add("@AGV_IP", SqlDbType.VarChar, 50).Value = AGV_IP;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        //
        public static DataTable SELECT_Power_INFO_ORDER(string AGV_RFID, int AGV_FW, string OrderNo, string AGV_IP)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Power_INFO_ORDER");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RFID", SqlDbType.VarChar, 50).Value = AGV_RFID;
            comm.Parameters.Add("@FW", SqlDbType.Int, 10).Value = AGV_FW;
            comm.Parameters.Add("@P_IP", SqlDbType.VarChar, 50).Value = OrderNo;
            comm.Parameters.Add("@AGV_IP", SqlDbType.VarChar, 50).Value = AGV_IP;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        //
        public static DataTable SELECT_AutoDoor_INFO_ORDER(string AGV_RFID, int AGV_FW, string OrderNo, string AGV_IP)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AutoDoor_INFO_ORDER");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RFID", SqlDbType.VarChar, 50).Value = AGV_RFID;
            comm.Parameters.Add("@FW", SqlDbType.Int, 10).Value = AGV_FW;
            comm.Parameters.Add("@P_IP", SqlDbType.VarChar, 50).Value = OrderNo;
            comm.Parameters.Add("@AGV_IP", SqlDbType.VarChar, 50).Value = AGV_IP;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        //
        public static DataTable SELECT_AuotDoor_INFO()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AuotDoor_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        //
        public static DataTable PSELECT_ELC()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_ELC");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static DataTable SELECT_CallBox()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_CallBox");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        //
        public static DataTable SELECT_CallBoxLogic()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_CallBoxLogic");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        #region Management
        public static DataTable SELECT_AGV_INFO_MAXREMARK()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_INFO_MAXREMARK");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static DataTable SELECT_ORDER_TYPE(int otype)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_ORDER_TYPE");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@otype", SqlDbType.Int, 10).Value = otype;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static int INSERT_ORDER_Info_FROM_AGV(string oagv, int otype, string ostring)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_ORDER_Info_FROM_AGV");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@oagv", SqlDbType.VarChar, 50).Value = oagv;
            comm.Parameters.Add("@otype", SqlDbType.Int, 10).Value = otype;
            comm.Parameters.Add("@ostring", SqlDbType.VarChar, 50).Value = ostring;

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
        public static int DELETE_ORDEF_INFO(int oid)
        {
            SqlCommand comm = new SqlCommand("PR_DELETE_ORDEF_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@oid", SqlDbType.Int, 10).Value = oid;
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
        public static DataTable SELECT_ORDER_INFO()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_ORDER_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        /// <summary>
        /// 删除路口管控
        /// </summary>
        /// <param name="AGV_ID"></param>
        /// <returns></returns>
        public static int DELETE_CROSS_INFO(int cid)
        {
            SqlCommand comm = new SqlCommand("PR_DELETE_CROSS_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@cid", SqlDbType.Int, 10).Value = cid;
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
        /// <summary>
        /// 添加路口管控（cross，order）
        /// </summary>
        /// <param name="RFID"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int INSERT_CrossingANDOrder_hand(string agvNo, string areaNo, string inRFID, int inDir, string outRFID, int outDir)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_CrossingANDOrder_hand");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@agvNo", SqlDbType.VarChar, 50).Value = agvNo;
            comm.Parameters.Add("@areaNo", SqlDbType.VarChar, 50).Value = areaNo;
            comm.Parameters.Add("@inRFID", SqlDbType.VarChar, 50).Value = inRFID;
            comm.Parameters.Add("@inDir", SqlDbType.Int, 10).Value = inDir;
            comm.Parameters.Add("@outRFID", SqlDbType.VarChar, 50).Value = outRFID;
            comm.Parameters.Add("@outDir", SqlDbType.Int, 10).Value = outDir;

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
        public static int INSERT_Crossing_hand(string areaNo, string inRFID, int inDir, string outRFID, int outDir)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_Crossing_hand");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@areaNo", SqlDbType.VarChar, 50).Value = areaNo;
            comm.Parameters.Add("@inRFID", SqlDbType.VarChar, 50).Value = inRFID;
            comm.Parameters.Add("@inDir", SqlDbType.Int, 10).Value = inDir;
            comm.Parameters.Add("@outRFID", SqlDbType.VarChar, 50).Value = outRFID;
            comm.Parameters.Add("@outDir", SqlDbType.Int, 10).Value = outDir;

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
        public static DataTable SELECT_USED_CNO_INCROSSING()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_USED_CNO_INCROSSING");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static int UPDATE_BufferInfo_Count(string RFID, int count)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_BufferInfo_Count");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RFID", SqlDbType.VarChar, 50).Value = RFID;
            comm.Parameters.Add("@count", SqlDbType.Int, 10).Value = count;

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
        public static DataTable SELECT_AGV_BYRFID(string RFID)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_BYRFID");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RFID", SqlDbType.VarChar, 50).Value = RFID;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static DataTable SELECT_BufferInfo_Count()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_BufferInfo_Count");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static DataTable SELECT_BufferInfo_RFID(string RFID)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_BufferInfo_RFID");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RFID", SqlDbType.VarChar, 50).Value = RFID;

            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static int INSERT_BufferInfo(string RFID, string AGVIP, int Count, string Date)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_BufferInfo");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RFID", SqlDbType.VarChar, 50).Value = RFID;
            comm.Parameters.Add("@AGVIP", SqlDbType.VarChar, 50).Value = AGVIP;
            comm.Parameters.Add("@Count", SqlDbType.Int, 10).Value = Count;
            comm.Parameters.Add("@Date", SqlDbType.VarChar, 50).Value = Date;

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

        public static int INSERT_Buffer_temp(string RFID, string AGVIP, string Date)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_Buffer_temp");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RFID", SqlDbType.VarChar, 50).Value = RFID;
            comm.Parameters.Add("@AGVIP", SqlDbType.VarChar, 50).Value = AGVIP;
            comm.Parameters.Add("@Date", SqlDbType.VarChar, 50).Value = Date;

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

        public static DataTable SELECT_BufferInfo()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_BufferInfo");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static DataTable SELECT_Proj()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Proj");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable SELECT_Area_All()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Area_All");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static int Insert_Area_All(string areaNo, string areaname, string RFIDs, string note)
        {
            SqlCommand comm = new SqlCommand("PR_Insert_Area_All");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@areaNo", SqlDbType.VarChar, 50).Value = areaNo;
            comm.Parameters.Add("@areaname", SqlDbType.VarChar, 50).Value = areaname;
            comm.Parameters.Add("@RFIDs", SqlDbType.VarChar, 50).Value = RFIDs;
            comm.Parameters.Add("@note", SqlDbType.VarChar, 50).Value = note;

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
        public static int DELETE_Area_AreaNo(string areaNo)
        {
            SqlCommand comm = new SqlCommand("PR_DELETE_Area_AreaNo");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@areaNo", SqlDbType.VarChar, 50).Value = areaNo;
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
        #endregion

        //
        public static DataTable SELECT_Power_INFO()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Power_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        //
        public static DataTable SELECT_ELC_IP(string IP)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_ELC_IP");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Sip", SqlDbType.VarChar, 50).Value = IP;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static DataTable SELECT_Cross_ALL()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Cross_ALL");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        public static DataTable SELECT_CallBox_ByCallBoxNo(string callBoxNo)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_CallBox_ByCallBoxNo");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@SCallBoxNo", SqlDbType.VarChar, 50).Value = callBoxNo;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        ///
        public static DataTable SELECT_AGV_BYIP(string IP)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_BYIP");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@ip", SqlDbType.VarChar, 50).Value = IP;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        //

        public static DataTable SELECT_Power_INFO_IP(string IP)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Power_INFO_IP");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@P_IP", SqlDbType.VarChar, 50).Value = IP;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable SELECT_Elec_IP(string IP)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Elec_IP");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Sip", SqlDbType.VarChar, 50).Value = IP;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        /// <summary>
        /// 增加RFID地图
        /// </summary>
        /// <param name="RFID_Tag">RFID编号</param>
        /// <param name="RFID_x">RFID坐标-X</param>
        /// <param name="RFID_y">RFID坐标-Y</param>
        /// <param name="RFID_type">1,功能性标签，2,路径标签</param>
        /// <param name="RFID_Last_Tag">上一个RFID</param>
        /// <param name="RFID_Cross_Type">1，直行经过，2，左转经过，3右转经过</param>
        /// <returns></returns>
        public static int INSERET_RFID_MAP(string RFID_Tag, string RFID_x, string RFID_y, int RFID_type, string RFID_Last_Tag, int RFID_Cross_Type, string CrossNo, string Lineno)
        {
            SqlCommand comm = new SqlCommand("PR_INSERET_RFID_MAP");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@TAGVAULE", SqlDbType.VarChar, 50).Value = RFID_Tag;
            comm.Parameters.Add("@TX", SqlDbType.VarChar, 50).Value = RFID_x;
            comm.Parameters.Add("@TY", SqlDbType.VarChar, 50).Value = RFID_y;
            comm.Parameters.Add("@TTYPE", SqlDbType.Int, 10).Value = RFID_type;
            comm.Parameters.Add("@TFROM", SqlDbType.VarChar, 50).Value = RFID_Last_Tag;
            comm.Parameters.Add("@TSF", SqlDbType.Int, 10).Value = RFID_Cross_Type;
            comm.Parameters.Add("@TCROSS", SqlDbType.VarChar, 50).Value = CrossNo;
            comm.Parameters.Add("@TLINENO", SqlDbType.VarChar, 50).Value = Lineno;
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

        //PR_DELETE_RFID_MAP
        /// <summary>
        /// 删除RFID MAP
        /// </summary>
        /// <param name="RFID_Tag"></param>
        /// <param name="RFID_x"></param>
        /// <param name="RFID_y"></param>
        /// <returns></returns>
        public static int DELETE_RFID_MAP(string RFID_Tag, string RFID_x, string RFID_y)
        {
            SqlCommand comm = new SqlCommand("PR_DELETE_RFID_MAP");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@TAGVAULE", SqlDbType.VarChar, 50).Value = RFID_Tag;
            comm.Parameters.Add("@TX", SqlDbType.VarChar, 50).Value = RFID_x;
            comm.Parameters.Add("@TY", SqlDbType.VarChar, 50).Value = RFID_y;
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

        /// <summary>
        /// 删除CallBox Map
        /// </summary>
        /// <param name="CallBox_Tag"></param>
        /// <param name="RFID_x"></param>
        /// <param name="RFID_y"></param>
        /// <returns></returns>
        public static int DELETE_CallBox_Map(string CallBox_Tag)
        {
            SqlCommand comm = new SqlCommand("PR_DELETE_CallBox_Map");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@SCallBoxNo", SqlDbType.VarChar, 50).Value = CallBox_Tag;
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
        public static int UPDATE_ELC_AGVNO(string ELE_IP, int AGV_No)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_ELC_AGVNO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@SIP", SqlDbType.VarChar, 50).Value = ELE_IP;
            comm.Parameters.Add("@SAGVNO", SqlDbType.Int, 10).Value = AGV_No;
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

        /// <summary>
        /// 更新AGV当前交互设备任务号
        /// </summary>
        /// <param name="AGV_IP"></param>
        /// <param name="AGV_LineNo"></param>
        /// <returns></returns>
        public static int UPDATE_AGV_INFO_FROM(string AGV_IP, string AGV_LineNo)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_INFO_FROM");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGVIP", SqlDbType.VarChar, 50).Value = AGV_IP;
            comm.Parameters.Add("@AGVFROM", SqlDbType.VarChar, 50).Value = AGV_LineNo;
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

        /// <summary>
        /// 更新当前路径任务号
        /// </summary>
        /// <param name="AGV_IP"></param>
        /// <param name="AGV_LineNo"></param>
        /// <returns></returns>
        public static int UPDATE_AGV_INFO_TO(string AGV_IP, string AGV_LineNo)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_INFO_TO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@AGVIP", SqlDbType.VarChar, 50).Value = AGV_IP;
            comm.Parameters.Add("@AGVTO", SqlDbType.VarChar, 50).Value = AGV_LineNo;
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
        /// <summary>
        /// 增加充电点
        /// </summary>
        /// <param name="Power_No">充电站编号</param>
        /// <param name="Power_IP">充电站IP</param>
        /// <param name="Power_LineNo">充电站路线编号</param>
        /// <param name="Power_St_RFID">充电站在席RFID</param>
        /// <param name="Power_X">充电站地图显示坐标X</param>
        /// <param name="Power_Y">充电站地图显示坐标Y</param>
        /// <param name="Power_InRFID">充电站入口RFID</param>
        /// <param name="Power_InFW">充电站进入方向</param>
        /// <param name="Power_In_LineNo">充电站进入路线号</param>
        /// <param name="Power_H">充电电量高阈值</param>
        /// <param name="Power_L">充电电量低阀值</param>
        /// <param name="Power_Out_LineNo">充电站出路线号</param>
        /// /// <param name="Power_Out_RFID">充电站</param>
        /// /// <param name="Power_Out_FW">充电站</param>
        /// <returns></returns>
        public static int INSERT_Power_INFO(string Power_No, string Power_IP, string Power_LineNo, string Power_St_RFID,
                                            int Power_X, int Power_Y, string Power_InRFID, int Power_InFW, string Power_In_LineNo,
                                            int Power_H, int Power_L, string Power_Out_LineNo, string Power_Out_RFID, int Power_Out_FW)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_Power_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@P_NO", SqlDbType.VarChar, 50).Value = Power_No;
            comm.Parameters.Add("@P_IP", SqlDbType.VarChar, 50).Value = Power_IP;
            comm.Parameters.Add("@P_LINENO", SqlDbType.VarChar, 50).Value = Power_LineNo;
            comm.Parameters.Add("@P_ST_RFID", SqlDbType.VarChar, 50).Value = Power_St_RFID;
            comm.Parameters.Add("@P_X", SqlDbType.Int).Value = Power_X;
            comm.Parameters.Add("@P_Y", SqlDbType.Int).Value = Power_Y;
            comm.Parameters.Add("@P_IN_RFID", SqlDbType.VarChar, 50).Value = Power_InRFID;
            comm.Parameters.Add("@P_IN_FW", SqlDbType.Int).Value = Power_InFW;
            comm.Parameters.Add("@P_IN_LINENO", SqlDbType.VarChar, 50).Value = Power_In_LineNo;
            comm.Parameters.Add("@P_POWER_H", SqlDbType.Int).Value = Power_H;
            comm.Parameters.Add("@P_POWER_L", SqlDbType.Int).Value = Power_L;
            comm.Parameters.Add("@P_OUT_lINENO", SqlDbType.VarChar, 50).Value = Power_Out_LineNo;//
            comm.Parameters.Add("@P_OUT_RFID", SqlDbType.VarChar, 50).Value = Power_Out_RFID;
            comm.Parameters.Add("@P_OUT_FW", SqlDbType.Int).Value = Power_Out_FW;
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
        /// <summary>
        /// 增加自动门
        /// </summary>
        /// <param name="AutoDoor_No">自动门编号</param>
        /// <param name="AutoDoor_IP">自动门IP</param>
        /// <param name="AutoDoor_LineNo">自动门路径编号</param>
        /// <param name="AutoDoor_X">自动门坐标X</param>
        /// <param name="AutoDoor_Y">自动门坐标Y</param>
        /// <param name="AutoDoor_InRFID">进入RFID</param>
        /// <param name="AutoDoor_FW">进入方向</param>
        /// <param name="AutoDoor_OutRFID">离开RFID</param>
        /// <returns></returns>
        public static int INSERT_AutoDoor_INFO(string AutoDoor_No,
                                               string AutoDoor_IP,
                                               string AutoDoor_LineNo,
                                               int AutoDoor_X,
                                               int AutoDoor_Y,
                                               string AutoDoor_InRFID,
                                               int AutoDoor_FW,
                                               string AutoDoor_OutRFID)
        {
            SqlCommand comm = new SqlCommand("PR_INSERT_AutoDoor_INFO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@D_NO", SqlDbType.VarChar, 50).Value = AutoDoor_No;
            comm.Parameters.Add("@D_IP", SqlDbType.VarChar, 50).Value = AutoDoor_IP;
            comm.Parameters.Add("@D_LINENO", SqlDbType.VarChar, 50).Value = AutoDoor_LineNo;
            comm.Parameters.Add("@D_IN_RFID", SqlDbType.VarChar, 50).Value = AutoDoor_InRFID;
            comm.Parameters.Add("@D_OUT_RFID", SqlDbType.VarChar, 50).Value = AutoDoor_OutRFID;
            comm.Parameters.Add("@D_FW", SqlDbType.Int).Value = AutoDoor_FW;
            comm.Parameters.Add("@D_X", SqlDbType.Int).Value = AutoDoor_X;
            comm.Parameters.Add("@D_Y", SqlDbType.Int).Value = AutoDoor_Y;

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
        /// <summary>
        /// 查找呼车任务
        /// </summary>
        /// <returns></returns>
        public static DataTable SELECT_CALL_MESSAGE()
        {
            SqlCommand comm = new SqlCommand("SELECT_CALL_ORDMESSAGE");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }
        /// <summary>
        /// 查找发车任务
        /// </summary>
        /// <returns></returns>
        public static DataTable SELECT_SEND_MESSAGE()
        {
            SqlCommand comm = new SqlCommand("SELECT_SEND_ORDMESSAGE");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable SEND_MESSAGE = null;
            try
            {
                SEND_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return SEND_MESSAGE;
        }
        /// <summary>
        /// 查找已完成任务
        /// </summary>
        /// <returns></returns>
        public static DataTable SELECT_USED_MESSAGE()
        {
            SqlCommand comm = new SqlCommand("SELECT_ORD_USED_MESSAGE");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable USED_MESSAGE = null;
            try
            {
                USED_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return USED_MESSAGE;
        }
        /// <summary>
        /// 查找执行中任务
        /// </summary>
        /// <returns></returns>
        public static DataTable SELECT_USING_MESSAGE()
        {
            SqlCommand comm = new SqlCommand("SELECT_ORD_USGING_MESSAGE");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable USING_MESSAGE = null;
            try
            {
                USING_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return USING_MESSAGE;
        }
        /// <summary>
        /// 查找所有任务
        /// </summary>
        /// <returns></returns>
        public static DataTable SELETCT_ALL_MESSAGE()
        {
            SqlCommand comm = new SqlCommand("SELECT_ALLORD_MESSAGE");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable USING_MESSAGE = null;
            try
            {
                USING_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return USING_MESSAGE;
        }

        public static DataTable SELECT_CARNO_MESSAGE(string RGB_CAR_NUMBER)
        {
            SqlCommand comm = new SqlCommand("SELECT_CARNO_MESSAGE");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RGB_CAR_NUMBER", SqlDbType.VarChar, 10).Value = RGB_CAR_NUMBER;
            DataTable USING_MESSAGE = null;
            try
            {
                USING_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return USING_MESSAGE;
        }

        /// <summary>
        /// 增加RFID位置
        /// </summary>
        /// <param name="RFID_No"></param>
        /// <param name="RFID_Location_X"></param>
        /// <param name="RFID_Location_Y"></param>
        /// <returns></returns>
        public static int Add_RFID(string RFID_No, int RFID_Location_X, int RFID_Location_Y)
        {
            SqlCommand comm = new SqlCommand("ADD_RGV_RFID");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@RFID_No", SqlDbType.NChar, 10).Value = RFID_No;
            comm.Parameters.Add("@RFID_Location_X", SqlDbType.Int, 2).Value = RFID_Location_X;
            comm.Parameters.Add("@RFID_Location_Y", SqlDbType.Int, 2).Value = RFID_Location_Y;
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


        //-------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPs">用户密码</param>
        /// <returns>i：-1连接错误；0用户名不对；1找到用户名</returns>
        public static int Usercheck(string userName, string userPs)
        {
            SqlCommand comm = new SqlCommand("lb_Usercheck");
            comm.CommandType = CommandType.StoredProcedure;
            int i = 0;
            comm.Parameters.Add("@userName", SqlDbType.Char, 20).Value = userName;
            comm.Parameters.Add("@userPS", SqlDbType.Char, 40).Value = userPs;
            try
            {
                i = SqlDBControl.ExecuteQuery(comm).Tables[0].Rows.Count;
            }
            catch (Exception)
            {
                i = -1;
            }
            return i;
        }
        /// <summary>
        /// 验证用户是否已存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>i：-1连接错误；0无此用户；1有此用户</returns>
        public static int Usercheck(string userName)
        {
            SqlCommand comm = new SqlCommand("lb_CheckUserHas");
            comm.CommandType = CommandType.StoredProcedure;
            int i = 0;
            comm.Parameters.Add("@userName", SqlDbType.Char, 20).Value = userName;
            try
            {
                i = SqlDBControl.ExecuteQuery(comm).Tables[0].Rows.Count;
            }
            catch (Exception)
            {
                i = -1;
            }
            return i;
        }

        /// <summary>
        /// 验证用户是否已存在
        /// </summary>
        /// <param name="userName">用户ID</param>
        /// <returns>i：-1连接错误；0无此用户；1有此用户</returns>
        public static int Usercheck(int userID, string userName)
        {
            SqlCommand comm = new SqlCommand("lb_CheckUserID");
            comm.CommandType = CommandType.StoredProcedure;
            int i = 0;
            comm.Parameters.Add("@userID", SqlDbType.Int, 20).Value = userID;
            comm.Parameters.Add("@userName", SqlDbType.Char, 20).Value = userName;
            try
            {
                int m = SqlDBControl.ExecuteQuery(comm).Tables.Count;
                if (m > 0)
                {
                    i = SqlDBControl.ExecuteQuery(comm).Tables[0].Rows.Count;
                }
                else
                {
                    i = m;
                }
            }
            catch (Exception)
            {
                i = -1;
            }
            return i;
        }

        /// <summary>
        /// 获取用户信息表
        /// </summary>
        /// <returns></returns>
        public static DataTable UserMessage()
        {
            SqlCommand comm = new SqlCommand("lb_SelectUser");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable userMess = null;
            try
            {
                userMess = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return userMess;
        }

        /// <summary>
        /// 增加员工信息
        /// </summary>
        /// <param name="UserName">员工编号</param>
        /// <param name="Userpass">密码</param>
        /// <param name="Userpower">权限</param>
        /// <param name="Userremark">备注</param>
        /// <returns></returns>
        public static int AddUserIf(string loginname, string UserName, string Userpass, int Userpower, string Userremark)
        {
            SqlCommand comm = new SqlCommand("lb_InsertUser");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@loginName", SqlDbType.Char, 20).Value = loginname;
            comm.Parameters.Add("@userName", SqlDbType.Char, 20).Value = UserName;
            comm.Parameters.Add("@userPass", SqlDbType.Char, 20).Value = Userpass;
            comm.Parameters.Add("@userpower", SqlDbType.Int, 10).Value = Userpower;
            comm.Parameters.Add("@userremark", SqlDbType.NVarChar, 50).Value = Userremark;
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

        /// <summary>
        /// 更改员工信息
        /// </summary>
        /// <param name="UserID">员工ID</param>
        /// <param name="UserName">员工编号</param>
        /// <param name="Userpass">密码</param>
        /// <param name="Userpower">权限</param>
        /// <param name="Userremark">备注</param>
        /// <returns></returns>
        public static int ChangeUserIf(string loginname, int UserID, string UserName, string Userpass, int Userpower, string Userremark)
        {
            SqlCommand comm = new SqlCommand("lb_ChangeUser");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@usernowname", SqlDbType.Char, 20).Value = loginname;
            comm.Parameters.Add("@userID", SqlDbType.Int, 10).Value = UserID;
            comm.Parameters.Add("@userName", SqlDbType.Char, 20).Value = UserName;
            comm.Parameters.Add("@userPass", SqlDbType.Char, 20).Value = Userpass;
            comm.Parameters.Add("@userpower", SqlDbType.Int, 10).Value = Userpower;
            comm.Parameters.Add("@userremark", SqlDbType.NVarChar, 50).Value = Userremark;
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

        /// <summary>
        /// 更改登录员工信息
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="Userpass"></param>
        /// <returns></returns>
        public static int ChangeLoginUserIf(string loginname, string Userpass)
        {
            SqlCommand comm = new SqlCommand("lb_ChangeLoginuserIF");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@usernowname", SqlDbType.Char, 20).Value = loginname;
            comm.Parameters.Add("@userPass", SqlDbType.Char, 20).Value = Userpass;
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

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="UserName">员工号</param>
        /// <returns></returns>
        public static int DeleteUser(string loginname, string UserName)
        {
            SqlCommand comm = new SqlCommand("lb_Deleteuser");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@loginName", SqlDbType.Char, 20).Value = loginname;
            comm.Parameters.Add("@UserName", SqlDbType.Char, 20).Value = UserName;
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

        /// <summary>
        /// 查找用户的权限
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>i：-1连接错误；0无此用户；1有此用户</returns>
        public static int SelectuserPower(string userName)
        {
            SqlCommand comm = new SqlCommand("lb_SelectuserPower");
            comm.CommandType = CommandType.StoredProcedure;
            int i = 0;
            comm.Parameters.Add("@userName", SqlDbType.Char, 20).Value = userName;
            try
            {
                int m = SqlDBControl.ExecuteQuery(comm).Tables.Count;
                if (m > 0)
                {
                    i = int.Parse(SqlDBControl.ExecuteQuery(comm).Tables[0].Rows[0][0].ToString().Trim());
                }
                else
                {
                    i = m;
                }
            }
            catch (Exception)
            {
                i = -1;
            }
            return i;
        }

        /// <summary>
        /// 查询对应条码明细
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns></returns>
        public static DataTable SelectBarcode(string barcode)
        {
            SqlCommand comm = new SqlCommand("lb_SelectBarcode");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable barcodetable = null;
            comm.Parameters.Add("@Barcode", SqlDbType.Char, 30).Value = barcode;
            try
            {

                barcodetable = SqlDBControl.ExecuteQuery(comm).Tables[0];

            }
            catch (Exception)
            {
                barcodetable = null;
            }
            return barcodetable;
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectDiray()
        {
            SqlCommand comm = new SqlCommand("lb_Selectdriay");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable barcodetable = null;
            try
            {
                barcodetable = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
                barcodetable = null;
            }
            return barcodetable;
        }

        /// <summary>
        /// 查询对应条码明细
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns></returns>
        public static DataTable Selectstorage()
        {
            SqlCommand comm = new SqlCommand("lb_SelectStorage");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable barcodetable = null;
            try
            {
                barcodetable = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
                barcodetable = null;
            }
            return barcodetable;
        }

        /// <summary>
        /// 增加改变日期的日志
        /// </summary>
        /// <param name="Loginname"></param>
        /// <param name="OldDate"></param>
        /// <param name="NewDate"></param>
        /// <returns></returns>
        public static int InsertChangeDriay(string Loginname, string OldDate, string NewDate)
        {
            SqlCommand comm = new SqlCommand("lb_InsertChangeDriay");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@loginname", SqlDbType.Char, 20).Value = Loginname;
            comm.Parameters.Add("@oldday", SqlDbType.Char, 5).Value = OldDate;
            comm.Parameters.Add("@newday", SqlDbType.Char, 5).Value = NewDate;
            int outmess = 0;
            try
            {
                outmess = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                outmess = -1;
            }
            return outmess;
        }


        /// <summary>
        /// 增加日记
        /// </summary>
        /// <param name="Loginname"></param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public static int InsertChangeDriay(string Loginname, string mess)
        {
            SqlCommand comm = new SqlCommand("lb_InsertDriay");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@loginname", SqlDbType.Char, 20).Value = Loginname;
            comm.Parameters.Add("@mess", SqlDbType.NVarChar, 30).Value = mess;
            int outmess = 0;
            try
            {
                outmess = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                outmess = -1;
            }
            return outmess;
        }

        /// <summary>
        /// 返回查询到的空库位的表
        /// </summary>
        /// <returns></returns>
        public static DataTable OutStorageNO()
        {
            SqlCommand comm = new SqlCommand("lb_SelectStorageTable");
            comm.CommandType = CommandType.StoredProcedure;

            DataTable retOutNO = null;

            try
            {
                retOutNO = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return retOutNO;
        }


        /// <summary>
        /// 返回查询到的库位出库的时间
        /// </summary>
        /// <returns></returns>
        public static string OutStorageTime(int Sid)
        {
            SqlCommand comm = new SqlCommand("lb_CheckThestorage");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Sid", SqlDbType.Int).Value = Sid;
            string retOutNO = null;
            try
            {
                if (SqlDBControl.ExecuteQuery(comm).Tables[0] != null)
                {
                    retOutNO = SqlDBControl.ExecuteQuery(comm).Tables[0].Rows[0][0].ToString().Trim();
                }
            }
            catch (Exception)
            {
                retOutNO = "ERR";
            }
            return retOutNO;
        }



        /// <summary>
        /// 返回查询库位成功
        /// </summary>
        /// <returns></returns>
        public static int FindStorageOK(int Sid, string Userno, DateTime Intime)
        {
            SqlCommand comm = new SqlCommand("lb_SelectStorageOK");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Sid", SqlDbType.Int).Value = Sid;
            comm.Parameters.Add("@userno", SqlDbType.Char, 20).Value = Userno;
            comm.Parameters.Add("@Intime", SqlDbType.DateTime).Value = Intime;
            int retOut = 0;
            try
            {
                retOut = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                retOut = -1;
            }
            return retOut;
        }


        /// <summary>
        /// 更新正在进行中的数据库
        /// </summary>
        /// <returns></returns>
        public static int UpdateInDoing(int Sid, string userno, string kajianumber, DateTime Intime, int Booktime, DateTime Outtime)
        {
            SqlCommand comm = new SqlCommand("lb_UpdateStorageRack");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Sid", SqlDbType.Int).Value = Sid;
            comm.Parameters.Add("@userno", SqlDbType.Char, 20).Value = userno;
            comm.Parameters.Add("@Skajianumber", SqlDbType.Char, 20).Value = kajianumber;
            comm.Parameters.Add("@Intime", SqlDbType.DateTime).Value = Intime;
            comm.Parameters.Add("@Booktime", SqlDbType.Int).Value = Booktime;
            comm.Parameters.Add("@OUTTIME", SqlDbType.DateTime).Value = Outtime;
            int retOutNO = 0;
            try
            {
                retOutNO = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                retOutNO = -1;
            }
            return retOutNO;
        }


        /// <summary>
        /// 更新需要出库的库位状态
        /// </summary>
        /// <returns></returns>
        public static int UpdateOutStorageAC()
        {
            SqlCommand comm = new SqlCommand("lb_OutStorageRack");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Nowday", SqlDbType.DateTime).Value = DateTime.Now.Date;
            int retOut = 0;
            try
            {
                retOut = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                retOut = -1;
            }
            return retOut;
        }


        /// <summary>
        /// 查找库位信息
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectStorageTable()
        {
            SqlCommand comm = new SqlCommand("lb_SelectStor");
            comm.CommandType = CommandType.StoredProcedure;

            DataTable retOut;
            try
            {
                retOut = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
                retOut = null;
            }
            return retOut;
        }

        /// <summary>
        /// 返回需要出库的表
        /// </summary>
        /// <returns></returns>
        public static DataTable OutStorageTable()
        {
            SqlCommand comm = new SqlCommand("lb_OutStorageRackTable");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable retable = null;
            try
            {
                retable = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {

            }
            return retable;
        }


        /// <summary>
        /// 返回正在运行的任务
        /// </summary>
        /// <returns></returns>
        public static DataTable NowStorageTable()
        {
            SqlCommand comm = new SqlCommand("lb_SelectNowDoing");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable table = null;

            try
            {
                table = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {

            }
            return table;
        }

        /// <summary>
        /// 更新强制完成或取消的任务
        /// </summary>
        /// <param name="Ccase"></param>
        /// <param name="Storage"></param>
        /// <returns></returns>
        public static int UpdateDo(int Ccase, string Storage)
        {
            SqlCommand comm = new SqlCommand("lb_UpdateStoeage");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@case", SqlDbType.Int).Value = Ccase;
            comm.Parameters.Add("@Sstorage", SqlDbType.NChar, 20).Value = Storage;
            int rerust = 0;

            try
            {
                rerust = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                rerust = -1;
            }

            return rerust;
        }


        /// <summary>
        /// 返回所查库位的状态
        /// </summary>
        /// <param name="SelectStoredNO"></param>
        /// <returns></returns>
        public static int OutStorageAC(int SelectStoredNO)
        {
            SqlCommand comm = new SqlCommand("lb_StorageAC");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Sid", SqlDbType.Int).Value = SelectStoredNO;
            int retAC = 0;

            try
            {
                DataTable rttable = SqlDBControl.ExecuteQuery(comm).Tables[0];
                if (rttable != null)
                {
                    retAC = int.Parse(rttable.Rows[0][0].ToString().Trim());
                }
            }
            catch (Exception)
            {

                retAC = -1;
            }
            return retAC;
        }


        /// <summary>
        /// 更新出库信息
        /// </summary>
        /// <param name="Sid"></param>
        /// <param name="userno"></param>
        /// <param name="kajianumber"></param>
        /// <param name="Outtime"></param>
        /// <param name="inplace">卡夹出的区域</param>
        /// <returns></returns>
        public static int UpdateOutDoing(int Sid, string userno, string kajianumber, DateTime Outtime, string inplace)
        {
            SqlCommand comm = new SqlCommand("lb_UpdateStorageRack");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Sid", SqlDbType.Int).Value = Sid;
            comm.Parameters.Add("@userno", SqlDbType.Char, 20).Value = userno;
            comm.Parameters.Add("@Skajianumber", SqlDbType.Char, 20).Value = kajianumber;
            comm.Parameters.Add("@outtime", SqlDbType.DateTime).Value = Outtime;
            comm.Parameters.Add("@InDaPeng", SqlDbType.Char, 10).Value = inplace;
            int retOutNO = 0;
            try
            {
                retOutNO = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                retOutNO = -1;
            }
            return retOutNO;
        }


        /// <summary>
        /// 删除库位数据
        /// </summary>
        /// <param name="StorageNo">库格号</param>
        /// <returns></returns>
        public static int DelectStorageData(string StorageNo, string Username)
        {
            SqlCommand comm = new SqlCommand("lb_DeleteStorageData");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Sstorage", SqlDbType.NChar, 20).Value = StorageNo;
            comm.Parameters.Add("@Username", SqlDbType.Char, 20).Value = Username;
            int OutRutrun = 0;

            try
            {
                OutRutrun = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                OutRutrun = -1;
            }

            return OutRutrun;
        }

        /// <summary>
        /// 更新库位的数据
        /// </summary>
        /// <param name="Storage">库位</param>
        /// <param name="KajiaNo">卡夹编号</param>
        /// <param name="Intime">入库时间</param>
        /// <param name="Booktime">设定发芽时间</param>
        /// <param name="Outtime">出库时间</param>
        /// <param name="State">状态</param>
        /// <returns>0：no；1：ok；-1：NG</returns>
        public static int 
            ChangeStorageData(string Storage, string KajiaNo, DateTime Intime, int Booktime, DateTime Outtime, int State)
        {
            SqlCommand comm = new SqlCommand("lb_ChangeStorageData");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Sstorage", SqlDbType.NChar, 20).Value = Storage;
            comm.Parameters.Add("@SkajiaNO", SqlDbType.Char, 20).Value = KajiaNo;
            comm.Parameters.Add("@Sintime", SqlDbType.DateTime).Value = Intime;
            comm.Parameters.Add("@Sboottime", SqlDbType.Int).Value = Booktime;
            comm.Parameters.Add("@Souttime", SqlDbType.DateTime).Value = Outtime;
            comm.Parameters.Add("@Sstate", SqlDbType.Int).Value = State;
            int OutRutrun = 0;

            try
            {
                OutRutrun = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception)
            {
                OutRutrun = -1;
            }

            return OutRutrun;

        }

        public static string retErr(string code)
        {
            string err = "未知故障";
            if (code == "0")
                err = "正常";
            if (code == "1")
                err = "坐标信息丢失";
            if (code == "2")
                err = "当前电压过低";
            if (code == "3")
                err = "左马达故障报警";
            if (code == "4")
                err = "左马达速度反馈检测故障";
            if (code == "5")
                err = "右马达故障报警";
            if (code == "6")
                err = "右马达速度反馈检测故障";
            if (code == "7")
                err = "旋转马达故障报警";
            if (code == "8")
                err = "旋转达速度反馈检测故障";
            if (code == "9")
                err = "升降马达故障报警";
            if (code == "10")
                err = "升降马达速度反馈检测故障";
            if (code == "11")
                err = "移动任务货架角度不是 0°，90°，180°，270°四个中的其中一个";
            if (code == "12")
                err = "偏移路线过大报警";
            if (code == "13")
                err = "陀螺仪报警故障";
            if (code == "14")
                err = "陀螺仪测定零点故障，是否开机的时候机器在抖动";
            if (code == "15")
                err = "陀螺仪通信超时故障";
            if (code == "16")
                err = "速度电压控制芯片通信超时";
            if (code == "17")
                err = "处理动作序列指令出错";
            if (code == "18")
                err = "小车打滑故障";
            if (code == "19")
                err = "丢失二维码故障";
            if (code == "20")
                err = "紧急停车故障";
            if (code == "21")
                err = "任务发布错误";
            if (code == "22")
                err = "左马达通讯超时";
            if (code == "23")
                err = "右马达通讯超时";
            if (code == "24")
                err = "旋转马达通讯超时";
            if (code == "25")
                err = "举升马达通讯超时";
            if (code == "26")
                err = "运行过程中角度偏差过大";
            if (code == "27")
                err = "起步时二维码偏移过大";
            if (code == "28")
                err = "目标方向达不到目标地点";
            if (code == "29")
                err = "开始执行任务时当前未知不在二维码上";
            return err;
        }
        public static string reArea(string code)
        {
            string area = "";
            DataTable res=new DataTable();

            res = SELECT_AREA_CODE(code);
            if (res != null && res.Rows.Count > 0)
            {
                area = res.Rows[0][0].ToString().Trim();
            }
            else
                area = "X：未定义区域";
            return area;
            //if (code == "1")
            //    return area = "1：1楼NC区域";
            //if (code == "2")
            //    area = "2：3楼CU区域";
            //if (code == "3")
            //    return area = "3:9#电梯区域";
            //if (code == "4")
            //    return area = "4:1楼NC区域";
            //if (code == "5")
            //    return area = "5:3楼7#电梯区域";
            //if (code == "6")
            //    return area = "6:3连廊南侧区域";
            //if (code == "7")
            //    return area = "7:3楼至2楼7#电梯区域";
            //if (code == "8")
            //    return area = "8:未定义区域";
            //if (code == "9")
            //    return area = "9:2楼2#电梯区域";
            //if (code == "10")
            //    return area = "10:2楼RT门前区域";
            //if (code == "11")
            //    return area = "11:3楼KS区域";
            //if (code == "12")
            //    return area = "12:KS与PD交汇区域";
            //if (code == "13")
            //    return area = "13:2楼1#电梯区域";
            //return code + "：未定义区域";
        }
        public static DataTable SELECT_AREA_CODE(string areaNo)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AREA_CODE");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@areaNo", SqlDbType.VarChar, 50).Value = areaNo;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        /// <summary>
        /// 全路径
        /// </summary>
        /// <returns></returns>
        public static DataTable SELECT_AGV_INFO_FORCross()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_AGV_INFO_FORCross");
            comm.CommandType = CommandType.StoredProcedure;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception)
            {
            }
            return CALL_MESSAGE;
        }

        public static DataTable PR_SELECT_AGV_ST(string ip)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_STATION");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@agvip", SqlDbType.VarChar, 50).Value = ip;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CALL_MESSAGE;
        }

        public static int KIVA_UPDATE_Station(string station,string agvIp)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_TBSTATION");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@sip", SqlDbType.VarChar, 50).Value = agvIp;
            comm.Parameters.Add("@stion", SqlDbType.Int).Value = station;

            int i = -1;
            try
            {
                i = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception ex)
            {
                throw ex;
                i = -1;
            }
            return i;
        }

        public static int KIVA_UPDATE_ChargeInfo(int len, string agvIp)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_CHARGEINFO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@LEN", SqlDbType.Int).Value = len;
            comm.Parameters.Add("@AGV_IP", SqlDbType.VarChar, 50).Value = agvIp;

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

        public static DataTable KIVA_SELECT_Charge_end2(int Lent)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_Charge_end2");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@Len", SqlDbType.Int).Value = Lent;
            DataTable CALL_MESSAGE = null;
            try
            {
                CALL_MESSAGE = SqlDBControl.ExecuteQuery(comm).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CALL_MESSAGE;
        }

        public static int KIVA_UPDATE_WCS_WSTRING_WAC(string wfloatno, string wstring, int wac, string NowRFID, string Charge_IP, string AGV_From, int Charge_state, string AGV_Ip)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_WCS_WSTRING_WAC_CHARGE");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@wfloatno", SqlDbType.VarChar, 50).Value = wfloatno;
            comm.Parameters.Add("@wstring", SqlDbType.VarChar, 1000).Value = wstring;
            comm.Parameters.Add("@wac", SqlDbType.Int).Value = wac;
            comm.Parameters.Add("@wNowRFID", SqlDbType.VarChar, 50).Value = NowRFID;
            comm.Parameters.Add("@Charge_IP", SqlDbType.VarChar, 50).Value = Charge_IP;
            comm.Parameters.Add("@AGV_From", SqlDbType.VarChar, 50).Value = AGV_From;
            comm.Parameters.Add("@Charge_state", SqlDbType.Int, 10).Value = Charge_state;
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            int i = -1;
            try
            {
                i = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception ex)
            {
                throw ex;
                i = -1;
            }
            return i;
        }

        public static int KIVA_UPDATE_WCS_WSTRING_WAC(string wfloatno, string wstring, int wac, string NowRFID, string station, string agvIp, string Charge_IP,string agv_From,string agv_To)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_WCS_WSTRING_WAC_STATION");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@wfloatno", SqlDbType.VarChar, 50).Value = wfloatno;
            comm.Parameters.Add("@wstring", SqlDbType.VarChar, 1000).Value = wstring;
            comm.Parameters.Add("@wac", SqlDbType.Int).Value = wac;
            comm.Parameters.Add("@wNowRFID", SqlDbType.VarChar, 50).Value = NowRFID;
            comm.Parameters.Add("@sip", SqlDbType.VarChar, 50).Value = agvIp;
            comm.Parameters.Add("@stion", SqlDbType.Int).Value = station;
            comm.Parameters.Add("@Charge_IP", SqlDbType.VarChar, 50).Value = Charge_IP;
            comm.Parameters.Add("@agv_from", SqlDbType.VarChar, 50).Value = agv_From;
            comm.Parameters.Add("@agv_to", SqlDbType.VarChar, 50).Value = agv_To;
            int i = -1;
           
            try
            {
                i = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception ex)
            {
                throw ex;
                i = -1;
            }
            return i;
        }

        /// <summary>
        /// 解析，更新PR_UPDATE_AGV_INFO_AND_STATE_Map_Info_State
        /// </summary>
        /// <param name="AGV_Ip"></param>
        /// <param name="AGV_Ac"></param>
        /// <param name="AGV_Now_X"></param>
        /// <param name="AGV_Now_Y"></param>
        /// <param name="AGV_Skip_No"></param>
        /// <param name="AGV_Voltage"></param>
        /// <param name="AGV_Electricity"></param>
        /// <param name="AGV_L_Speed"></param>
        /// <param name="AGV_R_Speed"></param>
        /// <param name="AGV_ErrorCord"></param>
        /// <param name="AGV_WarningCord"></param>
        /// <param name="AGV_Now_Ord"></param>
        /// <param name="AGV_Now_Ord_Count"></param>
        /// <param name="AGV_Remaining_Trip"></param>
        /// <param name="AGV_Angle"></param>
        /// <param name="AGV_Skip_Angle"></param>
        /// <param name="AGV_Lifting_Speed"></param>
        /// <param name="AGV_Rotating_Speed"></param>
        /// <param name="AGV_OrderNo"></param>
        /// <param name="AGV_AtErWeiMa"></param>
        /// <param name="NowStats"></param>
        /// <param name="AtStas"></param>
        /// <param name="TuoPan_Low"></param>
        /// <param name="TuoPan_High"></param>
        /// <param name="ZhangAi_Stop"></param>
        /// <param name="ZhangAi_Slow"></param>
        /// <param name="ZhangAi_SmallSlow"></param>
        /// <param name="TuoPan_Zero"></param>
        /// <param name="SkipSacn_Beffor"></param>
        /// <param name="HasSkip"></param>
        /// <param name="Map_X"></param>
        /// <param name="Map_Y"></param>
        /// <param name="Map_State"></param>
        /// <returns></returns>
        public static int KIVA_UPDATE_AGV_INFO_AND_STATE_Map_Info_State2(string AGV_Ip, int AGV_Ac, string AGV_Now_X, string AGV_Now_Y, string AGV_Skip_No,
            string AGV_Voltage, string AGV_Electricity, string AGV_L_Speed, string AGV_R_Speed, string AGV_ErrorCord, string AGV_WarningCord,
             string AGV_Now_Ord, int AGV_Now_Ord_Count, string AGV_Remaining_Trip, string AGV_Angle, string AGV_Skip_Angle, string AGV_Lifting_Speed,
            string AGV_Rotating_Speed, string AGV_OrderNo, int AGV_AtErWeiMa, string NowStats, int AtStas, string TuoPan_Low, string TuoPan_High, string ZhangAi_Stop, string ZhangAi_Slow, string ZhangAi_SmallSlow, string TuoPan_Zero, string SkipSacn_Beffor, string HasSkip
            , string Map_X, string Map_Y, int Map_State, int max_x, string wfloatno)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_AGV_INFO_AND_STATE_Map_Info_State2");
            comm.CommandType = CommandType.StoredProcedure;
            //1
            comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            //comm.Parameters.Add("@AGV_No", SqlDbType.VarChar, 50).Value = AGV_No;
            comm.Parameters.Add("@AGV_Ac", SqlDbType.Int, 10).Value = AGV_Ac;//1在线，0离线
            comm.Parameters.Add("@AGV_Now_X", SqlDbType.VarChar, 50).Value = AGV_Now_X;
            comm.Parameters.Add("@AGV_Now_Y", SqlDbType.VarChar, 50).Value = AGV_Now_Y;
            comm.Parameters.Add("@AGV_Skip_No", SqlDbType.VarChar, 50).Value = AGV_Skip_No;
            //comm.Parameters.Add("@AGV_From", SqlDbType.VarChar, 50).Value = AGV_From;//0
            //comm.Parameters.Add("@AGV_To", SqlDbType.VarChar, 50).Value = AGV_To;//0
            comm.Parameters.Add("@AGV_Voltage", SqlDbType.VarChar, 50).Value = AGV_Voltage;
            comm.Parameters.Add("@AGV_Electricity", SqlDbType.VarChar, 50).Value = AGV_Electricity;
            comm.Parameters.Add("@AGV_L_Speed", SqlDbType.VarChar, 50).Value = AGV_L_Speed;
            comm.Parameters.Add("@AGV_R_Speed", SqlDbType.VarChar, 50).Value = AGV_R_Speed;
            //comm.Parameters.Add("@AGV_LineNo", SqlDbType.VarChar, 50).Value = AGV_LineNo;//0
            //comm.Parameters.Add("@AGV_LineString", SqlDbType.VarChar, 50).Value = AGV_LineString;//0
            comm.Parameters.Add("@AGV_ErrorCord", SqlDbType.VarChar, 50).Value = AGV_ErrorCord;
            comm.Parameters.Add("@AGV_WarningCord", SqlDbType.VarChar, 50).Value = AGV_WarningCord;
            comm.Parameters.Add("@AGV_Now_Ord", SqlDbType.VarChar, 50).Value = AGV_Now_Ord;
            comm.Parameters.Add("@AGV_Now_Ord_Count", SqlDbType.Int, 10).Value = AGV_Now_Ord_Count;
            comm.Parameters.Add("@AGV_Remaining_Trip", SqlDbType.VarChar, 50).Value = AGV_Remaining_Trip;
            comm.Parameters.Add("@AGV_Angle", SqlDbType.VarChar, 50).Value = AGV_Angle;
            comm.Parameters.Add("@AGV_Skip_Angle", SqlDbType.VarChar, 50).Value = AGV_Skip_Angle;
            comm.Parameters.Add("@AGV_Lifting_Speed", SqlDbType.VarChar, 50).Value = AGV_Lifting_Speed;
            comm.Parameters.Add("@AGV_Rotating_Speed", SqlDbType.VarChar, 50).Value = AGV_Rotating_Speed;
            comm.Parameters.Add("@AGV_Order_No", SqlDbType.VarChar, 50).Value = AGV_OrderNo;
            comm.Parameters.Add("@AGV_AtErWeiMa", SqlDbType.Int, 10).Value = AGV_AtErWeiMa;//@
            comm.Parameters.Add("@AGV_Stats", SqlDbType.VarChar, 50).Value = NowStats;//AtStas
            comm.Parameters.Add("@AGV_AtStats", SqlDbType.Int).Value = AtStas;
            //2
            //comm.Parameters.Add("@AGV_Ip", SqlDbType.VarChar, 50).Value = AGV_Ip;
            comm.Parameters.Add("@TuoPan_Low", SqlDbType.VarChar, 50).Value = TuoPan_Low;
            comm.Parameters.Add("@TuoPan_High", SqlDbType.VarChar, 50).Value = TuoPan_High;
            comm.Parameters.Add("@ZhangAi_Stop", SqlDbType.VarChar, 50).Value = ZhangAi_Stop;
            comm.Parameters.Add("@ZhangAi_Slow", SqlDbType.VarChar, 50).Value = ZhangAi_Slow;
            comm.Parameters.Add("@ZhangAi_SmallSlow", SqlDbType.VarChar, 50).Value = ZhangAi_SmallSlow;
            comm.Parameters.Add("@TuoPan_Zero", SqlDbType.VarChar, 50).Value = TuoPan_Zero;
            comm.Parameters.Add("@SkipSacn_Beffor", SqlDbType.VarChar, 50).Value = SkipSacn_Beffor;
            comm.Parameters.Add("@HasSkip", SqlDbType.VarChar, 50).Value = HasSkip;
            //3
            comm.Parameters.Add("@Map_X", SqlDbType.VarChar, 50).Value = Map_X;
            comm.Parameters.Add("@Map_Y", SqlDbType.VarChar, 50).Value = Map_Y;
            comm.Parameters.Add("@Map_State", SqlDbType.Int, 10).Value = Map_State;
            comm.Parameters.Add("@MAX_X", SqlDbType.Int, 10).Value = max_x;
            comm.Parameters.Add("@wfloatno", SqlDbType.VarChar, 50).Value = wfloatno;
            int i = -1;
            try
            {
                i = SqlDBControl.ExecuteNonQuery(comm);
            }
            catch (Exception ex)
            {
                throw ex;
                i = -1;
            }
            return i;
        }

        public static int KIVA_UPDATE_tb_WCS_To_KIVA(int wac, string wfloatno)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_tb_WCS_To_KIVA_wac");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@wac", SqlDbType.Int).Value = wac;
            comm.Parameters.Add("@wfloatno", SqlDbType.VarChar, 50).Value = wfloatno;

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
    }
}
