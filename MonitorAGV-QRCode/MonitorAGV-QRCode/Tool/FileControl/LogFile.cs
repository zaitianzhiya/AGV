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
    }
}
