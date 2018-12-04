using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tools
{
	public class Encryption
	{
		public static string EncryPW(string Pass, string Key)
		{
			return Encryption.DesEncrypt(Pass, Key);
		}

		public static string DisEncryPW(string strPass, string Key)
		{
			return Encryption.DesDecrypt(strPass, Key);
		}

		private static string DesEncrypt(string encryptString, string key)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
			byte[] rgbIV = bytes;
			byte[] bytes2 = Encoding.UTF8.GetBytes(encryptString);
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
			cryptoStream.Write(bytes2, 0, bytes2.Length);
			cryptoStream.FlushFinalBlock();
			return Convert.ToBase64String(memoryStream.ToArray());
		}

		private static string DesDecrypt(string decryptString, string key)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
			byte[] rgbIV = bytes;
			byte[] array = Convert.FromBase64String(decryptString);
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.FlushFinalBlock();
			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}
	}
}
