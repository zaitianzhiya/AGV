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
using HslCommunication.Profinet.Melsec;
using Mitsubishi_Q_L_PLC;
using TS_RGB.Fuction;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using FileControl;
using System.Net.NetworkInformation;
using MyHelpers;
using Color = System.Windows.Media.Color;
using HslCommunication;
using HslCommunication.Profinet;

namespace TS_RGB
{
    public partial class frm_Main : Form
    {
        #region Params
        public int lineNo = 1;//线别
        private string rfidCharge, rfidExchange, rfidWait, rfidIn1, rfidIn2, rfidOut;//充电，交换，待机RFID地标编号,进料点1RFID，进料点2RFID
        private string plcOut, plcIn1, plcIn2;//PLC信息，1个出料两个进料
        //private int readCount;//PLC读取位数
        //private Dictionary<string, axTX_MS_QL_PLC> dicPlc;//PLC对象集合
        private Dictionary<string, MelsecMcNet> dicPlc2;//PLC对象集合

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
            comboBox_agvNo.SelectedIndex = comboBox_agvDir.SelectedIndex = 0;
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
            lineNo =
                int.Parse(
                    SetFileControl.ReadIniValue("LINETYPE", "LINENO", Application.StartupPath + @"\AGV_Set.ini").Trim());
            rfidCharge = FileControl.SetFileControl.ReadIniValue("RFID", "RFIDCHARGE", Application.StartupPath + @"\AGV_Set.ini").Trim();
            rfidExchange = FileControl.SetFileControl.ReadIniValue("RFID", "RFIDEXCHANGE", Application.StartupPath + @"\AGV_Set.ini").Trim();
            rfidWait = FileControl.SetFileControl.ReadIniValue("RFID", "RFIDWAIT", Application.StartupPath + @"\AGV_Set.ini").Trim();
            rfidIn1 = FileControl.SetFileControl.ReadIniValue("RFID", "RFIDIN1", Application.StartupPath + @"\AGV_Set.ini").Trim();
            rfidIn2 = FileControl.SetFileControl.ReadIniValue("RFID", "RFIDIN2", Application.StartupPath + @"\AGV_Set.ini").Trim();
            rfidOut = FileControl.SetFileControl.ReadIniValue("RFID", "RFIDOUT", Application.StartupPath + @"\AGV_Set.ini").Trim();

            #region 添加PLC通信信息
            #region 方式1 公司DLL
            //plcOut = FileControl.SetFileControl.ReadIniValue("PLC", "PLCOUT", Application.StartupPath + @"\AGV_Set.ini").Trim();
            //plcIn1 = FileControl.SetFileControl.ReadIniValue("PLC", "PLCIN1", Application.StartupPath + @"\AGV_Set.ini").Trim();
            //plcIn2 = FileControl.SetFileControl.ReadIniValue("PLC", "PLCIN2", Application.StartupPath + @"\AGV_Set.ini").Trim();
            //readCount = int.Parse(FileControl.SetFileControl.ReadIniValue("PLC", "LINECOUNT", Application.StartupPath + @"\AGV_Set.ini").Trim());

            //dicPlc = new Dictionary<string, axTX_MS_QL_PLC>();
            //string[] plcArray;
            //if (!string.IsNullOrEmpty(plcOut))
            //{
            //    plcArray = plcOut.Split(':');
            //    if (plcArray.Length > 1)
            //    {
            //        axTX_MS_QL_PLC axTxMsQlPlc1 = new axTX_MS_QL_PLC(plcArray[0], int.Parse(plcArray[1]));
            //        axTxMsQlPlc1.OpenLinkPLC();
            //        if (!dicPlc.Keys.Contains("O_" + plcArray[0]))
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
            //        axTX_MS_QL_PLC axTxMsQlPlc2 = new axTX_MS_QL_PLC(plcArray[0], int.Parse(plcArray[1]));
            //        axTxMsQlPlc2.OpenLinkPLC();
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
            //        axTX_MS_QL_PLC axTxMsQlPlc3 = new axTX_MS_QL_PLC(plcArray[0], int.Parse(plcArray[1]));
            //        axTxMsQlPlc3.OpenLinkPLC();
            //        if (!dicPlc.Keys.Contains(plcArray[0]))
            //        {
            //            dicPlc[plcArray[0]] = axTxMsQlPlc3;
            //        }
            //    }
            //}
            #endregion

            #region 方式2 hslCommunication
            plcOut = FileControl.SetFileControl.ReadIniValue("PLC", "PLCOUT", Application.StartupPath + @"\AGV_Set.ini").Trim();
            plcIn1 = FileControl.SetFileControl.ReadIniValue("PLC", "PLCIN1", Application.StartupPath + @"\AGV_Set.ini").Trim();
            plcIn2 = FileControl.SetFileControl.ReadIniValue("PLC", "PLCIN2", Application.StartupPath + @"\AGV_Set.ini").Trim();

            dicPlc2 = new Dictionary<string, MelsecMcNet>();
            string[] plcArray;
            if (!string.IsNullOrEmpty(plcOut))
            {
                plcArray = plcOut.Split(':');
                if (plcArray.Length > 1)
                {
                    MelsecMcNet melsecMcNet = new MelsecMcNet(plcArray[0], int.Parse(plcArray[1]));
                    melsecMcNet.ConnectServer();
                    if (!dicPlc2.Keys.Contains("O_" + plcArray[0]))
                    {
                        dicPlc2["O_" + plcArray[0]] = melsecMcNet;
                    }
                }
            }
            if (!string.IsNullOrEmpty(plcIn1))
            {
                plcArray = plcIn1.Split(':');
                if (plcArray.Length > 1)
                {
                    MelsecMcNet melsecMcNet = new MelsecMcNet(plcArray[0], int.Parse(plcArray[1]));
                    melsecMcNet.ConnectServer();
                    if (!dicPlc2.Keys.Contains(plcArray[0]))
                    {
                        dicPlc2[plcArray[0]] = melsecMcNet;
                    }
                }
            }
            if (!string.IsNullOrEmpty(plcIn2))
            {
                plcArray = plcIn2.Split(':');
                if (plcArray.Length > 1)
                {
                    MelsecMcNet melsecMcNet = new MelsecMcNet(plcArray[0], int.Parse(plcArray[1]));
                    melsecMcNet.ConnectServer();
                    if (!dicPlc2.Keys.Contains(plcArray[0]))
                    {
                        dicPlc2[plcArray[0]] = melsecMcNet;
                    }
                }
            }
            #endregion
            #endregion

            //----------------------------------------------------------
            //------------------初始化Socket Server---------------------
            connectedSocket = new Dictionary<string, Socket> { };
            SocketStatus = new Dictionary<string, int> { };
            ClientIps = new List<string> { };
            ipAddress = Dns.GetHostAddresses(HostIP)[0];
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            string CheckIP = "";
            for (int i = 0; i < ipHostInfo.AddressList.Count(); i++)
            {
                if (ipHostInfo.AddressList[i].ToString().Trim().Contains(".") && ipHostInfo.AddressList[i].ToString().Trim().Count() < 16)
                {
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

            frm_car = new frm_Monitor_Car(this);
            frm_CallBoxInfo = new frm_Monitor_CallBox(this);
            frm_PowerInfo = new frm_Monitor_Power(this);

            frm_car.MdiParent = this;
            frm_CallBoxInfo.MdiParent = this;
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

            ThreadStartPlc = new ThreadStart(RunPlc);
            ThreadPlc = new Thread(ThreadStartPlc);
            ThreadPlc.Start();

            mItems_RGVMonitor_Click(null, null);
            充电站监控ToolStripMenuItem_Click(null, null);
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
                ThreadPlc.Abort();
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
                DateTime dtStart = DateTime.Now;
                string str_s = "";
                for (int i = 0; i < bytesRead; i++)
                {
                    str_s = str_s + buffer[i].ToString("X2") + " ";
                }
                if (Params.recLog)
                {
                    LogFile.SaveLog_Rec("[" + ipaddr.Trim() + "]：" + str_s + "\r\n", ipaddr);
                }
                if (bytesRead / 16 > 0 && bytesRead % 16 == 0)//AGV
                {
                    for (int s = 0; s < bytesRead / 16; s++)
                    {
                        if (buffer[s * 16 + 0] == 0xfd && buffer[s * 16 + 15] == 0xff)
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

                            byte[] GetCRC = new byte[12];
                            Array.Copy(buffer, s * 16 + 1, GetCRC, 0, 12);
                            byte[] Get_CRC = CRC.CRC16(GetCRC);
                            if (buffer[s * 16 + 13] == Get_CRC[1] && buffer[s * 16 + 14] == Get_CRC[0])
                            {
                                string NowRFID = ((uint)(buffer[9] | buffer[8] << 8)).ToString();
                                string CarNo = buffer[1].ToString();
                                string ErrorCode = buffer[2].ToString();
                                string Power = ((uint)(buffer[4] | buffer[3] << 8)).ToString();
                                string Speed = buffer[5].ToString();
                                //string LineNo = ((uint)(buffer[7] | buffer[6] << 8)).ToString();
                                string LineNo = buffer[6].ToString();
                                string isEmpty = buffer[11].ToString();//是否推料完成
                                isEmpty = string.IsNullOrEmpty(isEmpty) ? "0" : isEmpty;
                                string isPush = buffer[12].ToString();//是否推料
                                isPush = string.IsNullOrEmpty(isPush) ? "1" : isPush;

                                int AGV_FW = (int)buffer[10];

                                //地标读取日志
                                if (Params.rfidLog)
                                {
                                    if (Params.Log_Count_forRFID[int.Parse(CarNo) - 1] != NowRFID)
                                    {
                                        LogFile.SaveLog_RFID(" " + CarNo + "号AGV(IP:" + ipaddr + ")读取到地标：" + NowRFID + "\r\n", ipaddr);
                                        Params.Log_Count_forRFID[int.Parse(CarNo) - 1] = NowRFID;
                                    }
                                }

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

                                Function.INSERET_AGV_INFO(ipaddr, 1, NowRFID, "", "", LineNo, CarNo, ErrorCode, Power, Speed, AGV_FW, int.Parse(isEmpty), int.Parse(isPush), lineNo);
                            }
                        }
                    }
                }
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
                            if (buffer[s * 7 + 4] == Get_CRC[1] && buffer[s * 7 + 5] == Get_CRC[0])
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
                                    Function.UPDATE_CallBoxLogic(buffer[1].ToString(), buffer[2].ToString(), "1");
                                    Function.UPDATE_CallBox(buffer[1].ToString(), "1");
                                }
                            }
                        }
                    }
                }
                TimeSpan span = DateTime.Now - dtStart;
                double sen = span.TotalSeconds;
            }
            catch (Exception ex)
            {
                LogFile.SaveExceptionLog("ConvertMsg:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        #endregion

        #region Main

        private void RunMain()
        {
            while (!bstop)
            {
                try
                {
                    Thread.Sleep(100);

                    #region AGV是否需要充电
                    //DataTable AGV_info = Function.SELETE_AGV_INFO(lineNo);

                    //if (AGV_info != null && AGV_info.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < AGV_info.Rows.Count; i++)
                    //    {
                    //        string AGV_No = AGV_info.Rows[i][7].ToString().Trim();//AGV编号
                    //        int AGV_Ac = int.Parse(AGV_info.Rows[i][2].ToString().Trim());//AGV状态
                    //        string AGV_RFID = AGV_info.Rows[i][3].ToString().Trim();//当前RFID
                    //        string AGV_TO = AGV_info.Rows[i][5].ToString().Trim();//AGV目标RFID编号
                    //        string AGV_IP = AGV_info.Rows[i][1].ToString().Trim();//AGVIP;
                    //        int AGV_Speed = int.Parse(AGV_info.Rows[i][10].ToString().Trim());//速度
                    //        int AGV_Power = int.Parse(AGV_info.Rows[i][9].ToString().Trim());//电量
                    //        int AGV_ISACTIVE = int.Parse(AGV_info.Rows[i]["AGV_ISACTIVE"].ToString().Trim());//是否使用中
                    //        int AGV_ISEMPTY = int.Parse(AGV_info.Rows[i]["AGV_ISEMPTY"].ToString().Trim());//料架是否为空
                    //        int AGV_ISPUSH = int.Parse(AGV_info.Rows[i]["AGV_ISPUSH"].ToString().Trim());//料架是否伸出

                    //        DataTable dtPowInfo = Function.SELECT_Power_INFO(lineNo);
                    //        if (dtPowInfo != null && dtPowInfo.Rows.Count > 0)
                    //        {
                    //            //充电条件:在线，自动调度，停止中，空料架，料架没有伸出,电压达到下限,在待机点上,充电桩没有在充电
                    //            if (AGV_Ac == 1 && AutoControl && AGV_Speed == 2 && AGV_ISEMPTY == 0 && AGV_RFID == AGV_TO && AGV_ISPUSH == 0)
                    //            {
                    //                #region 电量不足
                    //                if (AGV_Power <= int.Parse(dtPowInfo.Rows[0]["P_power_L"].ToString()))
                    //                {
                    //                    //使用中的AGV
                    //                    if (AGV_ISACTIVE == 1)
                    //                    {
                    //                        if (AGV_RFID == rfidWait && dtPowInfo.Rows[0]["P_InOut"].ToString() != "1")
                    //                        {
                    //                            Function.PR_UPDATE_AGV_INFO_ISACTIVE(AGV_IP, rfidExchange);
                    //                            //发送命令去交换点
                    //                            //SendAGV_ORD(int.Parse(AGV_No), 2, 1,5, AGV_IP);
                    //                            SendAGV_ORD(int.Parse(AGV_No), 2, 1, 37, AGV_IP);
                    //                            LogFile.SaveLog_Go("在地标 " + AGV_RFID + " 号给 " + AGV_No + " 号AGV发送启动去交换点/充电点，方向为：正向" + "\r\n", AGV_IP);
                    //                            break;
                    //                        }
                    //                    }
                    //                    //未使用中的AGV
                    //                    else
                    //                    {
                    //                        //在充电点电量不足
                    //                        if (AGV_RFID == rfidCharge)
                    //                        {
                    //                            if (dtPowInfo.Rows[0]["P_InOut"].ToString() != "1")
                    //                            {
                    //                                Function.PR_UPDATE_POWER_AGVIP(AGV_IP,
                    //                                    dtPowInfo.Rows[0]["P_IP"].ToString());
                    //                                //打开AGV充电回路
                    //                                SendAGV_OPERORD(int.Parse(AGV_No), 3, AGV_IP);
                    //                                //发送充电信号
                    //                                SendPower_ORD(int.Parse(dtPowInfo.Rows[0]["PNo"].ToString()), 1, AGV_IP);
                    //                                if (Params.powerLog)
                    //                                {
                    //                                    LogFile.SaveLog_Power(
                    //                                        "[" + dtPowInfo.Rows[0]["P_IP"].ToString().Trim() +
                    //                                        "]:AGV(IP:" + AGV_IP + ")开始充电" + "\r\n",
                    //                                        dtPowInfo.Rows[0]["P_IP"].ToString());
                    //                                }
                    //                            }
                    //                        }
                    //                        else if (AGV_RFID == rfidExchange)
                    //                        {
                    //                            DataRow[] dataRows = AGV_info.Select(string.Format("AGV_IP!='{0}'", AGV_IP));

                    //                            if (dataRows.Length > 0)
                    //                            {
                    //                                DataRow drOther = dataRows[0];
                    //                                if (drOther["AGV_RFID_Now"].ToString() == rfidWait)
                    //                                {
                    //                                    Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidCharge);
                    //                                    //发送命令去充电点
                    //                                    //SendAGV_ORD(int.Parse(AGV_No), 2, 1, 6, AGV_IP);
                    //                                    SendAGV_ORD(int.Parse(AGV_No), 2, 1, 37, AGV_IP);
                    //                                    LogFile.SaveLog_Go("在地标 " + AGV_RFID + " 号给 " + AGV_No + " 号AGV发送启动去充电点，方向为：正向" + "\r\n", AGV_IP);
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //                #endregion

                    //                #region 电量充足
                    //                else
                    //                {
                    //                    if (AGV_RFID == rfidCharge)
                    //                    {
                    //                        if (AGV_Power >= int.Parse(dtPowInfo.Rows[0]["P_power_H"].ToString()) && dtPowInfo.Rows[0]["P_InOut"].ToString() == "1")
                    //                        {
                    //                            Function.PR_UPDATE_POWER_AGVIP("", dtPowInfo.Rows[0]["P_IP"].ToString());
                    //                            //关闭AGV充电回路
                    //                            SendAGV_OPERORD(int.Parse(AGV_No), 4, AGV_IP);
                    //                            //发送停止充电信号
                    //                            SendPower_ORD(int.Parse(dtPowInfo.Rows[0]["PNo"].ToString()), 2, AGV_IP);
                    //                            if (Params.powerLog)
                    //                            {
                    //                                LogFile.SaveLog_Power(
                    //                                    "[" + dtPowInfo.Rows[0]["P_IP"].ToString().Trim() + "]:AGV(IP:" + AGV_IP + ")停止充电" + "\r\n",
                    //                                    dtPowInfo.Rows[0]["P_IP"].ToString());
                    //                            }
                    //                        }
                    //                    }
                    //                    //停止充电并且AGV使用中才允许去待机点
                    //                    if (AGV_ISACTIVE == 1 && dtPowInfo.Rows[0]["P_InOut"].ToString() == "2")
                    //                    {
                    //                        DataRow[] dataRows = AGV_info.Select(string.Format("AGV_IP!='{0}'", AGV_IP));
                    //                        if (dataRows.Length > 0)
                    //                        {
                    //                            DataRow drOther = dataRows[0];
                    //                            //电量不足AGV已到交换点
                    //                            if (drOther["AGV_Speed"].ToString() == "2" &&
                    //                                drOther["AGV_AC"].ToString() == "1" &&
                    //                                drOther["AGV_TO"].ToString() == rfidExchange &&
                    //                                drOther["AGV_RFID_Now"].ToString() == rfidExchange)
                    //                            {
                    //                                Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidWait);
                    //                                //发送命令去待机点
                    //                                //SendAGV_ORD(int.Parse(AGV_No), 2, 2, 7, AGV_IP);
                    //                                SendAGV_ORD(int.Parse(AGV_No), 2, 2, 98, AGV_IP);
                    //                                LogFile.SaveLog_Go("在地标 " + AGV_RFID + " 号给 " + AGV_No + " 号AGV发送启动去待机点，方向为：反向" + "\r\n", AGV_IP);
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //                #endregion
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion

                    #region 呼叫盒
                    DataTable CallBoxLogic_info = Function.SELECT_CallBoxLogic(lineNo);//IsDown!=0
                    if (CallBoxLogic_info != null && CallBoxLogic_info.Rows.Count > 0)
                    {
                        for (int i = 0; i < CallBoxLogic_info.Rows.Count; i++)
                        {
                            string callBoxNo = CallBoxLogic_info.Rows[i][1].ToString().Trim();
                            string AnJianNo = CallBoxLogic_info.Rows[i][2].ToString().Trim();

                            DataTable CallBox_info = Function.SELECT_CallBox_ByCallBoxNo(callBoxNo, lineNo);
                            string[] isRFID = null;

                            if (CallBox_info == null || CallBox_info.Rows.Count <= 0)
                            {
                                break;
                            }

                            string ip = CallBox_info.Rows[0]["Sip"].ToString();
                            if (CallBox_info.Rows[0][7].ToString().Trim() == string.Empty)
                            {
                                break;
                            }
                            else
                            {
                                isRFID = CallBox_info.Rows[0][7].ToString().Trim().Split(',');
                            }
                            DataTable agv = Function.SELETE_AGV_INFO_ACTIVE(lineNo);
                            if (agv != null && agv.Rows.Count > 0)
                            {
                                int AGV_No = int.Parse(agv.Rows[0][7].ToString().Trim());
                                string AGV_RFID_Now = agv.Rows[0][3].ToString().Trim();
                                int AGV_Ac = int.Parse(agv.Rows[0][2].ToString().Trim()); //AGV状态
                                int AGV_Speed = int.Parse(agv.Rows[0][10].ToString().Trim()); //速度
                                string AGV_IP = agv.Rows[0][1].ToString().Trim(); //AGVIP;
                                int AGV_ISEMPTY = int.Parse(agv.Rows[0]["AGV_ISEMPTY"].ToString().Trim());//料架是否为空
                                int AGV_ISPUSH = int.Parse(agv.Rows[0]["AGV_ISPUSH"].ToString().Trim());//料架是否伸出
                                string AGV_TO = agv.Rows[0][5].ToString().Trim();//AGV目标RFID编号
                                int AGV_ISCALLBOX = int.Parse(agv.Rows[0]["AGV_ISCALLBOX"].ToString().Trim());//是否是呼叫盒操作

                                if (AGV_Ac == 1 && AutoControl && AGV_Speed == 2 && AGV_RFID_Now == AGV_TO && AGV_ISPUSH == 0)
                                {
                                    //呼叫
                                    if (AnJianNo == "1")
                                    {
                                        //在待机点并且小车为空车时才能呼叫
                                        if (AGV_RFID_Now == rfidWait && AGV_ISEMPTY == 0)
                                        {
                                            if (isRFID[0] == rfidIn1)
                                            {
                                                Function.PR_UPDATE_AGV_TO_CALLBOX(AGV_IP, rfidIn1, 1);
                                                //清除IsDowm,按下状态为0
                                                Function.UPDATE_CallBoxLogic(callBoxNo, AnJianNo, "0");

                                                Send_to_CallBox(int.Parse(callBoxNo), int.Parse(AnJianNo), ip);
                                                //SendAGV_ORD(AGV_No, 2, 2, 1, AGV_IP);
                                                SendAGV_ORD(AGV_No, 2, 2, int.Parse(rfidIn1), AGV_IP);
                                                LogFile.SaveLog_Go(
                                                    "呼叫盒:" + callBoxNo + "在地标 " + isRFID[0] + " 号给 " + AGV_No +
                                                    " 号AGV发送启动到1#进料口，方向为：反向" + "\r\n", AGV_IP);
                                                break;
                                            }
                                            if (isRFID[0] == rfidIn2)
                                            {
                                                Function.PR_UPDATE_AGV_TO_CALLBOX(AGV_IP, rfidIn2, 1);
                                                //清除IsDowm,按下状态为0
                                                Function.UPDATE_CallBoxLogic(callBoxNo, AnJianNo, "0");

                                                Send_to_CallBox(int.Parse(callBoxNo), int.Parse(AnJianNo), ip);
                                                //SendAGV_ORD(AGV_No, 2, 2, 3, AGV_IP);
                                                SendAGV_ORD(AGV_No, 2, 2, int.Parse(rfidIn2), AGV_IP);
                                                LogFile.SaveLog_Go(
                                                    "呼叫盒:" + callBoxNo + "在地标 " + isRFID[0] + " 号给 " + AGV_No +
                                                    " 号AGV发送启动到2#进料口，方向为：反向" + "\r\n", AGV_IP);
                                                break;
                                            }
                                        }
                                    }
                                    //放行
                                    else
                                    {
                                        if (AGV_RFID_Now != rfidWait)
                                        {
                                            if (AGV_ISCALLBOX == 1)
                                            {
                                                if (AGV_RFID_Now == isRFID[0])
                                                {
                                                    Function.PR_UPDATE_AGV_TO_CALLBOX(AGV_IP, rfidWait, 0);
                                                    //清除IsDowm,按下状态为0
                                                    Function.UPDATE_CallBoxLogic(callBoxNo, AnJianNo, "0");

                                                    Send_to_CallBox(int.Parse(callBoxNo), int.Parse(AnJianNo), ip);
                                                    //SendAGV_ORD(AGV_No, 2, 1, 2, AGV_IP);
                                                    SendAGV_ORD(AGV_No, 2, 1, int.Parse(rfidWait), AGV_IP);
                                                    LogFile.SaveLog_Go(
                                                        "呼叫盒:" + callBoxNo + "在地标 " + isRFID[0] + " 号给 " + AGV_No +
                                                        " 号AGV发送启动到出料口，方向为：正向" + "\r\n", AGV_IP);
                                                }
                                                else
                                                {
                                                    if (!Function.PR_SELECT_CALLBOX_FIRST_ISDOWN(callBoxNo, "1"))
                                                    {
                                                        //清除IsDowm,按下状态为0
                                                        Function.UPDATE_CallBoxLogic(callBoxNo, AnJianNo, "0");
                                                        Send_to_CallBox(int.Parse(callBoxNo), int.Parse(AnJianNo), ip);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (!Function.PR_SELECT_CALLBOX_FIRST_ISDOWN(callBoxNo, "1"))
                                                {
                                                    //清除IsDowm,按下状态为0
                                                    Function.UPDATE_CallBoxLogic(callBoxNo, AnJianNo, "0");
                                                    Send_to_CallBox(int.Parse(callBoxNo), int.Parse(AnJianNo), ip);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //清除IsDowm,按下状态为0
                                            Function.UPDATE_CallBoxLogic(callBoxNo, AnJianNo, "0");
                                            Send_to_CallBox(int.Parse(callBoxNo), int.Parse(AnJianNo), ip);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region 网络状态
                    lock (ThreadControl_RGV)
                    {
                        if (MACH_OLD != null && MACH_OLD.Count > 0)
                        {
                            List<string> GetIP = new List<string>();
                            string F_ip = "";
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
                                }
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    LogFile.SaveExceptionLog("RunMain:" + ex.Message + "\r\n" + ex.StackTrace);
                }
            }
        }

        /// 循环与PLC通信
        /// <summary>
        /// 循环与PLC通信
        /// </summary>
        private void RunPlc()
        {
            #region 方式1 公司DLL
            /*
            axTX_MS_QL_PLC plcCurrent;
            int[] readData;
            int[] writeContent = new int[] { 1 };
            while (!bstop)
            {
                try
                {
                    Thread.Sleep(100);
                    //if (dicPlc["192.168.1.190"].Plcstatus == PlcStatus.Run)
                    //{
                    //    bool s_Read_M = dicPlc["192.168.1.190"].ax_ReadBit_hex("M1150", readCount, out readData);
                    //    //if (s_Read_M)
                    //    //{
                    //    //    dicPlc["O_192.168.1.120"].ax_WriteBit_hex("M1505", readCount, ref writeContent);
                    //    //}
                    //}
                    //continue;
                    DataTable dtAgvActive = Function.SELETE_AGV_INFO_ACTIVE(lineNo);
                    if (dtAgvActive != null && dtAgvActive.Rows.Count == 1)
                    {
                        string AGV_No = dtAgvActive.Rows[0][7].ToString().Trim(); //AGV编号
                        int AGV_Ac = int.Parse(dtAgvActive.Rows[0][2].ToString().Trim()); //AGV状态
                        string AGV_RFID = dtAgvActive.Rows[0][3].ToString().Trim(); //当前RFID
                        int AGV_FW = int.Parse(dtAgvActive.Rows[0][11].ToString().Trim()); //方向
                        string AGV_IP = dtAgvActive.Rows[0][1].ToString().Trim(); //AGVIP;
                        int AGV_Speed = int.Parse(dtAgvActive.Rows[0][10].ToString().Trim()); //速度
                        int AGV_ISEMPTY = int.Parse(dtAgvActive.Rows[0]["AGV_ISEMPTY"].ToString().Trim()); //料架是否为空
                        string AGV_TO = dtAgvActive.Rows[0][5].ToString().Trim();//AGV目标RFID编号
                        int AGV_Ispush = int.Parse(dtAgvActive.Rows[0]["AGV_ISPUSH"].ToString().Trim()); //是否推送完成
                        if (AGV_Ac == 1 && AutoControl && AGV_Speed == 2 && AGV_TO == AGV_RFID)
                        {
                            foreach (string key in dicPlc.Keys)
                            {
                                if (dicPlc[key].Plcstatus == PlcStatus.Run)
                                {
                                    plcCurrent = dicPlc[key];

                                    #region 出料PLC

                                    if (key.StartsWith("O_"))
                                    {
                                        if (plcCurrent.ax_ReadBit_hex("M1505", readCount, out readData))
                                        {
                                            if (readData.Contains(1))
                                            {
                                                if (Params.plcLog)
                                                {
                                                    LogFile.SavePlcLog("出料口呼叫AGV");
                                                }
                                                if (AGV_RFID == rfidWait)
                                                {
                                                    //料架上有料时执行
                                                    if (AGV_ISEMPTY == 1)
                                                    {
                                                        plcCurrent.ax_WriteBit_hex("M704", readCount, ref writeContent);
                                                        if (Params.plcLog)
                                                        {
                                                            LogFile.SavePlcLog("出料口写入到位");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (plcCurrent.ax_ReadBit_hex("M705", readCount, out readData))
                                        {
                                            if (readData.Contains(1))
                                            {
                                                if (Params.plcLog)
                                                {
                                                    LogFile.SavePlcLog("出料口推送托盘");
                                                }
                                                if (AGV_RFID == rfidWait)
                                                {
                                                    //料架上有料时执行
                                                    if (AGV_ISEMPTY == 1)
                                                    {
                                                        if (AGV_Ispush == 0)
                                                        {
                                                            //推送托盘
                                                            SendAGV_OPERORD(int.Parse(AGV_No), 1, AGV_IP);
                                                            LogFile.SaveLog_Go(
                                                                "在地标 " + rfidWait + " 号给 " + AGV_No + " 号AGV发送推送托盘" +
                                                                "\r\n", AGV_IP);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (AGV_Ispush == 1)
                                                        {
                                                            //收缩托盘
                                                            SendAGV_OPERORD(int.Parse(AGV_No), 2, AGV_IP);
                                                            LogFile.SaveLog_Go(
                                                                "在地标 " + AGV_RFID + " 号给 " + AGV_No + " 号AGV发送启动收缩托盘" +
                                                                "\r\n", AGV_IP);
                                                        }
                                                        else if (AGV_Ispush == 0)
                                                        {
                                                            //完成后写入标志
                                                            plcCurrent.ax_WriteBit_hex("M706", readCount,
                                                                ref writeContent);
                                                            if (Params.plcLog)
                                                            {
                                                                LogFile.SavePlcLog("出料口写入完成");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region 进料PLC
                                    else
                                    {
                                        //进料点
                                        if (plcCurrent.ax_ReadBit_hex("M1150", readCount, out readData))
                                        {
                                            if (readData.Contains(1))
                                            {
                                                //进料点B
                                                if (key == plcIn1.Split(':')[0])
                                                {
                                                    if (Params.plcLog)
                                                    {
                                                        LogFile.SavePlcLog("1#进料口呼叫AGV");
                                                    }
                                                    //料架上无料时执行
                                                    if (AGV_ISEMPTY == 0)
                                                    {
                                                        if (AGV_RFID == rfidWait && AGV_Ispush == 0)
                                                        {
                                                            //呼叫AGV
                                                            Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidIn1);
                                                            //发送命令去进料点1
                                                            //SendAGV_ORD(int.Parse(AGV_No), 2, 2, 1, AGV_IP);
                                                            SendAGV_ORD(int.Parse(AGV_No), 2, 2, int.Parse(rfidIn1),
                                                                AGV_IP);
                                                            LogFile.SaveLog_Go(
                                                                "在地标 " + AGV_RFID + " 号给 " + AGV_No +
                                                                " 号AGV发送启动去1#进料点，方向为：反向" + "\r\n", AGV_IP);
                                                        }
                                                        else if (AGV_RFID == rfidIn1 && AGV_Ispush == 0)
                                                        {
                                                            //写入到位标志
                                                            plcCurrent.ax_WriteBit_hex("M1151", readCount,
                                                                ref writeContent);
                                                            if (Params.plcLog)
                                                            {
                                                                LogFile.SavePlcLog("1#进料口写入到位");
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //进料点C
                                                    if (Params.plcLog)
                                                    {
                                                        LogFile.SavePlcLog("2#进料口呼叫AGV");
                                                    }
                                                    //料架上无料时执行
                                                    if (AGV_ISEMPTY == 0)
                                                    {
                                                        if (AGV_RFID == rfidWait && AGV_Ispush == 0)
                                                        {
                                                            //呼叫AGV
                                                            Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidIn2);
                                                            //发送命令去进料点2
                                                            //SendAGV_ORD(int.Parse(AGV_No), 2, 2, 3, AGV_IP);
                                                            SendAGV_ORD(int.Parse(AGV_No), 2, 2, int.Parse(rfidIn2),
                                                                AGV_IP);
                                                            LogFile.SaveLog_Go(
                                                                "在地标 " + AGV_RFID + " 号给 " + AGV_No +
                                                                " 号AGV发送启动去2#进料点，方向为：反向" + "\r\n", AGV_IP);
                                                        }
                                                        else if (AGV_RFID == rfidIn2 && AGV_Ispush == 0)
                                                        {
                                                            //写入到位标志
                                                            plcCurrent.ax_WriteBit_hex("M1151", readCount,
                                                                ref writeContent);
                                                            if (Params.plcLog)
                                                            {
                                                                LogFile.SavePlcLog("2#进料口写入到位");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (plcCurrent.ax_ReadBit_hex("M1152", readCount, out readData))
                                        {
                                            if (readData.Contains(1))
                                            {
                                                //进料点B
                                                if (key == plcIn1.Split(':')[0])
                                                {
                                                    if (Params.plcLog)
                                                    {
                                                        LogFile.SavePlcLog("1#进料口读取进料完成");
                                                    }
                                                    if (AGV_RFID == rfidIn1)
                                                    {
                                                        Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidWait);
                                                        //SendAGV_ORD(int.Parse(AGV_No), 2, 1, 2, AGV_IP);
                                                        SendAGV_ORD(int.Parse(AGV_No), 2, 1, int.Parse(rfidWait), AGV_IP);
                                                        LogFile.SaveLog_Go(
                                                            "在地标 " + AGV_RFID + " 号给 " + AGV_No + " 号AGV发送启动回待机点，方向为：正向" +
                                                            "\r\n", AGV_IP);
                                                    }
                                                }
                                                else
                                                {
                                                    //进料点C
                                                    if (Params.plcLog)
                                                    {
                                                        LogFile.SavePlcLog("2#进料口读取进料完成");
                                                    }
                                                    if (AGV_RFID == rfidIn2)
                                                    {
                                                        Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidWait);
                                                        //SendAGV_ORD(int.Parse(AGV_No), 2, 1, 4, AGV_IP);
                                                        SendAGV_ORD(int.Parse(AGV_No), 2, 1, int.Parse(rfidWait), AGV_IP);
                                                        LogFile.SaveLog_Go(
                                                            "在地标 " + AGV_RFID + " 号给 " + AGV_No + " 号AGV发送启动回待机点，方向为：正向" +
                                                            "\r\n", AGV_IP);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogFile.SaveExceptionLog("RunPlc:" + ex.Message + "\r\n" + ex.StackTrace);
                }
            }
            */
            #endregion

            #region 方式2 hslCommunication
            MelsecMcNet plcCurrent;
            //bool[] writeContent = new bool[] { true };
            bool writeContent = true;

            while (!bstop)
            {
                try
                {
                    DateTime dtStart = DateTime.Now;
                    Thread.Sleep(100);
                    //plcCurrent = dicPlc2["192.168.1.160"];
                    //bool s = plcCurrent.ReadBool("M1152").Content;

                    //plcCurrent.Write("M1151", true);
                    //continue;
                    DataTable dtAgvActive = Function.SELETE_AGV_INFO_ACTIVE(lineNo);
                    if (dtAgvActive != null && dtAgvActive.Rows.Count == 1)
                    {
                        string AGV_No = dtAgvActive.Rows[0][7].ToString().Trim(); //AGV编号
                        int AGV_Ac = int.Parse(dtAgvActive.Rows[0][2].ToString().Trim()); //AGV状态
                        string AGV_RFID = dtAgvActive.Rows[0][3].ToString().Trim(); //当前RFID
                        string AGV_IP = dtAgvActive.Rows[0][1].ToString().Trim(); //AGVIP;
                        int AGV_Speed = int.Parse(dtAgvActive.Rows[0][10].ToString().Trim()); //速度
                        int AGV_ISEMPTY = int.Parse(dtAgvActive.Rows[0]["AGV_ISEMPTY"].ToString().Trim()); //料架是否为空
                        string AGV_TO = dtAgvActive.Rows[0][5].ToString().Trim();//AGV目标RFID编号
                        int AGV_Ispush = int.Parse(dtAgvActive.Rows[0]["AGV_ISPUSH"].ToString().Trim()); //是否推送完成
                        if (AGV_Ac == 1 && AutoControl && AGV_Speed == 2 && AGV_TO == AGV_RFID)
                        {
                            foreach (string key in dicPlc2.Keys)
                            {
                                plcCurrent = dicPlc2[key];
                                //if (plcCurrent.ConnectServer().IsSuccess)
                                //{
                                #region 出料PLC
                                if (key.StartsWith("O_"))
                                {
                                    if (plcCurrent.ReadBool("M1505").Content)
                                    {
                                        if (Params.plcLog)
                                        {
                                            LogFile.SavePlcLog("出料口呼叫AGV");
                                        }
                                        if (AGV_RFID == rfidWait)
                                        {
                                            //料架上有料时执行
                                            if (AGV_ISEMPTY == 1)
                                            {
                                                plcCurrent.Write("M704", writeContent);
                                                if (Params.plcLog)
                                                {
                                                    LogFile.SavePlcLog("出料口写入到位");
                                                }
                                            }
                                        }
                                    }
                                    if (plcCurrent.ReadBool("M705").Content)
                                    {
                                        if (Params.plcLog)
                                        {
                                            LogFile.SavePlcLog("出料口推送托盘");
                                        }
                                        if (AGV_RFID == rfidWait)
                                        {
                                            //料架上有料时执行
                                            if (AGV_ISEMPTY == 1)
                                            {
                                                if (AGV_Ispush == 0)
                                                {
                                                    //推送托盘
                                                    SendAGV_OPERORD(int.Parse(AGV_No), 1, AGV_IP);
                                                    LogFile.SaveLog_Go("在地标 " + rfidWait + " 号给 " + AGV_No + " 号AGV发送推送托盘" + "\r\n", AGV_IP);
                                                }
                                            }
                                            else
                                            {
                                                if (AGV_Ispush == 1)
                                                {
                                                    //收缩托盘
                                                    SendAGV_OPERORD(int.Parse(AGV_No), 2, AGV_IP);
                                                    LogFile.SaveLog_Go("在地标 " + AGV_RFID + " 号给 " + AGV_No + " 号AGV发送启动收缩托盘" + "\r\n", AGV_IP);
                                                }
                                                else if (AGV_Ispush == 0)
                                                {
                                                    //完成后写入标志
                                                    plcCurrent.Write("M706", writeContent);
                                                    if (Params.plcLog)
                                                    {
                                                        LogFile.SavePlcLog("出料口写入完成");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region 进料PLC
                                else
                                {
                                    if (plcCurrent.ReadBool("M1150").Content)
                                    {
                                        //进料点B
                                        if (key == plcIn1.Split(':')[0])
                                        {
                                            if (Params.plcLog)
                                            {
                                                LogFile.SavePlcLog("1#进料口呼叫AGV");
                                            }
                                            //料架上无料时执行
                                            if (AGV_ISEMPTY == 0)
                                            {
                                                if (AGV_RFID == rfidWait && AGV_Ispush == 0)
                                                {
                                                    //呼叫AGV
                                                    Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidIn1);
                                                    //发送命令去进料点1
                                                    //SendAGV_ORD(int.Parse(AGV_No), 2, 2, 1, AGV_IP);
                                                    SendAGV_ORD(int.Parse(AGV_No), 2, 2, int.Parse(rfidIn1), AGV_IP);
                                                    LogFile.SaveLog_Go(
                                                        "在地标 " + AGV_RFID + " 号给 " + AGV_No +
                                                        " 号AGV发送启动去1#进料点，方向为：反向" + "\r\n", AGV_IP);
                                                }
                                                else if (AGV_RFID == rfidIn1 && AGV_Ispush == 0)
                                                {
                                                    //写入到位标志
                                                    plcCurrent.Write("M1151", writeContent);
                                                    if (Params.plcLog)
                                                    {
                                                        LogFile.SavePlcLog("1#进料口写入到位");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //进料点C
                                            if (Params.plcLog)
                                            {
                                                LogFile.SavePlcLog("2#进料口呼叫AGV");
                                            }
                                            //料架上无料时执行
                                            if (AGV_ISEMPTY == 0)
                                            {
                                                if (AGV_RFID == rfidWait && AGV_Ispush == 0)
                                                {
                                                    //呼叫AGV
                                                    Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidIn2);
                                                    //发送命令去进料点2
                                                    //SendAGV_ORD(int.Parse(AGV_No), 2, 2, 3, AGV_IP);
                                                    SendAGV_ORD(int.Parse(AGV_No), 2, 2, int.Parse(rfidIn2), AGV_IP);
                                                    LogFile.SaveLog_Go(
                                                        "在地标 " + AGV_RFID + " 号给 " + AGV_No +
                                                        " 号AGV发送启动去2#进料点，方向为：反向" + "\r\n", AGV_IP);
                                                }
                                                else if (AGV_RFID == rfidIn2 && AGV_Ispush == 0)
                                                {
                                                    //写入到位标志
                                                    plcCurrent.Write("M1151", writeContent);
                                                    if (Params.plcLog)
                                                    {
                                                        LogFile.SavePlcLog("2#进料口写入到位");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (plcCurrent.ReadBool("M1152").Content)
                                    {
                                        //进料点B
                                        if (key == plcIn1.Split(':')[0])
                                        {
                                            if (Params.plcLog)
                                            {
                                                LogFile.SavePlcLog("1#进料口读取进料完成");
                                            }
                                            if (AGV_RFID == rfidIn1)
                                            {
                                                Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidWait);
                                                //SendAGV_ORD(int.Parse(AGV_No), 2, 1, 2, AGV_IP);
                                                SendAGV_ORD(int.Parse(AGV_No), 2, 1, int.Parse(rfidWait), AGV_IP);
                                                LogFile.SaveLog_Go(
                                                    "在地标 " + AGV_RFID + " 号给 " + AGV_No + " 号AGV发送启动回待机点，方向为：正向" +
                                                    "\r\n", AGV_IP);
                                            }
                                        }
                                        else
                                        {
                                            //进料点C
                                            if (Params.plcLog)
                                            {
                                                LogFile.SavePlcLog("2#进料口读取进料完成");
                                            }
                                            if (AGV_RFID == rfidIn2)
                                            {
                                                Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidWait);
                                                //SendAGV_ORD(int.Parse(AGV_No), 2, 1, 4, AGV_IP);
                                                SendAGV_ORD(int.Parse(AGV_No), 2, 1, int.Parse(rfidWait), AGV_IP);
                                                LogFile.SaveLog_Go("在地标 " + AGV_RFID + " 号给 " + AGV_No + " 号AGV发送启动回待机点，方向为：正向" + "\r\n", AGV_IP);
                                            }
                                        }
                                    }
                                }
                                #endregion
                                //}
                            }
                        }
                    }
                    TimeSpan span = DateTime.Now - dtStart;
                    double s = span.TotalSeconds;
                }
                catch (Exception ex)
                {
                    LogFile.SaveExceptionLog("RunPlc:" + ex.Message + "\r\n" + ex.StackTrace);
                }
            }
            
            #endregion
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

        /// 货架推送，打开/关闭充电回路指令
        /// <summary>
        /// 货架推送，打开/关闭充电回路指令
        /// </summary>
        /// <param name="AGV_No"></param>
        /// <param name="operationType">动作类型1-推货架 2-缩货架 3-打开充电回路 4-关闭充电回路</param>
        /// <param name="IP"></param>
        private void SendAGV_OPERORD(int AGV_No, int operationType, string IP)
        {
            byte[] SendMESS = new byte[15];
            SendMESS[0] = 0xfd;
            SendMESS[1] = (byte)AGV_No;
            SendMESS[2] = 0x02;
            SendMESS[3] = 0x00;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x00;
            SendMESS[7] = 0x00;
            SendMESS[8] = 0x00;
            SendMESS[9] = (byte)operationType;
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
                if (MessageBox.Show("即将给 " + agvNo.ToString() + " 号AGV发送指令：" + comboBox_agvDir.Text + "，确认发送吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    //呼叫AGV
                    string AGV_IP = "192.168.1." + (192 + agvNo).ToString();
                    switch (agvDir)
                    {
                        case 1:
                            //A---B
                            Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidIn1);
                            //发送命令去进料点1
                            //SendAGV_ORD(agvNo, 2, 2, 1, AGV_IP);
                            SendAGV_ORD(agvNo, 2, 2, int.Parse(rfidIn1), AGV_IP);
                            break;
                        case 2:
                            //A---C
                            Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidIn2);
                            //发送命令去进料点1
                            //SendAGV_ORD(agvNo, 2, 2, 3, AGV_IP);
                            SendAGV_ORD(agvNo, 2, 2, int.Parse(rfidIn2), AGV_IP);
                            break;
                        case 3:
                            //B---A   C---A
                            Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidWait);
                            //SendAGV_ORD(agvNo, 2, 1, 2, AGV_IP);
                            SendAGV_ORD(agvNo, 2, 1, int.Parse(rfidWait), AGV_IP);
                            break;
                        case 4:
                            //A/D---E
                            Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidCharge);
                            SendAGV_ORD(agvNo, 2, 1, 37, AGV_IP);
                            break;
                        case 5:
                            //E---A
                            Function.UPDATE_AGV_INFO_TO(AGV_IP, rfidWait);
                            SendAGV_ORD(agvNo, 2, 2, 98, AGV_IP);
                            break;
                        case 6:
                            //推送架货
                            SendAGV_OPERORD(agvNo, 1, AGV_IP);
                            break;
                        case 7:
                            //缩回架货
                            SendAGV_OPERORD(agvNo, 2, AGV_IP);
                            break;
                        case 8:
                            //伸出充电桩
                            SendPower_ORD(1, 1, AGV_IP);
                            break;
                        case 9:
                            //缩回充电桩
                            SendPower_ORD(1, 2, AGV_IP);
                            break;
                        case 10:
                            //打开充电回路
                            SendAGV_OPERORD(agvNo, 3, AGV_IP);
                            break;
                        case 11:
                            //关闭充电回路
                            SendAGV_OPERORD(agvNo, 4, AGV_IP);
                            break;
                    }

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
                frm_mapshow.lineNo = lineNo;
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
            车与线体对应关系ToolStripMenuItem.Checked = !车与线体对应关系ToolStripMenuItem.Checked;
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
