using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVMAPWPF
{
    public class ComboxSource
    {
        public string display { get; set; }
        public string value { get; set; }

        public ComboxSource(string display, string value)
        {
            this.display = display;
            this.value = value;
        }
    }
}
