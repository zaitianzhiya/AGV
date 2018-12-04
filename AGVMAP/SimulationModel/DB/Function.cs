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
using Model.MDM;
using Model.MSM;
using Tools;


namespace AGVMAP
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

        public static string GetMaxValue(string proName)
        {
            SqlCommand comm = new SqlCommand(proName);
            comm.CommandType = CommandType.StoredProcedure;
            SqlParameter parameter = new SqlParameter("@MAXCARCODE", SqlDbType.Int);
            parameter.Direction = ParameterDirection.Output;
            comm.Parameters.Add(parameter);
            SqlDBControl.ExecuteQuery(comm);
            return string.IsNullOrEmpty(parameter.Value.ToString()) ? "1" : (int.Parse(parameter.Value.ToString()) + 1).ToString();
        }

        public static int Update_CoorInfo(string east, string south, string west, string north)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_TBAGVCOORDINATE");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@EAST", SqlDbType.Int).Value = east;
            comm.Parameters.Add("@SOUTH", SqlDbType.Int).Value = south;
            comm.Parameters.Add("@WEST", SqlDbType.Int).Value = west;
            comm.Parameters.Add("@NORTH", SqlDbType.Int).Value = north;
            return SqlDBControl.ExecuteNonQuery(comm);
        }

        public static int Update_tbPlanset(string fileName, byte[] fileContent, int isCreat2D, double zoom, int type)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_TBPLANSET");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@FILENAME", SqlDbType.VarChar).Value = fileName;
            comm.Parameters.Add("@UPTIME", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMddHHmmss");
            comm.Parameters.Add("@FILECONTENT", SqlDbType.Image).Value = fileContent;
            comm.Parameters.Add("@ISCREAT2D", SqlDbType.Int).Value = isCreat2D;
            comm.Parameters.Add("@ZOOM", SqlDbType.Float).Value = zoom;
            comm.Parameters.Add("@TYPE", SqlDbType.Int).Value = type;
            return SqlDBControl.ExecuteNonQuery(comm);
        }

        public static bool Update_tbCar(DataTable dt)
        {
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                foreach (DataRow dr in dt.Rows)
                {
                    cmd.Parameters.Clear();
                    if (dr.RowState == DataRowState.Added)
                    {
                        cmd.CommandText = @"INSERT INTO tbCar
                                            (
	                                            CarCode,
	                                            CarName,
	                                            CarType,
	                                            CarState,
	                                            CarIP,
	                                            CarPort,
	                                            StandbyLandMark
                                            )
                                            VALUES
                                            (
	                                            @CarCode,
	                                            @CarName,
	                                            @CarType,
	                                            @CarState,
	                                            @CarIP,
	                                            @CarPort,
	                                            @StandbyLandMark
                                            )";
                        cmd.Parameters.Add("@CarCode", SqlDbType.Int).Value = dr["CarCode"].ToString();
                        cmd.Parameters.Add("@CarName", SqlDbType.VarChar).Value = dr["CarName"].ToString();
                        cmd.Parameters.Add("@CarType", SqlDbType.Int).Value = dr["CarType"].ToString();
                        cmd.Parameters.Add("@CarState", SqlDbType.Int).Value = "0";
                        cmd.Parameters.Add("@CarIP", SqlDbType.VarChar).Value = dr["CarIP"].ToString();
                        cmd.Parameters.Add("@CarPort", SqlDbType.VarChar).Value = dr["CarPort"].ToString();
                        cmd.Parameters.Add("@StandbyLandMark", SqlDbType.VarChar).Value =
                            dr["StandbyLandMark"].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        cmd.CommandText = @"DELETE FROM tbCar WHERE CarCode=@CarCode";
                        cmd.Parameters.Add("@CarCode", SqlDbType.Int).Value = dr["CarCode", DataRowVersion.Original].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Modified)
                    {
                        cmd.CommandText = @"UPDATE tbCar
                                            SET CarName = @CarName,
	                                            CarType = @CarType,
	                                            CarIP = @CarIP,
	                                            CarPort = @CarPort,
	                                            StandbyLandMark = @StandbyLandMark
                                            WHERE CarCode=@CarCode";
                        cmd.Parameters.Add("@CarName", SqlDbType.VarChar).Value = dr["CarName"].ToString();
                        cmd.Parameters.Add("@CarType", SqlDbType.Int).Value = dr["CarType"].ToString();
                        cmd.Parameters.Add("@CarIP", SqlDbType.VarChar).Value = dr["CarIP"].ToString();
                        cmd.Parameters.Add("@CarPort", SqlDbType.VarChar).Value = dr["CarPort"].ToString();
                        cmd.Parameters.Add("@StandbyLandMark", SqlDbType.VarChar).Value =
                            dr["StandbyLandMark"].ToString();
                        cmd.Parameters.Add("@CarCode", SqlDbType.Int).Value = dr["CarCode"].ToString();
                        cmd.ExecuteNonQuery();
                    }
                }
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public static bool Update_tbSysParameter(DataTable dt)
        {
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                cmd.CommandText = "select count(1) from tbSysParamater";
                int count = int.Parse(cmd.ExecuteScalar().ToString());
                if (count == 0)
                {
                    cmd.CommandText = @"INSERT INTO tbSysParamater
	                                    (
		                                    ParameterCode,
		                                    ParameterValue
	                                    )
	                                    VALUES
	                                    (
		                                    @ParameterCode,
		                                    @ParameterValue
	                                    )";
                }
                else
                {
                    cmd.CommandText = @"UPDATE tbSysParamater
	                                    SET
		                                    ParameterValue = @ParameterValue
	                                    WHERE ParameterCode=@ParameterCode";
                }
                foreach (DataRow dr in dt.Rows)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@ParameterCode", SqlDbType.VarChar).Value = dr["ParameterCode"].ToString();
                    cmd.Parameters.Add("@ParameterValue", SqlDbType.VarChar).Value = dr["ParameterValue"].ToString();
                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public static int Del_DataByPk(string table, string pk)
        {
            SqlCommand comm = new SqlCommand("PR_DELELE_DATABYPK");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@TYPE", SqlDbType.VarChar).Value = table;
            comm.Parameters.Add("@PK", SqlDbType.VarChar).Value = pk;
            return SqlDBControl.ExecuteNonQuery(comm);
        }

        public static DataTable GetCallBox(string callBoxId)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_CALLBOX");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@CallBoxID", SqlDbType.Int).Value = callBoxId;
            return SqlDBControl.ExecuteQuery(comm).Tables[0];
        }

        public static bool Update_tbCallBox(string mode, string callId, string callName, string type, DataTable dt)
        {
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                if (mode == "N")
                {
                    cmd.CommandText = @"INSERT INTO tbCallBoxInfo
                                    (
	                                    CallBoxID,
	                                    CallBoxName,
	                                    CallBoxType
                                    )
                                    VALUES
                                    (
	                                    @CallBoxID,
	                                    @CallBoxName,
	                                    @CallBoxType
                                    )";
                }
                else
                {
                    cmd.CommandText = @"UPDATE tbCallBoxInfo
                                        SET CallBoxName = @CallBoxName,
	                                        CallBoxType = @CallBoxType
                                        WHERE CallBoxID=@CallBoxID";
                }
                cmd.Parameters.Add("@CallBoxID", SqlDbType.Int).Value = callId;
                cmd.Parameters.Add("@CallBoxName", SqlDbType.VarChar).Value = callName;
                cmd.Parameters.Add("@CallBoxType", SqlDbType.Int).Value = type;
                cmd.ExecuteNonQuery();

                foreach (DataRow dr in dt.Rows)
                {
                    cmd.Parameters.Clear();
                    if (dr.RowState == DataRowState.Added)
                    {
                        cmd.CommandText = @"INSERT INTO tbCallBoxDetail
                                            (
	                                            CallBoxID,
	                                            ButtonID,
	                                            TaskConditonCode,
	                                            OperaType,
	                                            LocationID,
	                                            LocationState
                                            )
                                            VALUES
                                            (
	                                            @CallBoxID,
	                                            @ButtonID,
	                                            @TaskConditonCode,
	                                            @OperaType,
	                                            @LocationID,
	                                            @LocationState
                                            )";
                        cmd.Parameters.Add("@CallBoxID", SqlDbType.Int).Value = dr["CallBoxID"].ToString();
                        cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = dr["ButtonID"].ToString();
                        cmd.Parameters.Add("@TaskConditonCode", SqlDbType.VarChar).Value =
                            dr["TaskConditonCode"].ToString();
                        cmd.Parameters.Add("@OperaType", SqlDbType.Int).Value = dr["OperaType"].ToString();
                        cmd.Parameters.Add("@LocationID", SqlDbType.Int).Value = dr["LocationID"].ToString();
                        cmd.Parameters.Add("@LocationState", SqlDbType.Int).Value = dr["LocationState"].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        cmd.CommandText =
                            "DELETE FROM tbCallBoxDetail WHERE CallBoxID=@CallBoxID AND ButtonID=@ButtonID";
                        cmd.Parameters.Add("@CallBoxID", SqlDbType.Int).Value = dr["CallBoxID", DataRowVersion.Original].ToString();
                        cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = dr["ButtonID", DataRowVersion.Original].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Modified)
                    {
                        cmd.CommandText = @"UPDATE tbCallBoxDetail
                                            SET	ButtonID =@ButtonID,
	                                            TaskConditonCode = @TaskConditonCode,
	                                            OperaType = @OperaType,
	                                            LocationID = @LocationID,
	                                            LocationState = @LocationState
                                            WHERE CallBoxID=@CallBoxID
                                            AND ButtonID=@ButtonIDOld";
                        cmd.Parameters.Add("@CallBoxID", SqlDbType.Int).Value = dr["CallBoxID"].ToString();
                        cmd.Parameters.Add("@ButtonID", SqlDbType.Int).Value = dr["ButtonID"].ToString();
                        cmd.Parameters.Add("@TaskConditonCode", SqlDbType.VarChar).Value =
                            dr["TaskConditonCode"].ToString();
                        cmd.Parameters.Add("@OperaType", SqlDbType.Int).Value = dr["OperaType"].ToString();
                        cmd.Parameters.Add("@LocationID", SqlDbType.Int).Value = dr["LocationID"].ToString();
                        cmd.Parameters.Add("@LocationState", SqlDbType.Int).Value = dr["LocationState"].ToString();
                        cmd.Parameters.Add("@ButtonIDOld", SqlDbType.Int).Value =
                            dr["ButtonID", DataRowVersion.Original].ToString();
                        cmd.ExecuteNonQuery();
                    }
                }
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public static bool Update_tbAreaInfo(DataTable dt)
        {
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                foreach (DataRow dr in dt.Rows)
                {
                    cmd.Parameters.Clear();
                    if (dr.RowState == DataRowState.Added)
                    {
                        cmd.CommandText = @"INSERT INTO tbAreaInfo
                                            (
	                                            OwnArea,
	                                            AreaName
                                            )
                                            VALUES
                                            (
	                                            @OwnArea,
	                                            @AreaName
                                            )";
                        cmd.Parameters.Add("@OwnArea", SqlDbType.Int).Value = dr["OwnArea"].ToString();
                        cmd.Parameters.Add("@AreaName", SqlDbType.VarChar).Value = dr["AreaName"].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        cmd.CommandText = @"DELETE FROM tbAreaInfo WHERE OwnArea=@OwnArea";
                        cmd.Parameters.Add("@OwnArea", SqlDbType.Int).Value = dr["OwnArea", DataRowVersion.Original].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Modified)
                    {
                        cmd.CommandText = @"UPDATE tbAreaInfo
                                            SET OwnArea = @OwnArea,
	                                            AreaName = @AreaName
                                            WHERE OwnArea=@OwnArea2";
                        cmd.Parameters.Add("@OwnArea", SqlDbType.Int).Value = dr["OwnArea"].ToString();
                        cmd.Parameters.Add("@AreaName", SqlDbType.VarChar).Value = dr["AreaName"].ToString();
                        cmd.Parameters.Add("@OwnArea2", SqlDbType.Int).Value = dr["OwnArea", DataRowVersion.Original].ToString();
                        cmd.ExecuteNonQuery();
                    }
                }
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public static bool Update_tbMaterialInfo(DataTable dt)
        {
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                foreach (DataRow dr in dt.Rows)
                {
                    cmd.Parameters.Clear();
                    if (dr.RowState == DataRowState.Added)
                    {
                        cmd.CommandText = @"INSERT INTO tbMaterialInfo
                                            (
	                                            MaterialType,
	                                            MaterialName
                                            )
                                            VALUES
                                            (
	                                            @MaterialType,
	                                            @MaterialName
                                            )";
                        cmd.Parameters.Add("@MaterialType", SqlDbType.Int).Value = dr["MaterialType"].ToString();
                        cmd.Parameters.Add("@MaterialName", SqlDbType.VarChar).Value = dr["MaterialName"].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        cmd.CommandText = @"DELETE FROM tbMaterialInfo WHERE MaterialType=@MaterialType";
                        cmd.Parameters.Add("@OwnArea", SqlDbType.Int).Value = dr["MaterialType", DataRowVersion.Original].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Modified)
                    {
                        cmd.CommandText = @"UPDATE tbMaterialInfo
                                            SET MaterialType = @MaterialType,
	                                            MaterialName = @MaterialName
                                            WHERE MaterialType=@MaterialType2";
                        cmd.Parameters.Add("@MaterialType", SqlDbType.Int).Value = dr["MaterialType"].ToString();
                        cmd.Parameters.Add("@MaterialName", SqlDbType.VarChar).Value = dr["MaterialName"].ToString();
                        cmd.Parameters.Add("@MaterialType2", SqlDbType.Int).Value = dr["MaterialType", DataRowVersion.Original].ToString();
                        cmd.ExecuteNonQuery();
                    }
                }
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public static bool Update_tbActionInfo(DataTable dt)
        {
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                foreach (DataRow dr in dt.Rows)
                {
                    cmd.Parameters.Clear();
                    if (dr.RowState == DataRowState.Added)
                    {
                        cmd.CommandText = @"INSERT INTO tbActionInfo
                                            (
	                                            ActionID,
	                                            ActionName,
	                                            WaitTime,
	                                            CommondText
                                            )
                                            VALUES
                                            (
	                                            @ActionID,
	                                            @ActionName,
	                                            @WaitTime,
	                                            @CommondText
                                            )";
                        cmd.Parameters.Add("@ActionID", SqlDbType.Int).Value = dr["ActionID"].ToString();
                        cmd.Parameters.Add("@ActionName", SqlDbType.VarChar).Value = dr["ActionName"].ToString();
                        cmd.Parameters.Add("@WaitTime", SqlDbType.Float).Value = dr["WaitTime"].ToString();
                        cmd.Parameters.Add("@CommondText", SqlDbType.VarChar).Value = dr["CommondText"].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        cmd.CommandText = @"DELETE FROM tbActionInfo WHERE ActionID=@ActionID";
                        cmd.Parameters.Add("@ActionID", SqlDbType.Int).Value = dr["ActionID", DataRowVersion.Original].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Modified)
                    {
                        cmd.CommandText = @"UPDATE tbActionInfo
                                            SET ActionID = @ActionID,
	                                            ActionName = @ActionName,
	                                            WaitTime =@WaitTime,
	                                            CommondText = @CommondText
                                            WHERE ActionID=@ActionID2";
                        cmd.Parameters.Add("@ActionID", SqlDbType.Int).Value = dr["ActionID"].ToString();
                        cmd.Parameters.Add("@ActionName", SqlDbType.VarChar).Value = dr["ActionName"].ToString();
                        cmd.Parameters.Add("@WaitTime", SqlDbType.Float).Value = dr["WaitTime"].ToString();
                        cmd.Parameters.Add("@CommondText", SqlDbType.VarChar).Value = dr["CommondText"].ToString();
                        cmd.Parameters.Add("@ActionID2", SqlDbType.Int).Value = dr["ActionID", DataRowVersion.Original].ToString();
                        cmd.ExecuteNonQuery();
                    }
                }
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public static DataTable GettbTaskConfigDetail(string taskConditonCode)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_TASKDETAILINFOBYID");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@TASKCONDITIONCODE", SqlDbType.VarChar).Value = taskConditonCode;
            return SqlDBControl.ExecuteQuery(comm).Tables[0];
        }

        public static DataTable GettbTaskMustPass(string taskConditonCode, string detailId)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_TASKMUSTPASS");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@CODE", SqlDbType.VarChar).Value = taskConditonCode;
            comm.Parameters.Add("@DETAILID", SqlDbType.Int).Value = detailId;
            return SqlDBControl.ExecuteQuery(comm).Tables[0];
        }

        public static string GetMaxDetailID(string code)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_MAXDETAILID");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@CODE", SqlDbType.VarChar).Value = code;
            SqlParameter parameter = new SqlParameter("@MAXID", SqlDbType.Int);
            parameter.Direction = ParameterDirection.Output;
            comm.Parameters.Add(parameter);
            SqlDBControl.ExecuteQuery(comm);
            return string.IsNullOrEmpty(parameter.Value.ToString()) ? "1" : (int.Parse(parameter.Value.ToString()) + 1).ToString();
        }

        public static bool Update_tbTaskConfigDetail(string mode, string code, string name, DataTable dt)
        {
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                if (mode == "N")
                {
                    cmd.CommandText = @"INSERT INTO tbTaskConfigInfo
                                        (
	                                        TaskConditonCode,
	                                        TaskConditonName
                                        )
                                        VALUES
                                        (
	                                        @TaskConditonCode,
	                                        @TaskConditonName
                                        )";
                }
                else
                {
                    cmd.CommandText = @"UPDATE tbTaskConfigInfo
                                        SET TaskConditonName = @TaskConditonName
                                        WHERE TaskConditonCode=@TaskConditonCode ";
                }
                cmd.Parameters.Add("@TaskConditonCode", SqlDbType.VarChar).Value = code;
                cmd.Parameters.Add("@TaskConditonName", SqlDbType.VarChar).Value = name;
                cmd.ExecuteNonQuery();

                foreach (DataRow dr in dt.Rows)
                {
                    cmd.Parameters.Clear();
                    if (dr.RowState == DataRowState.Added)
                    {
                        cmd.CommandText = @"INSERT INTO tbTaskConfigDetail
                                            (
	                                            TaskConditonCode,
	                                            DetailID,
	                                            ArmOwnArea,
	                                            StorageState,
	                                            MaterialType,
	                                            [Action],
	                                            IsWaitPass
                                            )
                                            VALUES
                                            (
	                                            @TaskConditonCode,
	                                            @DetailID,
	                                            @ArmOwnArea,
	                                            @StorageState,
	                                            @MaterialType,
	                                            @Action,
	                                            @IsWaitPass
                                            )";
                        cmd.Parameters.Add("@TaskConditonCode", SqlDbType.VarChar).Value = dr["TaskConditonCode"].ToString();
                        cmd.Parameters.Add("@DetailID", SqlDbType.Int).Value = dr["DetailID"].ToString();
                        cmd.Parameters.Add("@ArmOwnArea", SqlDbType.Int).Value = dr["ArmOwnArea"].ToString();
                        cmd.Parameters.Add("@StorageState", SqlDbType.Int).Value = dr["StorageState"].ToString();
                        cmd.Parameters.Add("@MaterialType", SqlDbType.Int).Value = dr["MaterialType"].ToString();
                        cmd.Parameters.Add("@Action", SqlDbType.Int).Value = dr["Action"].ToString();
                        cmd.Parameters.Add("@IsWaitPass", SqlDbType.Int).Value = dr["IsWaitPass"].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        cmd.CommandText = "DELETE FROM tbTaskConfigDetail WHERE TaskConditonCode=@TaskConditonCode AND DetailID=@DetailID";
                        cmd.Parameters.Add("@TaskConditonCode", SqlDbType.VarChar).Value = dr["TaskConditonCode", DataRowVersion.Original].ToString();
                        cmd.Parameters.Add("@DetailID", SqlDbType.Int).Value = dr["DetailID", DataRowVersion.Original].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Modified)
                    {
                        cmd.CommandText = @"UPDATE tbTaskConfigDetail
                                            SET ArmOwnArea = @ArmOwnArea,
	                                            StorageState = @StorageState,
	                                            MaterialType = @MaterialType,
	                                            [Action] = @Action,
	                                            IsWaitPass = @IsWaitPass
                                            WHERE TaskConditonCode=@TaskConditonCode
                                            AND DetailID=@DetailID";
                        cmd.Parameters.Add("@ArmOwnArea", SqlDbType.Int).Value = dr["ArmOwnArea"].ToString();
                        cmd.Parameters.Add("@StorageState", SqlDbType.Int).Value = dr["StorageState"].ToString();
                        cmd.Parameters.Add("@MaterialType", SqlDbType.Int).Value = dr["MaterialType"].ToString();
                        cmd.Parameters.Add("@Action", SqlDbType.Int).Value = dr["Action"].ToString();
                        cmd.Parameters.Add("@IsWaitPass", SqlDbType.Int).Value = dr["IsWaitPass"].ToString();
                        cmd.Parameters.Add("@TaskConditonCode", SqlDbType.VarChar).Value = dr["TaskConditonCode"].ToString();
                        cmd.Parameters.Add("@DetailID", SqlDbType.Int).Value = dr["DetailID"].ToString();
                        cmd.ExecuteNonQuery();
                    }
                }
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public static string GetMaxDetailID(string code, string detail)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_MAXID_MUSTPASS");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@CODE", SqlDbType.VarChar).Value = code;
            comm.Parameters.Add("@DETAILID", SqlDbType.Int).Value = detail;
            SqlParameter parameter = new SqlParameter("@MAXID", SqlDbType.Int);
            parameter.Direction = ParameterDirection.Output;
            comm.Parameters.Add(parameter);
            SqlDBControl.ExecuteQuery(comm);
            return string.IsNullOrEmpty(parameter.Value.ToString()) ? "1" : (int.Parse(parameter.Value.ToString()) + 1).ToString();
        }

        public static bool Update_tbTaskConfigMustPass(DataTable dt)
        {
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                foreach (DataRow dr in dt.Rows)
                {
                    cmd.Parameters.Clear();
                    if (dr.RowState == DataRowState.Added)
                    {
                        cmd.CommandText = @"INSERT INTO tbTaskConfigMustPass
                                            (
	                                            TaskConditonCode,
	                                            TaskConfigDetailID,
	                                            DetailID,
	                                            MustPassLandCode,
	                                            [Action]
                                            )
                                            VALUES
                                            (
	                                            @TaskConditonCode,
	                                            @TaskConfigDetailID,
	                                            @DetailID,
	                                            @MustPassLandCode,
	                                            @Action
                                            )";
                        cmd.Parameters.Add("@TaskConditonCode", SqlDbType.VarChar).Value = dr["TaskConditonCode"].ToString();
                        cmd.Parameters.Add("@TaskConfigDetailID", SqlDbType.Int).Value = dr["TaskConfigDetailID"].ToString();
                        cmd.Parameters.Add("@DetailID", SqlDbType.Int).Value = dr["DetailID"].ToString();
                        cmd.Parameters.Add("@MustPassLandCode", SqlDbType.VarChar).Value = dr["MustPassLandCode"].ToString();
                        cmd.Parameters.Add("@Action", SqlDbType.Int).Value = dr["Action"].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        cmd.CommandText =
                            "DELETE FROM tbTaskConfigMustPass WHERE TaskConditonCode=@TaskConditonCode AND TaskConfigDetailID=@TaskConfigDetailID AND DetailID=@DetailID";
                        cmd.Parameters.Add("@TaskConditonCode", SqlDbType.VarChar).Value = dr["TaskConditonCode", DataRowVersion.Original].ToString();
                        cmd.Parameters.Add("@TaskConfigDetailID", SqlDbType.Int).Value = dr["TaskConfigDetailID", DataRowVersion.Original].ToString();
                        cmd.Parameters.Add("@DetailID", SqlDbType.Int).Value = dr["DetailID", DataRowVersion.Original].ToString();
                        cmd.ExecuteNonQuery();
                    }
                    if (dr.RowState == DataRowState.Modified)
                    {
                        cmd.CommandText = @"UPDATE tbTaskConfigMustPass
                                            SET MustPassLandCode = @MustPassLandCode,
	                                            [Action] = @Action
                                            WHERE TaskConditonCode=@TaskConditonCode
                                            AND TaskConfigDetailID=@TaskConfigDetailID
                                            AND DetailID=@DetailID";
                        cmd.Parameters.Add("@MustPassLandCode", SqlDbType.VarChar).Value = dr["MustPassLandCode"].ToString();
                        cmd.Parameters.Add("@Action", SqlDbType.Int).Value = dr["Action"].ToString();
                        cmd.Parameters.Add("@TaskConditonCode", SqlDbType.VarChar).Value = dr["TaskConditonCode"].ToString();
                        cmd.Parameters.Add("@TaskConfigDetailID", SqlDbType.Int).Value = dr["TaskConfigDetailID"].ToString();
                        cmd.Parameters.Add("@DetailID", SqlDbType.Int).Value = dr["DetailID"].ToString();
                        cmd.ExecuteNonQuery();
                    }
                }
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public static DataTable GetParByCondition(string parameterCode)
        {
            SqlCommand comm = new SqlCommand("P_SELECT_PARAMATER_BY_CONDITION");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@ParameterCode", SqlDbType.VarChar).Value = parameterCode;
            return SqlDBControl.ExecuteQuery(comm).Tables[0];
        }

        public static OperateReturnInfo SaveMap(string FilePath, string FileName, float Zoom, IList<LandmarkInfo> Lands, IList<StorageInfo> stocks, IList<AllSegment> allsegment)
        {
            OperateReturnInfo result;
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                Hashtable hashtable = new Hashtable();
                hashtable["fileName"] = FileName;
                cmd.CommandText = "delete from tbPlanset where fileName=@fileName";
                SqlParameter para1 = new SqlParameter("fileName", SqlDbType.VarChar);
                para1.Value = FileName;
                cmd.Parameters.Add(para1);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "select max(isnull(ID,0))+1 from tbPlanset ";
                int new_ID = int.Parse(cmd.ExecuteScalar().ToString());

                cmd.Parameters.Clear();

                cmd.CommandText = @"INSERT INTO tbPlanset
                                      (
      	                                ID,
      	                                [fileName],
      	                                UpTime,
      	                                FileContent,
      	                                IsCreat2D,
      	                                Zoom,
      	                                [Type]
                                      )
                                      VALUES
                                      (
      	                                @ID,
      	                                @fileName,
      	                                @UpTime,
      	                                @FileContent,
      	                                0,
      	                                @Zoom,
      	                                0
                                      )";
                para1 = new SqlParameter("ID", SqlDbType.VarChar);
                para1.Value = new_ID;
                cmd.Parameters.Add(para1);

                SqlParameter para2 = new SqlParameter("fileName", SqlDbType.VarChar);
                para2.Value = FileName;
                cmd.Parameters.Add(para2);

                SqlParameter para3 = new SqlParameter("UpTime", SqlDbType.VarChar);
                para3.Value = DateTime.Now.ToString("yyyyMMddHHmmss");
                cmd.Parameters.Add(para3);

                FileStream fileStream = new FileStream(FilePath, FileMode.Open);
                BinaryReader binaryReader = new BinaryReader(fileStream);
                byte[] fileContent = binaryReader.ReadBytes((int)fileStream.Length);
                fileStream.Close();

                SqlParameter para4 = new SqlParameter("FileContent", SqlDbType.Image);
                para4.Value = fileContent;
                cmd.Parameters.Add(para4);

                SqlParameter para5 = new SqlParameter("Zoom", SqlDbType.Float);
                para5.Value = Zoom;
                cmd.Parameters.Add(para5);

                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                cmd.CommandText = "delete from tbLandMark";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"INSERT INTO tbLandMark
                                      (
      	                                LandCode,
      	                                LandName,
      	                                LandY,
      	                                LandX
                                      )
                                      VALUES
                                      (
      	                                @LandCode,
      	                                @LandName,
      	                                @LandY,
      	                                @LandX
                                      )";

                foreach (LandmarkInfo current in Lands)
                {
                    cmd.Parameters.Clear();
                    para1 = new SqlParameter("LandCode", SqlDbType.VarChar);
                    para1.Value = current.LandmarkCode;
                    cmd.Parameters.Add(para1);
                    para2 = new SqlParameter("LandName", SqlDbType.VarChar);
                    para2.Value = current.LandmarkName;
                    cmd.Parameters.Add(para2);
                    para3 = new SqlParameter("LandY", SqlDbType.Float);
                    para3.Value = current.LandY;
                    cmd.Parameters.Add(para3);
                    para4 = new SqlParameter("LandX", SqlDbType.Float);
                    para4.Value = current.LandX;
                    cmd.Parameters.Add(para4);

                    cmd.ExecuteNonQuery();
                }

                bool flag4 = stocks != null && stocks.Count > 0;
                if (flag4)
                {
                    string ids = "";
                    foreach (StorageInfo s in stocks)
                    {
                        ids += s.ID + ",";
                    }
                    cmd.Parameters.Clear();
                    cmd.CommandText =string.Format("delete from tbLocaton where ID not in ({0})",ids.TrimEnd(','));

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"IF EXISTS(SELECT 1 FROM tbLocaton AS tl WHERE tl.ID=@ID)
	                                    BEGIN
		                                    UPDATE tbLocaton
		                                    SET StorageName = @StorageName,
			                                    OwnArea = @OwnArea,
			                                    LankMarkCode = @LankMarkCode,
			                                    StorageState = @StorageState
		                                    WHERE ID=@ID
	                                    END 
                                    ELSE
	                                    BEGIN
		                                    INSERT INTO tbLocaton
		                                    (
			                                    ID,
			                                    StorageName,
			                                    OwnArea,
			                                    LankMarkCode,
			                                    StorageState
		                                    )
		                                    VALUES
		                                    (
			                                    @ID,
			                                    @StorageName,
			                                    @OwnArea,
			                                    @LankMarkCode,
			                                    @StorageState
		                                    )
	                                    END  ";
                    foreach (StorageInfo current2 in stocks)
                    {
                        cmd.Parameters.Clear();
                        para1 = new SqlParameter("ID", SqlDbType.Int);
                        para1.Value = current2.ID;
                        cmd.Parameters.Add(para1);
                        para2 = new SqlParameter("StorageName", SqlDbType.VarChar);
                        para2.Value = current2.StorageName;
                        cmd.Parameters.Add(para2);
                        para3 = new SqlParameter("OwnArea", SqlDbType.Int);
                        para3.Value = current2.OwnArea;
                        cmd.Parameters.Add(para3);
                        para4 = new SqlParameter("LankMarkCode", SqlDbType.VarChar);
                        para4.Value = current2.LankMarkCode;
                        cmd.Parameters.Add(para4);
                        para5 = new SqlParameter("StorageState", SqlDbType.Int);
                        para5.Value = current2.StorageState;
                        cmd.Parameters.Add(para5);

                        cmd.ExecuteNonQuery();
                    }
                }
                bool flag7 = allsegment != null && allsegment.Count > 0;
                if (flag7)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "delete from tbAllSegment";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = @"INSERT INTO tbAllSegment
                                    (
	                                    BeginLandMakCode,
	                                    EndLandMarkCode,
	                                    Length
                                    )
                                    VALUES
                                    (
	                                    @BeginLandMakCode,
	                                    @EndLandMarkCode,
	                                    @Length
                                    )";
                    foreach (AllSegment current3 in allsegment)
                    {
                        cmd.Parameters.Clear();
                        para1 = new SqlParameter("BeginLandMakCode", SqlDbType.VarChar);
                        para1.Value = current3.BeginLandMakCode;
                        cmd.Parameters.Add(para1);
                        para2 = new SqlParameter("EndLandMarkCode", SqlDbType.VarChar);
                        para2.Value = current3.EndLandMarkCode;
                        cmd.Parameters.Add(para2);
                        para3 = new SqlParameter("Length", SqlDbType.Float);
                        para3.Value = current3.Length;
                        cmd.Parameters.Add(para3);

                        cmd.ExecuteNonQuery();
                    }
                }
                tran.Commit();
                result = new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                result = new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return result;
        }

        public static DataTable GetCallBoxInfoById(int id)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_CALLBOXBYID");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@CallBoxID", SqlDbType.Int).Value = id;
            return SqlDBControl.ExecuteQuery(comm).Tables[0];
        }

        public static DataTable GetCallBoxDetailInfoById(int id)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_CALLBOXDETIALBYID");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@CallBoxID", SqlDbType.Int).Value = id;
            return SqlDBControl.ExecuteQuery(comm).Tables[0];
        }

        public static DataTable ChekAllowCreatTask(int id,string callLand)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_CHECK_CREATE_TASK");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@CallBoxID", SqlDbType.Int).Value = id;
            comm.Parameters.Add("@CallLand", SqlDbType.VarChar).Value = callLand;
            return SqlDBControl.ExecuteQuery(comm).Tables[0];
        }

        public static OperateReturnInfo SaveTask(DispatchTaskInfo taskInfo)
        {
            OperateReturnInfo result;
            SqlConnection con = null;
            SqlTransaction tran=null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                cmd.CommandText = @"INSERT INTO tbDispatchTaskInfo
                                    (
	                                    dispatchNo,
	                                    stationNo,
	                                    taskType,
	                                    BuildTime,
	                                    CallLand
                                    )
                                    VALUES
                                    (
                                        @dispatchNo,
	                                    @stationNo,
	                                    @taskType,
	                                    @BuildTime,
	                                    @CallLand
                                    )";

                SqlParameter para1 = new SqlParameter("dispatchNo", SqlDbType.VarChar);
                para1.Value = taskInfo.dispatchNo;
                cmd.Parameters.Add(para1);
                SqlParameter para2 = new SqlParameter("stationNo", SqlDbType.Int);
                para2.Value = taskInfo.stationNo;
                cmd.Parameters.Add(para2);
                SqlParameter para3 = new SqlParameter("taskType", SqlDbType.VarChar);
                para3.Value = "0";
                cmd.Parameters.Add(para3);
                SqlParameter para4 = new SqlParameter("BuildTime", SqlDbType.VarChar);
                para4.Value = DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss");
                cmd.Parameters.Add(para4);
                SqlParameter para5 = new SqlParameter("CallLand", SqlDbType.VarChar);
                para5.Value = taskInfo.CallLand;
                cmd.Parameters.Add(para5);

                cmd.ExecuteNonQuery();

                cmd.CommandText = @"INSERT INTO tbDispatchTaskDetail
                                    (
	                                    dispatchNo,
	                                    DetailID,
	                                    LandCode,
	                                    OperType,
	                                    IsAllowExcute,	
	                                    PutType
                                    )
                                    VALUES
                                    (
	                                    @dispatchNo,
	                                    @DetailID,
	                                    @LandCode,
	                                    @OperType,
	                                    @IsAllowExcute,	
	                                    @PutType
                                    )";
                SqlParameter para6;
                foreach (DispatchTaskDetail current in taskInfo.TaskDetail)
                {
                    cmd.Parameters.Clear();
                    para1 = new SqlParameter("dispatchNo", SqlDbType.VarChar);
                    para1.Value = taskInfo.dispatchNo;
                    cmd.Parameters.Add(para1);
                    para2 = new SqlParameter("DetailID", SqlDbType.Int);
                    para2.Value = current.DetailID.ToString();
                    cmd.Parameters.Add(para2);
                    para3 = new SqlParameter("LandCode", SqlDbType.VarChar);
                    para3.Value = current.LandCode;
                    cmd.Parameters.Add(para3);
                    para4 = new SqlParameter("OperType", SqlDbType.VarChar);
                    para4.Value = current.OperType;
                    cmd.Parameters.Add(para4);
                    para5 = new SqlParameter("IsAllowExcute", SqlDbType.VarChar);
                    para5.Value = current.IsAllowExcute.ToString();
                    cmd.Parameters.Add(para5);
                    para6 = new SqlParameter("PutType", SqlDbType.VarChar);
                    para6.Value = current.PutType;
                    cmd.Parameters.Add(para6);

                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
                result = new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                result = new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return result;
        }

        public static OperateReturnInfo ReleaseCar(string ExcuteTaksNo, string LandCode)
        {
            OperateReturnInfo result;
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                cmd.CommandText = @" UPDATE tbDispatchTaskDetail
                                      set IsAllowExcute=1
                                      where dispatchNo=@TaskNo
                                      and LandCode=@LandCode
                                      and DetailID=(
                                      select coalesce(min(DetailID),0)
                                      from tbDispatchTaskDetail
                                      where IsAllowExcute=0
                                      and dispatchNo=@TaskNo
                                      and LandCode=@LandCode
                                      )";

                SqlParameter para1 = new SqlParameter("TaskNo", SqlDbType.VarChar);
                para1.Value = ExcuteTaksNo;
                cmd.Parameters.Add(para1);
                SqlParameter para2 = new SqlParameter("LandCode", SqlDbType.VarChar);
                para2.Value = LandCode;
                cmd.Parameters.Add(para2);
                cmd.ExecuteNonQuery();

                tran.Commit();
                result = new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                result = new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return result;
        }

        public static OperateReturnInfo UpdateStore(int StoreID, int StorageState)
        {
            OperateReturnInfo result;
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                con = new SqlConnection(SqlDBControl._defultConnectionString);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                cmd.CommandText = @" UPDATE tbLocaton
                                      set StorageState=@StorageState
                                      where ID=@ID";

                SqlParameter para1 = new SqlParameter("StorageState", SqlDbType.Int);
                para1.Value = StorageState;
                cmd.Parameters.Add(para1);
                SqlParameter para2 = new SqlParameter("ID", SqlDbType.Int);
                para2.Value = StoreID;
                cmd.Parameters.Add(para2);
                cmd.ExecuteNonQuery();

                tran.Commit();
                result = new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                result = new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return result;
        }

        public static IList<DispatchTaskInfo> LoadDispatchTask()
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_DISPATCHTASKINFO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@PreDate", SqlDbType.VarChar).Value = DateTime.Now.AddDays(-1.0).ToString("yyyy/MM/dd");
            comm.Parameters.Add("@CurrDate", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyy/MM/dd");
            IList<DispatchTaskInfo> list =
                DataToObject.TableToEntity<DispatchTaskInfo>(SqlDBControl.ExecuteQuery(comm).Tables[0]);
            foreach (DispatchTaskInfo current in list)
            {
                current.TaskDetail = DataToObject.TableToEntity<DispatchTaskDetail>(GetDispatchtaskDetailInfoByNo(current.dispatchNo));
            }
            return list;
        }

        public static DataTable GetDispatchtaskDetailInfoByNo(string dispatchNo)
        {
            SqlCommand comm = new SqlCommand("PR_SELECT_DISPATCHTASKDETAILINFO");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@dispatchNo", SqlDbType.VarChar).Value = dispatchNo;
            return SqlDBControl.ExecuteQuery(comm).Tables[0];
        }

        public static int UpdateTaskDetailForFinish(string landCode, string dispatchNo, int taskDetailID)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_DISPATCHTASK");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@dispatchNo", SqlDbType.VarChar).Value = dispatchNo;
            comm.Parameters.Add("@LandCode", SqlDbType.VarChar).Value = landCode;
            comm.Parameters.Add("@DetailID", SqlDbType.Int).Value =taskDetailID;
            return SqlDBControl.ExecuteNonQuery(comm);
        }

        public static int UpdateLocation(string lankMarkCode, int storageState)
        {
            SqlCommand comm = new SqlCommand("PR_UPDATE_TBLOCATION");
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("@LankMarkCode", SqlDbType.VarChar).Value = lankMarkCode;
            comm.Parameters.Add("@StorageState", SqlDbType.Int).Value = storageState;
            return SqlDBControl.ExecuteNonQuery(comm);
        }

        public static int TaskHandle(string dispatchNo, int AGVID, int TaskState, string LandCode, int TaskDetialID)
        {
            if (TaskState==2)
            {
                SqlCommand comm = new SqlCommand("PR_UPDATE_DISPATCHTASKINFO");
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@TaskState", SqlDbType.Int).Value = TaskState;
                comm.Parameters.Add("@FinishTime", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss");
                comm.Parameters.Add("@dispatchNo", SqlDbType.VarChar).Value = dispatchNo;
                return SqlDBControl.ExecuteNonQuery(comm);
            }
            else
            {
                if (TaskState == 1)
                {
                    SqlCommand comm = new SqlCommand("PR_UPDATE_DISPATCHTASK2");
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.Add("@dispatchNo", SqlDbType.VarChar).Value = dispatchNo;
                    comm.Parameters.Add("@LandCode", SqlDbType.VarChar).Value = LandCode;
                    comm.Parameters.Add("@DetailID", SqlDbType.Int).Value = TaskDetialID;
                    comm.Parameters.Add("@TaskState", SqlDbType.Int).Value = TaskState;
                    comm.Parameters.Add("@ExeAgvID", SqlDbType.Int).Value = AGVID;
                    comm.Parameters.Add("@ExeTime", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss");
                    return SqlDBControl.ExecuteNonQuery(comm);
                }
            }
            return -1;
        }
    }
}
