using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media;
using Mitsubishi_Q_L_PLC;
using TS_RGB.Fuction;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using FileControl;
using System.Net.NetworkInformation;
using MyHelpers;
using Color = System.Windows.Media.Color;

namespace TS_RGB
{
    public partial class frm_Main : Form
    {
        #region Params

        private string plcOut, plcIn1, plcIn2;//PLC信息，1个出料两个进料
        private int readCount;//PLC读取位数
        private Dictionary<string, axTX_MS_QL_PLC> dicPlc;//PLC对象集合

        public static bool AutoControl = true;

        frm_Monitor_Car frm_car; 
        frm_Monitor_AutoDoor frm_autoDoorInfo;
        frm_Monitor_CallBox frm_CallBoxInfo;
        frm_Monitor_Power frm_PowerInfo;
        frm_Monitor_Elevator frm_ELECInfo;
        frm_map frm_mapshow;
        frm_Manage_CrossInfo frm_crossInfo;
        frm_Login frm_login;

        public static ToolStripStatusLabel toolStripStatusLabel_forPublic;
        public static ToolStripMenuItem ToolStripMenuItemAGVManage;
        public static ToolStripMenuItem ToolStripMenuItemAGV_orderManage;
        public static ToolStripMenuItem ToolStripMenuItemRFIDManage;
        public static ToolStripMenuItem ToolStripMenuItemAreaManage;
        public static ToolStripMenuItem ToolStripMenuItemTaskReset;

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private Thread ListenThread;
        private Socket listener;
        private Dictionary<string, Socket> connectedSocket;
        private Dictionary<string, int> SocketStatus;
        private List<string> ClientIps;
        bool bstop = false;
        public byte[] SendMess = new byte[13];
        IPAddress ipAddress;
        int hostPORT;
        List<string> Cstring;
        Dictionary<string, int> MACH_OLD;
        public static object ThreadControl_RGV = new object();

        Thread Thread_RGV;
        ThreadStart ThreadSt_RGV;
        private Thread ThreadPlc;
        private ThreadStart ThreadStartPlc;

        #endregion

        #region Page

        public frm_Main()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile(Application.StartupPath + "\\img\\log1.jpg");

            //用于权限设置
            toolStripStatusLabel_forPublic = toolStripStatusLabel5;
            ToolStripMenuItemAGVManage = aGV管理ToolStripMenuItem;
            ToolStripMenuItemAGV_orderManage = toolStripMenuItem2;
            ToolStripMenuItemAreaManage = 区域管理ToolStripMenuItem;

            LogFile.SaveLog_Start("[启动] AGV管理系统已启动..." + "\r\n");
        }
        protected override void WndProc(ref Message m)
        {            
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 0x01;
            const int HTCAPTION = 0x02;
            const int WM_NCLBUTTONDBLCLK = 0xA3;

            switch (m.Msg)
            {
                case 0x4e:
                case 0xd:
                case 0xe:
                case 0x14:
                    base.WndProc(ref m);
                    break;
                case WM_NCHITTEST://鼠标点任意位置后可以拖动窗体

                    this.DefWndProc(ref m);
                    if (m.Result.ToInt32() == HTCLIENT)
                    {
                        m.Result = new IntPtr(HTCAPTION);
                        return;
                    }
                    break;
                case WM_NCLBUTTONDBLCLK://禁止双击最大化
                    Console.WriteLine(this.WindowState);
                    return;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            Params.root_name = EncryptHelper.DecryptDES(FileControl.SetFileControl.ReadIniValue("ROOTUSERS", "ROOTUSERNAME", Application.StartupPath + @"\AGV_Set.ini").Trim(), Params.secretKey);
            Params.root_pwd = EncryptHelper.DecryptDES(FileControl.SetFileControl.ReadIniValue("ROOTUSERS", "ROOTPASSWORD", Application.StartupPath + @"\AGV_Set.ini").Trim(), Params.secretKey);

            MdiClient ctlMDI;
            foreach (Control ctl in this.Controls)
            {
                try
                {
                    ctlMDI = (MdiClient)ctl;
                    ctlMDI.BackColor = this.BackColor;
                }
                catch (InvalidCastException exc)
                {
                }
            }
            MACH_OLD = new Dictionary<string, int>();
            Cstring = new List<string>();
            string ConDB = File.ReadAllText(Application.StartupPath + @"\ConnectionString").ToString().Trim();
            Function.getConn(ConDB);
            //------------------取得IPaddress、port---------------------
            string HostIP = FileControl.SetFileControl.ReadIniValue("HOST", "HOSTIP", Application.StartupPath + @"\AGV_Set.ini").Trim();
            hostPORT = int.Parse(FileControl.SetFileControl.ReadIniValue("HOST", "HOSTPORT", Application.StartupPath + @"\AGV_Set.ini").Trim().Trim());

            #region 添加PLC通信信息
            //plcOut = FileControl.SetFileControl.ReadIniValue("PLC", "PLCOUT", Application.StartupPath + @"\AGV_Set.ini").Trim();
            //plcIn1 = FileControl.SetFileControl.ReadIniValue("PLC", "PLCIN1", Application.StartupPath + @"\AGV_Set.ini").Trim();
            //plcIn2 = FileControl.SetFileControl.ReadIniValue("PLC", "PLCIN2", Application.StartupPath + @"\AGV_Set.ini").Trim();
            //readCount = int.Parse(FileControl.SetFileControl.ReadIniValue("PLC", "LINECOUNT", Application.StartupPath + @"\AGV_Set.ini").Trim());

            //dicPlc=new Dictionary<string, axTX_MS_QL_PLC>();
            //string[] plcArray;
            //if (!string.IsNullOrEmpty(plcOut))
            //{
            //    plcArray = plcOut.Split(':');
            //    if (plcArray.Length > 1)
            //    {
            //        axTX_MS_QL_PLC axTxMsQlPlc1 = new axTX_MS_QL_PLC();
            //        axTxMsQlPlc1.ax_Connect(plcArray[0], int.Parse(plcArray[1]));
            //        if (!dicPlc.Keys.Contains("O_"+plcArray[0]))
            //        {
            //            dicPlc["O_" + plcArray[0]] = axTxMsQlPlc1;
            //        }
            //    }
            //}
            //if (!string.IsNullOrEmpty(plcIn1))
            //{
            //    plcArray = plcIn1.Split(':');
            //    if (plcArray.Length > 1)
            //    {
            //        axTX_MS_QL_PLC axTxMsQlPlc2 = new axTX_MS_QL_PLC();
            //        axTxMsQlPlc2.ax_Connect(plcArray[0], int.Parse(plcArray[1]));
            //        if (!dicPlc.Keys.Contains(plcArray[0]))
            //        {
            //            dicPlc[plcArray[0]] = axTxMsQlPlc2;
            //        }
            //    }
            //}
            //if (!string.IsNullOrEmpty(plcIn2))
            //{
            //    plcArray = plcIn2.Split(':');
            //    if (plcArray.Length > 1)
            //    {
            //        axTX_MS_QL_PLC axTxMsQlPlc3 = new axTX_MS_QL_PLC();
            //        axTxMsQlPlc3.ax_Connect(plcArray[0], int.Parse(plcArray[1]));
            //        if (!dicPlc.Keys.Contains(plcArray[0]))
            //        {
            //            dicPlc[plcArray[0]] = axTxMsQlPlc3;
            //        }
            //    }
            //}
            #endregion

            //----------------------------------------------------------
            //------------------初始化Socket Server---------------------
            connectedSocket = new Dictionary<string, Socket> { };
            SocketStatus = new Dictionary<string, int> { };
            ClientIps = new List<string> { };
            ipAddress = Dns.GetHostAddresses(HostIP)[0];
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            string GetIp = "";
            string CheckIP = "";
            for (int i = 0; i < ipHostInfo.AddressList.Count(); i++)
            {
                if (ipHostInfo.AddressList[i].ToString().Trim().Contains(".") && ipHostInfo.AddressList[i].ToString().Trim().Count() < 16)
                {
                    GetIp = ipHostInfo.AddressList[i].ToString().Trim();
                    if (ipHostInfo.AddressList[i].ToString().Trim() == HostIP)
                    {
                        CheckIP = ipHostInfo.AddressList[i].ToString().Trim();
                    }
                }
            }
            if (CheckIP != "")
            {
                Listen();
            }
            else
            {

            }
            frm_car = new frm_Monitor_Car(this);
            //frm_autoDoorInfo = new frm_Monitor_AutoDoor(this);
            frm_CallBoxInfo = new frm_Monitor_CallBox(this);
            frm_PowerInfo = new frm_Monitor_Power(this);
            //frm_ELECInfo = new frm_Monitor_Elevator(this);
            
            frm_car.MdiParent = this;
            //frm_autoDoorInfo.MdiParent = this;
            frm_CallBoxInfo.MdiParent = this;
            //frm_ELECInfo.MdiParent = this;
            frm_PowerInfo.MdiParent = this;

            Function.UPDATE_tb_AGV_INFO_init();

            timer1.Start();

            DataTable MAX_agvRemark = Function.SELECT_AGV_INFO_MAXREMARK();
            if (MAX_agvRemark != null && MAX_agvRemark.Rows.Count > 0)
            {
                int count = int.Parse(MAX_agvRemark.Rows[0][0].ToString()) > 99 ? 200 : 100;
                Params.Log_Count_forRFID = new string[count];
                for (int i = 0; i < count; i++)
                {
                    Params.Log_Count_forRFID[i] = "0";
                }
                Params.Log_Count_forErr = new string[count];
                for (int i = 0; i < count; i++)
                {
                    Params.Log_Count_forErr[i] = "-1";
                }
            }

            ThreadSt_RGV = new ThreadStart(RunMain);
            Thread_RGV = new Thread(ThreadSt_RGV);
            Thread_RGV.Start();

            //ThreadStartPlc = new ThreadStart(RunPlc);
            //ThreadPlc = new Thread(ThreadStartPlc);
            //ThreadPlc.Start();

            mItems_RGVMonitor_Click(null, null);
            //电梯监控ToolStripMenuItem_Click(null, null);
            充电站监控ToolStripMenuItem_Click(null,null);

             
        }
        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Params.userName_now == "")
            {
                if (MessageBox.Show("请先登录拥有此操作权限的账户！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (frm_login == null || frm_login.IsDisposed)
                    {
                        frm_login = new frm_Login();
                        frm_login.TopMost = true;
                        frm_login.Show();
                        e.Cancel = true;
                        return;
                    }
                }
                e.Cancel = true;
                return;
            }
            if (MessageBox.Show("退出后将无法管控AGV，确认退出吗？", "警告！", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            bstop = true;
            try
            {
                ListenThread.Abort();
                Thread_RGV.Abort();
            }
            catch (Exception)
            {
            }
            timer1.Stop();
            LogFile.SaveLog_Start("[关闭] AGV管控系统已关闭..." + "\r\n");
            Application.ExitThread();
            Application.Exit();
        }

        #endregion

        #region Socket

        //开始监听
        private void Listen()
        {
            bstop = false;
            ListenThread = new Thread(StartListening);
            ListenThread.Start();
        }
        //委托监听程序
        private void StartListening()
        {
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, hostPORT);
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(localEndPoint);
            listener.Listen(100);
            while (!bstop)
            {
                allDone.Reset();
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                allDone.WaitOne();
                Thread.Sleep(1);
            }
        }
        //接受连接回调函数
        private void AcceptCallback(IAsyncResult ar)
        {
            if (bstop) return;
            allDone.Set();
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            IPEndPoint iep = (IPEndPoint)handler.RemoteEndPoint;
            string ip = iep.Address.ToString();
            lock (this.connectedSocket)
            {
                if (connectedSocket.ContainsKey(ip) == false)
                {
                    try
                    {
                        connectedSocket.Add(ip, handler);
                        if (!SocketStatus.ContainsKey(ip))
                            SocketStatus.Add(ip, 0);
                        StateObject state = new StateObject();
                        state.workSocket = handler;
                        ClientIps.Add(ip);
                        //设置心跳
                        byte[] inValue = new byte[] { 1, 0, 0, 0, 0x88, 0x13, 0, 0, 0xd0, 0x07, 0, 0 };// 首次探测时间5 秒, 间隔侦测时间5 秒
                        handler.IOControl(IOControlCode.KeepAliveValues, inValue, null);
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    try
                    {
                        connectedSocket[ip].Close();
                        connectedSocket.Remove(ip);
                        ClientIps.Remove(ip);
                        connectedSocket.Add(ip, handler);
                        if (!SocketStatus.ContainsKey(ip))
                            SocketStatus.Add(ip, 0);
                        StateObject state = new StateObject();
                        state.workSocket = handler;
                        ClientIps.Add(ip);
                        //每四个字节一个整数，第一个整数1表示启用，第二个整数0x1388表示设置以后过5000毫秒开始发送心跳，第三个整数0x07d0表示每2000毫秒发送一次心跳。
                        byte[] inValue = new byte[] { 1, 0, 0, 0, 0x88, 0x13, 0, 0, 0xd0, 0x07, 0, 0 };
                        //在接受方接受连接或者发起方连接成功之后，对该连接的套接字设置iocontrol
                        handler.IOControl(IOControlCode.KeepAliveValues, inValue, null);
                        //接收时间 
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                        //ShowMsg("检测到" + ip + "的连接...");
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        public static object ThreadControl_One = new object();
        //接受回调函数
        private void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            try
            {                
                IPEndPoint iep = (IPEndPoint)handler.RemoteEndPoint;
                string ip = iep.Address.ToString();
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    lock (ThreadControl_One)
                    {
                        ConvertMsg(ip, state.buffer, bytesRead);
                    }
                }
                if (!bstop)
                {
                    lock (this.connectedSocket)
                    {
                        if (bytesRead == 0)
                        {
                            //state.workSocket.Close();
                            //connectedSocket.Remove(ip);
                            //SocketStatus.Remove(ip);
                            //ClientIps.Remove(ip);
                        }
                        if (state.workSocket.Connected == true)
                        {
                            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                        }
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    //lock (this.connectedSocket)
                    //{
                    //    StateObject state = (StateObject)ar.AsyncState;
                    //    Socket handler = state.workSocket;
                    //    IPEndPoint iep = (IPEndPoint)handler.RemoteEndPoint;
                    //    string ip = iep.Address.ToString();
                    //    if (connectedSocket.ContainsKey(ip))
                    //    {
                    //        state.workSocket.Close();
                    //        this.connectedSocket.Remove(ip);
                    //        this.SocketStatus.Remove(ip);
                    //        this.ClientIps.Remove(ip); //Keepalive检测网线断开引发的异常在这里捕获
                    //    }
                    //}
                }
                catch (Exception)
                {
                }
                return;
            }
        }
        //Socket Server接收数据处理
        private void ConvertMsg(string ipaddr, byte[] buffer, int bytesRead)
        {
            try
            {
                string str_s = "";
                for (int i = 0; i < bytesRead; i++)
                {
                    str_s = str_s + buffer[i].ToString("X2") + " ";
                }
                //LogFile.SaveLog1("[" + ipaddr.Trim() + "]：" + str_s + "\r\n");
                if (Params.recLog)
                {
                    LogFile.SaveLog_Rec("[" + ipaddr.Trim() + "]：" + str_s + "\r\n", ipaddr);
                }
                if (bytesRead / 15 > 0 && bytesRead % 15 == 0)//AGV
                {
                    for (int s = 0; s < bytesRead / 15; s++)
                    {
                        if (buffer[s * 15 + 0] == 0xfd && buffer[s * 15 + 14] == 0xff)
                        {

                            if (!MACH_OLD.ContainsKey(ipaddr))
                            {
                                lock (ThreadControl_RGV)
                                {
                                    MACH_OLD.Add(ipaddr, 0);
                                }
                            }
                            else
                            {
                                lock (ThreadControl_RGV)
                                {
                                    MACH_OLD[ipaddr] = 0;
                                }
                            }

                            byte[] GetCRC = new byte[11];
                            Array.Copy(buffer, s * 15 + 1, GetCRC, 0, 11);
                            byte[] Get_CRC = CRC.CRC16(GetCRC);
                            if (buffer[s * 15 + 12] == Get_CRC[1] && buffer[s * 15 + 13] == Get_CRC[0])
                            {
                                string NowRFID = ((uint)(buffer[9] | buffer[8] << 8)).ToString();
                                string CarNo = buffer[1].ToString();
                                string ErrorCode = buffer[2].ToString();
                                string Power = ((uint)(buffer[4] | buffer[3] << 8)).ToString();
                                string Speed = buffer[5].ToString();
                                //string LineNo = ((uint)(buffer[7] | buffer[6] << 8)).ToString();
                                string LineNo = buffer[6].ToString();

                                int AGV_FW = (int)buffer[10];


                                //任务状态置为0
                                //DataTable task = Function.SELECT_RESETTASK(NowRFID, AGV_FW.ToString(), CarNo);
                                //if (task != null && task.Rows.Count > 0)
                                //{
                                //    DataTable res = Function.SELECT_AGV_IP_BYRemark(CarNo);
                                //    if (res != null && res.Rows.Count > 0)
                                //    {
                                //        Function.INSERT_Buffer_temp(res.Rows[0][0].ToString().Trim(), "0");
                                //    }
                                //}

                                try
                                {
                                    //地标读取日志
                                    if (Params.rfidLog)
                                    {
                                        if (Params.Log_Count_forRFID[int.Parse(CarNo) - 1] != NowRFID)
                                        {
                                            LogFile.SaveLog_RFID(" " + CarNo + "号AGV(IP:" + ipaddr + ")读取到地标：" + NowRFID + "\r\n", ipaddr);
                                            Params.Log_Count_forRFID[int.Parse(CarNo) - 1] = NowRFID;
                                        }
                                    }
                                    //2017年9月18:    比较地标值
                                    //DataTable rfid_resetAll = Function.SELECT_RFID_ResetAll(NowRFID);
                                    //if (rfid_resetAll != null && rfid_resetAll.Rows.Count > 0)
                                    //{
                                    //    try
                                    //    {
                                    //        //路口管控
                                    //        Function.UPDATE_Cross_cac_agv(int.Parse(CarNo));
                                    //        //电梯
                                    //        Function.UPDATE_ELC_InAGVNo(int.Parse(CarNo));
                                    //    }
                                    //    catch (Exception)
                                    //    {
                                    //        throw;
                                    //    }
                                    //}


                                    //电压日志
                                    if (Params.volLog)
                                    {
                                        LogFile.SaveLog_Vol(" " + CarNo + "号AGV(IP:" + ipaddr + ")当前电压值：" + Power + "\r\n", ipaddr);
                                    }
                                    //故障日志
                                    if (Params.errLog)
                                    {
                                        if (Params.Log_Count_forErr[int.Parse(CarNo) - 1] != ErrorCode)
                                        {
                                            LogFile.SaveLog_Err(" " + CarNo + "号AGV(IP:" + ipaddr + ")状态信息：" + Function.retErr(ErrorCode) + "\r\n", ipaddr);
                                            Params.Log_Count_forErr[int.Parse(CarNo) - 1] = ErrorCode;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    throw;
                                }

                                Function.INSERET_AGV_INFO(ipaddr, 1, NowRFID, "", "", LineNo, CarNo, ErrorCode, Power, Speed, AGV_FW);
                            }
                        }
                    }
                }
                //else if (bytesRead / 10 > 0 && bytesRead % 10 == 0)//电梯
                //{
                //    for (int s = 0; s < bytesRead / 10; s++)
                //    {
                //        if (buffer[s * 10 + 0] == 0xfd && buffer[s * 10 + 9] == 0xff)
                //        {

                //            if (!MACH_OLD.ContainsKey(ipaddr))
                //            {
                //                lock (ThreadControl_RGV)
                //                {
                //                    MACH_OLD.Add(ipaddr, 0);
                //                }
                //            }
                //            else
                //            {
                //                lock (ThreadControl_RGV)
                //                {
                //                    MACH_OLD[ipaddr] = 0;
                //                }
                //            }

                //            byte[] GetCRC = new byte[6];
                //            Array.Copy(buffer, s * 10 + 1, GetCRC, 0, 6);
                //            byte[] Get_CRC = CRC.CRC16(GetCRC);
                //            if (buffer[s * 10 + 7] == Get_CRC[1] && buffer[s * 10 + 8] == Get_CRC[0])
                //            {
                //                Function.UPDATE_ELC(ipaddr, 1, (int)buffer[3], (int)buffer[4], 1);//0:闭合  1:打开
                //                //2017年9月11日16:50:15：   新增电梯内障碍物检测
                //                Function.UPDATE_ELC_obsturction(ipaddr, (int)buffer[5]);
                //            }
                //        }
                //    }
                //}
                #region unused
                //else if (bytesRead / 8 > 0 && bytesRead % 8 == 0)//自动门
                //{
                //    for (int s = 0; s < bytesRead / 8; s++)
                //    {
                //        if (buffer[s * 8 + 0] == 0xfd && buffer[s * 8 + 7] == 0xff)
                //        {

                //            if (!MACH_OLD.ContainsKey(ipaddr))
                //            {
                //                lock (ThreadControl_RGV)
                //                {
                //                    MACH_OLD.Add(ipaddr, 0);
                //                }
                //            }
                //            else
                //            {
                //                lock (ThreadControl_RGV)
                //                {
                //                    MACH_OLD[ipaddr] = 0;
                //                }
                //            }

                //            byte[] GetCRC = new byte[4];
                //            Array.Copy(buffer, s * 8 + 1, GetCRC, 0, 4);
                //            byte[] Get_CRC = CRC.CRC16(GetCRC);
                //            if (buffer[s * 8 + 5] == Get_CRC[1] && buffer[s * 8 + 6] == Get_CRC[0])
                //            {
                //                Function.UPDATE_AutoDoor(ipaddr, (int)buffer[3]);
                //            }
                //        }
                //    }                
                //}
                else if (bytesRead / 7 > 0 && bytesRead % 7 == 0)//充电桩
                {
                    for (int s = 0; s < bytesRead / 7; s++)
                    {
                        if (buffer[s * 7 + 0] == 0xfd && buffer[s * 7 + 6] == 0xff)
                        {

                            if (!MACH_OLD.ContainsKey(ipaddr))
                            {
                                lock (ThreadControl_RGV)
                                {
                                    MACH_OLD.Add(ipaddr, 0);
                                }
                            }
                            else
                            {
                                lock (ThreadControl_RGV)
                                {
                                    MACH_OLD[ipaddr] = 0;
                                }
                            }

                            byte[] GetCRC = new byte[3];
                            Array.Copy(buffer, s * 7 + 1, GetCRC, 0, 3);
                            byte[] Get_CRC = CRC.CRC16(GetCRC);
                            if (buffer[s * 8 + 4] == Get_CRC[1] && buffer[s * 8 + 5] == Get_CRC[0])
                            {
                                Function.UPDATE_POWER(ipaddr, (int)buffer[3]);
                            }
                        }
                    }
                }
                //呼叫盒
                else if (bytesRead / 6 > 0 && bytesRead % 6 == 0)
                {
                    for (int s = 0; s < bytesRead / 6; s++)
                    {
                        if (buffer[s * 6 + 0] == 0xfd && buffer[s * 6 + 5] == 0xff)
                        {
                            byte[] GetCRC = new byte[2];
                            Array.Copy(buffer, s * 6 + 1, GetCRC, 0, 2);
                            byte[] Get_CRC = CRC.CRC16(GetCRC);

                            if (buffer[s * 6 + 3] == Get_CRC[1] && buffer[s * 6 + 4] == Get_CRC[0])
                            {
                                bool hasWriteDown = false;
                                //判断是否已经按下
                                DataTable DT_hasWriteDown = Function.SELECT_CallBoxLogic_byTask(buffer[1].ToString(), buffer[2].ToString());
                                if (DT_hasWriteDown != null && DT_hasWriteDown.Rows.Count > 0)
                                {
                                    if (!"0".Equals(DT_hasWriteDown.Rows[0][0].ToString()))
                                        hasWriteDown = true;
                                }
                                if (!hasWriteDown)
                                {
                                    //记录按下的时间
                                    Function.UPDATE_CallBoxLogic(buffer[1].ToString(), buffer[2].ToString(), DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss：ffff"));
                                    Function.UPDATE_CallBox(buffer[1].ToString(), "1");
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception)
            {
               
            }
        }

        #endregion

        #region Main

        private void RunMain()
        {
            while (!bstop)
            {
                //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                //stopwatch.Start();

                Thread.Sleep(100);
                DataTable AGV_info = Function.SELETE_AGV_INFO();
                if (AGV_info != null && AGV_info.Rows.Count > 0)
                {
                    for (int i = 0; i < AGV_info.Rows.Count; i++)
                    {
                        string AGV_No = AGV_info.Rows[i][7].ToString().Trim();//AGV编号
                        int AGV_Ac = int.Parse(AGV_info.Rows[i][2].ToString().Trim());//AGV状态
                        string AGV_RFID = AGV_info.Rows[i][3].ToString().Trim();//当前RFID
                        int AGV_FW = int.Parse(AGV_info.Rows[i][11].ToString().Trim());//方向
                        string AGV_FROM = AGV_info.Rows[i][4].ToString().Trim();//AGV当前任务
                        string AGV_IP = AGV_info.Rows[i][1].ToString().Trim();//AGVIP;
                        int AGV_Speed = int.Parse(AGV_info.Rows[i][10].ToString().Trim());//速度
                        int AGV_Power = int.Parse(AGV_info.Rows[i][9].ToString().Trim());//电量
                        int Order = 0;
                        int FW = 0;
                        int SendRoad = 0;
                        Cstring.Clear();
                        if (AGV_Ac == 1)//正常状态
                        {
                            DataTable AGV_order = Function.SELECT_AGV_ORDER(AGV_No);//查找与其他设备交互任务
                            if (AGV_order != null && AGV_order.Rows.Count > 0)
                            {
                                if (AutoControl)
                                {
                                    #region 交管
                                    for (int j = 0; j < AGV_order.Rows.Count; j++)
                                    {
                                        int OrdType = int.Parse(AGV_order.Rows[j][2].ToString().Trim());
                                        string OrderString = AGV_order.Rows[j][3].ToString().Trim();
                                        if (OrdType == 1)//交叉管控
                                        {
                                            DataTable GetCross = Function.SELECT_CROSS_INFO(int.Parse(AGV_No), AGV_RFID, AGV_FW, OrderString, AGV_IP);//查找对应交管任务
                                            if (GetCross != null && GetCross.Rows.Count > 0)
                                            {
                                                if (GetCross.Rows[0][0].ToString().Trim() != "-1")
                                                {
                                                    //发送让AGV行走命令
                                                    if (AGV_Speed == 2)
                                                    {
                                                        if (Order >= 0)
                                                        {
                                                            Order = 2;
                                                            FW = AGV_FW;
                                                            Cstring.Add(OrderString);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Order = -1;
                                                    FW = -1;
                                                    //2017年9月5日11:03:03：管控区域有车不走也发一种信号，AGV锁死，播报语音，防止人员误操作
                                                    SendAGV_ORD(int.Parse(AGV_No), 3, FW, SendRoad, AGV_IP);

                                                }
                                            }
                                        }
                                    }
                                    if (Order > 0)
                                    {
                                        for (int m = 0; m < Cstring.Count(); m++)
                                        {
                                            Function.UPDATE_AGV_INFO_TO(AGV_IP, Cstring[m]);
                                        }
                                    }
                                    #endregion

                                    #region 电梯
                                    //for (int j = 0; j < AGV_order.Rows.Count; j++)
                                    //{
                                    //    int OrdType = int.Parse(AGV_order.Rows[j][2].ToString().Trim());
                                    //    string OrderString = AGV_order.Rows[j][3].ToString().Trim();
                                    //    if (OrdType == 2)//电梯程序
                                    //    {
                                    //        DataTable GetElec = Function.SELECT_ELC_MAINLINENO(OrderString, AGV_IP);//查找对应电梯任务
                                    //        if (GetElec != null && GetElec.Rows.Count > 0)
                                    //        {
                                    //            string ELEIP = GetElec.Rows[0][1].ToString().Trim();
                                    //            string InRFID = GetElec.Rows[0][4].ToString().Trim();
                                    //            string OutRFID = GetElec.Rows[0][8].ToString().Trim();
                                    //            int InFW = int.Parse(GetElec.Rows[0][6].ToString().Trim());
                                    //            int InLouceng = int.Parse(GetElec.Rows[0][5].ToString().Trim());
                                    //            int OutLouceng = int.Parse(GetElec.Rows[0][7].ToString().Trim());
                                    //            DataTable ELE_INFO = Function.SELECT_ELC_IP(ELEIP);
                                    //            if (ELE_INFO != null && ELE_INFO.Rows.Count > 0)//查找相关电梯
                                    //            {
                                    //                //2017年9月11日16:53:41：   新增电梯内障碍物检测
                                    //                //如果有障碍物obsturction==true，跳过进电梯的逻辑（车，电梯）
                                    //                bool res_obsturction = false;//默认无障碍物
                                    //                if (string.IsNullOrEmpty(ELE_INFO.Rows[0][12].ToString().Trim()))
                                    //                {
                                    //                    res_obsturction = false;//无障碍物
                                    //                }
                                    //                else if (int.Parse(ELE_INFO.Rows[0][12].ToString().Trim()) == 0)
                                    //                {
                                    //                    res_obsturction = false;//无障碍物                                            
                                    //                }
                                    //                else
                                    //                {
                                    //                    res_obsturction = true;//有障碍物  
                                    //                }
                                    //                //---------------------------------------------------

                                    //                int ELE_No = int.Parse(ELE_INFO.Rows[0][1].ToString().Trim());
                                    //                int ELE_AC = int.Parse(ELE_INFO.Rows[0][10].ToString().Trim());//电梯状态
                                    //                int ELE_Louceng = int.Parse(ELE_INFO.Rows[0][8].ToString().Trim());//电梯楼层
                                    //                int ELE_CanIn = int.Parse(ELE_INFO.Rows[0][9].ToString().Trim());//是否可入
                                    //                int ELE_Lock = int.Parse(ELE_INFO.Rows[0][11].ToString().Trim());//锁定AGV编号，未锁定为0
                                    //                string ELE_RFID = ELE_INFO.Rows[0][6].ToString().Trim();//RFID标签
                                    //                int ELE_Out_FW = int.Parse(ELE_INFO.Rows[0][5].ToString().Trim());//出电梯AGV方向
                                    //                if (ELE_AC == 1 && (ELE_Lock == 0 | ELE_Lock == int.Parse(AGV_No)))//判断是否被AGV锁定,或者为当前AGV
                                    //                {
                                    //                    if (InRFID == AGV_RFID && AGV_FW == InFW)//在电梯入口
                                    //                    {
                                    //                        //2017年9月11日17:15:14：   判断是否有障碍物
                                    //                        if (!res_obsturction)
                                    //                        {
                                    //                            //无障碍物
                                    //                            if (InLouceng != ELE_Louceng | (InLouceng == ELE_Louceng && ELE_CanIn != 1))
                                    //                            {
                                    //                                if (Order >= 0 && FW >= 0)
                                    //                                {
                                    //                                    //发送电梯命令到InLouceng
                                    //                                    if (SendELE_ORD(ELE_No, InLouceng, ELEIP))
                                    //                                    {
                                    //                                        LogFile.SaveLog_Go("呼叫 " + ELE_No.ToString() + " 号电梯到 " + InLouceng.ToString() + " 楼" + "\r\n", ELEIP);

                                    //                                        Order = 0;
                                    //                                        FW = 0;
                                    //                                        //更新tb_ELE SagvNo
                                    //                                        Function.UPDATE_ELC_AGVNO(ELEIP, int.Parse(AGV_No));
                                    //                                        Function.UPDATE_AGV_INFO_FROM(AGV_IP, OrderString);
                                    //                                    }
                                    //                                }
                                    //                            }
                                    //                            if (InLouceng == ELE_Louceng && AGV_Speed == 2 && ELE_CanIn == 1)
                                    //                            {
                                    //                                //发送AGV进电梯命令
                                    //                                if (Order >= 0 && FW >= 0)
                                    //                                {
                                    //                                    Order = 2;
                                    //                                    FW = InFW;
                                    //                                }
                                    //                                //add： 2017年8月29日09:03:05
                                    //                                Function.UPDATE_ELC_AGVNO(ELEIP, int.Parse(AGV_No));
                                    //                                //更新tb_AGV_INFO FROM=OrderString
                                    //                                //add:  2017年9月4日15:00:55
                                    //                                Function.UPDATE_AGV_INFO_FROM(AGV_IP, OrderString);
                                    //                                //SendAGV_ORD(int.Parse(AGV_No), 2, InFW, AGV_IP);
                                    //                            }
                                    //                        }
                                    //                    }
                                    //                    else if (ELE_RFID == AGV_RFID)//在电梯待机点
                                    //                    {
                                    //                        if (AGV_FROM == OrderString)
                                    //                        {
                                    //                            if (OutLouceng != ELE_Louceng)
                                    //                            {
                                    //                                //发送电梯命令到outLouceng
                                    //                                SendELE_ORD(ELE_No, OutLouceng, ELEIP);
                                    //                                LogFile.SaveLog_Go("发送 " + ELE_No.ToString() + " 号电梯到 " + OutLouceng.ToString() + " 楼" + "\r\n", ELEIP);

                                    //                            }
                                    //                            else if (ELE_CanIn == 1 && AGV_Speed == 2)
                                    //                            {
                                    //                                //发送AGV出电梯命令
                                    //                                SendAGV_ORD(int.Parse(AGV_No), 2, ELE_Out_FW, 0, AGV_IP);
                                    //                                LogFile.SaveLog_Go("[出电梯] 给" + AGV_No + " 号AGV发送启动，方向为：" + ((ELE_Out_FW == 1) ? "正向" : "反向") + "\r\n", AGV_IP);

                                    //                            }
                                    //                        }
                                    //                    }
                                    //                    else if (OutRFID == AGV_RFID && AGV_FW == ELE_Out_FW && AGV_FROM == OrderString)
                                    //                    {
                                    //                        try
                                    //                        {
                                    //                            //发送电梯解锁命令
                                    //                            if (SendELE_ORD(ELE_No, 0, ELEIP))
                                    //                            {
                                    //                                LogFile.SaveLog_Go("解除 " + ELE_No.ToString() + " 号电梯的锁定 " + "\r\n", ELEIP);

                                    //                                //更新tb_AGV_INFO FROM=""
                                    //                                Function.UPDATE_AGV_INFO_FROM(AGV_IP, "");
                                    //                                //更新tb_ELE SagvNo=0
                                    //                                Function.UPDATE_ELC_AGVNO(ELEIP, 0);
                                    //                            }
                                    //                        }
                                    //                        catch (Exception)
                                    //                        {
                                    //                            LogFile.SaveLog_Start("解锁电梯指令发送异常..." + "\r\n");
                                    //                            throw;
                                    //                        }
                                    //                    }
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    #endregion

                                    #region unused
                                    #region 自动门
                                    //for (int j = 0; j < AGV_order.Rows.Count; j++)
                                    //{
                                    //    int OrdType = int.Parse(AGV_order.Rows[j][2].ToString().Trim());
                                    //    string OrderString = AGV_order.Rows[j][3].ToString().Trim();
                                    //    if (OrdType == 3)//自动门
                                    //    {
                                    //        DataTable GetAuto = Function.SELECT_AutoDoor_INFO_ORDER(AGV_RFID, AGV_FW, OrderString, AGV_IP);//查找对应交管任务
                                    //    }
                                    //}
                                    #endregion

                                    #region 充电点
                                    for (int j = 0; j < AGV_order.Rows.Count; j++)
                                    {
                                        int OrdType = int.Parse(AGV_order.Rows[j][2].ToString().Trim());
                                        string OrderString = AGV_order.Rows[j][3].ToString().Trim();
                                        if (OrdType == 4 && AGV_Speed == 2)//充电点
                                        {
                                            DataTable GetPower = Function.SELECT_Power_INFO_ORDER(AGV_RFID, AGV_FW, OrderString, AGV_IP);//查找对应交管任务
                                            if (GetPower != null && GetPower.Rows.Count > 0)
                                            {
                                                int P_NO = int.Parse(GetPower.Rows[0][1].ToString());
                                                string P_St_RFID = GetPower.Rows[0][4].ToString();
                                                string P_In_RFID = GetPower.Rows[0][7].ToString();
                                                int P_In_FW = int.Parse(GetPower.Rows[0][8].ToString());
                                                string P_IN_LineNo = GetPower.Rows[0][11].ToString();
                                                int P_Power_H = int.Parse(GetPower.Rows[0][12].ToString());
                                                int P_Power_L = int.Parse(GetPower.Rows[0][13].ToString());
                                                int P_Out_FW = int.Parse(GetPower.Rows[0][10].ToString());
                                                string P_Out_LineNo = GetPower.Rows[0][14].ToString();
                                                int P_InOut = int.Parse(GetPower.Rows[0][16].ToString());
                                                string P_AGV_IP = GetPower.Rows[0][17].ToString();
                                                //
                                                string P_IP = GetPower.Rows[0][2].ToString();
                                                int P_AC = int.Parse(GetPower.Rows[0][15].ToString());
                                                if (AGV_IP == P_AGV_IP | P_AGV_IP == "")
                                                {
                                                    //2017年9月4日20:02:46 防止伸出状态下AGV还进入（充电站掉线）
                                                    if (AGV_Power <= P_Power_L && AGV_RFID == P_In_RFID && P_InOut == 2)//AGV在待命点
                                                    {
                                                        //发送AGV到充电点
                                                        if (Order >= 0 && FW >= 0)
                                                        {
                                                            Order = 2;
                                                            FW = P_In_FW;
                                                            SendRoad = int.Parse(P_IN_LineNo);
                                                            if (Params.powerLog)
                                                                LogFile.SaveLog_Power("[" + P_IP.Trim() + "]:AGV(IP:" + P_AGV_IP + ")前往充电点" + "\r\n", P_IP);
                                                        }
                                                    }
                                                    //2017年9月4日20:02:33 防止不伸出-8
                                                    else if (AGV_Power - 8 <= P_Power_L && AGV_RFID == P_St_RFID)//AGV在充电点
                                                    {
                                                        //SendAGV_ORD(int.Parse(AGV_No), 2, 0, 99, AGV_IP);
                                                        if (P_InOut != 1)//充电站未充电
                                                        {
                                                            //发送充电信号
                                                            SendPower_ORD(P_NO, 1, OrderString);
                                                            if (Order >= 0 | FW >= 0)
                                                            {
                                                                Order = 2;
                                                                FW = 3;
                                                                SendRoad = int.Parse(P_IN_LineNo);
                                                                if (Params.powerLog)
                                                                    LogFile.SaveLog_Power("[" + P_IP.Trim() + "]:AGV(IP:" + P_AGV_IP + ")开始充电" + "\r\n", P_IP);
                                                            }
                                                        }
                                                    }
                                                    else if (AGV_Power >= P_Power_H && AGV_RFID == P_St_RFID)
                                                    {
                                                        if (P_InOut == 1)//充电站正充电
                                                        {
                                                            //发送停止充电信号
                                                            SendPower_ORD(P_NO, 2, OrderString);
                                                            if (Params.powerLog)
                                                                LogFile.SaveLog_Power("[" + P_IP.Trim() + "]:AGV(IP:" + P_AGV_IP + ")停止充电" + "\r\n", P_IP);

                                                            //2017年9月5日09:19:31 充电完成后，充电头缩回后电压下降，导致车不走
                                                            SendAGV_ORD(int.Parse(AGV_No), 2, 4, int.Parse(P_Out_LineNo), AGV_IP);
                                                            if (Params.powerLog)
                                                                LogFile.SaveLog_Power("[" + P_IP.Trim() + "]:AGV(IP:" + P_AGV_IP + ")离开充电点5s" + "\r\n", P_IP);
                                                        }
                                                        else
                                                        {
                                                            //发送AGV离开信号
                                                            Order = 2;
                                                            FW = P_Out_FW;
                                                            SendRoad = int.Parse(P_Out_LineNo);
                                                            if (Params.powerLog)
                                                                LogFile.SaveLog_Power("[" + P_IP.Trim() + "]:AGV(IP:" + P_AGV_IP + ")离开充电点0s" + "\r\n", P_IP);
                                                        }
                                                    }
                                                    //2017年9月4日11:44:31 注释掉：AGV待机耗电异常，充电站掉线
                                                    //else if (AGV_Power >= P_Power_L && AGV_RFID == P_St_RFID)
                                                    //{
                                                    //    if (P_InOut == 2)//充电站停止充电
                                                    //    {
                                                    //        //发送AGV离开信号
                                                    //        Order = 2;
                                                    //        FW = P_Out_FW;
                                                    //        SendRoad = int.Parse(P_Out_LineNo);
                                                    //        if (Params.powerLog)
                                                    //            LogFile.SaveLog_Power("[" + P_IP.Trim() + "]:AGV(IP:" + P_AGV_IP + ")离开充电点" + "\r\n", P_IP);

                                                    //    }
                                                    //}
                                                }
                                                if (AGV_Power >= P_Power_L && AGV_RFID == P_In_RFID)//AGV在待命点
                                                {
                                                    //发送AGV到充电点
                                                    if (Order >= 0 && FW >= 0)
                                                    {
                                                        Order = 2;
                                                        FW = P_In_FW;
                                                        SendRoad = 0;
                                                        if (Params.powerLog)
                                                            LogFile.SaveLog_Power("[" + P_IP.Trim() + "]:AGV(IP:" + P_AGV_IP + ")充电点放行..." + "\r\n", P_IP);

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region 待机点
                                    //for (int j = 0; j < AGV_order.Rows.Count; j++)
                                    //{
                                    //    int OrdType = int.Parse(AGV_order.Rows[j][2].ToString().Trim());
                                    //    string OrderString = AGV_order.Rows[j][3].ToString().Trim();
                                    //    if (OrdType == 5)//待机点
                                    //    {

                                    //    }
                                    //}
                                    #endregion
                                    #endregion
                                }
                            }
                            if (Order > 0 && FW > 0)
                            {
                                SendAGV_ORD(int.Parse(AGV_No), Order, FW, SendRoad, AGV_IP);
                                LogFile.SaveLog_Go("在地标 " + AGV_RFID + " 号给 " + AGV_No + " 号AGV发送启动，方向为：" + ((FW == 1) ? "正向" : "反向") + "\r\n", AGV_IP);

                            }
                        }
                    }
                }
                #region unused

                #region 查找自动门程序
                //DataTable SelectDoor = Function.SELECT_AuotDoor_INFO();
                //if (SelectDoor != null && SelectDoor.Rows.Count > 0)
                //{
                //    for (int i = 0; i < SelectDoor.Rows.Count; i++)
                //    {
                //        string AuotDoorNo = SelectDoor.Rows[i][1].ToString().Trim();
                //        string AutoDoorIP = SelectDoor.Rows[i][2].ToString().Trim();
                //        int AutoDoorAC = int.Parse(SelectDoor.Rows[i][9].ToString().Trim());
                //        string AotoDoorAGV = SelectDoor.Rows[i][10].ToString().Trim();
                //        if (AutoDoorAC == 1 && AotoDoorAGV != "")
                //        {
                //            //发送开门命令
                //            SendAuto_ORD(int.Parse(AuotDoorNo), 1, AutoDoorIP);
                //        }
                //    }
                //}
                #endregion

                #region 呼叫盒
                DataTable CallBoxLogic_info = Function.SELECT_CallBoxLogic();//IsDown!=0
                if (CallBoxLogic_info != null && CallBoxLogic_info.Rows.Count > 0)
                {
                    for (int i = 0; i < CallBoxLogic_info.Rows.Count; i++)
                    {
                        string callBoxNo = CallBoxLogic_info.Rows[i][1].ToString().Trim();
                        string AnJianNo = CallBoxLogic_info.Rows[i][2].ToString().Trim();
                        string iptoAGV = CallBoxLogic_info.Rows[i][3].ToString().Trim();
                        string road = CallBoxLogic_info.Rows[i][5].ToString().Trim();//task

                        DataTable CallBox_info = Function.SELECT_CallBox_ByCallBoxNo(callBoxNo);
                        string[] isRFID = null;
                        string ip = "";

                        if (CallBox_info != null && CallBox_info.Rows.Count > 0)
                        {
                            ip = CallBox_info.Rows[0][2].ToString().Trim();
                        }
                        else
                            return;

                        if (CallBox_info.Rows[0][7].ToString().Trim() == string.Empty)
                            return;
                        else
                        {
                            isRFID = CallBox_info.Rows[0][7].ToString().Trim().Split(',');
                        }
                        DataTable agv = Function.SELECT_AGV_BYIP(iptoAGV);
                        if (agv != null && agv.Rows.Count > 0)
                        {
                            int AGV_No = int.Parse(agv.Rows[0][7].ToString().Trim());
                            int AGV_FW = int.Parse(agv.Rows[0][11].ToString().Trim());
                            string AGV_RFID_Now = agv.Rows[0][3].ToString().Trim();

                            DataTable buffer_temp_order = Function.SELECT_Buffer_temp_order(iptoAGV);
                            if (buffer_temp_order != null && buffer_temp_order.Rows.Count > 0)
                            {
                                if (buffer_temp_order.Rows[0][0].ToString().Trim() == "0")//1:任务进行中，0：空闲
                                {

                                    //判断是否在待机点（RFID）,先按先处理
                                    if (Function.IsIn(isRFID, AGV_RFID_Now))
                                    {
                                        bool res_toCallBox = false;
                                        bool res_toAGV = false;
                                        int failCount = 0;
                                        while ((!res_toCallBox || !res_toAGV) && failCount < 100)//一直发直到成功
                                        {
                                            if (Send_to_CallBox(int.Parse(callBoxNo), int.Parse(AnJianNo), ip))
                                                res_toCallBox = true;
                                            else
                                                failCount++;
                                            if (Send_to_AGV_ByCallBox(AGV_No, 2, AGV_FW, int.Parse(road), iptoAGV))
                                                res_toAGV = true;
                                            else
                                                failCount++;
                                        }
                                        if (res_toCallBox && res_toAGV)
                                        {
                                            //清除IsDowm,按下状态为0
                                            Function.UPDATE_CallBoxLogic(callBoxNo, AnJianNo, "0");

                                            //任务置为1，任务进行中
                                            Function.INSERT_Buffer_temp(iptoAGV, "1");
                                            //按下状态为0
                                            //Function.UPDATE_CallBoc_SAC();
                                        }
                                    }
                                    else
                                    {
                                        //清除IsDowm
                                        Function.UPDATE_CallBoxLogic(callBoxNo, AnJianNo, "0");
                                    }
                                }

                            }
                        }
                    }
                }
                #endregion

                #endregion

                #region 网络状态
                lock (ThreadControl_RGV)
                {
                    if (MACH_OLD != null && MACH_OLD.Count > 0)
                    {
                        try
                        {
                            List<string> GetIP = new List<string>();
                            string F_ip = "";
                            //LogFile.SaveLog("M_old:-----------------" + MACH_OLD.Count.ToString());
                            for (int i = 0; i < MACH_OLD.Count; i++)
                            {
                                foreach (string ips in MACH_OLD.Keys)
                                {
                                    if (ips != F_ip && !GetIP.Contains(ips))
                                    {
                                        F_ip = ips;
                                        GetIP.Add(ips);
                                        break;
                                    }
                                }
                                if (F_ip != "")
                                {
                                    if (MACH_OLD[F_ip] <= 200)
                                    {
                                        MACH_OLD[F_ip]++;
                                    }
                                    if (MACH_OLD[F_ip] >= 100)
                                    {
                                        Function.UPDATE_tb_AGV_INFO(F_ip);
                                    }
                                    //LogFile.SaveLog(F_ip + ":" + MACH_OLD[F_ip]);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogFile.SaveLog(ex.ToString());
                        }
                    }
                }
                #endregion

                //stopwatch.Stop();
                //TimeSpan timespan = stopwatch.Elapsed;
                //double milliseconds = timespan.TotalMilliseconds;
                //if (milliseconds > 350)
                //{
                //    LogFile.SaveLog("Loop timeout:" + milliseconds.ToString() + "-------------------------");
                //}

            }
        }

        /// 循环与PLC通信
        /// <summary>
        /// 循环与PLC通信
        /// </summary>
        private void RunPlc()
        {
            axTX_MS_QL_PLC plcCurrent;
            bool isReadOk;
            int[] readData;
            while (!bstop)
            {
                foreach (string key in dicPlc.Keys)
                {
                    if (dicPlc[key].Plcstatus == PlcStatus.Run)
                    {
                        plcCurrent = dicPlc[key];
                        //出料PLC
                        if (key.StartsWith("O_"))
                        {
                            if (plcCurrent.ax_ReadBit("M1505", readCount, out readData))
                            {
                                if (readData.Contains(1))
                                {
                                    //呼叫AGV



                                }
                            }
                            if (plcCurrent.ax_ReadBit("M705", readCount, out readData))
                            {
                                if (readData.Contains(1))
                                {
                                    //推送托盘


                                }
                            }
                        }
                        //进料PLC
                        else
                        {
                            if (plcCurrent.ax_ReadBit("M1760", readCount, out readData) || plcCurrent.ax_ReadBit("M1761", readCount, out readData))
                            {
                                if (readData.Contains(1))
                                {
                                    //呼叫AGV



                                }
                            }
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        #endregion

        #region Timer

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mItems_RGVMonitor.Checked)
            {
                frm_car.GetInfo();
            }
            //if (mItems_CANModel.Checked)
            //{
            //    frm_autoDoorInfo.GetInfo();
            //}
            if (充电站监控ToolStripMenuItem.Checked)
            {
                frm_PowerInfo.GetInfo();
            }
            //if (电梯监控ToolStripMenuItem.Checked)
            //{
            //    frm_ELECInfo.GetInfo();
            //}
            
            ts_Lable.Text = DateTime.Now.ToString("yyyy年MM月dd日  HH:mm:ss");
            label25.Text = AutoControl == true ? "自动调度中..." : "停止调度中...";
            toolStripStatusLabel6.Text = AutoControl == true ? "自动调度中..." : "停止调度中...";
        }

        #endregion

        #region 数据发送函数

        /// <summary>
        /// 默认发送函数
        /// </summary>
        /// <param name="sendMessage"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool SendDefault(byte[] sendMessage, string ip)
        {
            bool ret = false;
            try
            {
                connectedSocket[ip].Send(sendMessage);
                string str = System.Text.Encoding.Default.GetString(sendMessage);
                string mes = "";
                for (int i = 0; i < sendMessage.Length; i++)
                {
                    mes = mes + sendMessage[i].ToString("X2") + " ";
                }
                //LogFile.SaveLog2("[" + ip.Trim() + "]:    发送成功,发送数据为：" + mes + "\r\n");
                if (Params.sendLog)
                {
                    LogFile.SaveLog_Send("[" + ip.Trim() + "]:    发送成功,发送数据为：" + mes + "\r\n", ip);
                }
                //lbMessInfo.ForeColor = Color.Black;
                //lbMessInfo.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "[" + ip.Trim() + "]:    发送成功,发送数据为：" + mes;
                //if (ip == Canmode_IP[0] && sendMessage[12] != 0xef)
                //{
                //    LogFile.SaveLog2("[" + ip.Trim() + "]:    发送成功,发送数据为：" + mes);
                //}
                ret = true;
            }
            catch (Exception)
            {
                //lbMessInfo.ForeColor = Color.Red;
                string mes = "";
                for (int i = 0; i < sendMessage.Length; i++)
                {
                    mes = mes + sendMessage[i].ToString().Trim() + " ";
                }
                //lbMessInfo.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "[" + ip.Trim() + "]:    ERR，发送失败:" + mes;
                //LogFile.SaveLog2("[" + ip.Trim() + "]:    ERR，发送失败:" + mes);
            }
            return ret;
        }
        private void SendAGV_ORD(int AGV_No, int AGV_order, int AGV_FW, int Road, string IP)
        {
            byte[] SendMESS = new byte[15];
            SendMESS[0] = 0xfd;
            SendMESS[1] = (byte)AGV_No;
            SendMESS[2] = 0x01;
            SendMESS[3] = (byte)AGV_order;
            SendMESS[4] = (byte)AGV_FW;
            SendMESS[5] = (byte)Road;
            SendMESS[6] = 0x00;
            SendMESS[7] = 0x00;
            SendMESS[8] = 0x00;
            SendMESS[9] = 0x00;
            SendMESS[10] = 0x00;
            SendMESS[11] = 0x00;
            SendMESS[12] = 0x00;
            SendMESS[13] = 0x00;
            SendMESS[14] = 0xff;
            byte[] GetCRC = new byte[11];
            Array.Copy(SendMESS, 1, GetCRC, 0, 11);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[12] = Get_CRC[1];
            SendMESS[13] = Get_CRC[0];
            SendDefault(SendMESS, IP);
        }
        private void SendPower_ORD(int PowerNo, int Order, string IP)
        {
            byte[] SendMESS = new byte[7];
            SendMESS[0] = 0xfd;
            SendMESS[1] = (byte)PowerNo;
            SendMESS[2] = 0x02;
            SendMESS[3] = (byte)Order;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0xff;
            byte[] GetCRC = new byte[3];
            Array.Copy(SendMESS, 1, GetCRC, 0, 3);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[4] = Get_CRC[1];
            SendMESS[5] = Get_CRC[0];
            SendDefault(SendMESS, IP);
        }
        private void SendAuto_ORD(int AutoNo, int Order, string IP)
        {
            byte[] SendMESS = new byte[8];
            SendMESS[0] = 0xfd;
            SendMESS[1] = (byte)AutoNo;
            SendMESS[2] = 0x02;
            SendMESS[3] = (byte)Order;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x00;
            SendMESS[7] = 0xff;
            byte[] GetCRC = new byte[4];
            Array.Copy(SendMESS, 1, GetCRC, 0, 4);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[5] = Get_CRC[1];
            SendMESS[6] = Get_CRC[0];
            SendDefault(SendMESS, IP);
        }
        private bool SendELE_ORD(int ELE_No, int louceng, string IP)
        {
            byte[] SendMESS = new byte[10];
            SendMESS[0] = 0xfd;
            SendMESS[1] = (byte)ELE_No;
            SendMESS[2] = 0x02;
            SendMESS[3] = (byte)louceng;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x00;
            SendMESS[7] = 0x00;
            SendMESS[8] = 0x00;
            SendMESS[9] = 0xff;
            byte[] GetCRC = new byte[6];
            Array.Copy(SendMESS, 1, GetCRC, 0, 6);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[7] = Get_CRC[1];
            SendMESS[8] = Get_CRC[0];
            return SendDefault(SendMESS, IP);
        }
        //呼叫盒 host to CallBox 
        private bool Send_to_CallBox(int CallBox_LNo, int AnJianNo, string IP)
        {
            byte[] SendMESS = new byte[6];
            SendMESS[0] = 0xfd;
            SendMESS[1] = (byte)CallBox_LNo;
            SendMESS[2] = (byte)AnJianNo;
            SendMESS[3] = 0x00;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0xff;

            byte[] GetCRC = new byte[2];
            Array.Copy(SendMESS, 1, GetCRC, 0, 2);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[3] = Get_CRC[1];
            SendMESS[4] = Get_CRC[0];
            bool res = SendDefault(SendMESS, IP);
            return res;
        }
        //呼叫盒 host to AGV 
        private bool Send_to_AGV_ByCallBox(int AGV_No, int AGV_order, int AGV_FW, int Road, string IP)
        {
            byte[] SendMESS = new byte[15];
            SendMESS[0] = 0xfd;
            SendMESS[1] = (byte)AGV_No;
            SendMESS[2] = 0x02;
            SendMESS[3] = (byte)AGV_order;
            SendMESS[4] = (byte)AGV_FW;
            SendMESS[5] = (byte)Road;
            SendMESS[6] = 0x00;
            SendMESS[7] = 0x00;
            SendMESS[8] = 0x00;
            SendMESS[9] = 0x00;
            SendMESS[10] = 0x00;
            SendMESS[11] = 0x00;
            SendMESS[12] = 0x00;
            SendMESS[13] = 0x00;
            SendMESS[14] = 0xff;
            byte[] GetCRC = new byte[11];
            Array.Copy(SendMESS, 1, GetCRC, 0, 11);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[12] = Get_CRC[1];
            SendMESS[13] = Get_CRC[0];
            bool res = SendDefault(SendMESS, IP);
            return res;
        }
        //中转区 host to AGV 
        private bool Send_to_AGV_ByBuffer(int AGV_No, int AGV_order, int AGV_FW, int Road, string IP)
        {
            byte[] SendMESS = new byte[15];
            SendMESS[0] = 0xfd;
            SendMESS[1] = (byte)AGV_No;
            SendMESS[2] = 0x02;
            SendMESS[3] = (byte)AGV_order;
            SendMESS[4] = (byte)AGV_FW;
            SendMESS[5] = (byte)Road;
            SendMESS[6] = 0x00;
            SendMESS[7] = 0x00;
            SendMESS[8] = 0x00;
            SendMESS[9] = 0x00;
            SendMESS[10] = 0x00;
            SendMESS[11] = 0x00;
            SendMESS[12] = 0x00;
            SendMESS[13] = 0x00;
            SendMESS[14] = 0xff;
            byte[] GetCRC = new byte[11];
            Array.Copy(SendMESS, 1, GetCRC, 0, 11);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[12] = Get_CRC[1];
            SendMESS[13] = Get_CRC[0];
            bool res = SendDefault(SendMESS, IP);
            return res;
        }

        #endregion

        #region 菜单
        private void 手动控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Params.userName_now == "")
            {
                if (MessageBox.Show("请先登录拥有此操作权限的账户！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (frm_login == null || frm_login.IsDisposed)
                    {
                        frm_login = new frm_Login();
                        //frm_login.MdiParent = this;
                        frm_login.StartPosition = FormStartPosition.Manual;
                        frm_login.TopMost = true;

                        frm_login.Show();
                    }

                }
                return;
            }
            手动控制ToolStripMenuItem.Checked = !手动控制ToolStripMenuItem.Checked;
            if (手动控制ToolStripMenuItem.Checked)
            {
                panel2.Visible = true;
            }
            else
            {
                panel2.Visible = false;
            }
        }

        private void aGV放行日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\Log_Go");
            }
            catch
            {
                MessageBox.Show("没有找到指定的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int agvNo = comboBox_agvNo.SelectedIndex + 1;
            int agvDir = comboBox_agvDir.SelectedIndex + 1;
            if (agvNo != 0 && agvDir != 0)
            {
                if (MessageBox.Show("即将给 " + agvNo.ToString() + " 号AGV发送" + ((agvDir == 1) ? " 正向启动 " : " 反向启动 ") + "任务，确认发送吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    //
                    SendAGV_ORD(agvNo, 2, agvDir, 0, "192.168.1." + (50 + agvNo).ToString());

                    MessageBox.Show("ok");
                }
            }
            else
            {
                MessageBox.Show("参数选择出错，请从下拉框重新选择！", "提示");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "开始调度")
            {
                button2.Text = "停止调度";
                AutoControl = true;
            }
            else
            {
                button2.Text = "开始调度";
                AutoControl = false;
            }
        }
        private void mItems_RGVMonitor_Click(object sender, EventArgs e)
        {
            mItems_RGVMonitor.Checked = !mItems_RGVMonitor.Checked;
            if (mItems_RGVMonitor.Checked)
            {
                frm_car.StartPosition = FormStartPosition.Manual;
                frm_car.Show();

            }
            else
            {
                frm_car.Hide();
            }
        }
        private void mItems_CANModel_Click(object sender, EventArgs e)
        {
            mItems_CANModel.Checked = !mItems_CANModel.Checked;
            if (mItems_CANModel.Checked)
            {
                //if (frm_mapshow != null)
                //{
                //    if (frm_mapshow.Visible && frm_mapshow.WindowState == FormWindowState.Maximized)
                //    {
                //        frm_mapshow.WindowState = FormWindowState.Normal;
                //    }
                //frm_canbus.Show();
                frm_autoDoorInfo.StartPosition = FormStartPosition.Manual;
                frm_autoDoorInfo.Show();
                //}
                //else
                //{
                //    MessageBox.Show("请先打开地图界面！（路径：系统——地图）");
                //}
            }
            else
            {
                //frm_canbus.Hide();
                frm_autoDoorInfo.Hide();

            }
        }
        private void mItems_OrdMessage_Click(object sender, EventArgs e)
        {

        }
        private void senToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendPower_ORD(1, 1, "192.168.0.81");
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //byte[] SendMESS = new byte[10];
            //SendMESS[0] = 0xfd;
            //SendMESS[1] = 0x02;
            //SendMESS[2] = 0x02;
            //SendMESS[3] = 0x03;
            //SendMESS[4] = 0x00;
            //SendMESS[5] = 0x00;
            //SendMESS[6] = 0x00;
            //SendMESS[7] = 0x00;
            //SendMESS[8] = 0x00;
            //SendMESS[9] = 0xff;
            //byte[] GetCRC = new byte[6];
            //Array.Copy(SendMESS, 1, GetCRC, 0, 6);
            //byte[] Get_CRC = CRC.CRC16(GetCRC);
            //SendMESS[7] = Get_CRC[1];
            //SendMESS[8] = Get_CRC[0];
            //SendDefault(SendMESS, "192.168.0.93");
            SendAGV_ORD(2, 2, 1, 99, "192.168.0.52");
        }
       
        private void 电梯监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            电梯监控ToolStripMenuItem.Checked = !电梯监控ToolStripMenuItem.Checked;
            if (电梯监控ToolStripMenuItem.Checked)
            {
                frm_ELECInfo.StartPosition = FormStartPosition.Manual;

                frm_ELECInfo.Show();
                frm_ELECInfo.Location = new Point(0, 390);
            }
            else
            {
                frm_ELECInfo.Hide();
            }
        }
        private void 充电站监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            充电站监控ToolStripMenuItem.Checked = !充电站监控ToolStripMenuItem.Checked;
            if (充电站监控ToolStripMenuItem.Checked)
            {
                frm_PowerInfo.StartPosition = FormStartPosition.Manual;

                frm_PowerInfo.Show();
                frm_PowerInfo.Location = new Point(0, 390);
            }
            else
            {
                frm_PowerInfo.Hide();
            }
        }
        private void 交通管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_crossInfo = new frm_Manage_CrossInfo();
            frm_crossInfo.MdiParent = this;
            frm_crossInfo.StartPosition = FormStartPosition.Manual;

            frm_crossInfo.Show();
            frm_crossInfo.Location = new Point(809, 0);

        }
        private void 区域管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Manage_Area area = new frm_Manage_Area();
            area.MdiParent = this;
            area.StartPosition = FormStartPosition.Manual;

            area.Show();
        }
        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            if (frm_login == null || frm_login.IsDisposed)
            {
                frm_login = new frm_Login();
                frm_login.MdiParent = this;
                frm_login.StartPosition = FormStartPosition.Manual;
                frm_login.TopMost = true;

                frm_login.Show();
            }
        }
        private void 接收日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\Log_Reveive");
            }
            catch
            {
                MessageBox.Show("没有找到指定的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void 发送日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\Log_Send");
            }
            catch
            {
                MessageBox.Show("没有找到指定的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
       
        private void 充电日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\Log_Charge");
            }
            catch
            {
                MessageBox.Show("没有找到指定的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_About frm_about = new frm_About();
            frm_about.MdiParent = this;
            frm_about.Show();
        }
        private void 帮助文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\AGV控制系统使用说明书.pdf");
            }
            catch
            {
                MessageBox.Show("没有找到指定的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void rFID读取日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\Log_RFID");
            }
            catch
            {
                MessageBox.Show("没有找到指定的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void aGV管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Manage_AGV frm_m_agv = new frm_Manage_AGV(this);
            frm_m_agv.MdiParent = this;
            frm_m_agv.StartPosition = FormStartPosition.Manual;

            frm_m_agv.Show();
        }
        private void mItems_RGV_Alams_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\Log_Error");
            }
            catch
            {
                MessageBox.Show("没有找到指定的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
       
        private void aGV电压日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\Log_Voltage");
            }
            catch
            {
                MessageBox.Show("没有找到指定的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Params.userName_now == "")
            {
                if (MessageBox.Show("请先登录拥有此操作权限的账户！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (frm_login == null || frm_login.IsDisposed)
                    {
                        frm_login = new frm_Login();
                        //frm_login.MdiParent = this;
                        frm_login.StartPosition = FormStartPosition.Manual;
                        frm_login.TopMost = true;

                        frm_login.Show();
                    }

                }
                return;
            }
            frm_Manage_Param frm = new frm_Manage_Param();
            frm.MdiParent = this;
            frm.StartPosition = FormStartPosition.Manual;

            frm.Show();
        }
        private void toolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            frm_Manage_OrderInfo frm_m_ord = new frm_Manage_OrderInfo();
            frm_m_ord.MdiParent = this;
            frm_m_ord.StartPosition = FormStartPosition.Manual;

            frm_m_ord.Show();
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            toolStripMenuItem6.Checked = !toolStripMenuItem6.Checked;
            if (toolStripMenuItem6.Checked)
            {

                frm_mapshow = new frm_map();
                frm_mapshow.MdiParent = this;
                frm_mapshow.Show();
            }
            else
            {
                frm_mapshow.Close();
            }
        }
       
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void 软件启停日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\Log_System");
            }
            catch
            {
                MessageBox.Show("没有找到指定的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
       
        private void toolStripStatusLabel5_Click(object sender, EventArgs e)
        {
            if (frm_login == null || frm_login.IsDisposed)
            {
                frm_login = new frm_Login();
                frm_login.MdiParent = this;
                frm_login.StartPosition = FormStartPosition.Manual;
                frm_login.TopMost = true;

                frm_login.Show();
            }
        }

        private void 车与线体对应关系ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            车与线体对应关系ToolStripMenuItem.Checked=!车与线体对应关系ToolStripMenuItem.Checked;
            if (车与线体对应关系ToolStripMenuItem.Checked)
            {
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
            }
        }

        #endregion
    }
}
