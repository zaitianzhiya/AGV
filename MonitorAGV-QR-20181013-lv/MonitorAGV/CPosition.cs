using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class CPosition
    {
        //public int[] posFlag;
        public List<int> posFlag = new List<int>();
        public int forbiddenCount;
        public int chargeCount;
        public int shellCount;
        public bool isLocked;

        public CPosition()
        {
        }
        public CPosition(int a, int b, int c, int d, int e, int f, bool g)
        {                        
            posFlag.Add(a);
            posFlag.Add(b);
            posFlag.Add(c);
            forbiddenCount = d;
            chargeCount = e;
            shellCount = f;        
            isLocked = g;
        }
    }
}
