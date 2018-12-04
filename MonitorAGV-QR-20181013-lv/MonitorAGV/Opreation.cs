using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public enum mIdele { 移动到目标位置, 升起料架顶杆, 降下料架顶杆, 顶盘归零 }
 
    public class Opreation
    {
        public string MissionNumber { get; set; }
        public mIdele MissionIdentity { get; set; }
        public string MissionOrder { get; set; }
        public string PositonX { get; set; }
        public string PositionY { get; set; }
        public string AGVAngle { get; set; }
        public string ShellAngle { get; set; }
        public string Obligatie { get; set; }
    }
}
