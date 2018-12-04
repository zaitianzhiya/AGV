using Model.Comoon;
using SocketClient;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Tools
{
    public class ApplicationVar
    {
        public static DataBaseInfo DBase
        {
            get;
            set;
        }

        public static ClientConfig serverconfig
        {
            get;
            set;
        }

        public static void setDBase()
        {
            try
            {
                DataTable dataTable = ApplicationVar.TxtToDT();
                bool flag = dataTable != null && dataTable.Rows.Count > 0;
                if (flag)
                {
                    ApplicationVar.DBase = DataToObject.CreateDeepCopy<DataBaseInfo>(new DataBaseInfo
                    {
                        DataBaseName = dataTable.Rows[0]["DBName"].ToString(),
                        DbSource = dataTable.Rows[0]["DBIP"].ToString(),
                        Pwd = dataTable.Rows[0]["DBPass"].ToString(),
                        Uid = dataTable.Rows[0]["DBUser"].ToString()
                    });
                    ApplicationVar.serverconfig = new ClientConfig
                    {
                        ServerIP = dataTable.Rows[0]["ServerIP"].ToString(),
                        Port = Convert.ToInt32(dataTable.Rows[0]["ServerPort"].ToString()),
                        TimeOut = 10,
                        ReceiveBufferSize = 128
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable TxtToDT()
        {
            DataTable dataTable = null;
            string path = Application.StartupPath + @"\AGV_Set.ini";
            if (File.Exists(path))
            {
                string dataBase = FileControl.SetFileControl.ReadIniValue("DBSETUP", "DATABASE", path);
                string server = FileControl.SetFileControl.ReadIniValue("DBSETUP", "SERVER", path);
                string uid = FileControl.SetFileControl.ReadIniValue("DBSETUP", "UID", path);
                string pwd = FileControl.SetFileControl.ReadIniValue("DBSETUP", "PWD", path);
                string ip = FileControl.SetFileControl.ReadIniValue("HOST", "HOSTIP", path);
                string port = FileControl.SetFileControl.ReadIniValue("HOST", "HOSTPORT", path);

                bool flag2 = !string.IsNullOrEmpty(dataBase);
                if (flag2)
                {
                    dataTable = GetDataTableStruct();
                    DataRow row = dataTable.NewRow();
                    row["DBIP"] = server;
                    row["DBName"] = dataBase;
                    row["DBUser"] = uid;
                    row["DBPass"] = pwd;
                    row["ServerIP"] = ip;
                    row["ServerPort"] = port;
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        public static DataTable GetDataTableStruct()
        {
            DataTable result;
            try
            {
                DataTable dataTable = new DataTable();
                DataColumn column = new DataColumn("DBIP", typeof(string));
                dataTable.Columns.Add(column);
                DataColumn column2 = new DataColumn("DBName", typeof(string));
                dataTable.Columns.Add(column2);
                DataColumn column3 = new DataColumn("DBUser", typeof(string));
                dataTable.Columns.Add(column3);
                DataColumn column4 = new DataColumn("DBPass", typeof(string));
                dataTable.Columns.Add(column4);
                DataColumn column5 = new DataColumn("ServerIP", typeof(string));
                dataTable.Columns.Add(column5);
                DataColumn column6 = new DataColumn("ServerPort", typeof(string));
                dataTable.Columns.Add(column6);
                result = dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
