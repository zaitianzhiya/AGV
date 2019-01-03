using System;
using System.Collections.Generic;
using System.Text;

namespace TS_RGB
{
    public class CVehicle
    {   
        /// <summary>
        /// X+Y+角度+车编号
        /// </summary>
        public int[] posangid = new int[5];
        /// <summary>
        /// 是否上锁
        /// </summary>
        public int islocked;
        /// <summary>
        /// 是否计算路径
        /// </summary>
        public int isPach;
        /// <summary>
        /// 车是否在线
        /// </summary>
        public int AGV_AC;
    }
}
