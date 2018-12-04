using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
//using System.Windows.Forms;

namespace AGVMAPWPF
{
    public class SqlDBControl
    {
        /// <summary>
        /// 默认的数据库连接字符串
        /// </summary>
        public static string _defultConnectionString;
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
