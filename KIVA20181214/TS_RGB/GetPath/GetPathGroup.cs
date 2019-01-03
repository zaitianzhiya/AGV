using FileControl;
using Fuction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetPath
{
    public class GetPathGroup
    {
        int Lent = 650;
        int C_X, C_Y = 0;//TJ2
        CVertex[] vertex1;
        int[,] mygrid1;
        public GetPathGroup(int c_x, int c_y, List<string> BreakPoint)
        {
            C_X = c_x;
            C_Y = c_y;           
            int SIZE = C_X * C_Y;
            vertex1 = new CVertex[SIZE];
            for (int m = 0; m < SIZE; ++m)
            {
                vertex1[m] = new CVertex(4);
            }
            mygrid1 = new int[SIZE, 2];
            for (int i = 0; i < SIZE; i++)
            {
                mygrid1[i, 0] = i % C_X; //FQ
                mygrid1[i, 1] = i / C_X; //FQ
            }
            //反转禁用点
            //DataTable GetMap1 = Function.PR_SELECT_Map_Info();
            //if (GetMap1 != null && GetMap1.Rows.Count == SIZE)
            //{
            //    for (int i = 0; i < SIZE; i++)
            //    {
            //        if (GetMap1.Rows[i][4].ToString().Trim() != "0")
            //        {
            //            Function.PR_UPDATE_Map_Info_UN_used(i + 1);
            //        }
            //    }
            //}
            //DataTable GetMap = null;
            DataTable GetMap = Function.PR_SELECT_Map_Info();
            if (GetMap != null && GetMap.Rows.Count == SIZE)
            {
                for (int i = 0; i < SIZE; i++)
                {
                    int[] a = new int[4] { 0, 0, 0, 0 }; //FQ SIZE4
                    List<int> A_1 = new List<int>();
                    int num = 0;
                    if (GetMap.Rows[i][4].ToString().Trim() == "0")
                    {
                        if (int.Parse(GetMap.Rows[i][6].ToString().Trim()) > 0)
                        {
                            A_1.Add(int.Parse(GetMap.Rows[i][6].ToString().Trim()) - 1);
                            num++;
                        }
                        if (int.Parse(GetMap.Rows[i][7].ToString().Trim()) > 0)
                        {
                            A_1.Add(int.Parse(GetMap.Rows[i][7].ToString().Trim()) - 1);
                            num++;
                        }
                        if (int.Parse(GetMap.Rows[i][8].ToString().Trim()) > 0)
                        {
                            A_1.Add(int.Parse(GetMap.Rows[i][8].ToString().Trim()) - 1);
                            num++;
                        }
                        if (int.Parse(GetMap.Rows[i][9].ToString().Trim()) > 0)
                        {
                            A_1.Add(int.Parse(GetMap.Rows[i][9].ToString().Trim()) - 1);
                            num++;
                        }

                        for (int f = 0; f < A_1.Count; f++)
                        {
                            a[f] = A_1[f];
                        }
                    }
                    vertex1[i].Initialize(num, a);
                }
            }
            else if (GetMap == null)
            {
                for (int i = 0; i < SIZE; i++)
                {
                    //if (!BreakPoint.Contains(i.ToString()))
                    //{
                        int[] a = new int[4] { 0, 0, 0, 0}; //FQ SIZE4
                        int[] A = new int[4] { 0, 0, 0, 0 };
                        int num = 0;
                        for (int j = 0; j < SIZE; j++)
                        {
                            //if (!BreakPoint.Contains(j.ToString()))
                            //{
                                if ((System.Math.Abs(mygrid1[j, 0] - mygrid1[i, 0]) + System.Math.Abs(mygrid1[j, 1] - mygrid1[i, 1])) == 1)
                                {
                                    a[num] = j;
                                    A[num] = j + 1;
                                    num++;
                                    if (num == 4) //FQ
                                        break;  //FQ
                                }
                            //}
                        }
                        //vertex1[i].Initialize(num, a);
                        Function.PR_INSERT_Map_Info(i, (i % C_X).ToString(), (i / C_X).ToString(), A[0], A[1], A[2], A[3], 0);
                    //}
                }
            }
        }

        public List<CPath> GetPatnGroup(int F_X_len, int F_Y_len, int T_X_len, int T_Y_len, int MaxLine, int MaxBend)
        {
            CVertex[] vertex=vertex1;
            int[,] mygrid=mygrid1;
            int selection1 = F_Y_len * C_X + F_X_len;
            int selection2 = T_Y_len * C_X + T_X_len;
            int maxver = MaxLine;
            int maxwan = MaxBend;
            List<CPath> paths = new List<CPath>();
            Stack<int> mystack = new Stack<int>();
            Stack<int> tempstack = new Stack<int>();
            mystack.Push(selection1); //将起点压栈  
            vertex[selection1].setIsin(true);  //标记为已入栈  
            int path_num = 0;
            int wancount = 0;//TJ
            int timeOut = 0;
            while (mystack.Count() != 0)  //判断栈是否空  
            {
                timeOut++;
                int flag = vertex[mystack.Peek()].getOne();  //得到相邻的顶点            
                if (flag == -1)    //如果相邻顶点全部访问过    
                {
                    if (mystack.Count() > 2)
                    {
                        int temp3, temp4, temp5;
                        temp5 = mystack.ElementAt(0);
                        temp4 = mystack.ElementAt(1);
                        temp3 = mystack.ElementAt(2);
                        if ((System.Math.Abs(mygrid[temp3, 0] - mygrid[temp4, 0]) + System.Math.Abs(mygrid[temp4, 0] - mygrid[temp5, 0])) == 1)
                            wancount--;
                    }
                    int tpop = mystack.Pop(); //栈弹出一个元素               
                    vertex[tpop].resetFlag();  //该顶点相邻的顶点标记为未访问  
                    vertex[tpop].setIsin(false); //该顶点标记为未入栈  
                    continue; //取栈顶的相邻节点  
                }
                if (vertex[flag].isIn())  //若已经在栈中，取下一个顶点  
                {
                    continue;
                }

                if (T_Y_len > F_Y_len) //X,Y轴最多值
                {
                    if (F_Y_len - 8 > 0 && (flag / C_X) < (F_Y_len - 8))
                    {
                        continue;
                    }
                    if (T_Y_len + 8 < 100 && (flag / C_X) > (T_Y_len + 8))
                    {
                        continue;
                    }
                }
                else if (T_Y_len <= F_Y_len)
                {
                    if (T_Y_len - 8 > 0 && (flag / C_X) < (T_Y_len - 8))
                    {
                        continue;
                    }
                    if (F_Y_len + 8 < 100 && (flag / C_X) > (F_Y_len + 8))
                    {
                        continue;
                    }
                }

                if (mystack.Count() > maxver - 1) //判断栈中个数是否超过了用户要求的 ，这里是限制了一条路径节点的最大个数  
                {
                    if (mystack.Count() > 2)
                    {
                        int temp9, temp10, temp11;
                        temp11 = mystack.ElementAt(0);
                        temp10 = mystack.ElementAt(1);
                        temp9 = mystack.ElementAt(2);
                        if ((System.Math.Abs(mygrid[temp9, 0] - mygrid[temp10, 0]) + System.Math.Abs(mygrid[temp10, 0] - mygrid[temp11, 0])) == 1)
                            wancount--;
                    }
                    int tpop = mystack.Pop();
                    vertex[tpop].resetFlag();
                    vertex[tpop].setIsin(false);
                    continue;
                }

                // 判断wan
                if (mystack.Count() > 1)
                {
                    int temp1, temp2;
                    //int wancount=0; // ?????
                    temp2 = mystack.ElementAt(0);
                    temp1 = mystack.ElementAt(1);
                    //mystack.Push(temp2);
                    if ((System.Math.Abs(mygrid[temp1, 0] - mygrid[temp2, 0]) + System.Math.Abs(mygrid[temp2, 0] - mygrid[flag, 0])) == 1)
                        wancount++;
                    if (wancount > maxwan)
                    {
                        wancount--;
                        continue;
                    }
                }

                mystack.Push(flag); //将该顶点入栈          
                vertex[flag].setIsin(true);  //记为已入栈           
                if (mystack.Peek() == selection2)   //如果栈顶已经为所求，将此路径记录  
                {
                    //保存路径的代码省略  
                    //tempstack = mystack;
                    //for (int k=0; k<tempstack.size();++k) // this is wrong due to the change of tempstack.size()
                    //while (tempstack.Count() != 0)
                    List<int> singlepath = new List<int>();
                    foreach (int y in mystack)
                    {
                        singlepath.Add(y);
                    }
                    CPath onepath = new CPath(C_X);
                    onepath.setpath(singlepath);//TJ
                    onepath.setwan(wancount);
                    onepath.filterpath(); //TJ2
                    paths.Add(onepath);//TJ           
                    path_num++;  //TJ
                    //wancount=0;  //TJ
                    //singlepath.Clear();

                    if (mystack.Count() > 2)
                    {
                        int temp6, temp7, temp8;
                        temp8 = mystack.Pop();
                        temp7 = mystack.Pop();
                        temp6 = mystack.Peek();
                        mystack.Push(temp7);
                        mystack.Push(temp8);
                        if ((System.Math.Abs(mygrid[temp6, 0] - mygrid[temp7, 0]) + System.Math.Abs(mygrid[temp7, 0] - mygrid[temp8, 0])) == 1)
                            wancount--;
                    }
                    int tpop = mystack.Pop(); //将其弹出，继续探索              
                    vertex[tpop].setIsin(false); //清空入栈的标志位  
                }
            }
            return paths;
        }

        
        /// <summary>
        /// 返回最佳路径字符串并保存(Onow=0)
        /// </summary>
        /// <param name="agvIP"></param>
        /// <param name="skipNo"></param>
        /// <param name="fromX"></param>
        /// <param name="toX"></param>
        /// <param name="fromY"></param>
        /// <param name="toY"></param>
        /// <param name="MaxLine"></param>
        /// <param name="MaxBend"></param>
        /// <returns></returns>
        public bool retBestPathInOrders(string agvIP, string skipNo, string fromX, string toX, string fromY, string toY, int MaxLine, int MaxBend, out CPath bestCPath)
        {
            bool Rt = false;
            bestCPath = new CPath(0);
            if (fromX != toX | fromY != toY)
            {
                string orders = "";               
                List<CPath> paths = this.GetPatnGroup(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), MaxLine, MaxBend);
                List<int> loc_state0 = new List<int>();
                DataTable loc = null;// = Function.KIVA_SELECT_Map_Info_State(int.Parse(agvIP.Split('.')[3]) - 200);//Map_stae==0

                DataTable rulers = null;
                if (int.Parse(fromY) < int.Parse(toY))
                {
                    rulers = Function.KIVA_SELECT_Ruler(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), 1);
                }
                if (int.Parse(fromY) > int.Parse(toY))
                {
                    rulers = Function.KIVA_SELECT_Ruler(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), -1);
                }

                //?   2018-06-27
                int betterPoint = -1;
                //if (fromY == toY && int.Parse(toX) != 0 && int.Parse(fromX) >= 4)
                //    rulers = null;
                //if (Math.Abs(int.Parse(fromY) - int.Parse(toY)) == 2 && int.Parse(toX) != 0 && int.Parse(fromX) >= 4 && int.Parse(fromY) >= 8)
                //{
                //    rulers = null;
                //    betterPoint = int.Parse(toY) * C_X + 12;
                //}
                //?   2018-06-27

                if (paths != null && paths.Count > 0)
                {
                    bestCPath = paths[0];
                    int bestNo = -1;
                    for (int m = 0; m < paths.Count; m++)
                    {
                        if (rulers != null && rulers.Rows.Count > 0)
                        {
                            int CheckRoad = 0;
                            for (int i = 0; i < rulers.Rows.Count; i++)
                            {
                                int From_TO = int.Parse(rulers.Rows[i][3].ToString());
                                int Type_X = int.Parse(rulers.Rows[i][4].ToString());
                                int R_X = Int32.Parse(rulers.Rows[i][5].ToString());
                                int Type_Y = int.Parse(rulers.Rows[i][6].ToString());
                                int R_Y = Int32.Parse(rulers.Rows[i][7].ToString());
                                int Point_X = 0;
                                int Point_Y = 0;
                                if (From_TO == 1)//From
                                {
                                    if (Type_X == 1)
                                    {
                                        Point_X = int.Parse(fromX) + R_X;
                                    }
                                    else
                                    {
                                        Point_X = R_X;
                                    }
                                    if (Type_Y == 1)
                                    {
                                        Point_Y = int.Parse(fromY) + R_Y;
                                    }
                                    else
                                    {
                                        Point_Y = R_Y;
                                    }
                                }
                                else//To
                                {
                                    if (Type_X == 1)
                                    {
                                        Point_X = int.Parse(toX) + R_X;
                                    }
                                    else
                                    {
                                        Point_X = R_X;
                                    }
                                    if (Type_Y == 1)
                                    {
                                        Point_Y = int.Parse(toY) + R_Y;
                                    }
                                    else
                                    {
                                        Point_Y = R_Y;
                                    }
                                }
                                if (!paths[m].m_path.Contains(Point_Y * C_X + Point_X))
                                {
                                    //LogFile.SaveLog_Power("（" + Point_X + "，" + Point_Y + "）", "0");
                                    CheckRoad++;
                                    break;
                                }
                            }
                            if (CheckRoad == 0)
                            {
                                if (loc != null && loc.Rows.Count > 0)
                                {
                                    int CheckHasb = 0;
                                    for (int s = 0; s < loc.Rows.Count; s++)
                                    {
                                        if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                        {
                                            CheckHasb++;
                                            break;
                                        }
                                    }
                                    if (CheckHasb == 0)
                                    {
                                        if (bestNo == -1)
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                        else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                    }
                                }
                                else
                                {
                                    if (bestNo == -1)
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                    else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                    //else if (bestCPath.getlen() > paths[m].getlen())
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                }
                            }
                        }
                        else
                        {                           
                            if (paths[m].m_path.Contains(betterPoint) || betterPoint == -1)
                            {
                                if (loc != null && loc.Rows.Count > 0)
                                {
                                    int CheckHasb = 0;
                                    for (int s = 0; s < loc.Rows.Count; s++)
                                    {
                                        if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                        {
                                            CheckHasb++;
                                            break;
                                        }
                                    }
                                    if (CheckHasb == 0)
                                    {
                                        if (bestNo == -1)
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                        else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                        //else if (bestCPath.getlen() > paths[m].getlen())
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                    }
                                }
                                else
                                {
                                    if (bestNo == -1)
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                    else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                    //else if (bestCPath.getlen() > paths[m].getlen())
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                }
                            }
                        }
                    }
                    if (bestNo == -1)
                    {
                        
                    }
                    else
                    {
                        //bestCPath = paths[bestNo];                       
                        Rt = true;
                    }
                }
                else
                {
                    //retBestPathInOrders(agvIP, skipNo, fromX, toX, fromY, toY, MaxLine + 2, MaxBend);
                }
            }
            return Rt;
        }

        public bool retBestPathInOrders_Charge(string agvIP, string skipNo, string fromX, string toX, string fromY, string toY, int MaxLine, int MaxBend, out CPath bestCPath)
        {
            bool Rt = false;
            bestCPath = new CPath(0);
            if (fromX != toX | fromY != toY)
            {
                string orders = "";
                List<CPath> paths = this.GetPatnGroup(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), MaxLine, MaxBend);
                List<int> loc_state0 = new List<int>();
                DataTable loc = null;// = Function.KIVA_SELECT_Map_Info_State(int.Parse(agvIP.Split('.')[3]) - 200);//Map_stae==0

                DataTable rulers = null;
                if (int.Parse(fromY) < int.Parse(toY))
                {
                    //rulers = Function.KIVA_SELECT_Ruler(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), 1);
                }
                if (int.Parse(fromY) > int.Parse(toY))
                {
                    //rulers = Function.KIVA_SELECT_Ruler(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), -1);
                }

                //?
                int betterPoint = -1;
                if (fromY == toY && int.Parse(toX) != 0 && int.Parse(fromX) >= 4)
                    rulers = null;
                if (Math.Abs(int.Parse(fromY) - int.Parse(toY)) == 2 && int.Parse(toX) != 0 && int.Parse(fromX) >= 4 && int.Parse(fromY) >= 8)
                {
                    rulers = null;
                    betterPoint = int.Parse(toY) * C_X + 12;
                }
                //?

                if (paths != null && paths.Count > 0)
                {
                    bestCPath = paths[0];
                    int bestNo = -1;
                    for (int m = 0; m < paths.Count; m++)
                    {
                        if (rulers != null && rulers.Rows.Count > 0)
                        {
                            int CheckRoad = 0;
                            for (int i = 0; i < rulers.Rows.Count; i++)
                            {
                                int From_TO = int.Parse(rulers.Rows[i][3].ToString());
                                int Type_X = int.Parse(rulers.Rows[i][4].ToString());
                                int R_X = Int32.Parse(rulers.Rows[i][5].ToString());
                                int Type_Y = int.Parse(rulers.Rows[i][6].ToString());
                                int R_Y = Int32.Parse(rulers.Rows[i][7].ToString());
                                int Point_X = 0;
                                int Point_Y = 0;
                                if (From_TO == 1)//From
                                {
                                    if (Type_X == 1)
                                    {
                                        Point_X = int.Parse(fromX) + R_X;
                                    }
                                    else
                                    {
                                        Point_X = R_X;
                                    }
                                    if (Type_Y == 1)
                                    {
                                        Point_Y = int.Parse(fromY) + R_Y;
                                    }
                                    else
                                    {
                                        Point_Y = R_Y;
                                    }
                                }
                                else//To
                                {
                                    if (Type_X == 1)
                                    {
                                        Point_X = int.Parse(toX) + R_X;
                                    }
                                    else
                                    {
                                        Point_X = R_X;
                                    }
                                    if (Type_Y == 1)
                                    {
                                        Point_Y = int.Parse(toY) + R_Y;
                                    }
                                    else
                                    {
                                        Point_Y = R_Y;
                                    }
                                }
                                if (!paths[m].m_path.Contains(Point_Y * C_X + Point_X))
                                {
                                    //LogFile.SaveLog_Power("（" + Point_X + "，" + Point_Y + "）", "0");
                                    CheckRoad++;
                                    break;
                                }
                            }
                            if (CheckRoad == 0)
                            {
                                if (loc != null && loc.Rows.Count > 0)
                                {
                                    int CheckHasb = 0;
                                    for (int s = 0; s < loc.Rows.Count; s++)
                                    {
                                        if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                        {
                                            CheckHasb++;
                                            break;
                                        }
                                    }
                                    if (CheckHasb == 0)
                                    {
                                        if (bestNo == -1)
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                        else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                    }
                                }
                                else
                                {
                                    if (bestNo == -1)
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                    else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                    //else if (bestCPath.getlen() > paths[m].getlen())
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (paths[m].m_path.Contains(betterPoint) || betterPoint == -1)
                            {
                                if (loc != null && loc.Rows.Count > 0)
                                {
                                    int CheckHasb = 0;
                                    for (int s = 0; s < loc.Rows.Count; s++)
                                    {
                                        if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                        {
                                            CheckHasb++;
                                            break;
                                        }
                                    }
                                    if (CheckHasb == 0)
                                    {
                                        if (bestNo == -1)
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                        else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                        //else if (bestCPath.getlen() > paths[m].getlen())
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                    }
                                }
                                else
                                {
                                    if (bestNo == -1)
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                    else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                    //else if (bestCPath.getlen() > paths[m].getlen())
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                }
                            }
                        }
                    }
                    if (bestNo == -1)
                    {

                    }
                    else
                    {
                        //bestCPath = paths[bestNo];                       
                        Rt = true;
                    }
                }
                else
                {
                    //retBestPathInOrders(agvIP, skipNo, fromX, toX, fromY, toY, MaxLine + 2, MaxBend);
                }
            }
            return Rt;
        }




        /// <summary>
        /// 用于重新计算路径： 返回最佳路径字符串并保存(Onow=0)
        /// </summary>
        /// <param name="agvIP"></param>
        /// <param name="skipNo"></param>
        /// <param name="fromX"></param>
        /// <param name="toX"></param>
        /// <param name="fromY"></param>
        /// <param name="toY"></param>
        /// <param name="MaxLine"></param>
        /// <param name="MaxBend"></param>
        /// <returns></returns>
        public bool Again_retBestPathInOrders(string agvIP, int vs_carNo, string skipNo, string fromX, string toX, string fromY, string toY, int MaxLine, int MaxBend,out string BestRoad)
        {
            bool Rt = false;
            BestRoad = "";
            if (fromX != toX | fromY != toY)
            {
                int vs_CarX = 0;
                int vs_CarY = 0;
                DataTable agv_info = Function.KIVA_SELECT_AGV_Info_ByIP("192.168.1." + (vs_carNo + 200).ToString());
                if (agv_info != null && agv_info.Rows.Count > 0)
                {
                    string temp_x = agv_info.Rows[0][3].ToString();
                    string temp_y = agv_info.Rows[0][4].ToString();
                    vs_CarX = int.Parse(FormartXY(temp_x, temp_y).Split(',')[0]);
                    vs_CarY = int.Parse(FormartXY(temp_x, temp_y).Split(',')[1]);
                }
                List<int> loc_state0 = new List<int>();
                DataTable loc = Function.KIVA_SELECT_Map_Info_State(int.Parse(agvIP.Split('.')[3])-200);//Map_stae==0                
                string orders = "";
                Function.KIVA_UPDATE_WCS_To_KIVA_wac(skipNo, 0);//wac=0   
                List<CPath> paths = this.GetPatnGroup(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), MaxLine, MaxBend);
                if (paths != null && paths.Count > 0)
                {
                    CPath bestCPath = paths[0];
                    int bestNo = -1;
                    for (int m = 0; m < paths.Count; m++)
                    {                      

                            //if ((int.Parse(fromX) > 8 && int.Parse(fromY) > 16 && int.Parse(fromY) < 69 && paths[m].m_path.Contains((int.Parse(fromY) + 1) * 20 + (int.Parse(fromX)))) | (int.Parse(fromX) <= 8) | (int.Parse(fromX) > 8 && int.Parse(fromY) < 16 && int.Parse(fromY) > 69))
                            //{
                            //    if ((int.Parse(toX) > 8 && int.Parse(toY) > 16 && int.Parse(toX) < 69 && paths[m].m_path.Contains((int.Parse(toY) - 1) * 20 + (int.Parse(toX)))) | (int.Parse(toX) <= 8) | (int.Parse(toX) > 8 && int.Parse(toY) < 16 && int.Parse(toX) > 69))
                            //    {
                            //相对立的车 此时的XY

                        if ((!paths[m].m_path.Contains(vs_CarY * C_X + vs_CarX + 1) | (vs_CarY * C_X + vs_CarX + 1) == (int.Parse(fromY) * C_X + int.Parse(fromX)))
                                && (!paths[m].m_path.Contains(vs_CarY * C_X + vs_CarX - 1) | (vs_CarY * C_X + vs_CarX - 1) == (int.Parse(fromY) * C_X + int.Parse(fromX)))
                                && (!paths[m].m_path.Contains((vs_CarY + 1) * C_X + vs_CarX) | ((vs_CarY + 1) * C_X + vs_CarX) == (int.Parse(fromY) * C_X + int.Parse(fromX)))
                                && (!paths[m].m_path.Contains((vs_CarY - 1) * C_X + vs_CarX) | ((vs_CarY - 1) * C_X + vs_CarX) == (int.Parse(fromY) * C_X + int.Parse(fromX)))
                                )
                            {
                                {

                                    if (loc != null && loc.Rows.Count > 0)
                                    {
                                        int CheckHasb = 0;
                                        for (int s = 0; s < loc.Rows.Count; s++)
                                        {
                                            if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                            {
                                                CheckHasb++;
                                                break;
                                            }
                                        }
                                        if (CheckHasb == 0)
                                        {
                                            if (bestNo == -1)
                                            {
                                                bestNo = m;
                                            }
                                            else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                            {
                                                bestCPath = paths[m];
                                                bestNo = m;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (bestNo == -1)
                                        {
                                            bestNo = m;
                                        }
                                        else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                    }                                    
                                }
                            }
                        
                        //}
                        //}
                    }
                    //......
                    if (bestNo == -1)
                    {
                        //Again_retBestPathInOrders(agvIP, vs_carNo, skipNo, fromX, toX, fromY, toY, MaxLine, MaxBend + 1);
                        //retBestPathInOrders(agvIP, skipNo, fromX, toX, fromY, toY, MaxLine + 2, MaxBend);
                    }
                    else
                    {
                        bestCPath = paths[bestNo];
                        List<int> bestCPath_loc = bestCPath.subm_path;
                        List<int> aa = bestCPath.m_path;

                        List<int> bestCPath_angle = bestCPath.agvangle;
                        for (int q = 0; q < bestCPath_angle.Count; q++)
                        {
                            orders += bestCPath_loc[q + 1].ToString() + "," + bestCPath_angle[q].ToString() + ";";
                        }
                        BestRoad = orders;                        
                        Rt = true;
                    }
                }
                else
                {
                    //retBestPathInOrders(agvIP, skipNo, fromX, toX, fromY, toY, MaxLine + 2, MaxBend);
                }
            }
            return Rt;
        }


        /// <summary>
        /// 用于重新计算路径： 返回最佳路径字符串并保存(Onow=0)
        /// </summary>
        /// <param name="agvIP"></param>
        /// <param name="skipNo"></param>
        /// <param name="fromX"></param>
        /// <param name="toX"></param>
        /// <param name="fromY"></param>
        /// <param name="toY"></param>
        /// <param name="MaxLine"></param>
        /// <param name="MaxBend"></param>
        /// <returns></returns>
        public bool Again_retBestPathInOrders_s(string agvIP, int vs_carNo, string skipNo, string fromX, string toX, string fromY, string toY, int MaxLine, int MaxBend, out string BestRoad)
        {
            bool Rt = false;
            BestRoad = "";
            if (fromX != toX | fromY != toY)
            {
                int vs_CarX = 0;
                int vs_CarY = 0;
                DataTable agv_info = Function.KIVA_SELECT_AGV_Info_ByIP("192.168.1." + (vs_carNo + 200).ToString());
                if (agv_info != null && agv_info.Rows.Count > 0)
                {
                    string temp_x = agv_info.Rows[0][3].ToString();
                    string temp_y = agv_info.Rows[0][4].ToString();
                    vs_CarX = int.Parse(FormartXY(temp_x, temp_y).Split(',')[0]);
                    vs_CarY = int.Parse(FormartXY(temp_x, temp_y).Split(',')[1]);
                }
                
                List<int> loc_state0 = new List<int>();
                DataTable loc = Function.KIVA_SELECT_Map_Info_State(int.Parse(agvIP.Split('.')[3]) - 200);//Map_stae==0 
                int Macw = 0;
                if (int.Parse(toX) == vs_CarX && int.Parse(toY) == vs_CarY)
                {
                    loc = Function.KIVA_SELECT_Map_Info_State(vs_carNo);
                    Macw = 2;
                }
                string orders = "";
                Function.KIVA_UPDATE_WCS_To_KIVA_wac(skipNo, 0);//wac=0   
                List<CPath> paths = this.GetPatnGroup(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), MaxLine, MaxBend);
                if (paths != null && paths.Count > 0)
                {                    
                    CPath bestCPath = paths[0];
                    int bestNo = -1;
                    for (int m = 0; m < paths.Count; m++)
                    {
                        if (paths[m].getwan() > Macw)
                        {
                            if (loc != null && loc.Rows.Count > 0)
                            {
                                int CheckHasb = 0;
                                for (int s = 0; s < loc.Rows.Count; s++)
                                {
                                    if (int.Parse(loc.Rows[s][1].ToString()) != int.Parse(fromX) + int.Parse(fromY) * C_X)
                                    {
                                        if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                        {
                                            CheckHasb++;
                                            break;
                                        }
                                    }
                                }
                                if (CheckHasb == 0)
                                {
                                    if (bestNo == -1)
                                    {
                                        bestNo = m;
                                    }
                                    else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                }
                            }
                            else
                            {
                                if (bestNo == -1)
                                {
                                    bestNo = m;
                                }
                                else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                {
                                    bestCPath = paths[m];
                                    bestNo = m;
                                }
                            }
                        }

                        //}
                        //}
                    }
                    //......
                    if (bestNo == -1)
                    {
                        //Again_retBestPathInOrders(agvIP, vs_carNo, skipNo, fromX, toX, fromY, toY, MaxLine, MaxBend + 1);
                        //retBestPathInOrders(agvIP, skipNo, fromX, toX, fromY, toY, MaxLine + 2, MaxBend);
                    }
                    else
                    {
                        bestCPath = paths[bestNo];
                        List<int> bestCPath_loc = bestCPath.subm_path;
                        List<int> aa = bestCPath.m_path;

                        List<int> bestCPath_angle = bestCPath.agvangle;
                        for (int q = 0; q < bestCPath_angle.Count; q++)
                        {
                            orders += bestCPath_loc[q + 1].ToString() + "," + bestCPath_angle[q].ToString() + ";";
                        }
                        BestRoad = orders;
                        Rt = true;
                    }
                }
                else
                {
                    //retBestPathInOrders(agvIP, skipNo, fromX, toX, fromY, toY, MaxLine + 2, MaxBend);
                }
            }
            return Rt;
        }

        /// <summary>
        /// 故障时重新计算路径
        /// </summary>
        /// <param name="agvIP"></param>
        /// <param name="skipNo"></param>
        /// <param name="fromX"></param>
        /// <param name="toX"></param>
        /// <param name="fromY"></param>
        /// <param name="toY"></param>
        /// <param name="MaxLine"></param>
        /// <param name="MaxBend"></param>
        /// <returns></returns>
        public bool Error_retBestPathInOrders(string agvIP, string skipNo, string fromX, string toX, string fromY, string toY, int MaxLine, int MaxBend)
        {
            bool Rt = false;
            if (fromX != toX | fromY != toY)
            {
                string orders = "";
                Function.KIVA_UPDATE_WCS_To_KIVA_wac(skipNo, 0);//wac=0   
                List<CPath> paths = this.GetPatnGroup(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), MaxLine, MaxBend);

                List<int> loc_state0 = new List<int>();
                DataTable loc = Function.KIVA_SELECT_Map_Info_State(int.Parse(agvIP.Split('.')[3]) - 200);//Map_stae==0
                //DataTable rulers = Function.KIVA_SELECT_Ruler(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY));

                //
                DataTable rulers = null;
                if (int.Parse(fromY) < int.Parse(toY))
                {
                    rulers = Function.KIVA_SELECT_Ruler(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), 1);
                }
                if (int.Parse(fromY) > int.Parse(toY))
                {
                    rulers = Function.KIVA_SELECT_Ruler(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), -1);
                }
                //

                if (paths != null && paths.Count > 0)
                {
                    CPath bestCPath = paths[0];
                    int bestNo = -1;
                    for (int m = 0; m < paths.Count; m++)
                    {
                        if (rulers != null && rulers.Rows.Count > 0)
                        {
                            int CheckRoad = 0;
                            for (int i = 0; i < rulers.Rows.Count; i++)
                            {
                                int From_TO = int.Parse(rulers.Rows[i][3].ToString());
                                int Type_X = int.Parse(rulers.Rows[i][4].ToString());
                                int R_X = Int32.Parse(rulers.Rows[i][5].ToString());
                                int Type_Y = int.Parse(rulers.Rows[i][6].ToString());
                                int R_Y = Int32.Parse(rulers.Rows[i][7].ToString());
                                int Point_X = 0;
                                int Point_Y = 0;
                                if (From_TO == 1)//From
                                {
                                    if (Type_X == 1)
                                    {
                                        Point_X = int.Parse(fromX) + R_X;
                                    }
                                    else
                                    {
                                        Point_X = R_X;
                                    }
                                    if (Type_Y == 1)
                                    {
                                        Point_Y = int.Parse(fromY) + R_Y;
                                    }
                                    else
                                    {
                                        Point_Y = R_Y;
                                    }
                                }
                                else//To
                                {
                                    if (Type_X == 1)
                                    {
                                        Point_X = int.Parse(toX) + R_X;
                                    }
                                    else
                                    {
                                        Point_X = R_X;
                                    }
                                    if (Type_Y == 1)
                                    {
                                        Point_Y = int.Parse(toY) + R_Y;
                                    }
                                    else
                                    {
                                        Point_Y = R_Y;
                                    }
                                }
                                if (!paths[m].m_path.Contains(Point_Y * C_X + Point_X))
                                {
                                    CheckRoad++;
                                    break;
                                }
                            }
                            if (CheckRoad == 0)
                            {
                                if (loc != null && loc.Rows.Count > 0)
                                {
                                    int CheckHasb = 0;
                                    for (int s = 0; s < loc.Rows.Count; s++)
                                    {
                                        if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                        {
                                            CheckHasb++;
                                            break;
                                        }
                                    }
                                    if (CheckHasb == 0)
                                    {
                                        if (bestNo == -1)
                                        {
                                            bestNo = m;
                                        }
                                        else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                    }
                                }
                                else
                                {
                                    if (bestNo == -1)
                                    {
                                        bestNo = m;
                                    }
                                    else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                }
                            }
                        }//
                        else
                        {
                            if (loc != null && loc.Rows.Count > 0)
                            {
                                int CheckHasb = 0;
                                for (int s = 0; s < loc.Rows.Count; s++)
                                {
                                    if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                    {
                                        CheckHasb++;
                                        break;
                                    }
                                }
                                if (CheckHasb == 0)
                                {
                                    if (bestNo == -1)
                                    {
                                        bestNo = m;
                                    }
                                    else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                }
                            }
                            else
                            {
                                if (bestNo == -1)
                                {
                                    bestNo = m;
                                }
                                else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                {
                                    bestCPath = paths[m];
                                    bestNo = m;
                                }
                            }
                        }
                    }
                    if (bestNo == -1)
                    {

                    }
                    else
                    {
                        bestCPath = paths[bestNo];
                        List<int> bestCPath_loc = bestCPath.subm_path;
                        List<int> aa = bestCPath.m_path;

                        List<int> bestCPath_angle = bestCPath.agvangle;
                        for (int q = 0; q < bestCPath_angle.Count; q++)
                        {
                            orders += bestCPath_loc[q + 1].ToString() + "," + bestCPath_angle[q].ToString() + ";";
                        }
                        //将最优路径存入order表
                        Function.KIVA_UPDATE_AGV_Order(agvIP, 0, orders, "0", int.Parse(fromX).ToString() + "," + int.Parse(fromY).ToString(), int.Parse(toX).ToString() + "," + int.Parse(toY).ToString());
                        Rt = true;
                    }
                }
                else
                {
                    //retBestPathInOrders(agvIP, skipNo, fromX, toX, fromY, toY, MaxLine + 2, MaxBend);
                }
            }
            return Rt;
        }

        /// <summary>
        /// 计算去充电路径
        /// </summary>
        /// <param name="agvIP"></param>
        /// <param name="skipNo"></param>
        /// <param name="fromX"></param>
        /// <param name="toX"></param>
        /// <param name="fromY"></param>
        /// <param name="toY"></param>
        /// <param name="MaxLine"></param>
        /// <param name="MaxBend"></param>
        /// <returns></returns>
        public bool GoCharge_retBestPathInOrders(string agvIP, string skipNo, string fromX, string toX, string fromY, string toY, int MaxLine, int MaxBend, out CPath bestCPath)
        {
            bool Rt = false;
            bestCPath = new CPath(0);
            if (fromX != toX | fromY != toY)
            {
                string orders = "";               
                List<CPath> paths = this.GetPatnGroup(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), MaxLine, MaxBend);
                List<int> loc_state0 = new List<int>();
                DataTable loc = null;// = Function.KIVA_SELECT_Map_Info_State(int.Parse(agvIP.Split('.')[3]) - 200);//Map_stae==0

                DataTable rulers = null;
                if (int.Parse(fromY) < int.Parse(toY))
                {
                    rulers = Function.KIVA_SELECT_Ruler(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), 1);
                }
                if (int.Parse(fromY) > int.Parse(toY))
                {
                    rulers = Function.KIVA_SELECT_Ruler(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), -1);
                }

                //?
                int betterPoint = -1;
                if (fromY == toY && int.Parse(toX) != 2 && int.Parse(fromX)>=10)
                    rulers = null;
                if (Math.Abs(int.Parse(fromY) - int.Parse(toY)) <= 2 && int.Parse(toX) != 2 && int.Parse(fromX) >= 10)
                {
                    rulers = null;
                    betterPoint = ((int.Parse(fromY) + int.Parse(toY)) / 2) * C_X + int.Parse(fromX);
                }
                //?

                if (paths != null && paths.Count > 0)
                {
                    bestCPath = paths[0];
                    int bestNo = -1;
                    for (int m = 0; m < paths.Count; m++)
                    {
                        if (rulers != null && rulers.Rows.Count > 0)
                        {
                            int CheckRoad = 0;
                            for (int i = 0; i < rulers.Rows.Count; i++)
                            {
                                int From_TO = int.Parse(rulers.Rows[i][3].ToString());
                                int Type_X = int.Parse(rulers.Rows[i][4].ToString());
                                int R_X = Int32.Parse(rulers.Rows[i][5].ToString());
                                int Type_Y = int.Parse(rulers.Rows[i][6].ToString());
                                int R_Y = Int32.Parse(rulers.Rows[i][7].ToString());
                                int Point_X = 0;
                                int Point_Y = 0;
                                if (From_TO == 1)//From
                                {
                                    if (Type_X == 1)
                                    {
                                        Point_X = int.Parse(fromX) + R_X;
                                    }
                                    else
                                    {
                                        Point_X = R_X;
                                    }
                                    if (Type_Y == 1)
                                    {
                                        Point_Y = int.Parse(fromY) + R_Y;
                                    }
                                    else
                                    {
                                        Point_Y = R_Y;
                                    }
                                }
                                else//To
                                {
                                    if (Type_X == 1)
                                    {
                                        Point_X = int.Parse(toX) + R_X;
                                    }
                                    else
                                    {
                                        Point_X = R_X;
                                    }
                                    if (Type_Y == 1)
                                    {
                                        Point_Y = int.Parse(toY) + R_Y;
                                    }
                                    else
                                    {
                                        Point_Y = R_Y;
                                    }
                                }
                                if (!paths[m].m_path.Contains(Point_Y * C_X + Point_X))
                                {
                                    //LogFile.SaveLog_Power("（" + Point_X + "，" + Point_Y + "）", "0");
                                    CheckRoad++;
                                    break;
                                }
                            }
                            if (CheckRoad == 0)
                            {
                                if (loc != null && loc.Rows.Count > 0)
                                {
                                    int CheckHasb = 0;
                                    for (int s = 0; s < loc.Rows.Count; s++)
                                    {
                                        if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                        {
                                            CheckHasb++;
                                            break;
                                        }
                                    }
                                    if (CheckHasb == 0)
                                    {
                                        if (bestNo == -1)
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                        else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                    }
                                }
                                else
                                {
                                    if (bestNo == -1)
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                    else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                    //else if (bestCPath.getlen() > paths[m].getlen())
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                }
                            }
                        }
                        else
                        {                           
                            if (paths[m].m_path.Contains(betterPoint) || betterPoint == -1)
                            {
                                if (loc != null && loc.Rows.Count > 0)
                                {
                                    int CheckHasb = 0;
                                    for (int s = 0; s < loc.Rows.Count; s++)
                                    {
                                        if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                        {
                                            CheckHasb++;
                                            break;
                                        }
                                    }
                                    if (CheckHasb == 0)
                                    {
                                        if (bestNo == -1)
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                        else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                        //else if (bestCPath.getlen() > paths[m].getlen())
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                    }
                                }
                                else
                                {
                                    if (bestNo == -1)
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                    else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                    //else if (bestCPath.getlen() > paths[m].getlen())
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                }
                            }
                        }
                    }
                    if (bestNo == -1)
                    {
                        
                    }
                    else
                    {
                        //bestCPath = paths[bestNo];                       
                        Rt = true;
                    }
                }
                else
                {
                    //retBestPathInOrders(agvIP, skipNo, fromX, toX, fromY, toY, MaxLine + 2, MaxBend);
                }
            }
            return Rt;
        }
             
        /// <summary>
        /// 计算离开充电路径
        /// </summary>
        /// <param name="agvIP"></param>
        /// <param name="skipNo"></param>
        /// <param name="fromX"></param>
        /// <param name="toX"></param>
        /// <param name="fromY"></param>
        /// <param name="toY"></param>
        /// <param name="MaxLine"></param>
        /// <param name="MaxBend"></param>
        /// <returns></returns>
        public bool LeaveCharge_retBestPathInOrders(string agvIP, string skipNo, string fromX, string toX, string fromY, string toY, int MaxLine, int MaxBend, out CPath bestCPath)
        {
            bool Rt = false;
            bestCPath = new CPath(0);
            if (fromX != toX | fromY != toY)
            {
                string orders = "";
                List<CPath> paths = this.GetPatnGroup(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), MaxLine, MaxBend);
                List<int> loc_state0 = new List<int>();
                DataTable loc = null;// = Function.KIVA_SELECT_Map_Info_State(int.Parse(agvIP.Split('.')[3]) - 200);//Map_stae==0

                DataTable rulers = null;
                if (int.Parse(fromY) < int.Parse(toY))
                {
                    rulers = Function.KIVA_SELECT_Ruler(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), 1);
                }
                if (int.Parse(fromY) > int.Parse(toY))
                {
                    rulers = Function.KIVA_SELECT_Ruler(int.Parse(fromX), int.Parse(fromY), int.Parse(toX), int.Parse(toY), -1);
                }

                //?
                int betterPoint = -1;
                if (fromY == toY && int.Parse(toX) != 2 && int.Parse(fromX) >= 10)
                    rulers = null;
                if (Math.Abs(int.Parse(fromY) - int.Parse(toY)) <= 2 && int.Parse(toX) != 2 && int.Parse(fromX) >= 10)
                {
                    rulers = null;
                    betterPoint = ((int.Parse(fromY) + int.Parse(toY)) / 2) * C_X + int.Parse(fromX);
                }
                //?

                if (paths != null && paths.Count > 0)
                {
                    bestCPath = paths[0];
                    int bestNo = -1;
                    for (int m = 0; m < paths.Count; m++)
                    {
                        if (rulers != null && rulers.Rows.Count > 0)
                        {
                            int CheckRoad = 0;
                            for (int i = 0; i < rulers.Rows.Count; i++)
                            {
                                int From_TO = int.Parse(rulers.Rows[i][3].ToString());
                                int Type_X = int.Parse(rulers.Rows[i][4].ToString());
                                int R_X = Int32.Parse(rulers.Rows[i][5].ToString());
                                int Type_Y = int.Parse(rulers.Rows[i][6].ToString());
                                int R_Y = Int32.Parse(rulers.Rows[i][7].ToString());
                                int Point_X = 0;
                                int Point_Y = 0;
                                if (From_TO == 1)//From
                                {
                                    if (Type_X == 1)
                                    {
                                        Point_X = int.Parse(fromX) + R_X;
                                    }
                                    else
                                    {
                                        Point_X = R_X;
                                    }
                                    if (Type_Y == 1)
                                    {
                                        Point_Y = int.Parse(fromY) + R_Y;
                                    }
                                    else
                                    {
                                        Point_Y = R_Y;
                                    }
                                }
                                else//To
                                {
                                    if (Type_X == 1)
                                    {
                                        Point_X = int.Parse(toX) + R_X;
                                    }
                                    else
                                    {
                                        Point_X = R_X;
                                    }
                                    if (Type_Y == 1)
                                    {
                                        Point_Y = int.Parse(toY) + R_Y;
                                    }
                                    else
                                    {
                                        Point_Y = R_Y;
                                    }
                                }
                                if (!paths[m].m_path.Contains(Point_Y * C_X + Point_X))
                                {
                                    //LogFile.SaveLog_Power("（" + Point_X + "，" + Point_Y + "）", "0");
                                    CheckRoad++;
                                    break;
                                }
                            }
                            if (CheckRoad == 0)
                            {
                                if (loc != null && loc.Rows.Count > 0)
                                {
                                    int CheckHasb = 0;
                                    for (int s = 0; s < loc.Rows.Count; s++)
                                    {
                                        if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                        {
                                            CheckHasb++;
                                            break;
                                        }
                                    }
                                    if (CheckHasb == 0)
                                    {
                                        if (bestNo == -1)
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                        else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                    }
                                }
                                else
                                {
                                    if (bestNo == -1)
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                    else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                    //else if (bestCPath.getlen() > paths[m].getlen())
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (paths[m].m_path.Contains(betterPoint) || betterPoint == -1)
                            {
                                if (loc != null && loc.Rows.Count > 0)
                                {
                                    int CheckHasb = 0;
                                    for (int s = 0; s < loc.Rows.Count; s++)
                                    {
                                        if (paths[m].m_path.Contains(int.Parse(loc.Rows[s][1].ToString())))
                                        {
                                            CheckHasb++;
                                            break;
                                        }
                                    }
                                    if (CheckHasb == 0)
                                    {
                                        if (bestNo == -1)
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                        else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                        //else if (bestCPath.getlen() > paths[m].getlen())
                                        {
                                            bestCPath = paths[m];
                                            bestNo = m;
                                        }
                                    }
                                }
                                else
                                {
                                    if (bestNo == -1)
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                    else if (bestCPath.getwan() > paths[m].getwan() | (bestCPath.getwan() == paths[m].getwan() && bestCPath.getlen() > paths[m].getlen()))
                                    //else if (bestCPath.getlen() > paths[m].getlen())
                                    {
                                        bestCPath = paths[m];
                                        bestNo = m;
                                    }
                                }
                            }
                        }
                    }
                    if (bestNo == -1)
                    {

                    }
                    else
                    {
                        //bestCPath = paths[bestNo];                       
                        Rt = true;
                    }
                }
                else
                {
                    //retBestPathInOrders(agvIP, skipNo, fromX, toX, fromY, toY, MaxLine + 2, MaxBend);
                }
            }
            return Rt;
        }


        public string FormartXY(string x, string y)
        {
            int Xadd = 0;
            if (int.Parse(x) % Lent >= (Lent/2))
            {
                Xadd++;
            }
            int Yadd = 0;
            if (int.Parse(y) % Lent >= (Lent/2))
            {
                Yadd++;
            }
            string X = (int.Parse(x) / Lent + Xadd).ToString();
            string Y = (int.Parse(y) / Lent + Yadd).ToString();

            return X + "," + Y;
        }
        public bool Is_Path_In_X(int x, List<int> path)
        {
            int flag = 0;
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i] % C_X == x)
                    flag++;
            }
            if (flag <= 2)
                return false;

            else
                return true;
        }
        public bool Is_Path_In_Y(int y, List<int> path)
        {
            int flag = 0;
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i] / C_X == y)
                    flag++;
            }
            if (flag <= 2)
                return false;

            else
                return true;
        }

    }
}
