using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AGVMAPWPF
{
    public class Member
    {
        public string uid { get; set; }
        public string property { get; set; }
        public string val { get; set; }
        public Visibility vistxt { get; set; }
        public Visibility visbtn { get; set; }
        public Visibility viscolor { get; set; }

        public Member(string uid, string property, string val, Visibility vistxt, Visibility visbtn,Visibility viscolor)
        {
            this.uid = uid;
            this.property = property;
            this.val = val;
            this.vistxt = vistxt;
            this.visbtn = visbtn;
            this.viscolor = viscolor;
        }
    }
}
