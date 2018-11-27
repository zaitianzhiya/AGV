using System;

namespace Model.Comoon
{
	[Serializable]
	public class DataBaseInfo
	{
		private string dbSource;

		private string uid;

		private string pwd;

		private string dataBaseName;

		private int dbType;

		private bool isNew;

		private string defaultConnDbName;

		private string dbTypeName;

		private double version = 5.0;

		public string DbSource
		{
			get
			{
				return this.dbSource;
			}
			set
			{
				this.dbSource = value;
			}
		}

		public string Uid
		{
			get
			{
				return this.uid;
			}
			set
			{
				this.uid = value;
			}
		}

		public string Pwd
		{
			get
			{
				return this.pwd;
			}
			set
			{
				this.pwd = value;
			}
		}

		public string DataBaseName
		{
			get
			{
				return this.dataBaseName;
			}
			set
			{
				this.dataBaseName = value;
			}
		}

		public string Port
		{
			get
			{
				return this.dbSource.Substring(this.dbSource.IndexOf(':') + 1);
			}
		}

		public int DbType
		{
			get
			{
				return this.dbType;
			}
			set
			{
				this.dbType = value;
			}
		}

		public bool IsNew
		{
			get
			{
				return this.isNew;
			}
			set
			{
				this.isNew = value;
			}
		}

		public string DefaultConnDbName
		{
			get
			{
				bool flag = this.DbType == 0;
				string result;
				if (flag)
				{
					result = "master";
				}
				else
				{
					result = this.DataBaseName;
				}
				return result;
			}
			set
			{
				this.defaultConnDbName = value;
			}
		}

		public string DbTypeName
		{
			get
			{
				return this.dbTypeName;
			}
			set
			{
				this.dbTypeName = value;
				bool flag = this.dbTypeName.ToUpper().Contains("SQLSERVER");
				if (flag)
				{
					this.DbType = 0;
				}
				else
				{
					bool flag2 = this.dbTypeName.ToUpper().Contains("ORACLE");
					if (flag2)
					{
						this.DbType = 1;
					}
					else
					{
						this.DbType = 0;
					}
				}
			}
		}

		public double Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		public DataBaseInfo()
		{
		}

		public DataBaseInfo(string serverName, string userName, string passWord, string dbName)
		{
			this.dbSource = serverName;
			this.uid = userName;
			this.pwd = passWord;
			this.dataBaseName = dbName;
		}

		public DataBaseInfo(string serverName, string userName, string passWord, string dbName, int dbType)
		{
			this.dbSource = serverName;
			this.uid = userName;
			this.pwd = passWord;
			this.dataBaseName = dbName;
			this.dbType = dbType;
		}

		public DataBaseInfo(DataBaseInfo dataBaseInfo, bool isNew)
		{
			this.dbSource = dataBaseInfo.DbSource;
			this.uid = dataBaseInfo.Uid;
			this.pwd = dataBaseInfo.Pwd;
			this.dataBaseName = dataBaseInfo.DataBaseName;
			this.dbType = dataBaseInfo.DbType;
			this.isNew = isNew;
		}

		public DataBaseInfo(DataBaseInfo dataBaseInfo)
		{
			this.dbSource = dataBaseInfo.DbSource;
			this.uid = dataBaseInfo.Uid;
			this.pwd = dataBaseInfo.Pwd;
			this.dataBaseName = dataBaseInfo.DataBaseName;
			this.isNew = dataBaseInfo.IsNew;
			this.DbType = dataBaseInfo.DbType;
		}
	}
}
