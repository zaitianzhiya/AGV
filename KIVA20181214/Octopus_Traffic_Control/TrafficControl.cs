using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;

namespace Octopus_Traffic_Control
{
    public class TrafficControl
    {
        int MaxLine = 4;
        int Flowme = 1;
        public TrafficControl(int GetMaxLine,int flow)
        {
            MaxLine = GetMaxLine;
            Flowme = flow;
        }

        public void GetTrafficControl(DataTable AGV_Road_Info,out DataTable Control_Info)
        {
            Control_Info = new DataTable();
            if (AGV_Road_Info != null && AGV_Road_Info.Rows.Count > 0)
            {
                int Road_Count = AGV_Road_Info.Rows.Count;
                Control_Info.Columns.Add("AGV_IP");
                Control_Info.Columns.Add("AGV_No");
                Control_Info.Columns.Add("AGV_Control");//AGV_EXTEND_INPUT
                Control_Info.Columns.Add("AGV_Islock");
                Control_Info.Columns.Add("AGV_Speed");
                Control_Info.Columns.Add("AGV_RUNST");
                bool[] Control_AGV = new bool[Road_Count];
                for (int i = 0; i < Road_Count; i++)
                {
                    DataRow dr=Control_Info.NewRow();
                    dr["AGV_IP"] = AGV_Road_Info.Rows[i][0].ToString().Trim();
                    dr["AGV_No"] = AGV_Road_Info.Rows[i][1].ToString().Trim();
                    dr["AGV_Control"] = 1;
                    dr["AGV_RUNST"] = AGV_Road_Info.Rows[i][4].ToString().Trim();
                    dr["AGV_Speed"] = AGV_Road_Info.Rows[i][5].ToString().Trim();
                    dr["AGV_Islock"] = AGV_Road_Info.Rows[i][6].ToString().Trim();
                    Control_AGV[i] = true;
                    Control_Info.Rows.Add(dr);
                }
                for (int i = 0; i < Road_Count; i++)
                {
                    string AGV_IP_A = AGV_Road_Info.Rows[i][0].ToString().Trim();
                    string AGV_No_A = AGV_Road_Info.Rows[i][1].ToString().Trim();
                    string AGV_Now_Point_A = AGV_Road_Info.Rows[i][2].ToString().Trim();
                    string AGV_All_Point_A = AGV_Road_Info.Rows[i][3].ToString().Trim();                    
                    List<string> AGV_All_List_A = new List<string>(AGV_All_Point_A.Split(';'));
                    AGV_All_List_A.Remove("");
                    int GetNowIndex_A = AGV_All_List_A.IndexOf(AGV_Now_Point_A);// AGV_All_Point_A.IndexOf(AGV_Now_Point_A);
                    if (GetNowIndex_A >= 0)
                    {
                        AGV_All_List_A.RemoveRange(0, GetNowIndex_A);
                        for (int j = i + 1; j < Road_Count; j++)
                        {
                            string AGV_IP_B = AGV_Road_Info.Rows[j][0].ToString().Trim();
                            string AGV_No_B = AGV_Road_Info.Rows[j][1].ToString().Trim();
                            string AGV_Now_Point_B = AGV_Road_Info.Rows[j][2].ToString().Trim();
                            string AGV_All_Point_B = AGV_Road_Info.Rows[j][3].ToString().Trim();
                            List<string> AGV_All_List_B = new List<string>(AGV_All_Point_B.Split(';'));
                            AGV_All_List_B.Remove("");
                            int GetNowIndex_B = AGV_All_List_B.IndexOf(AGV_Now_Point_B);
                            if (GetNowIndex_B >= 0)
                            {
                                AGV_All_List_B.RemoveRange(0, GetNowIndex_B);
                                if (Control_AGV[i] || Control_AGV[j])
                                {
                                    var list = AGV_All_List_A.Intersect(AGV_All_List_B).ToArray();//交集
                                    if (list != null && list.Count() > 0)
                                    {
                                        int Indexof_A = AGV_All_List_A.IndexOf(list[0]);
                                        int Indexof_B = AGV_All_List_B.IndexOf(list[0]);
                                        if (Indexof_A == 0 | Indexof_B==0)
                                        {
                                            if (Indexof_A + Indexof_B <= Flowme)
                                            {
                                                SaveLogTest(AGV_IP_A + "@" + AGV_IP_B);
                                                if (Indexof_A >= Indexof_B)
                                                {
                                                    Control_AGV[i] = false;
                                                }
                                                else
                                                {
                                                    Control_AGV[j] = false;
                                                }
                                            }
                                        }
                                        else if (Indexof_A + Indexof_B <= MaxLine)
                                        {
                                            SaveLogTest(AGV_IP_A + "@@" + AGV_IP_B);
                                            if (Indexof_A >= Indexof_B)
                                            {
                                                Control_AGV[i] = false;
                                            }
                                            else
                                            {
                                                Control_AGV[j] = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Control_Info.Rows[i]["AGV_Control"] = (Control_AGV[i] == true ? 1 : 0);
                }
            }
        }

        public static void SaveLogTest(string fileMsg)
        {
            try
            {
                using (FileStream _fStream = new FileStream(@"D:\logtra.txt", FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(DateTime.Now + "  " + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception )
            {
                
            }
        }
    }
}
