using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHSCARE_WMS.Models
{
    public class Box
    {
        public string BOX_BARCODE { get; set; }
        public DateTime CREATE_TIME { get; set; }
        public string CREATE_USERID { get; set; }
        public DateTime UPDATE_TIME { get; set; }
        public string UPDATE_USERID { get; set; }
        public string REMARK { get; set; }
        public string BOX_NO { get; set; }
    }
}