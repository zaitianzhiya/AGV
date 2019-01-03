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

        //===========================================================
        /// <summary>
        /// 充电日志开关
        /// </summary>
        public static bool powerLog = false;//默认开启
        /// <summary>
        /// RFID读取日志开关
        /// </summary>
        public static bool rfidLog = false;
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
        public static bool errLog = false;
        /// <summary>
        /// 电压日志开关
        /// </summary>
        public static bool volLog = false;
        //===========================================================
        

        /// <summary>
        /// 缓存区计数标志
        /// </summary>
        public static int addCount50 = 0;
        public static int addCount52 = 0;

        /// <summary>
        /// AGV总数量(打开软件时的数量)
        /// </summary>
        public static int AGV_COUNT = 0;
        /// <summary>
        /// AGV在线数量
        /// </summary>
        public static int AGV_COUNT_AC = 0;
        /// <summary>
        /// AGV总数量(实时数量)
        /// </summary>
        public static int AGV_COUNT_NOW = 0;
        //===========================================================        
        /// <summary>
        /// RFID日志记录标志
        /// </summary>
        public static string[] Log_Count_forRFID;

        /// <summary>
        /// 故障日志记录标志
        /// </summary>
        public static string[] Log_Count_forErr;
        //===========================================================


    }
}
