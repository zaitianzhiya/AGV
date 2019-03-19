using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHSCARE_WMS.Models
{
    public class DataModel<T>
    {
        public IEnumerable<T>  Products { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalNum { get; set; }
    }
}