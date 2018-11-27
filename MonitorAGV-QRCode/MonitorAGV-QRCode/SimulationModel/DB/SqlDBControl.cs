using System.Net.Mime;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
//using System.Windows.Forms;

namespace SimulationModel
{
    public class SqlDBControl
    {
        /// <summary>
        /// 默认的数据库连接字符串
        /// </summary>
        public static string _defultConnectionString = conStr;

        private static string conStr
        {
            get
            {
                string path= System.Windows.Forms.Application.StartupPath + @"\AGV_Set.ini";
                string dataBase = FileControl.SetFileControl.ReadIniValue("DBSETUP", "DATABASE", path);
                string server = FileControl.SetFileControl.ReadIniValue("DBSETUP", "SERVER", path);
                string maxPool = FileControl.SetFileControl.ReadIniValue("DBSETUP", "MaxPoolSize", path);
                string minPool = FileControl.SetFileControl.ReadIniValue("DBSETUP", "MinPoolSize", path);
                string uid = FileControl.SetFileControl.ReadIniValue("DBSETUP", "UID", path);
                string pwd = FileControl.SetFileControl.ReadIniValue("DBSETUP", "PWD", path);
                return string.Format("database={0};server={1};Max Pool Size={2};Min Pool Size={3};uid={4};pwd={5}", dataBase, server,
                    maxPool, minPool, uid, pwd);
            }
        }
        /// <summary>
        /// 重载一:获取数据库连接对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        public static SqlConnection SqlDBConnection(string connectionString)
        {
            SqlConnection _sqlConnection = new SqlConnection();
            _sqlConnection.ConnectionString = connectionString;
            _sqlConnection.Open();
            return _sqlConnection;
        }
        /// <summary>
        /// 重载二:获取数据库连接对象
        /// </summary>
        /// <returns></returns>
        private static SqlConnection SqlDBConnection()
        {
            SqlConnection _sqlConnection = new SqlConnection();
            _sqlConnection.ConnectionString = _defultConnectionString;
            _sqlConnection.Open();
            return _sqlConnection;
        }
        /// <summary>
        /// 关闭数据库连接对象
        /// </summary>
        /// <param name="_sqlConnection">连接中的数据库对象</param>
        private static void CloseSqlConnection(SqlConnection _sqlConnection)
        {
            if (_sqlConnection.State == ConnectionState.Open)
            {
                _sqlConnection.Close();
            }
        }
        /// <summary>
        /// 获取多行数据
        /// </summary>
        /// <param name="_sqlCommand">对数据库的操作方式</param>
        /// <returns></returns>
        public static DataSet ExecuteQuery(SqlCommand _sqlCommand)
        {
            using (SqlConnection _sqlConnection = SqlDBConnection())
            {
                _sqlCommand.Connection = _sqlConnection;
                DataSet _sqlDataSet = new DataSet();
                SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter();
                _sqlDataAdapter.SelectCommand = _sqlCommand;
                _sqlDataAdapter.Fill(_sqlDataSet);
                CloseSqlConnection(_sqlConnection);
                return _sqlDataSet;
            }
        }
        /// <summary>
        /// 获取影响的行数
        /// </summary>
        /// <param name="comm"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlCommand _sqlCommand)
        {
            using (SqlConnection _sqlConnection = SqlDBConnection())
            {
                _sqlCommand.Connection = _sqlConnection;
                int rowsCount = _sqlCommand.ExecuteNonQuery();
                CloseSqlConnection(_sqlConnection);
                return rowsCount;
            }
        }
    }
}
