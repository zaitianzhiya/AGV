using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHSCARE_WMS.Models
{
    public class User
    {
        public string USER_NO { get; set; }
        public string USER_NAME { get; set; }
        public string USER_PWD { get; set; }
        public int USER_GROUPID { get; set; }
        public int USER_IS_USED { get; set; }
        public string GROUP_NAME { get; set; }
    }
}