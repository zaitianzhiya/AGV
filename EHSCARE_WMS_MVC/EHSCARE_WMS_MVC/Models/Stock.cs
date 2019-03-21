using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHSCARE_WMS.Models
{
    public class Stock
    {
        public string BOX_NO { get; set; }
        public string BOX_BARCODE { get; set; }
        public string STORE_NO { get; set; }
        public string STORE_NAME { get; set; }
        public string WH_NO { get; set; }
        public string WH_NAME { get; set; }
        public int WH_IS_USED { get; set; }
        public string WH_IS_USED_NAME { get; set; }
        public string FORBBIDEN_REASON { get; set; }
        public DateTime CREATE_TIME { get; set; }
        public string CREATE_USERID { get; set; }
        public string CREATE_USERNAME { get; set; }
        public DateTime UPDATE_TIME { get; set; }
        public string UPDATE_USERID { get; set; }
        public string UPDATE_USERNAME { get; set; }
        public int DEMOSUM { get; set; }
    }
}
