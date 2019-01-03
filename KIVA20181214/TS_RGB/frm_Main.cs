using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Fuction;
using System.Net.Sockets;
using System.Threading;
using PLCControl;
using System.Net;
using FileControl;
using System.Net.NetworkInformation;
using GetPath;
using Octopus_Traffic_Control;

namespace TS_RGB
{
    public partial class frm_Main : Form
    {
        #region params
        TrafficControl trafficControl;
        int trafficControl_num = 6;
        int FlowControl_num = 4;
        int Lent = 650;
        public int Map_X = 60;
        int Map_Y = 60;
        public int MaxBend = 5;
        public static string down = "620";//下料点
        public static string charge1 = "494";//第1个预充电点
        public static string charge2 = "514";//第2个预充电点
        public static string charge3 = "534";//第3个预充电点
        public static string charge4 = "554";//第4个预充电点
        public static string charge5 = "574";//第5个预充电点

        
        public  GetPathGroup getPathGroup;
        public  List<string> b;//禁止地图编号集合

        public int[] AgvPosTemp;

        int again_send_time = 0;
        public static int LOW_V = 50000;
        public static int HIGH_V = 58500;
        public static int CHARGE_ANG = 90;
        //----------------------------------------------     
        frm_Car frm_car;// = new frm_Car(this);      
        frm_WCS frm_wcs;     
        frm_PowerInfo frm_PowerInfo;
        frm_AGV_Order frm_agv_order;

        public static ToolStripStatusLabel toolStripStatusLabel_forPublic;
        public static ToolStripMenuItem ToolStripMenuItemAGVManage;
        public static ToolStripMenuItem ToolStripMenuItemAGV_orderManage;
        public static ToolStripMenuItem ToolStripMenuItemRFIDManage;
        public static ToolStripMenuItem ToolStripMenuItemAreaManage;
        //----------------------------------------------------------
        //-----------------------SOCKET Server----------------------
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private Thread ListenThread;
        private Socket listener;
        private Dictionary<string, Socket> connectedSocket;//连接的通信套接字集合
        private Dictionary<string, int> SocketStatus;//连接的通信套接字状态集合
        private List<string> ClientIps;//连接的IP集合
        bool bstop = false;
        public byte[] SendMess = new byte[13];
        IPAddress ipAddress;
        int hostPORT;
        int Get = 0;
        List<string> Cstring;
        Dictionary<string, int> MACH_OLD;       
        //----------------------------------------------------------
        //----------------------------------------------------------
        public static object ThreadControl_RGV = new object();
        Thread Thread_RGV;
        ThreadStart ThreadSt_RGV;
        Thread Thread_Cross;
        ThreadStart ThreadSt_Cross;
        //---------------------------------------------------------- 
        //KIVA
        Thread Thread_KIVA_State;
        ThreadStart ThreadSt_KIVA_State;
        //end

        List<CVehicle> vehicles = new List<CVehicle>();

        List<string> PowerIP;//充电桩IP集合

        #endregion

        public frm_Main()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile(Application.StartupPath + "\\img\\log1.jpg");

            toolStripStatusLabel_forPublic = toolStripStatusLabel5;

            LogFile.SaveLog_Start("AGV管控系统已启动..." + "\r\n");
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 0x01;
            const int HTCAPTION = 0x02;
            const int WM_SYSCOMMAND = 0x112;
            const int SC_MAXMIZE = 0xF030;
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
            PowerIP = new List<string>();

            Function.PR_FORMART_tb_AGV_INFO();


            DataTable a = Function.KIVA_SELECT_MAPNo_used();
            b = new List<string>();
            for (int t = 0; t < a.Rows.Count; t++)
            {
                b.Add(a.Rows[t][0].ToString());
            }

            FormartGetPath(Map_X, Map_Y);

            int PowerCount = int.Parse(FileControl.SetFileControl.ReadIniValue("POWER", "POWERCOUNT", Application.StartupPath + @"\AGV_Set.ini").Trim());
            for (int i = 0; i < PowerCount; i++)
            {
                string PowerIP_s = FileControl.SetFileControl.ReadIniValue("POWER", "POWERIP" + (i + 1).ToString(), Application.StartupPath + @"\AGV_Set.ini").Trim();
                if (!PowerIP.Contains(PowerIP_s))
                {
                    PowerIP.Add(PowerIP_s);
                }
            }

            //------------------取得IPaddress、port---------------------
            string HostIP = FileControl.SetFileControl.ReadIniValue("HOST", "HOSTIP", Application.StartupPath + @"\AGV_Set.ini").Trim();
            hostPORT = int.Parse(FileControl.SetFileControl.ReadIniValue("HOST", "HOSTPORT", Application.StartupPath + @"\AGV_Set.ini").Trim().Trim());
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
            DataTable agv = Function.KIVA_SELECT_AGV_Info();
            if (agv != null && agv.Rows.Count > 0)
            {
                AgvPosTemp = new int[agv.Rows.Count];
                for (int i = 0; i < agv.Rows.Count; i++)
                {
                    AgvPosTemp[i] = -1;
                }
            }
            frm_car = new frm_Car(this);
            frm_PowerInfo = new frm_PowerInfo(this);
            frm_wcs = new frm_WCS(this);
            frm_agv_order = new frm_AGV_Order(this);
            //----------------------------------------------------------
            frm_wcs.MdiParent = this;
            frm_car.MdiParent = this;
            timer1.Start();
            //
            DataTable AGV_info = Function.SELETE_AGV_INFO();
            DataTable MAX_agvRemark = Function.SELECT_AGV_INFO_MAXREMARK();

            if (AGV_info != null && AGV_info.Rows.Count > 0)
            {
                Params.AGV_COUNT = AGV_info.Rows.Count;
            }
            if (MAX_agvRemark != null && MAX_agvRemark.Rows.Count > 0)
            {
                int count = int.Parse(MAX_agvRemark.Rows[0][0].ToString()) > 99 ? 200 : 100;
                Params.Log_Count_forRFID = new string[count];
                for (int i = 0; i < count; i++)
                {
                    Params.Log_Count_forRFID[i] = "0";
                }
                //
                Params.Log_Count_forErr = new string[count];
                for (int i = 0; i < count; i++)
                {
                    Params.Log_Count_forErr[i] = "-1";
                }
            }

            trafficControl = new TrafficControl(trafficControl_num,FlowControl_num);
            //----
            ThreadSt_RGV = new ThreadStart(RunMain);
            Thread_RGV = new Thread(ThreadSt_RGV);
            Thread_RGV.Start();

            ThreadSt_Cross = new ThreadStart(RunCross);
            Thread_Cross = new Thread(ThreadSt_Cross);
            Thread_Cross.Start();
            //KIVA
            ThreadSt_KIVA_State = new ThreadStart(RunChargeState);
            Thread_KIVA_State = new Thread(ThreadSt_KIVA_State);
            Thread_KIVA_State.Start();
            //end

            //int no = 0;
            //for (int i = 0; i <= 19; i++)
            //{
            //    for (int j = 0; j <= 100; j++)
            //    {
            //        int RES = Function.KIVA_UPDATE_Map_Info((j * 20 + i).ToString(), i.ToString(), j.ToString(), 0);
            //        no++;
            //    }
            //}

        }

        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Params.userName_now == "")
            {
                if (MessageBox.Show("请先登录拥有此操作权限的账户！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    frm_Login login = new frm_Login();
                    login.Show();
                    e.Cancel = true;
                    return;
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
            LogFile.SaveLog_Start("AGV管控系统已关闭..." + "\r\n");
            Application.ExitThread();
            Application.Exit();
        }

        //---------------------------------------------------------------------------

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
                        ////设置心跳
                        //byte[] inValue = new byte[] { 1, 0, 0, 0, 0x88, 0x13, 0, 0, 0xd0, 0x07, 0, 0 };// 首次探测时间5 秒, 间隔侦测时间5 秒
                        //handler.IOControl(IOControlCode.KeepAliveValues, inValue, null);
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);
                    }
                    catch (Exception)
                    {
                        StateObject state = new StateObject();
                        state.workSocket = handler;
                        //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);
                    }
                }
                else
                {
                    try
                    {
                        //connectedSocket[ip].Close();
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
                        //StateObject state = new StateObject();
                        //state.workSocket = handler;
                        //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);
                    }
                }
            }
        }
        public static object ThreadControl_One = new object();
        /// <summary>
        /// 接受回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;
                IPEndPoint iep = (IPEndPoint)handler.RemoteEndPoint;
                string ip = iep.Address.ToString();
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    //lock (ThreadControl_One)
                    //{
                        ConvertMsg(ip, state.buffer, bytesRead);
                    //}
                }
                if (!bstop)
                {
                    lock (this.connectedSocket)
                    {
                        if (bytesRead == 0)
                        {
                            state.workSocket.Close();
                            connectedSocket.Remove(ip);
                            SocketStatus.Remove(ip);
                            ClientIps.Remove(ip);
                        }
                        if (state.workSocket.Connected == true)
                        {
                            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                try
                {
                    lock (this.connectedSocket)
                    {
                        StateObject state = (StateObject)ar.AsyncState;
                        Socket handler = state.workSocket;
                        IPEndPoint iep = (IPEndPoint)handler.RemoteEndPoint;
                        string ip = iep.Address.ToString();
                        if (connectedSocket.ContainsKey(ip))
                        {
                            state.workSocket.Close();
                            this.connectedSocket.Remove(ip);
                            this.SocketStatus.Remove(ip);
                            this.ClientIps.Remove(ip); //Keepalive检测网线断开引发的异常在这里捕获
                        }
                    }
                }
                catch (Exception)
                {
                }
                return;
            }
        }
        /// <summary>
        /// Socket Server接收数据处理
        /// </summary>
        /// <param name="ipaddr"></param>
        /// <param name="buffer"></param>
        /// <param name="bytesRead"></param>
        private void ConvertMsg(string ipaddr, byte[] buffer, int bytesRead)
        {
            try
            {
                string str_s = "";
                string content = "";
                for (int i = 0; i < bytesRead; i++)
                {
                    if (i >= 6 && i < bytesRead - 3)
                    {
                        str_s = str_s + buffer[i].ToString("X2") + " ";
                    }
                    content = content + ((int)buffer[i]).ToString("X2");
                }
                if (Params.recLog) //&& bytesRead!=45
                {
                    LogFile.SaveLog_Rec("[" + ipaddr.Trim() + "]R:" + content + "\r\n", ipaddr);                      

                }
                if (bytesRead / 45 > 0 && bytesRead % 45 == 0)
                {
                    for (int s = 0; s < bytesRead / 45; s++)
                    {
                        if (buffer[s * 45 + 0] == 0xfc && buffer[s * 45 + 44] == 0x5a)
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
                            #region 字符串解析出字段
                            string x, y, angc, angl, tnum, tcount, sjour, lwheel, rwheel, liftdj, circledj, dy, dl, errcod, warncod, ljid;
                            int X, Y, Angc, Angl, Tnum, Tcount, Sjour, Lwheel, Rwheel, Liftdj, Circledj, Dy, Dl, Errcod, Warncod, Ljid;

                            string startcod, framelen, destaddr, srcaddr, subcommand, framecod, data2, data3, crchigh, crclow, endcod;
                            int Startcod, Framelen, Destaddr, Srcaddr, Subcommand, Framecod, Data2, Data3, Crchigh, Crclow, Endcod;
                            startcod = content.Substring(0, 2);
                            framelen = content.Substring(2, 2);
                            destaddr = content.Substring(4, 4);
                            srcaddr = content.Substring(8, 4);
                            subcommand = content.Substring(12, 2);
                            framecod = content.Substring(14, 4);
                            data2 = content.Substring(18, 2);//是否在二维码上
                            data3 = content.Substring(20, 2);
                            crchigh = content.Substring(84, 2);
                            crclow = content.Substring(86, 2);
                            endcod = content.Substring(88, 2);
                            Startcod = Int32.Parse(startcod, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Framelen = Int32.Parse(framelen, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Destaddr = Int32.Parse(destaddr, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Srcaddr = Int32.Parse(srcaddr, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Subcommand = Int32.Parse(subcommand, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Framecod = Int32.Parse(framecod, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Data2 = Int32.Parse(data2, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Data3 = Int32.Parse(data3, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Crchigh = Int32.Parse(crchigh, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Crclow = Int32.Parse(crclow, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Endcod = Int32.Parse(endcod, System.Globalization.NumberStyles.AllowHexSpecifier);

                            string AGV_all_AC = Convert.ToString(Data3, 2).PadLeft(8, '0');
                            int[] AGV_all_AC_bit = new int[8];
                            for (int i = 0; i < 8; i++)
                            {
                                AGV_all_AC_bit[i] = int.Parse(AGV_all_AC.Substring(7 - i, 1));
                            }
                            //(小车状态标志2)
                            //bit0 顶盘下限感应 
                            int TuoPan_Low = AGV_all_AC_bit[0];
                            //bit1 顶盘上限感应
                            int TuoPan_High = AGV_all_AC_bit[1];
                            //bit2 激光感应器近距离感应状态（停车） 0-off 1-on
                            int ZhangAi_Stop = AGV_all_AC_bit[2];
                            //bit3 激光感应器中距离感应状态（减速） 0-off 1-on
                            int ZhangAi_Slow = AGV_all_AC_bit[3];
                            //bit4 激光感应器远距离感应状态（小减速） 0-off 1-on
                            int ZhangAi_SmallSlow = AGV_all_AC_bit[4];
                            //bit5 转盘原点检测状态         0-off 1-on
                            int TuoPan_Zero = AGV_all_AC_bit[5];
                            //bit6 前端料架感应器状态       0-off 1-on
                            int SkipSacn_Beffor = AGV_all_AC_bit[6];
                            //bit7 小车当前检测到料架标志   0 未检测到 1检测到了
                            int HasSkip = AGV_all_AC_bit[7];

                            x = content.Substring(22, 8);
                            y = content.Substring(30, 8);
                            angc = content.Substring(38, 4);
                            angl = content.Substring(42, 4);
                            tnum = content.Substring(46, 4);
                            tcount = content.Substring(50, 2);
                            sjour = content.Substring(52, 4);
                            lwheel = content.Substring(56, 4);
                            rwheel = content.Substring(60, 4);
                            liftdj = content.Substring(64, 2);
                            circledj = content.Substring(66, 2);
                            dy = content.Substring(68, 4);
                            dl = content.Substring(72, 4);
                            errcod = content.Substring(76, 2);
                            warncod = content.Substring(78, 2);
                            ljid = content.Substring(80, 4);

                            //将字符串形式十六进制数字转化为十进制数字
                            X = Int32.Parse(x, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Y = Int32.Parse(y, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Angc = Int32.Parse(angc, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Angl = Int32.Parse(angl, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Tnum = Int32.Parse(tnum, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Tcount = Int32.Parse(tcount, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Sjour = Int32.Parse(sjour, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Lwheel = Int32.Parse(lwheel, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Rwheel = Int32.Parse(rwheel, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Liftdj = Int32.Parse(liftdj, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Circledj = Int32.Parse(circledj, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Dy = Int32.Parse(dy, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Dl = Int32.Parse(dl, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Errcod = Int32.Parse(errcod, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Warncod = Int32.Parse(warncod, System.Globalization.NumberStyles.AllowHexSpecifier);
                            Ljid = Int32.Parse(ljid, System.Globalization.NumberStyles.AllowHexSpecifier);

                            #endregion

                            //新增 是否在二维码上 字段
                            //Function.KIVA_UPDATE_AGV_INFO(ipaddr, 1, X.ToString(), Y.ToString(), Ljid.ToString(), Dy.ToString(),
                            //    Dl.ToString(), Lwheel.ToString(), Rwheel.ToString(), Errcod.ToString(), Warncod.ToString(), Tnum.ToString(),
                            //    Tcount, Sjour.ToString(), Angc.ToString(), Angl.ToString(), Liftdj.ToString(), Circledj.ToString(),
                            //    framecod.ToString(), Data2, FormartXY(X.ToString(), Y.ToString()),
                            //    int.Parse(FormartXY(X.ToString(), Y.ToString()).Split(',')[0]) + int.Parse(FormartXY(X.ToString(), Y.ToString()).Split(',')[1]) * 20);

                            //更新tb_Map_Info的Map_State字段为小车编号
                            //Function.KIVA_UPDATE_Map_Info_State(FormartXY(X.ToString(), Y.ToString()).Split(',')[0], FormartXY(X.ToString(), Y.ToString()).Split(',')[1], int.Parse(ipaddr.Split('.')[3]) - 200);

                            //-----
                            //Function.KIVA_UPDATE_AGV_INFO_AND_STATE_Map_Info_State(ipaddr, 1, X.ToString(), Y.ToString(), Ljid.ToString(), Dy.ToString(),
                            //    Dl.ToString(), Lwheel.ToString(), Rwheel.ToString(), Errcod.ToString(), Warncod.ToString(), Tnum.ToString(),
                            //    Tcount, Sjour.ToString(), Angc.ToString(), Angl.ToString(), Liftdj.ToString(), Circledj.ToString(),
                            //    framecod.ToString(), Data2, FormartXY(X.ToString(), Y.ToString()),
                            //    int.Parse(FormartXY(X.ToString(), Y.ToString()).Split(',')[0]) + int.Parse(FormartXY(X.ToString(), Y.ToString()).Split(',')[1]) * Map_X
                            //    , TuoPan_Low.ToString(), TuoPan_High.ToString(), ZhangAi_Stop.ToString(), ZhangAi_Slow.ToString(), ZhangAi_SmallSlow.ToString(), TuoPan_Zero.ToString(), SkipSacn_Beffor.ToString(), HasSkip.ToString()
                            //    , FormartXY(X.ToString(), Y.ToString()).Split(',')[0], FormartXY(X.ToString(), Y.ToString()).Split(',')[1], int.Parse(ipaddr.Split('.')[3]) - 200);
                            Function.KIVA_UPDATE_AGV_INFO_AND_STATE_Map_Info_State2(ipaddr, 1, X.ToString(), Y.ToString(), Ljid.ToString(), Dy.ToString(),
                            Dl.ToString(), Lwheel.ToString(), Rwheel.ToString(), Errcod.ToString(), Warncod.ToString(), Tnum.ToString(),
                            Tcount, Sjour.ToString(), Angc.ToString(), Angl.ToString(), Liftdj.ToString(), Circledj.ToString(),
                            framecod.ToString(), Data2, FormartXY(X.ToString(), Y.ToString()),
                            int.Parse(FormartXY(X.ToString(), Y.ToString()).Split(',')[0]) + int.Parse(FormartXY(X.ToString(), Y.ToString()).Split(',')[1]) * Map_X
                            , TuoPan_Low.ToString(), TuoPan_High.ToString(), ZhangAi_Stop.ToString(), ZhangAi_Slow.ToString(), ZhangAi_SmallSlow.ToString(), TuoPan_Zero.ToString(), SkipSacn_Beffor.ToString(), HasSkip.ToString()
                            , FormartXY(X.ToString(), Y.ToString()).Split(',')[0], FormartXY(X.ToString(), Y.ToString()).Split(',')[1], int.Parse(ipaddr.Split('.')[3]) - 200, Map_X, ipaddr.Substring(ipaddr.Length - 2).TrimStart('0'));
                            //Function.KIVA_UPDATE_AGV_INFO_AND_STATE_Map_Info_State2(ipaddr, 1, X.ToString(), Y.ToString(), Ljid.ToString(), Dy.ToString(),
                            //Dl.ToString(), Lwheel.ToString(), Rwheel.ToString(), Errcod.ToString(), Warncod.ToString(), Tnum.ToString(),
                            //Tcount, Sjour.ToString(), Angc.ToString(), Angl.ToString(), Liftdj.ToString(), Circledj.ToString(),
                            //framecod.ToString(), Data2, FormartXY(X.ToString(), Y.ToString()),
                            //int.Parse(FormartXY(X.ToString(), Y.ToString()).Split(',')[0]) + int.Parse(FormartXY(X.ToString(), Y.ToString()).Split(',')[1]) * Map_X
                            //, TuoPan_Low.ToString(), TuoPan_High.ToString(), ZhangAi_Stop.ToString(), ZhangAi_Slow.ToString(), ZhangAi_SmallSlow.ToString(), TuoPan_Zero.ToString(), SkipSacn_Beffor.ToString(), HasSkip.ToString()
                            //, FormartXY(X.ToString(), Y.ToString()).Split(',')[0], FormartXY(X.ToString(), Y.ToString()).Split(',')[1], int.Parse(ipaddr.Split('.')[3]) - 200, Map_X, ipaddr.Substring(ipaddr.Length-2));
                            //Function.KIVA_UPDATE_ChargeInfo(Lent, ipaddr);
                            //----

                            //更新tb_AGV_Order的OLock的字段 --1：锁定  0:未锁定
                            //if (Warncod == 1)
                            //    Function.KIVA_UPDATE_AGV_ORDER_OLOCK(ipaddr, 1);
                            //else
                            //    Function.KIVA_UPDATE_AGV_ORDER_OLOCK(ipaddr, 0);

                        }
                    }
                }
                else if (bytesRead / 11 > 0 && bytesRead % 11 == 0)//充电站
                {
                    if (PowerIP.Contains(ipaddr))
                    {
                        LogFile.SaveLog_Power("[" + ipaddr.Trim() + "]R:" + str_s + "\r\n", ipaddr);
                        for (int s = 0; s < bytesRead / 11; s++)
                        {
                            if (buffer[s * 11 + 0] == 0xfc && buffer[s * 11 + 10] == 0x5a)
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
                                //byte[] GetCRC = new byte[11];
                                //Array.Copy(buffer, s * 11, GetCRC, 0, 8);
                                //byte[] Get_CRC = CRC.CRC16(GetCRC);
                                //if (buffer[s * 11 + 9] == Get_CRC[1] && buffer[s * 11 + 10] == Get_CRC[0])
                                //{
                              
                                Function.KIVA_UPDATE_Charge(ipaddr, (int) buffer[7]);
                                //}
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SaveLogExTest("ConvertMsg:" + ex.StackTrace+"\r\n errorMsg:"+ex.Message);
            }        
        }
        /// <summary>
        /// Socket Server发送函数
        /// </summary>
        /// <param name="sendMessage"></param>
        /// <param name="ip"></param>
        public bool SendDefault(byte[] sendMessage, string ip)
        {
            bool ret = false;
            try
            {
                if (connectedSocket.ContainsKey(ip))
                {
                    connectedSocket[ip].Send(sendMessage);
                    string mes = "";
                    for (int i = 0; i < sendMessage.Length; i++)
                    {
                        mes = mes + sendMessage[i].ToString("X2") + " ";
                    }
                    if (Params.sendLog && sendMessage[6]!=0x02)// && sendMessage.Length > 10
                    {
                        LogFile.SaveLog_Send("[" + ip.Trim() + "]S:" + mes + "\r\n", ip);
                    }
                    ret = true;
                }
            }
            catch (Exception)
            {
                string mes = "";
                for (int i = 0; i < sendMessage.Length; i++)
                {
                    mes = mes + sendMessage[i].ToString().Trim() + " ";
                }
            }
            return ret;
        }

        private int RtAngel(int InAngel)
        {
            int Rt = 0;
            if (InAngel < 45 | InAngel > 315)
                Rt = 0;
            if (InAngel > 45 && InAngel < 135)
                Rt = 90;
            if (InAngel > 135 && InAngel < 225)
                Rt = 180;
            if (InAngel > 225 && InAngel < 315)
                Rt = 270;
            return Rt;
        }

        //---------------------------------------------------------------------------

        private void RunMain()
        {
            while (!bstop)
            {
                try
                {
                    #region 询问AGV状态
                    DataTable ALL_AGV_Info = Function.SELETE_AGV_INFO();
                    if (ALL_AGV_Info != null && ALL_AGV_Info.Rows.Count > 0)
                    {
                        for (int i = 0; i < ALL_AGV_Info.Rows.Count; i++)
                        {
                            string AGV_IP = ALL_AGV_Info.Rows[i][1].ToString().Trim();
                            int AGV_Ac = int.Parse(ALL_AGV_Info.Rows[i][2].ToString().Trim());
                            if (ClientIps.Contains(AGV_IP))
                            {
                                KIVA_SendTo_AGV_AskState(AGV_IP);
                            }
                        }
                    }
                    #endregion

                    #region 生成路径new
                    try
                    {
                        Thread.Sleep(200);
                    }
                    catch (Exception)
                    {
                        
                    }
                    //WCS下发旋转命令
                    //WCS下发去下一个点命令

                    //WCS下发命令
                    DataTable KIVA_Info = Function.KIVA_SELECT_WCS_To_KIVA();
                    if (KIVA_Info != null && KIVA_Info.Rows.Count > 0)
                    {
                        for (int m = 0; m < KIVA_Info.Rows.Count; m++)
                        {
                            //---------------WCS-TO-KIVA----------------------------
                            //------------------------------------------------------
                            string wstring = KIVA_Info.Rows[m]["wstring"].ToString().Trim();//任务总点数：@489;506;1365;1346;
                            int wac = int.Parse(KIVA_Info.Rows[m]["wac"].ToString().Trim());//WCS->KIVA 1，发任务 2，回原点 KIVA-->WCS 0:已执行
                            string wagvat = KIVA_Info.Rows[m]["wagvat"].ToString().Trim();
                            string skipNo = KIVA_Info.Rows[m]["wfloatno"].ToString().Trim();
                            int wcs_skipAng = int.Parse(KIVA_Info.Rows[m]["wRevSkipAng"].ToString().Trim());//WCS旋转料架命令
                            string warivest = KIVA_Info.Rows[m]["warivest"].ToString().Trim();//wagvat、、AGV_Skip_Angle
                            //------------------------------------------------------
                            //---------------KIVA_INFO-------------------------------
                            string agvIP = KIVA_Info.Rows[m][11].ToString().Trim();
                            int agvac = int.Parse(KIVA_Info.Rows[m][12].ToString().Trim());
                            int Now_X = (int.Parse(KIVA_Info.Rows[m][13].ToString()) / Lent) + (int.Parse(KIVA_Info.Rows[m][13].ToString()) % Lent > (Lent / 2) ? 1 : 0);
                            int Now_Y = (int.Parse(KIVA_Info.Rows[m][14].ToString()) / Lent) + (int.Parse(KIVA_Info.Rows[m][14].ToString()) % Lent > (Lent / 2) ? 1 : 0);
                            string warning_code = KIVA_Info.Rows[m][25].ToString();
                            int ERRor = int.Parse(KIVA_Info.Rows[m][24].ToString());
                            int AllOrd = int.Parse(KIVA_Info.Rows[m][26].ToString()) + int.Parse(KIVA_Info.Rows[m][27].ToString());
                            int AtRFID = int.Parse(KIVA_Info.Rows[m][34].ToString());
                            string AGV_FROM = KIVA_Info.Rows[m][16].ToString();
                            string AGV_TO = KIVA_Info.Rows[m][17].ToString();
                            int AGV_Angle = RtAngel(int.Parse(KIVA_Info.Rows[m]["AGV_Angle"].ToString()));//当前AGV实际角度
                            int AGV_Skip_Angle = RtAngel(int.Parse(KIVA_Info.Rows[m]["AGV_Skip_Angle"].ToString()));//当前料车实际角度           
                            int AGV_Command = Int32.Parse(KIVA_Info.Rows[m][33].ToString(), System.Globalization.NumberStyles.HexNumber);
                            //------------------------------------------------------
                            int AGV_at = Now_Y * Map_X + Now_X;

                            if (agvac == 1 && ERRor == 0 && AllOrd == 0 && AtRFID == 1)
                            {
                                #region AGV异常计算路径
                                if (warning_code == "2")
                                {
                                    if (AGV_FROM != Now_X.ToString() + "," + Now_Y.ToString() && AGV_FROM.Contains(',') && AGV_TO.Contains(','))
                                    {
                                        int toX = int.Parse(AGV_TO.Split(',')[0]);
                                        int toY = int.Parse(AGV_TO.Split(',')[1]);
                                        if (toX == Now_X && toY == Now_Y)
                                        {
                                            KIVA_SendTo_AGV_Initialize(agvIP);
                                        }
                                        else
                                        {
                                            int MaxLine = Math.Abs(Now_X - toX) + Math.Abs(Now_Y - toY) + 26;
                                            //当前点 + AGV_To，计算新路径存入order表，更新now=0 ，同正常情况！
                                            CPath Mpath = new CPath(0);
                                            if (getPathGroup.retBestPathInOrders(agvIP, skipNo, Now_X.ToString(), toX.ToString(), Now_Y.ToString(), toY.ToString(), MaxLine, MaxBend, out Mpath))
                                            {
                                                string Orders = "";
                                                List<int> bestCPath_loc = Mpath.subm_path;
                                                List<int> aa = Mpath.m_path;
                                                List<int> bestCPath_angle = Mpath.agvangle;
                                                for (int q = 0; q < bestCPath_angle.Count; q++)
                                                {
                                                    Orders += bestCPath_loc[q + 1].ToString() + "," + bestCPath_angle[q].ToString() + ";";
                                                }

                                                string all_Orders = "";
                                                List<int> full_loc = Mpath.m_path;
                                                for (int t = 0; t < full_loc.Count; t++)
                                                {
                                                    all_Orders += full_loc[t].ToString() + ";";
                                                }

                                                Function.KIVA_UPDATE_Order_At_Wac_Arivest_To(agvIP, 0, Orders, "0", Now_X.ToString() + "," + Now_Y.ToString(), toX.ToString() + "," + toY.ToString()
                                                   , skipNo, (toX + toY * Map_X).ToString(), 0, skipNo, 0, all_Orders);
                                                Function.KIVA_UPDATE_AGV_ORDER_COMMAND(agvIP, AGV_Command + 1, bestCPath_angle.Count);
                                            }
                                        }
                                    }
                                    else//初始化
                                    {
                                        KIVA_SendTo_AGV_Initialize(agvIP);
                                    }
                                }
                                #endregion

                                else if (AGV_Skip_Angle != wcs_skipAng)////生成旋转路径--发送
                                {
                                    string orders = (Now_Y * Map_X + Now_X).ToString() + "," + AGV_Angle.ToString() + ";";
                                    KIVA_SendTo_AGV_Turn(agvIP, orders, AGV_Command + 1, wcs_skipAng.ToString());
                                }

                                #region 生成正常路径
                                else if (wac == 1) 
                                {
                                    //if (!string.IsNullOrEmpty(wstring) && wagvat == AGV_at.ToString())
                                    if (!string.IsNullOrEmpty(wstring))
                                    {
                                        string wstring_ord = wstring.Substring(1, wstring.Length - 1);
                                        if (!b.Contains(wagvat))
                                        {
                                            string[] wstring_loc = wstring_ord.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);//每一个任务点
                                            int nextIndex = 0;
                                            if (wstring_loc.Contains(wagvat))
                                            {
                                                for (int k = 0; k < wstring_loc.Length; k++)
                                                {
                                                    if (wstring_loc[k] == wagvat)
                                                    {
                                                        //获取下一个任务点
                                                        nextIndex = k + 1;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (nextIndex < wstring_loc.Length)//任务点还有，继续
                                            {
                                                string toX = (int.Parse(wstring_loc[nextIndex]) % Map_X).ToString();
                                                string toY = (int.Parse(wstring_loc[nextIndex]) / Map_X).ToString();
                                                int MaxLine = Math.Abs(Now_X - int.Parse(toX)) + Math.Abs(Now_Y - int.Parse(toY)) + 26;
                                                //返回最佳路径字符串 并更新了order表： OString,  ONow=0, From, To
                                                CPath Mpath = new CPath(0);

                                                //2017年12月10日16:09:49
                                                DataTable Charge_Small = Function.KIVA_SELECT_charge_Info_byagvIP(agvIP);
                                                //在充电
                                                if (Charge_Small != null && Charge_Small.Rows.Count > 0)
                                                {
                                                    if (getPathGroup.retBestPathInOrders_Charge(agvIP, skipNo, Now_X.ToString(), toX, Now_Y.ToString(), toY, MaxLine, MaxBend, out Mpath))
                                                    {
                                                        string Orders = "";
                                                        List<int> bestCPath_loc = Mpath.subm_path;
                                                        List<int> bestCPath_angle = Mpath.agvangle;
                                                        for (int q = 0; q < bestCPath_angle.Count; q++)
                                                        {
                                                            Orders += bestCPath_loc[q + 1].ToString() + "," + bestCPath_angle[q].ToString() + ";";
                                                        }

                                                        string all_Orders = "";
                                                        List<int> full_loc = Mpath.m_path;
                                                        for (int t = 0; t < full_loc.Count; t++)
                                                        {
                                                            all_Orders += full_loc[t].ToString() + ";";
                                                        }
                                                        Function.KIVA_UPDATE_Order_At_Wac_Arivest_To(agvIP, 0, Orders, "0", Now_X.ToString() + "," + Now_Y.ToString(), toX + "," + toY
                                                   , skipNo, wstring_loc[nextIndex], 0, skipNo, 0, all_Orders);
                                                        Function.KIVA_UPDATE_AGV_ORDER_COMMAND(agvIP, AGV_Command + 1, bestCPath_angle.Count);
                                                    }
                                                }
                                                //end
                                                else
                                                {
                                                    if (getPathGroup.retBestPathInOrders(agvIP, skipNo, Now_X.ToString(), toX, Now_Y.ToString(), toY, MaxLine, MaxBend, out Mpath))
                                                    {
                                                        string Orders = "";
                                                        List<int> bestCPath_loc = Mpath.subm_path;
                                                        List<int> bestCPath_angle = Mpath.agvangle;
                                                        for (int q = 0; q < bestCPath_angle.Count; q++)
                                                        {
                                                            Orders += bestCPath_loc[q + 1].ToString() + "," + bestCPath_angle[q].ToString() + ";";
                                                        }

                                                        string all_Orders = "";
                                                        List<int> full_loc = Mpath.m_path;
                                                        for (int t = 0; t < full_loc.Count; t++)
                                                        {
                                                            all_Orders += full_loc[t].ToString() + ";";
                                                        }
                                                        Function.KIVA_UPDATE_Order_At_Wac_Arivest_To(agvIP, 0, Orders, "0", Now_X.ToString() + "," + Now_Y.ToString(), toX + "," + toY
                                                   , skipNo, wstring_loc[nextIndex], 0, skipNo, 0, all_Orders);
                                                        Function.KIVA_UPDATE_AGV_ORDER_COMMAND(agvIP, AGV_Command + 1, bestCPath_angle.Count);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                #region 最后一条指令已执行，找到X=2的空位
                                                //DataTable nullLocation = Function.KIVA_SELECT_MAP_Null(gongWeiRow.ToString());
                                                DataTable nullLocation = Function.PR_SELECT_AGV_ST(agvIP, 1);
                                                if (nullLocation != null && nullLocation.Rows.Count > 0)
                                                {
                                                    int finalNullLocation_X = int.Parse(nullLocation.Rows[0][0].ToString()) % Map_X;
                                                    int finalNullLocation_Y = int.Parse(nullLocation.Rows[0][0].ToString()) / Map_X;
                                                    int MaxLine = Math.Abs(Now_X - finalNullLocation_X) + Math.Abs(Now_Y - finalNullLocation_Y) + 26;
                                                    CPath Mpath = new CPath(0);
                                                    if (getPathGroup.retBestPathInOrders(agvIP, skipNo, Now_X.ToString(), finalNullLocation_X.ToString(), Now_Y.ToString(), finalNullLocation_Y.ToString(), MaxLine, MaxBend, out Mpath))
                                                    {
                                                        string Orders = "";
                                                        List<int> bestCPath_loc = Mpath.subm_path;
                                                        List<int> aa = Mpath.m_path;
                                                        List<int> bestCPath_angle = Mpath.agvangle;
                                                        for (int q = 0; q < bestCPath_angle.Count; q++)
                                                        {
                                                            Orders += bestCPath_loc[q + 1].ToString() + "," + bestCPath_angle[q].ToString() + ";";
                                                        }

                                                        string all_Orders = "";
                                                        List<int> full_loc = Mpath.m_path;
                                                        for (int t = 0; t < full_loc.Count; t++)
                                                        {
                                                            all_Orders += full_loc[t].ToString() + ";";
                                                        }
                                                        //更新料架角度为0
                                                        Function.KIVA_UPDATE_WCS_To_KIVA_RecSkipAng(skipNo, "0");
                                                        Function.KIVA_UPDATE_Order_At_Wac_Arivest_To(agvIP, 0, Orders, "0", Now_X.ToString() + "," + Now_Y.ToString(), finalNullLocation_X.ToString() + "," + finalNullLocation_Y.ToString()
                                                  , skipNo, (finalNullLocation_Y * Map_X + finalNullLocation_X).ToString(), 0, skipNo, 0, all_Orders);

                                                        Function.KIVA_UPDATE_AGV_ORDER_COMMAND(agvIP, AGV_Command + 1, bestCPath_angle.Count);
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region 返回起点
                                else if (wac == 2)
                                {
                                    //bool isSymbol = true;//路径是否全部执行
                                    //DataTable dtTemp = Function.PR_SELECT_AGV_ST(agvIP, 1);
                                    //if (dtTemp != null && dtTemp.Rows.Count > 0)
                                    //{
                                    //    int finalNullLocation_X = int.Parse(dtTemp.Rows[0][0].ToString()) % Map_X;
                                    //    int finalNullLocation_Y = int.Parse(dtTemp.Rows[0][0].ToString()) / Map_X;
                                    //    if (finalNullLocation_X != Now_X ||finalNullLocation_Y != Now_Y)
                                    //    {
                                    //        isSymbol = false;
                                    //        Function.KIVA_UPDATE_tb_WCS_To_KIVA(1,
                                    //            agvIP.Substring(agvIP.Length - 2).TrimStart('0'));
                                    //        SavePathLog("wac == 20");
                                    //    }
                                    //}
                                    //if (isSymbol)
                                    //{
                                        int CheckRt = 0;
                                        DataTable nullLocationf = Function.PR_SELECT_AGV_ST(agvIP, 3);
                                        if (nullLocationf != null && nullLocationf.Rows.Count > 0)
                                        {
                                            string toX =
                                                (int.Parse(nullLocationf.Rows[0][0].ToString())%Map_X).ToString();
                                            string toY =
                                                (int.Parse(nullLocationf.Rows[0][0].ToString())/Map_X).ToString();
                                            if (Now_X != int.Parse(toX) || Now_Y != int.Parse(toY))
                                            {
                                                CheckRt = 1;
                                                string finalNo = (int.Parse(toY)*Map_X + int.Parse(toX)).ToString();
                                                //计算路径，返回“起始点”
                                                int MaxLine = Math.Abs(Now_X - int.Parse(toX)) +
                                                              Math.Abs(Now_Y - int.Parse(toY)) + 26;
                                                CPath Mpath = new CPath(0);
                                                if (getPathGroup.retBestPathInOrders(agvIP, skipNo, Now_X.ToString(),
                                                    toX, Now_Y.ToString(), toY, MaxLine, MaxBend, out Mpath))
                                                {
                                                    string Orders = "";
                                                    List<int> bestCPath_loc = Mpath.subm_path;
                                                    List<int> aa = Mpath.m_path;
                                                    List<int> bestCPath_angle = Mpath.agvangle;
                                                    for (int q = 0; q < bestCPath_angle.Count; q++)
                                                    {
                                                        Orders += bestCPath_loc[q + 1].ToString() + "," +
                                                                  bestCPath_angle[q].ToString() + ";";
                                                    }

                                                    string all_Orders = "";
                                                    List<int> full_loc = Mpath.m_path;
                                                    for (int t = 0; t < full_loc.Count; t++)
                                                    {
                                                        all_Orders += full_loc[t].ToString() + ";";
                                                    }

                                                    //将最优路径存入order表
                                                    Function.KIVA_UPDATE_Order_At_Wac_Arivest_To(agvIP, 0, Orders, "0",
                                                        Now_X.ToString() + "," + Now_Y.ToString(), toX + "," + toY
                                                        , skipNo, (int.Parse(toY)*Map_X + int.Parse(toX)).ToString(), 0,
                                                        skipNo, 0, all_Orders);
                                                    Function.KIVA_UPDATE_AGV_ORDER_COMMAND(agvIP, AGV_Command + 1,
                                                        bestCPath_angle.Count);
                                                }
                                            }
                                        }
                                        //DataTable nullLocation = Function.PR_SELECT_AGV_ST(agvIP, 2);
                                        DataTable nullLocation = Function.PR_SELECT_AGV_ST(agvIP);
                                        if (CheckRt == 0 && nullLocation != null && nullLocation.Rows.Count > 0 &&
                                            !string.IsNullOrEmpty(nullLocation.Rows[0][0].ToString()))
                                        {
                                            string toX =
                                                (int.Parse(nullLocation.Rows[0][0].ToString())%Map_X).ToString();
                                            string toY =
                                                (int.Parse(nullLocation.Rows[0][0].ToString())/Map_X).ToString();
                                            string finalNo = (int.Parse(toY)*Map_X + int.Parse(toX)).ToString();
                                            //计算路径，返回“起始点”
                                            int MaxLine = Math.Abs(Now_X - int.Parse(toX)) +
                                                          Math.Abs(Now_Y - int.Parse(toY)) + 26;
                                            CPath Mpath = new CPath(0);
                                            if (getPathGroup.retBestPathInOrders(agvIP, skipNo, Now_X.ToString(), toX,
                                                Now_Y.ToString(), toY, MaxLine, MaxBend, out Mpath))
                                            {
                                                string Orders = "";
                                                List<int> bestCPath_loc = Mpath.subm_path;
                                                List<int> aa = Mpath.m_path;
                                                List<int> bestCPath_angle = Mpath.agvangle;
                                                for (int q = 0; q < bestCPath_angle.Count; q++)
                                                {
                                                    Orders += bestCPath_loc[q + 1].ToString() + "," +
                                                              bestCPath_angle[q].ToString() + ";";
                                                }

                                                string all_Orders = "";
                                                List<int> full_loc = Mpath.m_path;
                                                for (int t = 0; t < full_loc.Count; t++)
                                                {
                                                    all_Orders += full_loc[t].ToString() + ";";
                                                }

                                                //将最优路径存入order表
                                                //Function.KIVA_UPDATE_Station(nullLocation.Rows[0][0].ToString(), agvIP);
                                                Function.KIVA_UPDATE_Order_At_Wac_Arivest_To(agvIP, 0, Orders, "0",
                                                    Now_X.ToString() + "," + Now_Y.ToString(), toX + "," + toY
                                                    , skipNo, (int.Parse(toY)*Map_X + int.Parse(toX)).ToString(), 0,
                                                    skipNo, 0, all_Orders);
                                                Function.KIVA_UPDATE_AGV_ORDER_COMMAND(agvIP, AGV_Command + 1,
                                                    bestCPath_angle.Count);
                                                Function.KIVA_UPDATE_WCS_To_KIVA_RecSkipAng(skipNo, "90");
                                            }
                                        }
                                    //}
                                }
                                #endregion
                            }
                        }
                    }

                    #endregion

                    #region 更新warivest
                    DataTable agv_arivest = Function.KIVA_SELECT_AGV_ARIVEST();
                    if (agv_arivest != null && agv_arivest.Rows.Count > 0)
                    {
                        for (int i = 0; i < agv_arivest.Rows.Count; i++)
                        {
                            int wfloatno = int.Parse(agv_arivest.Rows[i][0].ToString());
                            string AGV_Now_X = agv_arivest.Rows[i][1].ToString();
                            string AGV_Now_Y = agv_arivest.Rows[i][2].ToString();
                            int wagvat = int.Parse(agv_arivest.Rows[i][5].ToString());
                            int X = int.Parse(FormartXY(AGV_Now_X, AGV_Now_Y).Split(',')[0]);
                            int Y = int.Parse(FormartXY(AGV_Now_X, AGV_Now_Y).Split(',')[1]);
                            if (AgvPosTemp[wfloatno - 1] != (Y * Map_X + X) && AgvPosTemp[wfloatno - 1] != -1)
                            {
                                if (Y * Map_X + X == wagvat)
                                {
                                    Function.KIVA_UPDATE_AGV_ARIVEST(wfloatno.ToString(), 1);
                                }
                                AgvPosTemp[wfloatno - 1] = Y * Map_X + X;
                            }
                        }
                    }
                    #endregion

                    #region 更新网络状态
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
                                        if (MACH_OLD[F_ip] <= 100)
                                        {
                                            MACH_OLD[F_ip]++;
                                        }
                                        if (MACH_OLD[F_ip] >= 50)
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

                    #region 旋转料架
                    //DataTable turn_angle = Function.KIVA_SELECT_Turn_Angle();
                    //if (turn_angle != null && turn_angle.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < turn_angle.Rows.Count; i++)
                    //    {
                    //        if (!string.IsNullOrEmpty(turn_angle.Rows[i][4].ToString()))
                    //        {
                    //            string agv_x_temp = turn_angle.Rows[i][0].ToString();
                    //            string agv_y_temp = turn_angle.Rows[i][1].ToString();
                    //            string temp = FormartXY(agv_x_temp, agv_y_temp);
                    //            string agv_x = temp.Split(',')[0];
                    //            string agv_y = temp.Split(',')[1];
                    //            #region 角度处理
                    //            int agv_angle = int.Parse(turn_angle.Rows[i][2].ToString());
                    //            if (agv_angle < 45 | agv_angle > 315)
                    //                agv_angle = 0;
                    //            if (agv_angle > 45 && agv_angle < 135)
                    //                agv_angle = 90;
                    //            if (agv_angle > 135 && agv_angle < 225)
                    //                agv_angle = 180;
                    //            if (agv_angle > 225 && agv_angle < 315)
                    //                agv_angle = 270;

                    //            int skip_angle = int.Parse(turn_angle.Rows[i][3].ToString());
                    //            if (skip_angle < 45 | skip_angle > 315)
                    //                skip_angle = 0;
                    //            if (skip_angle > 45 && skip_angle < 135)
                    //                skip_angle = 90;
                    //            if (skip_angle > 135 && skip_angle < 225)
                    //                skip_angle = 180;
                    //            if (skip_angle > 225 && skip_angle < 315)
                    //                skip_angle = 270;
                    //            #endregion
                    //            string to_skip_angle = turn_angle.Rows[i][4].ToString();
                    //            string agv_ip = turn_angle.Rows[i][5].ToString();
                    //            string orderNo = turn_angle.Rows[i][6].ToString().Trim();
                    //            string orders = (int.Parse(agv_y) * Map_X + int.Parse(agv_x)).ToString() + "," + agv_angle.ToString() + ";";
                    //            string agv_V = turn_angle.Rows[i][7].ToString();
                    //            //2017年12月7日09:52:28：在充电桩附近时，不旋转。
                    //            if (!(int.Parse(agv_x) == 15 && (int.Parse(agv_y) == 24 || int.Parse(agv_y) == 25 || int.Parse(agv_y) == 26 || int.Parse(agv_y) == 27 || int.Parse(agv_y) == 28)))
                    //            {
                    //                //判断左右
                    //                DataTable map_Around = Function.KIVA_SELECT_MAP_Around(agv_x, agv_y);
                    //                int around_AGV_Count = 0;
                    //                if (map_Around != null && map_Around.Rows.Count > 0)
                    //                {
                    //                    around_AGV_Count = int.Parse(map_Around.Rows[0][0].ToString());
                    //                }

                    //                //2017年12月20日14:51:33 低电压旋转至0度
                    //                if (((int.Parse(agv_y) == 1 && int.Parse(agv_x) > 5 && int.Parse(agv_x) < 14)
                    //                || (int.Parse(agv_y) == 3 && int.Parse(agv_x) > 5 && int.Parse(agv_x) < 13))
                    //                && int.Parse(agv_V) < 50000)
                    //                {
                    //                    if (skip_angle != 0)
                    //                    {
                    //                        KIVA_SendTo_AGV_Turn(agv_ip, orders, Int32.Parse(orderNo, System.Globalization.NumberStyles.HexNumber) + 2, "0");
                    //                    }
                    //                }
                    //                //if (skip_angle != int.Parse(to_skip_angle) && around_AGV_Count == 0)
                    //                //else if (skip_angle != int.Parse(to_skip_angle))
                    //                else if (skip_angle != int.Parse(to_skip_angle) && around_AGV_Count == 0)
                    //                {
                    //                    KIVA_SendTo_AGV_Turn(agv_ip, orders, Int32.Parse(orderNo, System.Globalization.NumberStyles.HexNumber) + 2, to_skip_angle);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion

                    #region 发送路径
                    //发送的条件 1：有路径（Ostring）,且Onow==0
                    DataTable AGV_Order = Function.KIVA_SELECT_AGV_Order_S();//A.OAGV=B.AGV_Ip  AND  B.AGV_Ac=1
                    if (AGV_Order != null && AGV_Order.Rows.Count > 0)
                    {
                        for (int i = 0; i < AGV_Order.Rows.Count; i++)
                        {
                            int AGV_Now_X = int.Parse(AGV_Order.Rows[i][0].ToString().Trim()) / Lent + (int.Parse(AGV_Order.Rows[i][0].ToString().Trim()) % Lent > (Lent / 2) ? 1 : 0);
                            int AGV_Now_Y = int.Parse(AGV_Order.Rows[i][1].ToString().Trim()) / Lent + (int.Parse(AGV_Order.Rows[i][1].ToString().Trim()) % Lent > (Lent / 2) ? 1 : 0);
                            string AGV_IP = AGV_Order.Rows[i][2].ToString().Trim();
                            string Orders = AGV_Order.Rows[i][3].ToString().Trim();
                            string AGV_TO = AGV_Order.Rows[i][6].ToString().Trim();
                            int warivest = int.Parse(AGV_Order.Rows[i]["warivest"].ToString().Trim());
                            int Lspeed = int.Parse(AGV_Order.Rows[i][7].ToString().Trim());
                            int Rspeed = int.Parse(AGV_Order.Rows[i][8].ToString().Trim());
                            int now_order = int.Parse(AGV_Order.Rows[i][9].ToString().Trim());
                            int now_order_count = int.Parse(AGV_Order.Rows[i][10].ToString().Trim());
                            int islock = int.Parse(AGV_Order.Rows[i][11].ToString().Trim());
                            int Err = int.Parse(AGV_Order.Rows[i][12].ToString().Trim());
                            int AtRFID = int.Parse(AGV_Order.Rows[i][14].ToString().Trim());
                            int command = int.Parse(AGV_Order.Rows[i][16].ToString().Trim());
                            int my_order_count = int.Parse(AGV_Order.Rows[i][17].ToString().Trim());
                            string SKIP_Ang = "90";
                            if (!string.IsNullOrEmpty(AGV_Order.Rows[i][15].ToString().Trim()))
                            {
                                SKIP_Ang = AGV_Order.Rows[i][15].ToString().Trim();
                            }
                            int order_cfmat = Int32.Parse(AGV_Order.Rows[i][13].ToString().Trim(), System.Globalization.NumberStyles.HexNumber);
                            //发送条件 2：有路径（Ostring） AGV在线 且停止 且无故障  且无任务
                            if (!string.IsNullOrEmpty(Orders) && (Math.Abs(Lspeed) + Math.Abs(Rspeed)) < 1200
                                    && Err == 0 && now_order_count == 0 && now_order == 0 && AtRFID == 1
                                    && AGV_TO != (AGV_Now_X.ToString() + "," + AGV_Now_Y.ToString()))
                            {
                                int ordersCount = Orders.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Length;
                                string send_orders = "";//指令内容
                                for (int m = 0; m < ordersCount; m++)
                                {
                                    send_orders += Orders.Split(';')[m] + ";";
                                }
                                this.KIVA_SendTo_AGV_UNLOCK(AGV_IP);
                                Thread.Sleep(50);

                                this.KIVA_SendTo_AGV_Run(AGV_IP, send_orders, command, SKIP_Ang);
                                Thread.Sleep(50);
                                Function.KIVA_UPDATE_AGV_Order_ONow(AGV_IP, ordersCount, 1);//UPDATE tb_AGV_Order SET ONow=@ONow WHERE OAGV=@OAGV 
                                Thread.Sleep(50);
                            }
                            else if (now_order_count + now_order == my_order_count + 1 && !string.IsNullOrEmpty(Orders))
                            {
                                Function.KIVA_UPDATE_AGV_Order(AGV_IP, 0, "", "0", AGV_Now_X.ToString() + "," + AGV_Now_Y.ToString(), AGV_TO);
                            }
                        }
                    }
                    #endregion

                    #region 判断是否需要充电
                    //AGV在线   无故障   无任务  未锁定   停止   电压<50000
                    DataTable agv_for_Charge = Function.KIVA_SELECT_AGV_Info_FORCHARGE();
                    if (agv_for_Charge != null && agv_for_Charge.Rows.Count > 0)
                    {
                        string agv_IP = "";
                        string skip_No = "";
                        string agv_temp_xy = "";
                        string agv_now_x = "";
                        string agv_now_y = "";
                        string charge_no = "";
                        string charge_x = "";
                        string charge_y = "";
                        for (int i = 0; i < agv_for_Charge.Rows.Count; i++)
                        {
                            agv_IP = agv_for_Charge.Rows[0][1].ToString().Trim();
                            skip_No = agv_for_Charge.Rows[0][5].ToString().Trim();
                            agv_temp_xy = FormartXY(agv_for_Charge.Rows[0][3].ToString().Trim(), agv_for_Charge.Rows[0][4].ToString().Trim());
                            agv_now_x = agv_temp_xy.Split(',')[0];
                            agv_now_y = agv_temp_xy.Split(',')[1];

                            DataTable hasCharge = Function.KIVA_SELECT_charge_Info_byagvIP(agv_IP);
                            if (hasCharge == null || hasCharge.Rows.Count == 0)
                            {
                                string NOW = (int.Parse(agv_now_x) + int.Parse(agv_now_y) * Map_X).ToString();

                                //判断位置是否为待机点
                                if (int.Parse(agv_now_x) == 0 && int.Parse(agv_now_y) > 10)
                                {
                                    //寻找 最近的 可用的 充电站
                                    string bestXY = FindBestCharge(agv_now_x, agv_now_y);
                                    if (!string.IsNullOrEmpty(bestXY))
                                    {
                                        charge_no = bestXY.Split(',')[0];
                                        charge_x = bestXY.Split(',')[1];
                                        charge_y = bestXY.Split(',')[2];

                                        //生成一圈充电路径：@592;598;558;552;412;418;                                   
                                        string CHARGE_power_st = (int.Parse(charge_x) + int.Parse(charge_y) * Map_X).ToString();

                                        string WSTRING = "@" + NOW + ";";
                                        WSTRING += CHARGE_power_st + ";";

                                        ////在 tb_Charge_INFO 中记录 AGV从 哪个点 来充电站的, AGVIP
                                        //Function.KIVA_UPDATE_Charge1("192.168.1." + (int.Parse(charge_no) + 160).ToString(), agv_temp_xy, 1, agv_IP);

                                        ////存入tb_WCS_To_KIVA：wstring ,wac=1
                                        //Function.KIVA_UPDATE_WCS_WSTRING_WAC(skip_No, WSTRING, 1, NOW);

                                        Function.KIVA_UPDATE_WCS_WSTRING_WAC(skip_No, WSTRING, 1, NOW, "192.168.1." + (int.Parse(charge_no) + 160).ToString(), agv_temp_xy, 1, agv_IP);
                                    }
                                    else//没电，且没充电站可用，料架旋转至0度
                                    {


                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region AGV到位，开始充电
                    //到位条件：AGV IP等于charge表里的Charge_AGV,   且AGV_X,Y==Charge_X,Y      且电压<50000
                    DataTable charge_agv = Function.KIVA_SELECT_Charge_Ready(Lent);
                    if (charge_agv != null && charge_agv.Rows.Count > 0)
                    {
                        for (int i = 0; i < charge_agv.Rows.Count; i++)
                        {
                            string charge_agv_ip = charge_agv.Rows[i][8].ToString();
                            string charge_ip = charge_agv.Rows[i][1].ToString();
                            string charge_ang = charge_agv.Rows[i][9].ToString();//充电所需角度
                            int agv_v = int.Parse(charge_agv.Rows[i][10].ToString());
                            int agv_ang = int.Parse(charge_agv.Rows[i][11].ToString());//AGV角度
                            string agv_x_temp = charge_agv.Rows[i][12].ToString();
                            string agv_y_temp = charge_agv.Rows[i][13].ToString();
                            string temp = FormartXY(agv_x_temp, agv_y_temp);
                            string agv_x = temp.Split(',')[0];
                            string agv_y = temp.Split(',')[1];
                            string orderNo = charge_agv.Rows[i][14].ToString().Trim();
                            int skip_ang = RtAngel(int.Parse(charge_agv.Rows[i][15].ToString().Trim()));
                            int agv_I = int.Parse(charge_agv.Rows[i][16].ToString().Trim());
                            int charge_state = int.Parse(charge_agv.Rows[i][4].ToString().Trim());
                            string orders = (int.Parse(agv_y) * Map_X + int.Parse(agv_x)).ToString() + "," + charge_ang + ";";
                            int NowOrderNo = int.Parse(charge_agv.Rows[i]["AGV_Now_Ord"].ToString());
                            int NowOrderCount = int.Parse(charge_agv.Rows[i]["AGV_Now_Ord_Count"].ToString());
                            if (charge_state == 1 && agv_I < 10000)
                            {
                                if (Math.Abs(int.Parse(charge_ang) - agv_ang) < 7 || Math.Abs(int.Parse(charge_ang) -(360- agv_ang)) < 7)//小于7度
                                {
                                    //打开小车回路
                                    KIVA_SendTo_AGV_OpenHuiLu(charge_agv_ip);
                                    //伸出充电头
                                    KIVA_SendTo_Charge_AskorControl(charge_ip, 2);
                                    Function.PR_UPDATE_Charge_Info_datetime(charge_ip, charge_agv_ip);
                                }
                                else
                                {
                                    if (NowOrderNo + NowOrderCount == 0)
                                    {
                                        KIVA_SendTo_AGV_Turn(charge_agv_ip, orders, Int32.Parse(orderNo, System.Globalization.NumberStyles.HexNumber) + 1, skip_ang.ToString());
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region 充电完成
                    //充电完成条件：charge_AGV有IP，且AGV_X,Y==Charge_X,Y, 电压>58500
                    //DataTable charge_end2 = Function.KIVA_SELECT_Charge_end2(Lent);
                    //if (charge_end2 != null && charge_end2.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < charge_end2.Rows.Count; i++)
                    //    {
                    //        string charge_agv_ip = charge_end2.Rows[i][8].ToString();
                    //        string charge_ip = charge_end2.Rows[i][1].ToString();
                    //        int charge_state = int.Parse(charge_end2.Rows[i][4].ToString());
                    //        string charge_x = charge_end2.Rows[i][5].ToString();
                    //        string charge_y = charge_end2.Rows[i][6].ToString();
                    //        //string charge_from = charge_end.Rows[i][7].ToString();
                    //        //string to_x = charge_from.Split(',')[0];
                    //        //string to_y = charge_from.Split(',')[1];
                    //        string skip_no = charge_end2.Rows[i][10].ToString();
                    //        int agv_v = int.Parse(charge_end2.Rows[i][11].ToString());
                    //        int charge_agv_orderNo = Int32.Parse(charge_end2.Rows[i][12].ToString(),
                    //            System.Globalization.NumberStyles.HexNumber);

                    //        if (charge_state == 3)
                    //        {
                    //            DataTable nullLocation = Function.PR_SELECT_AGV_ST();
                    //            string to_x = (int.Parse(nullLocation.Rows[0][0].ToString()) % Map_X).ToString();
                    //            string to_y = (int.Parse(nullLocation.Rows[0][0].ToString()) / Map_X).ToString();
                    //            //返回路径
                    //            string NOW = (int.Parse(charge_x) + int.Parse(charge_y) * Map_X).ToString();
                    //            string BackFrom = (int.Parse(to_x) + int.Parse(to_y) * Map_X).ToString();
                    //            string CHARGE_READY_RET = (int.Parse(NOW) - 1).ToString();
                    //            string WSTRING_RETURN = "@" + NOW + ";";
                    //            //WSTRING_RETURN += CHARGE_READY_RET + ";";
                    //            WSTRING_RETURN += BackFrom + ";";

                    //            Function.KIVA_UPDATE_WCS_WSTRING_WAC(skip_no, WSTRING_RETURN, 1, NOW);

                    //            //更新：Charge_From='0'   Charge_AGV='0'
                    //            //Function.KIVA_UPDATE_Charge1(charge_ip, "0", 1, "0");
                    //            Function.KIVA_UPDATE_Station(nullLocation.Rows[0][0].ToString(), charge_agv_ip);
                    //        }
                    //    }
                    //}

                    DataTable charge_end = Function.KIVA_SELECT_Charge_end(Lent);
                    if (charge_end != null && charge_end.Rows.Count > 0)
                    {
                        for (int i = 0; i < charge_end.Rows.Count; i++)
                        {
                            string charge_agv_ip = charge_end.Rows[i][8].ToString();
                            string charge_ip = charge_end.Rows[i][1].ToString();
                            int charge_state = int.Parse(charge_end.Rows[i][4].ToString());
                            string charge_x = charge_end.Rows[i][5].ToString();
                            string charge_y = charge_end.Rows[i][6].ToString();
                            //string charge_from = charge_end.Rows[i][7].ToString();
                            //string to_x = charge_from.Split(',')[0];
                            //string to_y = charge_from.Split(',')[1];
                            string skip_no = charge_end.Rows[i][10].ToString();
                            //int agv_v = int.Parse(charge_end.Rows[i][11].ToString());
                            //int charge_agv_orderNo = Int32.Parse(charge_end.Rows[i][12].ToString(),
                            //    System.Globalization.NumberStyles.HexNumber);

                            if (charge_state == 2)
                            {
                                //关闭小车回路
                                KIVA_SendTo_AGV_CloseHuiLu(charge_agv_ip);
                                //缩回充电头
                                KIVA_SendTo_Charge_AskorControl(charge_ip, 1);
                                //Function.KIVA_UPDATE_Charge(charge_ip, 3);
                            }
                            else if (charge_state == 1)
                            {
                                DataTable nullLocation = Function.PR_SELECT_AGV_ST(charge_agv_ip);
                                if (nullLocation != null && nullLocation.Rows.Count > 0 && !string.IsNullOrEmpty(nullLocation.Rows[0][0].ToString()))
                                {
                                    string to_x = (int.Parse(nullLocation.Rows[0][0].ToString())%Map_X).ToString();
                                    string to_y = (int.Parse(nullLocation.Rows[0][0].ToString())/Map_X).ToString();
                                    //返回路径
                                    string NOW = (int.Parse(charge_x) + int.Parse(charge_y)*Map_X).ToString();
                                    string BackFrom = (int.Parse(to_x) + int.Parse(to_y)*Map_X).ToString();
                                    //string CHARGE_READY_RET = (int.Parse(NOW) - 1).ToString();
                                    string WSTRING_RETURN = "@" + NOW + ";";
                                    //WSTRING_RETURN += CHARGE_READY_RET + ";";
                                    WSTRING_RETURN += BackFrom + ";";

                                    //Function.KIVA_UPDATE_WCS_WSTRING_WAC(skip_no, WSTRING_RETURN, 1, NOW);

                                    ////更新：Charge_From='0'   Charge_AGV='0'
                                    ////Function.KIVA_UPDATE_Charge1(charge_ip, "0", 1, "0");
                                    //Function.KIVA_UPDATE_Station(nullLocation.Rows[0][0].ToString(), charge_agv_ip);

                                    Function.KIVA_UPDATE_WCS_WSTRING_WAC(skip_no, WSTRING_RETURN, 1, NOW,
                                        nullLocation.Rows[0][0].ToString(), charge_agv_ip, charge_ip, charge_x+","+charge_y,to_x+","+to_y);
                                }
                            }
                        }
                    }
                    #endregion

                    #region 更新Onow
                    try
                    {
                        Thread.Sleep(100);
                    }
                    catch (Exception)
                    {
                        
                    }
                    DataTable agv_order_info = Function.KIVA_SELECT_AGV_Order();
                    if (agv_order_info != null && agv_order_info.Rows.Count > 0)
                    {
                        for (int i = 0; i < agv_order_info.Rows.Count; i++)
                        {
                            string temp_AGV_Now_X = agv_order_info.Rows[i][0].ToString();
                            string temp_AGV_Now_Y = agv_order_info.Rows[i][1].ToString();
                            string AGV_L_Speed = agv_order_info.Rows[i][7].ToString();
                            string AGV_R_Speed = agv_order_info.Rows[i][8].ToString();
                            string AGV_Now_Ord = agv_order_info.Rows[i][9].ToString();
                            string AGV_Now_Ord_Count = agv_order_info.Rows[i][10].ToString();
                            int Xadd = 0;
                            if (int.Parse(temp_AGV_Now_X) % Lent >= (Lent / 2))
                            {
                                Xadd++;
                            }
                            int Yadd = 0;
                            if (int.Parse(temp_AGV_Now_Y) % Lent >= (Lent / 2))
                            {
                                Yadd++;
                            }
                            //获取AGV当前位置
                            string AGV_Now_X = (int.Parse(temp_AGV_Now_X) / Lent + Xadd).ToString();
                            string AGV_Now_Y = (int.Parse(temp_AGV_Now_Y) / Lent + Yadd).ToString();

                            //起点==AGV当前位置 
                            if (!string.IsNullOrEmpty(agv_order_info.Rows[i][3].ToString().Trim()) && agv_order_info.Rows[i][5].ToString().Trim() == AGV_Now_X + "," + AGV_Now_Y
                                && (Math.Abs(int.Parse(AGV_L_Speed)) + Math.Abs(int.Parse(AGV_R_Speed))) < 1500
                                && int.Parse(AGV_Now_Ord) == 0 && int.Parse(AGV_Now_Ord_Count) == 0)
                            {
                                again_send_time++;
                                if (again_send_time > 30)
                                {
                                    //UPDATE tb_AGV_Order SET ONow=0 
                                    Function.KIVA_UPDATE_AGV_Order_ONow(agv_order_info.Rows[i][2].ToString(), 0, 1);
                                    //?导致AGV在起点时一直发送任务，以为Onow==0，判定为停止，且无任务
                                    again_send_time = 0;
                                }
                            }
                            //终点==AGV当前位置
                            else if (agv_order_info.Rows[i][6].ToString().Trim() == AGV_Now_X + "," + AGV_Now_Y)
                            {
                                //UPDATE tb_AGV_Order SET OString='',ONow=0 
                                Function.KIVA_UPDATE_AGV_Order_ONow(agv_order_info.Rows[i][2].ToString(), 0, 0);
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    SaveLogExTest("Main:"+ex.StackTrace+"\r\n Error:"+ex.Message);
                }
            }
        }

        private void RunCross()
        {
            while (!bstop)
            {
                try
                {
                    #region 锁定/解锁-----章鱼触角//可定义触角长度--交管
                    DataTable GetAGV_FullRoad = Function.SELECT_AGV_INFO_FORCross();
                    if (GetAGV_FullRoad != null && GetAGV_FullRoad.Rows.Count > 0)
                    {
                        DataTable OutControl_AGV;
                        trafficControl.GetTrafficControl(GetAGV_FullRoad, out OutControl_AGV);
                        if (OutControl_AGV != null && OutControl_AGV.Rows.Count > 0)
                        {
                            for (int i = 0; i < OutControl_AGV.Rows.Count; i++)
                            {
                                string AGV_IP = OutControl_AGV.Rows[i]["AGV_IP"].ToString().Trim();
                                int AGV_NO = int.Parse(OutControl_AGV.Rows[i]["AGV_No"].ToString().Trim());
                                int AGV_Control = int.Parse(OutControl_AGV.Rows[i]["AGV_Control"].ToString().Trim());
                                int AGV_lock = int.Parse(OutControl_AGV.Rows[i]["AGV_Islock"].ToString().Trim());//
                                int AGV_RunSt = int.Parse(OutControl_AGV.Rows[i]["AGV_RUNST"].ToString().Trim());//
                                int AGV_Speed = int.Parse(OutControl_AGV.Rows[i]["AGV_Speed"].ToString().Trim());//
                                if (AGV_Control == 0)
                                {
                                    //停止
                                    if (AGV_lock == 0 && AGV_RunSt > 0)
                                    {
                                        KIVA_SendTo_AGV_LOCK(AGV_IP, 0);
                                    }
                                }
                                else
                                {
                                    //放行
                                    if (AGV_lock == 1 && AGV_RunSt > 0)
                                    {
                                        KIVA_SendTo_AGV_UNLOCK(AGV_IP);
                                    }
                                }
                            }
                        }
                    }
                    Thread.Sleep(50);
                    #endregion

                    #region 判断是否需要重新计算路径

                    #endregion
                }
                catch (Exception ex)
                {
                    SaveLogExTest("RunCross:" + ex.StackTrace + "\r\n Error:" + ex.Message);
                }
            }
        }

        //充电控制
        private void RunChargeState()
        {
            while (!bstop)
            {
                Thread.Sleep(150);
                DataTable Charge_Info = Function.KIVA_SELECT_Charge();
                if (Charge_Info != null && Charge_Info.Rows.Count > 0)
                {
                    for (int i = 0; i < Charge_Info.Rows.Count; i++)
                    {
                        string Charge_IP = Charge_Info.Rows[i][1].ToString().Trim();
                        KIVA_SendTo_Charge_AskorControl(Charge_IP, 0);
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mItems_RGVMonitor.Checked)
            {
                frm_car.GetInfo();
            }
            
            if (wCS监控ToolStripMenuItem.Checked)
            {
                frm_wcs.GetInfo();
            }
            
            if (充电站监控ToolStripMenuItem.Checked)
            {
                frm_PowerInfo.GetInfo();
            }
            
            //if (toolStripMenuItem4.Checked)
            //{
            //    frm_info_buffer.GetInfo();
            //}



            ts_Lable.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        }

        //---------------------------------------------------------------------------

        #region KIVA指令发送
        /// <summary>
        /// 发布运行任务--移动到目标位置：0x01
        /// </summary>
        /// <param name="IP">目标IP</param>
        /// <param name="sendorders">5,90;6,90;9,180;</param>
        private void KIVA_SendTo_AGV_Run(string IP, string sendorders,int Comfmart,string SKIP_Ang)
        {
            try
            {
                string[] temp = Function.splitOrders(sendorders).Split('.');
                string XYAs = temp[0];//XYA
                string orderCount = temp[1];//指令数量
                int Int_orderCount = int.Parse(orderCount);

                string[] XYA = new string[Int_orderCount];
                string[] temps = XYAs.Split(',');

                for (int i = 0; i < int.Parse(orderCount); i++)
                {
                    XYA[i] = temps[3 * i] + "," + temps[3 * i + 1] + "," + temps[3 * i + 2];
                }

                int length = 10;//整帧长度=10 + 数据部分orders长度
                byte[] SendMESS = new byte[10 + 3 + 170];
                byte[] Orders = new byte[3 + 170];

                int agvNo = int.Parse(IP.Split('.')[3]) - 200;

                SendMESS[0] = 0xfc;
                SendMESS[2] = 0x00;
                SendMESS[3] = (byte)agvNo;
                SendMESS[4] = 0x00;
                SendMESS[5] = 0x00;
                SendMESS[6] = 0x01;

                Orders[0] = (byte)(Comfmart / 256);//任务帧编号
                Orders[1] = (byte)(Comfmart % 256);
                Orders[2] = (byte)Int_orderCount;

                for (int i = 0; i < Int_orderCount; i++)
                {
                    Orders[17 * i + 3] = 0x01;//命令标识

                    Orders[17 * i + 4] = 0x00;//命令序号
                    Orders[17 * i + 5] = (byte)(i + 1);
                    //X
                    Orders[17 * i + 6] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(0, 2), 16));
                    Orders[17 * i + 7] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(2, 2), 16));
                    Orders[17 * i + 8] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(4, 2), 16));
                    Orders[17 * i + 9] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(6, 2), 16));
                    //Y                                          
                    Orders[17 * i + 10] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(0, 2), 16));
                    Orders[17 * i + 11] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(2, 2), 16));
                    Orders[17 * i + 12] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(4, 2), 16));
                    Orders[17 * i + 13] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(6, 2), 16));
                    //AGV_A                                   
                    Orders[17 * i + 14] = (byte)(Convert.ToInt32(int.Parse((XYA[i].Split(',')[2])).ToString("X4").Substring(0, 2), 16));
                    Orders[17 * i + 15] = (byte)(Convert.ToInt32(int.Parse((XYA[i].Split(',')[2])).ToString("X4").Substring(2, 2), 16));
                    //SKIP_A
                    Orders[17 * i + 16] = (byte)(Convert.ToInt32(int.Parse(SKIP_Ang).ToString("X4").Substring(0, 2), 16));//SKIP_A
                    Orders[17 * i + 17] = (byte)(Convert.ToInt32(int.Parse(SKIP_Ang).ToString("X4").Substring(2, 2), 16));

                    Orders[17 * i + 18] = 0x00;//预留位
                    Orders[17 * i + 19] = 0x00;//预留位
                    int Arrver_X = int.Parse(XYA[i].Split(',')[0]);
                    int Arrver_Y = int.Parse(XYA[i].Split(',')[1]);
                    DataTable GetNull = Function.PR_SELECT_Charge_Info(Arrver_X, Arrver_Y);
                    if (GetNull != null && GetNull.Rows.Count > 0)
                    {
                        Orders[17 * i + 19] = 0x01;//预留位
                    }
                    
                }
                int dateLength = 17 * Int_orderCount + 3;
                length += dateLength;
                SendMESS[1] = (byte)length;
                Array.Copy(Orders, 0, SendMESS, 7, dateLength);

                byte[] GetCRC = new byte[10 + dateLength];
                Array.Copy(SendMESS, 0, GetCRC, 0, (7 + dateLength));
                byte[] Get_CRC = CRC.CRC16(GetCRC);
                SendMESS[7 + dateLength] = Get_CRC[0]; ;//效验码高位
                SendMESS[7 + dateLength + 1] = Get_CRC[1];//效验码低位
                SendMESS[7 + dateLength + 2] = 0x5a;//结束码

                byte[] FinalSendMESS = new byte[10 + dateLength];
                Array.Copy(SendMESS, 0, FinalSendMESS, 0, 10 + dateLength);

                SendDefault(FinalSendMESS, IP);
            }
            catch (Exception e)
            {
                FileControl.LogFile.SaveLog_Err(e.ToString(), IP);
            }
        }

        /// <summary>
        /// 预充电点去往充电点，缩小障碍物
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="sendorders"></param>
        /// <param name="Comfmart"></param>
        /// <param name="SKIP_Ang"></param>
        private void KIVA_SendTo_AGV_Run_01(string IP, string sendorders, int Comfmart, string SKIP_Ang)
        {
            try
            {
                string[] temp = Function.splitOrders(sendorders).Split('.');
                string XYAs = temp[0];//XYA
                string orderCount = temp[1];//指令数量
                int Int_orderCount = int.Parse(orderCount);

                string[] XYA = new string[Int_orderCount];
                string[] temps = XYAs.Split(',');

                for (int i = 0; i < int.Parse(orderCount); i++)
                {
                    XYA[i] = temps[3 * i] + "," + temps[3 * i + 1] + "," + temps[3 * i + 2];
                }

                int length = 10;//整帧长度=10 + 数据部分orders长度
                byte[] SendMESS = new byte[10 + 3 + 170];
                byte[] Orders = new byte[3 + 170];

                int agvNo = int.Parse(IP.Split('.')[3]) - 200;

                SendMESS[0] = 0xfc;
                SendMESS[2] = 0x00;
                SendMESS[3] = (byte)agvNo;
                SendMESS[4] = 0x00;
                SendMESS[5] = 0x00;
                SendMESS[6] = 0x01;

                Orders[0] = (byte)(Comfmart / 256);//任务帧编号
                Orders[1] = (byte)(Comfmart % 256);
                Orders[2] = (byte)Int_orderCount;

                for (int i = 0; i < Int_orderCount; i++)
                {
                    Orders[17 * i + 3] = 0x01;//命令标识

                    Orders[17 * i + 4] = 0x00;//命令序号
                    Orders[17 * i + 5] = (byte)(i + 1);
                    //X
                    Orders[17 * i + 6] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(0, 2), 16));
                    Orders[17 * i + 7] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(2, 2), 16));
                    Orders[17 * i + 8] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(4, 2), 16));
                    Orders[17 * i + 9] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(6, 2), 16));
                    //Y                                          
                    Orders[17 * i + 10] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(0, 2), 16));
                    Orders[17 * i + 11] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(2, 2), 16));
                    Orders[17 * i + 12] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(4, 2), 16));
                    Orders[17 * i + 13] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(6, 2), 16));
                    //AGV_A                                   
                    Orders[17 * i + 14] = (byte)(Convert.ToInt32(int.Parse((XYA[i].Split(',')[2])).ToString("X4").Substring(0, 2), 16));
                    Orders[17 * i + 15] = (byte)(Convert.ToInt32(int.Parse((XYA[i].Split(',')[2])).ToString("X4").Substring(2, 2), 16));

                    Orders[17 * i + 16] = (byte)(Convert.ToInt32(int.Parse(SKIP_Ang).ToString("X4").Substring(0, 2), 16));//SKIP_A
                    Orders[17 * i + 17] = (byte)(Convert.ToInt32(int.Parse(SKIP_Ang).ToString("X4").Substring(2, 2), 16));

                    Orders[17 * i + 18] = 0x00;//预留位
                    Orders[17 * i + 19] = 0x01;//预留位
                }
                int dateLength = 17 * Int_orderCount + 3;
                length += dateLength;
                SendMESS[1] = (byte)length;
                Array.Copy(Orders, 0, SendMESS, 7, dateLength);

                byte[] GetCRC = new byte[10 + dateLength];
                Array.Copy(SendMESS, 0, GetCRC, 0, (7 + dateLength));
                byte[] Get_CRC = CRC.CRC16(GetCRC);
                SendMESS[7 + dateLength] = Get_CRC[0]; ;//效验码高位
                SendMESS[7 + dateLength + 1] = Get_CRC[1];//效验码低位
                SendMESS[7 + dateLength + 2] = 0x5a;//结束码

                byte[] FinalSendMESS = new byte[10 + dateLength];
                Array.Copy(SendMESS, 0, FinalSendMESS, 0, 10 + dateLength);

                SendDefault(FinalSendMESS, IP);
            }
            catch (Exception e)
            {
                FileControl.LogFile.SaveLog_Err(e.ToString(), IP);
            }
        }

        /// <summary>
        /// 旋转料车
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="sendorders"></param>
        /// <param name="Comfmart"></param>
        private void KIVA_SendTo_AGV_Turn(string IP, string sendorders, int Comfmart,string angle)
        {
            try
            {
                string[] temp = Function.splitOrders(sendorders).Split('.');
                string XYAs = temp[0];//XYA
                string orderCount = temp[1];//指令数量
                int Int_orderCount = int.Parse(orderCount);

                string[] XYA = new string[Int_orderCount];
                string[] temps = XYAs.Split(',');

                for (int i = 0; i < int.Parse(orderCount); i++)
                {
                    XYA[i] = temps[3 * i] + "," + temps[3 * i + 1] + "," + temps[3 * i + 2];
                }

                int length = 10;//整帧长度=10 + 数据部分orders长度
                byte[] SendMESS = new byte[10 + 3 + 170];
                byte[] Orders = new byte[3 + 170];

                int agvNo = int.Parse(IP.Split('.')[3]) - 200;

                SendMESS[0] = 0xfc;
                SendMESS[2] = 0x00;
                SendMESS[3] = (byte)agvNo;
                SendMESS[4] = 0x00;
                SendMESS[5] = 0x00;
                SendMESS[6] = 0x01;

                Orders[0] = (byte)(Comfmart / 256);//任务帧编号
                Orders[1] = (byte)(Comfmart % 256);
                Orders[2] = (byte)Int_orderCount;

                for (int i = 0; i < Int_orderCount; i++)
                {
                    Orders[17 * i + 3] = 0x01;//命令标识

                    Orders[17 * i + 4] = 0x00;//命令序号
                    Orders[17 * i + 5] = (byte)(i + 1);
                    //X
                    Orders[17 * i + 6] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(0, 2), 16));
                    Orders[17 * i + 7] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(2, 2), 16));
                    Orders[17 * i + 8] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(4, 2), 16));
                    Orders[17 * i + 9] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[0])) * Lent).ToString("X8").Substring(6, 2), 16));
                    //Y                                          
                    Orders[17 * i + 10] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(0, 2), 16));
                    Orders[17 * i + 11] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(2, 2), 16));
                    Orders[17 * i + 12] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(4, 2), 16));
                    Orders[17 * i + 13] = (byte)(Convert.ToInt32(((int.Parse(XYA[i].Split(',')[1])) * Lent).ToString("X8").Substring(6, 2), 16));
                    //AGV_A                                   
                    Orders[17 * i + 14] = (byte)(Convert.ToInt32(int.Parse((XYA[i].Split(',')[2])).ToString("X4").Substring(0, 2), 16));
                    Orders[17 * i + 15] = (byte)(Convert.ToInt32(int.Parse((XYA[i].Split(',')[2])).ToString("X4").Substring(2, 2), 16));
                    //skip_A
                    Orders[17 * i + 16] = (byte)(Convert.ToInt32(int.Parse(angle).ToString("X4").Substring(0, 2), 16));
                    Orders[17 * i + 17] = (byte)(Convert.ToInt32(int.Parse(angle).ToString("X4").Substring(2, 2), 16));

                    Orders[17 * i + 18] = 0x00;//预留位
                    Orders[17 * i + 19] = 0x00;//预留位
                }
                int dateLength = 17 * Int_orderCount + 3;
                length += dateLength;
                SendMESS[1] = (byte)length;
                Array.Copy(Orders, 0, SendMESS, 7, dateLength);

                byte[] GetCRC = new byte[10 + dateLength];
                Array.Copy(SendMESS, 0, GetCRC, 0, (7 + dateLength));
                byte[] Get_CRC = CRC.CRC16(GetCRC);
                SendMESS[7 + dateLength] = Get_CRC[0]; ;//效验码高位
                SendMESS[7 + dateLength + 1] = Get_CRC[1];//效验码低位
                SendMESS[7 + dateLength + 2] = 0x5a;//结束码

                byte[] FinalSendMESS = new byte[10 + dateLength];
                Array.Copy(SendMESS, 0, FinalSendMESS, 0, 10 + dateLength);

                SendDefault(FinalSendMESS, IP);
            }
            catch (Exception e)
            {
                FileControl.LogFile.SaveLog_Err(e.ToString(), IP);
            }
        }

        /// <summary>
        /// 发布运行任务--升起料架顶杆:0x02
        /// </summary>
        /// <param name="IP"></param>
        public void KIVA_SendTo_AGV_SkipUp(string IP,string x,string y,string agv_ang,string skip_ang)
        {
            byte[] SendMESS = new byte[10 + 3 + 16];
            int agvNo = int.Parse(IP.Split('.')[3]) - 200;

            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)agvNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x01;

            SendMESS[7] = SendMESS[7] = (byte)(DateTime.Now.Millisecond / 256 > 255 ? 255 : DateTime.Now.Millisecond / 256);
            SendMESS[8] = (byte)(DateTime.Now.Millisecond % 256);
            SendMESS[9] = 0x01;//任务个数
            SendMESS[10] = 0x02;//命令标识
            SendMESS[11] = 0x00;//命令序号
            SendMESS[12] = 0x01;//命令序号
            //X
            SendMESS[13] = (byte)(Convert.ToInt32((int.Parse(x) * Lent).ToString("X8").Substring(0, 2), 16));
            SendMESS[14] = (byte)(Convert.ToInt32((int.Parse(x) * Lent).ToString("X8").Substring(2, 2), 16));
            SendMESS[15] = (byte)(Convert.ToInt32((int.Parse(x) * Lent).ToString("X8").Substring(4, 2), 16));
            SendMESS[16] = (byte)(Convert.ToInt32((int.Parse(x) * Lent).ToString("X8").Substring(6, 2), 16));
            //Y                                          
            SendMESS[17] = (byte)(Convert.ToInt32((int.Parse(y) * Lent).ToString("X8").Substring(0, 2), 16));
            SendMESS[18] = (byte)(Convert.ToInt32((int.Parse(y) * Lent).ToString("X8").Substring(2, 2), 16));
            SendMESS[19] = (byte)(Convert.ToInt32((int.Parse(y) * Lent).ToString("X8").Substring(4, 2), 16));
            SendMESS[20] = (byte)(Convert.ToInt32((int.Parse(y) * Lent).ToString("X8").Substring(6, 2), 16));
            //AGV_A                                   
            SendMESS[21] = (byte)(Convert.ToInt32(int.Parse(agv_ang).ToString("X4").Substring(0, 2), 16));
            SendMESS[22] = (byte)(Convert.ToInt32(int.Parse(agv_ang).ToString("X4").Substring(2, 2), 16));
            //SKIP_A
            SendMESS[23] = (byte)(Convert.ToInt32(int.Parse(skip_ang).ToString("X4").Substring(0, 2), 16));
            SendMESS[24] = (byte)(Convert.ToInt32(int.Parse(skip_ang).ToString("X4").Substring(2, 2), 16));
            SendMESS[25] = 0x00;


            byte[] GetCRC = new byte[26];
            Array.Copy(SendMESS, 0, GetCRC, 0, (7 + 3 + 16));
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[7 + 3 + 16] = Get_CRC[0];//效验码高位
            SendMESS[7 + 3 + 16 + 1] = Get_CRC[1];//效验码低位
            SendMESS[7 + 3 + 16 + 2] = 0x5a;//结束码

            SendDefault(SendMESS, IP);
        }

        /// <summary>
        /// 发布运行任务--降下料架顶杆:0x03
        /// </summary>
        /// <param name="IP"></param>
        public void KIVA_SendTo_AGV_SkipDown(string IP, string x, string y, string agv_ang, string skip_ang)
        {
            byte[] SendMESS = new byte[10 + 3 + 16];
            int agvNo = int.Parse(IP.Split('.')[3]) - 200;

            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)agvNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x01;

            SendMESS[7] = SendMESS[7] = (byte)(DateTime.Now.Millisecond / 256 > 255 ? 255 : DateTime.Now.Millisecond / 256);
            SendMESS[8] = (byte)(DateTime.Now.Millisecond % 256);
            SendMESS[9] = 0x01;//任务个数
            SendMESS[10] = 0x03;//命令标识
            SendMESS[11] = 0x00;//命令序号
            SendMESS[12] = 0x01;//命令序号
            //X
            SendMESS[13] = (byte)(Convert.ToInt32((int.Parse(x) * Lent).ToString("X8").Substring(0, 2), 16));
            SendMESS[14] = (byte)(Convert.ToInt32((int.Parse(x) * Lent).ToString("X8").Substring(2, 2), 16));
            SendMESS[15] = (byte)(Convert.ToInt32((int.Parse(x) * Lent).ToString("X8").Substring(4, 2), 16));
            SendMESS[16] = (byte)(Convert.ToInt32((int.Parse(x) * Lent).ToString("X8").Substring(6, 2), 16));
            //Y                                          
            SendMESS[17] = (byte)(Convert.ToInt32((int.Parse(y) * Lent).ToString("X8").Substring(0, 2), 16));
            SendMESS[18] = (byte)(Convert.ToInt32((int.Parse(y) * Lent).ToString("X8").Substring(2, 2), 16));
            SendMESS[19] = (byte)(Convert.ToInt32((int.Parse(y) * Lent).ToString("X8").Substring(4, 2), 16));
            SendMESS[20] = (byte)(Convert.ToInt32((int.Parse(y) * Lent).ToString("X8").Substring(6, 2), 16));
            //AGV_A                                   
            SendMESS[21] = (byte)(Convert.ToInt32(int.Parse(agv_ang).ToString("X4").Substring(0, 2), 16));
            SendMESS[22] = (byte)(Convert.ToInt32(int.Parse(agv_ang).ToString("X4").Substring(2, 2), 16));
            //SKIP_A
            SendMESS[23] = (byte)(Convert.ToInt32(int.Parse(skip_ang).ToString("X4").Substring(0, 2), 16));
            SendMESS[24] = (byte)(Convert.ToInt32(int.Parse(skip_ang).ToString("X4").Substring(2, 2), 16));
            SendMESS[25] = 0x00;

            byte[] GetCRC = new byte[26];
            Array.Copy(SendMESS, 0, GetCRC, 0, (7 + 3 + 16));
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[7 + 3 + 16] = Get_CRC[0];//效验码高位
            SendMESS[7 + 3 + 16 + 1] = Get_CRC[1];//效验码低位
            SendMESS[7 + 3 + 16 + 2] = 0x5a;//结束码

            SendDefault(SendMESS, IP);
        }

        /// <summary>
        /// 发布运行任务--顶升盘复位:0x04
        /// </summary>
        /// <param name="IP"></param>
        public void KIVA_SendTo_AGV_TuoPanReset(string IP)
        {
            byte[] SendMESS = new byte[10 + 3 + 16];
            int agvNo = int.Parse(IP.Split('.')[3]) - 200;

            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)agvNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x01;

            SendMESS[7] = 0x00;
            SendMESS[8] = 0x00;
            SendMESS[9] = 0x01;//任务个数
            SendMESS[10] = 0x04;//命令标识
            SendMESS[11] = 0x11;//命令序号
            SendMESS[12] = 0x01;//命令序号
            SendMESS[13] = 0x00;//预留
            SendMESS[14] = 0x00;
            SendMESS[15] = 0x00;
            SendMESS[16] = 0x00;
            SendMESS[17] = 0x00;
            SendMESS[18] = 0x00;
            SendMESS[19] = 0x00;
            SendMESS[20] = 0x00;
            SendMESS[21] = 0x00;
            SendMESS[22] = 0x00;
            SendMESS[23] = 0x00;
            SendMESS[24] = 0x00;
            SendMESS[25] = 0x00;

            byte[] GetCRC = new byte[26];
            Array.Copy(SendMESS, 0, GetCRC, 0, (7 + 3 + 16));
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[7 + 3 + 16] = Get_CRC[0];//效验码高位
            SendMESS[7 + 3 + 16 + 1] = Get_CRC[1];//效验码低位
            SendMESS[7 + 3 + 16 + 2] = 0x5a;//结束码

            SendDefault(SendMESS, IP);
        }

        /// <summary>
        /// 询问AGV状态（整贞长度length=10+0=10）
        /// </summary>
        /// <param name="IP"></param>
        private void KIVA_SendTo_AGV_AskState(string IP)
        {
            byte[] SendMESS = new byte[10];
            int agvNo = int.Parse(IP.Split('.')[3]) - 200;

            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;//数据长度
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)agvNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x02;
            byte[] GetCRC = new byte[7];
            Array.Copy(SendMESS, 0, GetCRC, 0, 7);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[7] = Get_CRC[0];//效验码高位
            SendMESS[8] = Get_CRC[1];//效验码低位
            SendMESS[9] = 0x5a;//结束码
            SendDefault(SendMESS, IP);
        }

        /// <summary>
        /// 询问/控制充电站（0询问，1缩回，2伸出）
        /// </summary>
        /// <param name="IP"></param>
        public void KIVA_SendTo_Charge_AskorControl(string IP,int state)
        {
            byte[] SendMESS = new byte[11];
            int chargeNo = int.Parse(IP.Split('.')[3]) - 160;

            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;//数据长度
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)chargeNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0xf1;
            SendMESS[7] = (byte)state;


            byte[] GetCRC = new byte[8];
            Array.Copy(SendMESS, 0, GetCRC, 0, 8);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[8] = Get_CRC[0];//效验码高位
            SendMESS[9] = Get_CRC[1];//效验码低位
            SendMESS[10] = 0x5a;//结束码
            SendDefault(SendMESS, IP);
        }

        /// <summary>
        /// 打开AGV充电回路
        /// </summary>
        /// <param name="IP"></param>
        public void KIVA_SendTo_AGV_OpenHuiLu(string IP)
        {
            byte[] SendMESS = new byte[12];
            int agvNo = int.Parse(IP.Split('.')[3]) - 200;

            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)agvNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x07;//打开充电回路

            SendMESS[7] = (byte)(DateTime.Now.Millisecond / 256 > 255 ? 255 : DateTime.Now.Millisecond / 256);
            SendMESS[8] = (byte)(DateTime.Now.Millisecond % 256);

            byte[] GetCRC = new byte[9];
            Array.Copy(SendMESS, 0, GetCRC, 0, 9);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[9] = Get_CRC[0];//效验码高位
            SendMESS[10] = Get_CRC[1];//效验码低位
            SendMESS[11] = 0x5a;//结束码

            SendDefault(SendMESS, IP);
        }

        /// <summary>
        /// 关闭AGV充电回路
        /// </summary>
        /// <param name="IP"></param>
        public void KIVA_SendTo_AGV_CloseHuiLu(string IP)
        {
            byte[] SendMESS = new byte[12];
            int agvNo = int.Parse(IP.Split('.')[3]) - 200;

            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)agvNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x08;//关闭充电回路

            SendMESS[7] = (byte)(DateTime.Now.Millisecond / 256 > 255 ? 255 : DateTime.Now.Millisecond / 256);
            SendMESS[8] = (byte)(DateTime.Now.Millisecond % 256);

            byte[] GetCRC = new byte[9];
            Array.Copy(SendMESS, 0, GetCRC, 0, 9);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[9] = Get_CRC[0];//效验码高位
            SendMESS[10] = Get_CRC[1];//效验码低位
            SendMESS[11] = 0x5a;//结束码

            SendDefault(SendMESS, IP);
        }


        /// <summary>
        /// 暂停AGV
        /// </summary>
        /// <param name="IP"></param>
        private void KIVA_SendTo_AGV_PAUSE(string IP)
        {
            byte[] SendMESS = new byte[12];
            int agvNo = int.Parse(IP.Split('.')[3]) - 200;

            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)agvNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x06;//暂停

            SendMESS[7] = (byte)(DateTime.Now.Millisecond / 256 > 255 ? 255 : DateTime.Now.Millisecond / 256);
            SendMESS[8] = (byte)(DateTime.Now.Millisecond % 256);

            byte[] GetCRC = new byte[9];
            Array.Copy(SendMESS, 0, GetCRC, 0, 9);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[9] = Get_CRC[0];//效验码高位
            SendMESS[10] = Get_CRC[1];//效验码低位
            SendMESS[11] = 0x5a;//结束码

            SendDefault(SendMESS, IP);
        }

        /// <summary>
        /// 初始化AGV
        /// </summary>
        /// <param name="IP"></param>
        public void KIVA_SendTo_AGV_Initialize(string IP)
        {
            byte[] SendMESS = new byte[12];
            int agvNo = int.Parse(IP.Split('.')[3]) - 200;
            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)agvNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x04;//初始化
            SendMESS[7] = (byte)(DateTime.Now.Millisecond / 256 > 255 ? 255 : DateTime.Now.Millisecond / 256);
            SendMESS[8] = (byte)(DateTime.Now.Millisecond % 256);
            byte[] GetCRC = new byte[9];
            Array.Copy(SendMESS, 0, GetCRC, 0, 9);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[9] = Get_CRC[0];//效验码高位
            SendMESS[10] = Get_CRC[1];//效验码低位
            SendMESS[11] = 0x5a;//结束码

            SendDefault(SendMESS, IP);
        }


        private void KIVA_SendTo_AGV_Repair(string IP)
        {
            byte[] SendMESS = new byte[12];
            int agvNo = int.Parse(IP.Split('.')[3]) - 200;
            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)agvNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x03;//复位
            SendMESS[7] = (byte)(DateTime.Now.Millisecond / 256 > 255 ? 255 : DateTime.Now.Millisecond / 256);
            SendMESS[8] = (byte)(DateTime.Now.Millisecond % 256);
            byte[] GetCRC = new byte[9];
            Array.Copy(SendMESS, 0, GetCRC, 0, 9);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[9] = Get_CRC[0];//效验码高位
            SendMESS[10] = Get_CRC[1];//效验码低位
            SendMESS[11] = 0x5a;//结束码

            SendDefault(SendMESS, IP);
        }

        /// <summary>
        /// 锁定AGV
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="sendorders"></param>
        /// <param name="Comfmart"></param>
        private void KIVA_SendTo_AGV_LOCK(string IP,int IsPath)
        {
            byte[] SendMESS = new byte[13];
            int agvNo = int.Parse(IP.Split('.')[3]) - 200;
            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)agvNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x0b;//锁定
            SendMESS[7] = (byte)(DateTime.Now.Millisecond / 256 > 255 ? 255 : DateTime.Now.Millisecond / 256);
            SendMESS[8] = (byte)(DateTime.Now.Millisecond % 256);
            SendMESS[9] = (byte)IsPath;
            byte[] GetCRC = new byte[10];
            Array.Copy(SendMESS, 0, GetCRC, 0, 10);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[10] = Get_CRC[0];//效验码高位
            SendMESS[11] = Get_CRC[1];//效验码低位
            SendMESS[12] = 0x5a;//结束码
            SendDefault(SendMESS, IP);
        }

        /// <summary>
        /// 解锁AGV
        /// </summary>
        /// <param name="IP"></param>
        private void KIVA_SendTo_AGV_UNLOCK(string IP)
        {
            byte[] SendMESS = new byte[12];
            int agvNo = int.Parse(IP.Split('.')[3]) - 200;

            SendMESS[0] = 0xfc;
            SendMESS[1] = (byte)SendMESS.Length;
            SendMESS[2] = 0x00;
            SendMESS[3] = (byte)agvNo;
            SendMESS[4] = 0x00;
            SendMESS[5] = 0x00;
            SendMESS[6] = 0x0c;//解锁

            SendMESS[7] = (byte)(DateTime.Now.Millisecond / 256 > 255 ? 255 : DateTime.Now.Millisecond / 256);
            SendMESS[8] = (byte)(DateTime.Now.Millisecond % 256);

            byte[] GetCRC = new byte[9];
            Array.Copy(SendMESS, 0, GetCRC, 0, 9);
            byte[] Get_CRC = CRC.CRC16(GetCRC);
            SendMESS[9] = Get_CRC[0];//效验码高位
            SendMESS[10] = Get_CRC[1];//效验码低位
            SendMESS[11] = 0x5a;//结束码

            SendDefault(SendMESS, IP);
        }

        #endregion

        //--------------------------------------------------------------------------

        #region long long ago  
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

        private void mItems_OrdMessage_Click(object sender, EventArgs e)
        {

        }
        
        private void 充电站监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            充电站监控ToolStripMenuItem.Checked = !充电站监控ToolStripMenuItem.Checked;
            if (充电站监控ToolStripMenuItem.Checked)
            {
                frm_PowerInfo.StartPosition = FormStartPosition.Manual;

                frm_PowerInfo.Show();
            }
            else
            {
                frm_PowerInfo.Hide();
            }
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            frm_Login frm_login = new frm_Login();
            frm_login.MdiParent = this;
            frm_login.StartPosition = FormStartPosition.Manual;

            frm_login.Show();
        }

        private void 接收日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\RGVrevDATA");
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
                System.Diagnostics.Process.Start(Application.StartupPath + @"\RGVsendDATA");
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
                System.Diagnostics.Process.Start(Application.StartupPath + @"\RGVpowerDATA");
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
                System.Diagnostics.Process.Start(Application.StartupPath + @"\RGVrfidDATA");
            }
            catch
            {
                MessageBox.Show("没有找到指定的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void mItems_RGV_Alams_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\RGVerrorDATA");
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
                System.Diagnostics.Process.Start(Application.StartupPath + @"\RGVvolDATA");
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
                if (MessageBox.Show("请先登录拥有此操作权限的账户！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    frm_Login login = new frm_Login();
                    login.Show();
                    return;
                }
                return;
            }
            frm_Manage_Param frm = new frm_Manage_Param();
            frm.MdiParent = this;
            frm.StartPosition = FormStartPosition.Manual;

            frm.Show();
        }

       
        private void 软件启停日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\SystemLog");
            }
            catch
            {
                MessageBox.Show("没有找到指定的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        #endregion
       
        /// <summary>
        /// 初始化地图大小
        /// </summary>
        /// <param name="X_Count"></param>
        /// <param name="Y_Count"></param>
        public void FormartGetPath(int X_Count, int Y_Count)
        {
            getPathGroup = new GetPathGroup(X_Count, Y_Count, b);
        }

        /// <summary>
        /// 返回XY除以Lent后的整数值:X,Y
        /// </summary>
        /// <param name="x">x000</param>
        /// <param name="y">y000</param>
        /// <returns></returns>
        public string FormartXY(string x, string y)
        {
            int Xadd = 0;
            if ((int.Parse(x) > 0 ? int.Parse(x) : 0) % Lent >= (Lent / 2))
            {
                Xadd++;
            }
            int Yadd = 0;
            if ((int.Parse(y) > 0 ? int.Parse(y) : 0) % Lent >= (Lent / 2))
            {
                Yadd++;
            }
            string X = ((int.Parse(x) > 0 ? int.Parse(x) : 0) / Lent + Xadd).ToString();
            string Y = ((int.Parse(y) > 0 ? int.Parse(y) : 0) / Lent + Yadd).ToString();
            return X + "," + Y;
        }

        /// <summary>
        /// 判断目标点在不在锁定车四周的点
        /// </summary>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <param name="locX"></param>
        /// <param name="locY"></param>
        /// <returns></returns>
        public  bool IsInLockLocation(int toX, int toY, int vs_carNo)
        {
            DataTable agv_info = Function.KIVA_SELECT_AGV_Info_ByIP("192.168.1." + (vs_carNo + 200).ToString());
            if (agv_info != null && agv_info.Rows.Count > 0)
            {
                string temp_x = agv_info.Rows[0][3].ToString();
                string temp_y = agv_info.Rows[0][4].ToString();
                int vs_CarX = int.Parse(FormartXY(temp_x, temp_y).Split(',')[0]);
                int vs_CarY = int.Parse(FormartXY(temp_x, temp_y).Split(',')[1]);
                if (toY * Map_X + toX == (vs_CarY * Map_X + vs_CarX + 1)
                    || toY * Map_X + toX == (vs_CarY * Map_X + vs_CarX - 1)
                    || toY * Map_X + toX == ((vs_CarY + 1) * Map_X + vs_CarX)
                    || toY * Map_X + toX == ((vs_CarY - 1) * Map_X + vs_CarX))
                {
                    return true;
                }
                else
                    return false;

            }
            else
                return false;
        }

        /// <summary>
        /// 根据车编号获取坐标：“X,Y”
        /// </summary>
        /// <param name="vs_carNo"></param>
        /// <returns></returns>
        public string NowLocation(int vs_carNo)
        {
            DataTable agv_info = Function.KIVA_SELECT_AGV_Info_ByIP("192.168.1." + (vs_carNo + 200).ToString());
            if (agv_info != null && agv_info.Rows.Count > 0)
            {
                string temp_x = agv_info.Rows[0][3].ToString();
                string temp_y = agv_info.Rows[0][4].ToString();
                return FormartXY(temp_x, temp_y);
            }
            else
                return null;
        }

        /// <summary>
        /// 更新锁定车四周的锁定点的状态为0
        /// </summary>
        /// <param name="lockNo">锁定车的编号</param>
        public void Update_lock_location(int lockCarNo)
        {
            string lock_loc = NowLocation(lockCarNo);
            string x = lock_loc.Split(',')[0];
            string y = lock_loc.Split(',')[1];
            Function.KIVA_UPDATE_Map_Info_State(x, y, 0);
        }

        private void 任务监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            任务监控ToolStripMenuItem.Checked = !任务监控ToolStripMenuItem.Checked;
            if (任务监控ToolStripMenuItem.Checked)
            {
                frm_agv_order.StartPosition = FormStartPosition.Manual;
                frm_agv_order.Show();
            }
            else
            {
                frm_agv_order.Hide();
            }
        }

        private void wCS监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wCS监控ToolStripMenuItem.Checked = !wCS监控ToolStripMenuItem.Checked;
            if (wCS监控ToolStripMenuItem.Checked)
            {
                frm_wcs.StartPosition = FormStartPosition.Manual;
                frm_wcs.Show();
            }
            else
            {
                frm_wcs.Hide();
            }
        }

        /// <summary>
        /// 返回任务的终点坐标编号
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public string Get_orders_finalLocation(string orders)
        {
            string res = "";
            if (!string.IsNullOrEmpty(orders))
            {
                string[] order = orders.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                res = order[order.Length - 1];
                return res.Split(',')[0];
            }
            return res;
        }

        private void 手松充电ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Charge_ByHand f_c_b = new frm_Charge_ByHand(this);
            f_c_b.StartPosition = FormStartPosition.Manual;
            f_c_b.Show();
        }

        /// <summary>
        /// 查找 最近的 可用的 充电站：no,x,y
        /// </summary>
        /// <param name="now_agv_x">x</param>
        /// <param name="now_agv_y">y</param>
        /// <returns></returns>
        public string FindBestCharge(string now_agv_x, string now_agv_y)
        {
            //充电站状态： 0-掉线 1-空闲（缩回） 2-充电（伸出）
            DataTable charge = Function.KIVA_SELECT_Charge_canuse();
            if (charge != null && charge.Rows.Count > 0)
            {
                int[] all_distances = new int[charge.Rows.Count];
                for (int i = 0; i < charge.Rows.Count; i++)
                {
                    string x = charge.Rows[i][5].ToString();
                    string y = charge.Rows[i][6].ToString();
                    all_distances[i] = Math.Abs(int.Parse(x) - int.Parse(now_agv_x)) + Math.Abs(int.Parse(y) - int.Parse(now_agv_y));
                }
                int best = 0;
                for (int j = 0; j < all_distances.Length; j++)
                {
                    if (all_distances[best] < all_distances[j])
                        best = j;
                }
                return (int.Parse(charge.Rows[best][1].ToString().Substring(10, 3)) - 160).ToString() + "," + charge.Rows[best][5].ToString() + "," + charge.Rows[best][6].ToString();

            }
            return null;
        }

        /// <summary>
        /// 依据车或料架编号返回“起始点”:x,y
        /// </summary>
        /// <param name="agvNo"></param>
        /// <returns></returns>
        public string RetStartPoint(string agvNo)
        {
            return (int.Parse(agvNo) + 9).ToString() + "," + "11";
        }

        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
           KIVA_SendTo_AGV_SkipUp("192.168.1.207", "16", "11", "0", "90");
           //KIVA_SendTo_AGV_Run("192.168.1.207", "236,90", 99, "90");
        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //KIVA_SendTo_AGV_SkipDown("192.168.1.202", "11", "11", "90", "90");
            KIVA_SendTo_AGV_SkipUp("192.168.1.207", "16", "11", "90", "90");

        }

        public int GetAng(string ang)
        {
            int angle = int.Parse(ang);
            if (angle < 45 | angle > 315)
                return 0;
            else if (angle > 45 && angle < 135)
                return 90;
            else if (angle > 135 && angle < 225)
                return 180;
            else if (angle > 225 && angle < 315)
                return 270;
            else
                return -1;
        }

        /// <summary>
        /// x,y————>no
        /// </summary>
        /// <param name="xandy"></param>
        /// <returns></returns>
        public string RetLocationNo(string xandy)
        {
            int x = int.Parse(xandy.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0]);
            int y = int.Parse(xandy.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            int res = y * Map_X + x;
            return res.ToString();
        }

        public static void SaveLogExTest(string fileMsg)
        {
            try
            {
                using (FileStream _fStream = new FileStream(@"D:\exlog.txt", FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(DateTime.Now + "  " + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void SavePathLog(string fileMsg)
        {
            try
            {
                using (FileStream _fStream = new FileStream(@"D:\pathlog.txt", FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter _sWrite = new StreamWriter(_fStream))
                    {
                        _sWrite.WriteLine(DateTime.Now + "  " + fileMsg);
                        _sWrite.Close();
                        _fStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
