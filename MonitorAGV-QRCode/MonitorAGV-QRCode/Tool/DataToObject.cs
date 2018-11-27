using Model.Comoon;
//using SocketClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Tools
{
	public class DataToObject
	{
		public static DataBaseInfo DBase
		{
			get;
			set;
		}

        //public static ClientConfig serverconfig
        //{
        //    get;
        //    set;
        //}

        //private static DataTable GetDataTableStruct()
        //{
        //    DataTable result;
        //    try
        //    {
        //        DataTable dataTable = new DataTable();
        //        DataColumn column = new DataColumn("DBIP", typeof(string));
        //        dataTable.Columns.Add(column);
        //        DataColumn column2 = new DataColumn("DBName", typeof(string));
        //        dataTable.Columns.Add(column2);
        //        DataColumn column3 = new DataColumn("DBUser", typeof(string));
        //        dataTable.Columns.Add(column3);
        //        DataColumn column4 = new DataColumn("DBPass", typeof(string));
        //        dataTable.Columns.Add(column4);
        //        DataColumn column5 = new DataColumn("ServerIP", typeof(string));
        //        dataTable.Columns.Add(column5);
        //        DataColumn column6 = new DataColumn("ServerPort", typeof(string));
        //        dataTable.Columns.Add(column6);
        //        result = dataTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        //private static DataTable TxtToDT()
        //{
        //    DataTable dataTable = null;
        //    string path = Application.StartupPath + @"\AGV_Set.ini";
        //    if (File.Exists(path))
        //    {
        //        string dataBase = FileControl.SetFileControl.ReadIniValue("DBSETUP", "DATABASE", path);
        //        string server = FileControl.SetFileControl.ReadIniValue("DBSETUP", "SERVER", path);
        //        string uid = FileControl.SetFileControl.ReadIniValue("DBSETUP", "UID", path);
        //        string pwd = FileControl.SetFileControl.ReadIniValue("DBSETUP", "PWD", path);
        //        string ip = FileControl.SetFileControl.ReadIniValue("HOST", "HOSTIP", path);
        //        string port = FileControl.SetFileControl.ReadIniValue("HOST", "HOSTPORT", path);

        //        bool flag2 = !string.IsNullOrEmpty(dataBase);
        //        if (flag2)
        //        {
        //            dataTable = GetDataTableStruct();
        //            DataRow row = dataTable.NewRow();
        //            row["DBIP"] = server;
        //            row["DBName"] = dataBase;
        //            row["DBUser"] = uid;
        //            row["DBPass"] = pwd;
        //            row["ServerIP"] = ip;
        //            row["ServerPort"] = port;
        //            dataTable.Rows.Add(row);
        //        }
        //    }

        //    return dataTable;
        //}

        //public static void setDBase()
        //{
        //    try
        //    {
        //        DataTable dataTable = DataToObject.TxtToDT();
        //        bool flag = dataTable != null && DataToObject.TxtToDT().Rows.Count > 0;
        //        if (flag)
        //        {
        //            DataToObject.DBase = DataToObject.CreateDeepCopy<DataBaseInfo>(new DataBaseInfo
        //            {
        //                DataBaseName = dataTable.Rows[0]["DBName"].ToString(),
        //                DbSource = dataTable.Rows[0]["DBIP"].ToString(),
        //                Pwd = dataTable.Rows[0]["DBPass"].ToString(),
        //                Uid = dataTable.Rows[0]["DBUser"].ToString()
        //            });
        //            DataToObject.serverconfig = new ClientConfig
        //            {
        //                ServerIP = dataTable.Rows[0]["ServerIP"].ToString(),
        //                Port = Convert.ToInt32(dataTable.Rows[0]["ServerPort"].ToString()),
        //                TimeOut = 6,
        //                ReceiveBufferSize = 128
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

		public static IList<T> TableToEntity<T>(DataTable dt) where T : class, new()
		{
			Type typeFromHandle = typeof(T);
			List<T> list = new List<T>();
			foreach (DataRow dataRow in dt.Rows)
			{
				PropertyInfo[] properties = typeFromHandle.GetProperties();
				T t = Activator.CreateInstance<T>();
				PropertyInfo[] array = properties;
				for (int i = 0; i < array.Length; i++)
				{
					PropertyInfo propertyInfo = array[i];
					bool flag = !dataRow.Table.Columns.Contains(propertyInfo.Name);
					if (!flag)
					{
						object obj = dataRow[propertyInfo.Name];
						bool flag2 = propertyInfo.PropertyType == typeof(string);
						if (flag2)
						{
							bool flag3 = obj != null;
							if (flag3)
							{
								string text = obj.ToString();
								text = text.Trim();
								obj = text;
							}
							else
							{
								obj = string.Empty;
							}
						}
						else
						{
							bool flag4 = propertyInfo.PropertyType == typeof(bool);
							if (flag4)
							{
								int num = Convert.ToInt32(obj);
								obj = (num != 0);
							}
							else
							{
								bool flag5 = propertyInfo.PropertyType == typeof(double);
								if (flag5)
								{
									double num2 = Convert.ToDouble(obj);
									obj = num2;
								}
								else
								{
									bool flag6 = propertyInfo.PropertyType == typeof(int);
									if (flag6)
									{
										int num3 = Convert.ToInt32(obj);
										obj = num3;
									}
									else
									{
										bool flag7 = propertyInfo.PropertyType == typeof(long);
										if (flag7)
										{
											long num4 = Convert.ToInt64(obj);
											obj = num4;
										}
										else
										{
											bool flag8 = dataRow[propertyInfo.Name] is long;
											if (flag8)
											{
												propertyInfo.SetValue(t, Convert.ToInt32(dataRow[propertyInfo.Name]), null);
												goto IL_207;
											}
										}
									}
								}
							}
						}
						propertyInfo.SetValue(t, obj, null);
					}
					IL_207:;
				}
				list.Add(t);
			}
			return list;
		}

		public static T CreateDeepCopy<T>(T obj)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			memoryStream.Position = 0L;
			return (T)((object)binaryFormatter.Deserialize(memoryStream));
		}
	}
}
