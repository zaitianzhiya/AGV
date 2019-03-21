using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHSCARE_WMS.Models
{
    public class Boxs
    {
        public string BARCODE { get; set; }
        public string BOX_BARCODE { get; set; }
        public DateTime CREATE_TIME { get; set; }
        public string CREATE_USERID { get; set; }
        public DateTime UPDATE_TIME { get; set; }
        public string UPDATE_USERID { get; set; }
        public string REMARK { get; set; }
        public string DEMO_NO { get; set; }
        public DateTime CHECK_DATE { get; set; }
        public DateTime SAVE_DATE { get; set; }
        public string CHECK_DATE_STR { get; set; }
        public string SAVE_DATE_STR { get; set; }
        public int STATE { get; set; }
        public string STATENAME { get; set; }
    }
}