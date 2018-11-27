using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MonitorAGV_QRCode
{
    public class SetFileControl
    {
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 读取INI文档
        /// </summary>
        /// <param name="Section">区段名</param>
        /// <param name="Key">键</param>
        /// <param name="file">文件路径</param>
        /// <returns>Value</returns>
        public static string ReadIniValue(string Section, string Key, string file)
        {
            StringBuilder _temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, Key, "", _temp, 1024, file);
            return _temp.ToString();
        }
        /// <summary>
        /// 写入INI文档
        /// </summary>
        /// <param name="Section">区段名</param>
        /// <param name="Key">键</param>
        /// <param name="value">Value</param>
        /// <param name="file">文件路劲</param>
        public static long WriteIniValue(string Section, string Key, string value, string file)
        {
            long rtu = WritePrivateProfileString(Section, Key, value, file);
            return rtu;
        }

    }
}
