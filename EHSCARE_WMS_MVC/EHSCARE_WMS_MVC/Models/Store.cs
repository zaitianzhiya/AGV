using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHSCARE_WMS.Models
{
    public class Store
    {
        public string STORE_NO { get; set; }
        public string STORE_NAME { get; set; }
        public DateTime CREATE_TIME { get; set; }
        public string CREATE_USERID { get; set; }
        public DateTime UPDATE_TIME { get; set; }
        public string UPDATE_USERID { get; set; }
    }
}