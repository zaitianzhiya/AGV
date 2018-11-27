using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SimulationModel
{
	public class LogHelper
	{
		private static object lockerrobj;

		private static object lockobj;

		static LogHelper()
		{
			LogHelper.lockerrobj = new object();
			LogHelper.lockobj = new object();
		}

		public LogHelper()
		{
		}

		public static void DeleteLogFile()
		{
			try
			{
				string str = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\");
				FileInfo[] files = (new DirectoryInfo(str)).GetFiles();
				for (int i = 0; i < (int)files.Length; i++)
				{
					FileInfo fileInfo = files[i];
					if ((fileInfo.CreationTime >= DateTime.Today.AddDays(-5) ? false : fileInfo.Name.ToLower() != "1.txt"))
					{
						try
						{
							fileInfo.Delete();
						}
						catch (Exception exception)
						{
						}
					}
				}
			}
			catch (Exception exception1)
			{
			}
		}

		public static async void WriteAGVChargeLog(string msg)
		{
			await Task.Factory.StartNew(() => {
				lock (LogHelper.lockobj)
				{
					try
					{
						if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\1.txt")))
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							DateTime now = DateTime.Now;
							string str = string.Concat(baseDirectory, "\\CoreLog\\ChargeLog", now.ToString("yyyyMMddHH"), ".txt");
							now = DateTime.Now;
							File.AppendAllText(str, string.Concat(now.ToString("yyyy-MM-dd  HH:mm:ss"), " ", msg, Environment.NewLine));
						}
					}
					catch (Exception exception)
					{
					}
				}
			});
		}

		public static async void WriteAGVWarnMessLog(string msg)
		{
			await Task.Factory.StartNew(() => {
				lock (LogHelper.lockobj)
				{
					try
					{
						if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\1.txt")))
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							DateTime now = DateTime.Now;
							string str = string.Concat(baseDirectory, "\\CoreLog\\AGVWarnMessLog", now.ToString("yyyyMMddHH"), ".txt");
							now = DateTime.Now;
							File.AppendAllText(str, string.Concat(now.ToString("yyyy-MM-dd  HH:mm:ss"), " ", msg, Environment.NewLine));
						}
					}
					catch (Exception exception)
					{
					}
				}
			});
		}

		public static async void WriteCallBoxLog(string msg)
		{
			await Task.Factory.StartNew(() => {
				lock (LogHelper.lockobj)
				{
					try
					{
						if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\1.txt")))
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							DateTime now = DateTime.Now;
							string str = string.Concat(baseDirectory, "\\CoreLog\\CallBoxOperLog", now.ToString("yyyyMMddHH"), ".txt");
							now = DateTime.Now;
							File.AppendAllText(str, string.Concat(now.ToString("yyyy-MM-dd  HH:mm:ss"), ": ", msg, Environment.NewLine));
						}
					}
					catch (Exception exception)
					{
					}
				}
			});
		}

		public static async void WriteCreatTaskLog(string msg)
		{
			await Task.Factory.StartNew(() => {
				lock (LogHelper.lockobj)
				{
					try
					{
						if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\1.txt")))
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							DateTime now = DateTime.Now;
							string str = string.Concat(baseDirectory, "\\CoreLog\\TaskLog", now.ToString("yyyyMMddHH"), ".txt");
							now = DateTime.Now;
							File.AppendAllText(str, string.Concat(now.ToString("yyyy-MM-dd  HH:mm:ss"), ": ", msg, Environment.NewLine));
						}
					}
					catch (Exception exception)
					{
					}
				}
			});
		}

		public static void WriteErrorLog(Exception ex)
		{
			lock (LogHelper.lockerrobj)
			{
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				DateTime now = DateTime.Now;
				string str = string.Concat(baseDirectory, "\\ErrorLog\\ErrorLog", now.ToString("yyyyMMdd"), ".txt");
				string[] message = new string[5];
				now = DateTime.Now;
				message[0] = now.ToString("yyyy-MM-dd  HH:mm:ss");
				message[1] = " ";
				message[2] = ex.Message;
				message[3] = ex.StackTrace;
				message[4] = Environment.NewLine;
				File.AppendAllText(str, string.Concat(message));
			}
		}

		public static async void WriteLog(string msg)
		{
			await Task.Factory.StartNew(() => {
				lock (LogHelper.lockobj)
				{
					try
					{
						if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\1.txt")))
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							DateTime now = DateTime.Now;
							string str = string.Concat(baseDirectory, "\\CoreLog\\Log", now.ToString("yyyyMMddHH"), ".txt");
							now = DateTime.Now;
							File.AppendAllText(str, string.Concat(now.ToString("yyyy-MM-dd  HH:mm:ss"), " ", msg, Environment.NewLine));
						}
					}
					catch (Exception exception)
					{
					}
				}
			});
		}

		public static async void WriteOffLineLog(string msg)
		{
			await Task.Factory.StartNew(() => {
				lock (LogHelper.lockobj)
				{
					try
					{
						if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\1.txt")))
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							DateTime now = DateTime.Now;
							string str = string.Concat(baseDirectory, "\\CoreLog\\OffLineLog", now.ToString("yyyyMMddHH"), ".txt");
							now = DateTime.Now;
							File.AppendAllText(str, string.Concat(now.ToString("yyyy-MM-dd  HH:mm:ss"), " ", msg, Environment.NewLine));
						}
					}
					catch (Exception exception)
					{
					}
				}
			});
		}

		public static async void WriteReciveAGVMessLog(string msg)
		{
			await Task.Factory.StartNew(() => {
				lock (LogHelper.lockobj)
				{
					try
					{
						if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\1.txt")))
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							DateTime now = DateTime.Now;
							string str = string.Concat(baseDirectory, "\\CoreLog\\ReciveAGVMess", now.ToString("yyyyMMddHH"), ".txt");
							now = DateTime.Now;
							File.AppendAllText(str, string.Concat(now.ToString("yyyy-MM-dd  HH:mm:ss"), " ", msg, Environment.NewLine));
						}
					}
					catch (Exception exception)
					{
					}
				}
			});
		}

		public static async void WriteReciveChargeMessLog(string msg)
		{
			await Task.Factory.StartNew(() => {
				lock (LogHelper.lockobj)
				{
					try
					{
						if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\1.txt")))
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							DateTime now = DateTime.Now;
							string str = string.Concat(baseDirectory, "\\CoreLog\\ReciveChargeMess", now.ToString("yyyyMMddHH"), ".txt");
							now = DateTime.Now;
							File.AppendAllText(str, string.Concat(now.ToString("yyyy-MM-dd  HH:mm:ss"), " ", msg, Environment.NewLine));
						}
					}
					catch (Exception exception)
					{
					}
				}
			});
		}

		public static async void WriteSendAGVMessLog(string msg)
		{
			await Task.Factory.StartNew(() => {
				lock (LogHelper.lockobj)
				{
					try
					{
						if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\1.txt")))
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							DateTime now = DateTime.Now;
							string str = string.Concat(baseDirectory, "\\CoreLog\\SendAGVMess", now.ToString("yyyyMMddHH"), ".txt");
							now = DateTime.Now;
							File.AppendAllText(str, string.Concat(now.ToString("yyyy-MM-dd  HH:mm:ss"), " ", msg, Environment.NewLine));
						}
					}
					catch (Exception exception)
					{
					}
				}
			});
		}

		public static async void WriteSendChargeMessLog(string msg)
		{
			await Task.Factory.StartNew(() => {
				lock (LogHelper.lockobj)
				{
					try
					{
						if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\1.txt")))
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							DateTime now = DateTime.Now;
							string str = string.Concat(baseDirectory, "\\CoreLog\\SendChargeMess", now.ToString("yyyyMMddHH"), ".txt");
							now = DateTime.Now;
							File.AppendAllText(str, string.Concat(now.ToString("yyyy-MM-dd  HH:mm:ss"), " ", msg, Environment.NewLine));
						}
					}
					catch (Exception exception)
					{
					}
				}
			});
		}

		public static async void WriteTrafficLog(string msg)
		{
			await Task.Factory.StartNew(() => {
				lock (LogHelper.lockobj)
				{
					try
					{
						if (File.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\CoreLog\\1.txt")))
						{
							string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
							DateTime now = DateTime.Now;
							string str = string.Concat(baseDirectory, "\\CoreLog\\Traffic", now.ToString("yyyyMMddHH"), ".txt");
							now = DateTime.Now;
							File.AppendAllText(str, string.Concat(now.ToString("yyyy-MM-dd  HH:mm:ss"), " ", msg, Environment.NewLine));
						}
					}
					catch (Exception exception)
					{
					}
				}
			});
		}
	}
}