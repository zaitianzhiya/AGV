using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FileControl
{
    public class LogFile
    {
        public static void SaveLog(string fileMsg) 
        {
            try
            {
                using (FileStream _fStream = new FileStream(GetFilePath(), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(GetCurrentTimeString() + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception)
            {
                SaveLog(fileMsg);
            }
        }

        public static void SaveLog1(string fileMsg)
        {
            try
            {
                using (FileStream _fStream = new FileStream(GetFilePath1(), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(GetCurrentTimeString() + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception)
            {
                SaveLog1(fileMsg);
            }
        }

        public static void SaveLog2(string fileMsg)
        {
            try
            {
                using (FileStream _fStream = new FileStream(GetFilePath2(), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(GetCurrentTimeString() + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception)
            {
                SaveLog2(fileMsg);
            }
        }
        private static string GetCurrentTimeString()
        {
            return DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString("000") + "     ";
        }

        private static string GetFilePath()
        {
            string path = Application.StartupPath + @"\SystemLog";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + @"\" + DateTime.Now.ToString("yyyy_MM_dd") + "_.log";
            //return Application.StartupPath + @"\SystemLog\" + DateTime.Now.ToString() + "_.log";
        }

        private static string GetFilePath1()
        {
            string path = Application.StartupPath + @"\RGVrevDATA";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + @"\" + DateTime.Now.ToString("yyyy_MM_dd") + "_RGVrev.log";
            //return Application.StartupPath + @"\SystemLog\" + DateTime.Now.ToString() + "_.log";
        }

        private static string GetFilePath2()
        {
            string path = Application.StartupPath + @"\RGVsendDATA";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + @"\" + DateTime.Now.ToString("yyyy_MM_dd") + "_RGVsend.log";
            //return Application.StartupPath + @"\SystemLog\" + DateTime.Now.ToString() + "_.log";
        }

        #region Log_Rec
        public static void SaveLog_Rec(string fileMsg, string ip)
        {
            try
            {
                using (FileStream _fStream = new FileStream(GetFilePath_Rec(ip), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(GetCurrentTimeString() + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception)
            {
                SaveLog_Rec(fileMsg, ip);
            }
        }

        private static string GetFilePath_Rec(string ip)
        {
            string path = Application.StartupPath + @"\RGVrevDATA" + @"\" + DateTime.Now.ToString("yyyy_MM_dd");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + @"\" + ip + "_rev.log";
        }
        #endregion

        #region Log_Send

        public static void SaveLog_Send(string fileMsg, string ip)
        {
            try
            {
                using (FileStream _fStream = new FileStream(GetFilePath_Send(ip), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(GetCurrentTimeString() + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception)
            {
                SaveLog_Send(fileMsg, ip);
            }
        }
        private static string GetFilePath_Send(string ip)
        {
            string path = Application.StartupPath + @"\RGVsendDATA" + @"\" + DateTime.Now.ToString("yyyy_MM_dd");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + @"\" + ip + "_send.log";
        }
        #endregion

        #region power_log
        public static void SaveLog_Power(string fileMsg, string ip)
        {
            try
            {
                using (FileStream _fStream = new FileStream(GetFilePath_Power(ip), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(GetCurrentTimeString() + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception)
            {
                SaveLog_Power(fileMsg, ip);
            }
        }
        private static string GetFilePath_Power(string ip)
        {
            string path = Application.StartupPath + @"\RGVpowerDATA" + @"\" + DateTime.Now.ToString("yyyy_MM_dd");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + @"\" + ip + "_power.log";
        }
        #endregion

        #region err Log
        public static void SaveLog_Err(string fileMsg, string ip)
        {
            try
            {
                using (FileStream _fStream = new FileStream(GetFilePath_err(ip), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(GetCurrentTimeString() + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception)
            {
                SaveLog_Err(fileMsg, ip);
            }
        }
        private static string GetFilePath_err(string ip)
        {
            string path = Application.StartupPath + @"\RGVerrorDATA" + @"\" + DateTime.Now.ToString("yyyy_MM_dd");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + @"\" + ip + "_error.log";
        }
        #endregion

        #region rfid_Log
        public static void SaveLog_RFID(string fileMsg, string ip)
        {
            try
            {
                using (FileStream _fStream = new FileStream(GetFilePath_rfid(ip), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(GetCurrentTimeString() + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception)
            {
                SaveLog_RFID(fileMsg, ip);
            }
        }
        private static string GetFilePath_rfid(string ip)
        {
            string path = Application.StartupPath + @"\RGVrfidDATA" + @"\" + DateTime.Now.ToString("yyyy_MM_dd");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + @"\" + ip + "_rfid.log";
        }
        #endregion

        #region vol_log
        public static void SaveLog_Vol(string fileMsg, string ip)
        {
            try
            {
                using (FileStream _fStream = new FileStream(GetFilePath_Vol(ip), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(GetCurrentTimeString() + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception)
            {
                SaveLog_Vol(fileMsg, ip);
            }
        }
        private static string GetFilePath_Vol(string ip)
        {
            string path = Application.StartupPath + @"\RGVvolDATA" + @"\" + DateTime.Now.ToString("yyyy_MM_dd");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + @"\" + ip + "_vol.log";
        }
        #endregion

        #region sysStart_log
        public static void SaveLog_Start(string fileMsg)
        {
            try
            {
                using (FileStream _fStream = new FileStream(GetFilePath_Start(), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(GetCurrentTimeString() + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception)
            {
                SaveLog_Start(fileMsg);
            }
        }
        private static string GetFilePath_Start()
        {
            string path = Application.StartupPath + @"\SystemLog";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path + @"\" + DateTime.Now.ToString("yyyy_MM_dd") + ".log";
        }
        #endregion
    }
}
