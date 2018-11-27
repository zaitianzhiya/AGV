using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Tools
{
	public class OperatIniFile
	{
		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

		public static void WriteIni(string Assesion, string Key, string Context, string FilePath)
		{
			try
			{
				OperatIniFile.WritePrivateProfileString(Assesion, Key, Context, FilePath);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static string ReadIni(string Assesion, string Key, string FilePath)
		{
			string result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				OperatIniFile.GetPrivateProfileString(Assesion, Key, "", stringBuilder, 255, FilePath);
				result = stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
	}
}
