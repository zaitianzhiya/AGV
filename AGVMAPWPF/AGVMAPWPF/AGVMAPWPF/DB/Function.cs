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
using System.Windows;


namespace AGVMAPWPF
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
                        cmd.Parameters.Add("@OwnArea2", SqlDbType.Int).Value = dr["OwnArea",DataRowVersion.Original].ToString();
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

        public static DataTable GettbTaskMustPass(string taskConditonCode,string detailId)
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
                        cmd.CommandText ="DELETE FROM tbTaskConfigDetail WHERE TaskConditonCode=@TaskConditonCode AND DetailID=@DetailID";
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
                        cmd.Parameters.Add("@DetailID", SqlDbType.Int).Value =dr["DetailID"].ToString();
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

        public static string GetMaxDetailID(string code,string detail)
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

        public static int Insert_tbOrder(string num)
        {
            //SqlCommand comm = new SqlCommand("PR_INSERT_TBORDER");
            //comm.CommandType = CommandType.StoredProcedure;
            //comm.Parameters.Add("@FINALPOINT", SqlDbType.Int).Value = num;
            //return SqlDBControl.ExecuteNonQuery(comm);

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString =
                    "database=db_AGVSystem_CT;server=www.kwell-tech.cn;Max Pool Size=30;Min Pool Size=1;uid=sa;pwd=qazwsx12!@";

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_INSERT_TBORDER";
                cmd.Parameters.Add("@FINALPOINT", SqlDbType.Int).Value = num;
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static bool Update_tbMapInfo(Dictionary<string,UIElement> dic,List<string> lstRelation,Dictionary<string,Point> dicLocation)
        {
            SqlConnection con = null;
            SqlTransaction tran = null;
            try
            {
                string strCon = "database=db_AGVSystem_CT;server=www.kwell-tech.cn;Max Pool Size=30;Min Pool Size=1;uid=sa;pwd=qazwsx12!@";
                con = new SqlConnection(strCon);
                SqlCommand cmd = con.CreateCommand();
                con.Open();
                tran = con.BeginTransaction();
                cmd.Transaction = tran;

                cmd.CommandText = "DELETE FROM tb_MapInfo";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"INSERT INTO tb_MapInfo
                                    (
	                                    Now_RFID,
	                                    Next_RFID,
	                                    Go_Direction,
	                                    Turn_Direction,
	                                    [Weight],
	                                    Turn,
	                                    X,
	                                    Y
                                    )
                                    VALUES
                                    (
	                                    @Now_RFID,
	                                    @Next_RFID,
	                                    @Go_Direction,
	                                    @Turn_Direction,
	                                    @Weight,
	                                    @Turn,
	                                    @X,
	                                    @Y
                                    )";
                string[] array;
                StationWindow stationWindow1, stationWindow2;
                Point point1, point2;
                foreach (UIElement u in dic.Values)
                {
                    stationWindow1 = u as StationWindow;
                    if (stationWindow1 != null)
                    {
                        foreach (string s in lstRelation)
                        {
                            array = s.Split('@');
                            if (array[0] == stationWindow1.Uid)
                            {
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add("Now_RFID", SqlDbType.Int).Value = stationWindow1.numValue;
                                stationWindow2 = dic[array[1]] as StationWindow;
                                if (stationWindow2 == null)
                                {
                                    cmd.Parameters.Add("Next_RFID", SqlDbType.Int).Value = DBNull.Value;
                                }
                                else
                                {
                                     cmd.Parameters.Add("Next_RFID", SqlDbType.Int).Value =stationWindow2.numValue;
                                }
                                point1 = dicLocation[array[0]];
                                point2 = dicLocation[array[1]];
                                cmd.Parameters.Add("Go_Direction", SqlDbType.Int).Value = "1";
                                cmd.Parameters.Add("Turn_Direction", SqlDbType.Int).Value = LocationCompare(point1, point2);
                                cmd.Parameters.Add("Weight", SqlDbType.Int).Value = LengthCalc(point1, point2);
                                cmd.Parameters.Add("Turn", SqlDbType.Int).Value = "1";
                                cmd.Parameters.Add("X", SqlDbType.Int).Value = point1.X+10;
                                cmd.Parameters.Add("Y", SqlDbType.Int).Value = point1.X +30;
                                cmd.ExecuteNonQuery();
                            }
                        }
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

        /// 计算两点间位置关系
        /// <summary>
        /// 计算两点间位置关系
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private static int LocationCompare(Point p1,Point p2)
        {
            if (p1.X == p2.X)
            {
                if (p1.Y == p2.Y)
                {
                    return 0;
                }
                else if (p1.Y > p2.Y)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else if (p1.X > p2.X)
            {
                if (p1.Y == p2.Y)
                {
                    return 3;
                }
                else if (p1.Y > p2.Y)
                {
                    return 4;
                }
                else
                {
                    return 5;
                }
            }
            else
            {
                if (p1.Y == p2.Y)
                {
                    return 6;
                }
                else if (p1.Y > p2.Y)
                {
                    return 7;
                }
                else
                {
                    return 8;
                }
            }
            return -1;
        }

        /// 计算两点间直线距离
        /// <summary>
        /// 计算两点间直线距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private static int LengthCalc(Point p1, Point p2)
        {
            double x = Math.Abs(p1.X-p2.X);
            double y= Math.Abs(p1.Y - p2.Y);
            return (int) (Math.Sqrt(x*x + y*y)+0.5);
        }
    }
}
