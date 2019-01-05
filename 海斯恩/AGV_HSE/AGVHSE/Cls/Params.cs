using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS_RGB
{
    class Params
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        public static string userName = "";
        public static string passWord = "";
        public static string userName_now = "";
        public static string passWord_now = "";

        public static string root_name = "";
        public static string root_pwd = "";

        public static string secretKey = "12345678";
        //===========================================================


        /// <summary>
        /// 充电日志开关
        /// </summary>
        public static bool powerLog = false;//默认开启
        /// <summary>
        /// RFID读取日志开关
        /// </summary>
        public static bool rfidLog = true;
        /// <summary>
        /// 数据接收日志开关
        /// </summary>
        public static bool recLog = true;
        /// <summary>
        /// 数据发送日志开关
        /// </summary>
        public static bool sendLog = true;
        /// <summary>
        /// 故障日志开关
        /// </summary>
        public static bool errLog = true;
        /// <summary>
        /// 电压日志开关
        /// </summary>
        public static bool volLog = false;
        /// <summary>
        /// RFID日志记录标志
        /// </summary>
        public static string[] Log_Count_forRFID;
        /// <summary>
        /// 故障日志记录标志
        /// </summary>
        public static string[] Log_Count_forErr;
        //===========================================================


        /// <summary>
        /// 系统功能状态
        /// </summary>
        public static bool SYS_STSATE = true;

    }
}
