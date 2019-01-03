using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetPath
{
    //顶点对象代码
    public class CVertex
    {
        private int m_num;//保存与该顶点相邻的顶点个数  
        private int[] m_nei; //与该顶点相邻的顶点序号  
        private int[] m_flag; //与该顶点相邻的顶点是否访问过  
        private Boolean isin; //该顶点是否入栈          

        public CVertex()
        {          
        }

        public CVertex(int M_num)
        {
            m_num = M_num;
            m_nei = new int[m_num];
            m_flag = new int[m_num];
            isin = false;
            for (int i = 0; i < m_num; i++)
            {
                m_flag[i] = 0;
            }
        }

        public void Initialize(int num, int[] a)
        {
            m_num = num;
            for (int i = 0; i < m_num; i++)
            {
                m_nei[i] = a[i];
            }
        }

        public int getOne() //得到一个与该顶点相邻的顶点
        {
            int i = 0;
            for (i = 0; i < m_num; i++)
            {
                if (m_flag[i] == 0)   //判断是否访问过  
                {
                    m_flag[i] = 1;   //表示这个顶点已经被访问，并将其返回  
                    return m_nei[i];
                }
            }
            return -1;  //所有顶点都已访问过则返回-1
        }

        public void resetFlag() //与该顶点相邻的顶点全被标记为未访问
        {
            for (int i = 0; i < m_num; i++)
            {
                m_flag[i] = 0;
            }
        }

        public void setIsin(Boolean a) //标记该顶点是否入栈
        {
            isin = a;
        }

        public Boolean isIn()  //判断该顶点是否入栈
        {
            return isin;
        }

        public void Reset()//将isin和所有flag置0
        {
            for (int i = 0; i < m_num; i++)
            {
                m_flag[i] = 0;
            }
            isin = false;
        }
    }
}
