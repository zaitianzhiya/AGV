using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHSCARE_WMS.Models
{
    public class StockRecord
    {
        public int ID { get; set; }
        public string BOX_BARCODE { get; set; }
        public int ACT_TYPE { get; set; }
        public int WH_OLD { get; set; }
        public int WH_NEW { get; set; }
        public string WH_NO { get; set; }
        public int BORROW_PLACE { get; set; }
        public string WH_IN { get; set; }
        public string STORE_NAME { get; set; }
        public DateTime CREATE_TIME { get; set; }
        public string CREATE_USERID { get; set; }
        public string CREATE_USER_NAME { get; set; }
        public DateTime UPDATE_TIME { get; set; }
        public string UPDATE_USERID { get; set; }
        public string UPDATE_USERP_NAME { get; set; }
        public string REMARK { get; set; }
        public int STA { get; set; }

        public string STASTR
        {
            get
            {
                switch (STA)
                {
                    case 0:
                        return "未完成";
                    case 1:
                        return "已完成";
                    case 2:
                        return "进行中";
                    default:
                        return "";
                }
            }
            set { STASTR = value; }
        }
    }
}