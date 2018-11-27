using System;
using System.Security.Cryptography;
using System.Text;

namespace Tools
{
	public class MD5Encrypt
	{
		public static string MD5Encrypt64(string txt)
		{
			MD5 mD = MD5.Create();
			byte[] inArray = mD.ComputeHash(Encoding.UTF8.GetBytes(txt));
			return Convert.ToBase64String(inArray);
		}
	}
}
