using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetPath
{
    public class CPath
    {
        private int svertex, zvertex;
        private int len;
        private int wan;
        private int wan2;
        public List<int> m_path = new List<int>();
        public List<int> subm_path = new List<int>();
        public List<int> agvangle = new List<int>();
        int Gx = 0;

        public CPath(int Ps)
        {
            Gx = Ps;
        }       
        public void setpath(List<int> tempath)
        {
            m_path = tempath;
            m_path.Reverse();           
            svertex = m_path[0];
            zvertex = m_path[m_path.Count() - 1];
            len = m_path.Count() - 1;
        }
        public void filterpath()
        {
            subm_path.Add(svertex);
            int size = m_path.Count();
            for (int k = 1; k < size - 1; k++)
            {
                
                if (System.Math.Abs(m_path[k] % Gx - m_path[k - 1] % Gx) + System.Math.Abs(m_path[k + 1] % Gx - m_path[k] % Gx) == 1)
                {
                    subm_path.Add(m_path[k]);
                    if ((m_path[k] % Gx - m_path[k - 1] % Gx) < 0)
                        agvangle.Add(180);
                    if ((m_path[k] % Gx - m_path[k - 1] % Gx) > 0)
                        agvangle.Add(0);
                    if ((m_path[k] / Gx - m_path[k - 1] / Gx) < 0)
                        agvangle.Add(270);
                    if ((m_path[k] / Gx - m_path[k - 1] / Gx) > 0)
                        agvangle.Add(90);
                }                
            }
            subm_path.Add(m_path[size - 1]);
            if ((m_path[size-1] % Gx - m_path[size-2] % Gx) < 0)
                agvangle.Add(180);
            if ((m_path[size - 1] % Gx - m_path[size - 2] % Gx) > 0)
                agvangle.Add(0);
            if ((m_path[size - 1] / Gx - m_path[size - 2] / Gx) < 0)
                agvangle.Add(270);
            if ((m_path[size - 1] / Gx - m_path[size - 2] / Gx) > 0)
                agvangle.Add(90);
            //agvangle.Add((m_path[size-1]%Gx - m_path[size-2]%Gx>0) ? 0:180);
            //agvangle.Add((m_path[size - 1] / Gx - m_path[size - 2] / Gx > 0 )? 90 : 270);
        }
        public void setwan(int i)
        {
            wan = i;
        }

        public int getlen()
        {
            return len;
        }
        public int getwan()
        {
            return wan;
        }
    }
}
