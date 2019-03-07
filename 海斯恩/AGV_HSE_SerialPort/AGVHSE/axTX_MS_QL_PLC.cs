using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Globalization;

namespace Mitsubishi_Q_L_PLC
{
    public enum PlcStatus
    {
        Init,
        Close,
        Run
    }
    class axTX_MS_QL_PLC
    {
        public PlcStatus Plcstatus = PlcStatus.Init;
        public string IPAdress;
        public bool connected = false;
        public Socket clientSocket;
        private IPEndPoint hostEndPoint;
        private Byte[] SendDataPro;
        private Byte[] RecvDataPro;
        private Byte[] SendData;
        private Byte[] RecvData;
        private Byte SendOrRecv;
        private  AutoResetEvent autoConnectEvent = new AutoResetEvent(false);
        private SocketAsyncEventArgs lisnterSocketAsyncEventArgs;

        public delegate void StartListeHandler();
        public event StartListeHandler StartListen;

        public delegate void ReceiveMsgHandler(byte[] info);
        public event ReceiveMsgHandler OnMsgReceived;

        private List<SocketAsyncEventArgs> s_lst = new List<SocketAsyncEventArgs>();

        public axTX_MS_QL_PLC(string hostName, int port)
        {
            IPAdress = hostName;
            IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
            this.hostEndPoint = new IPEndPoint(hostAddresses[hostAddresses.Length - 1], port);
            this.clientSocket = new Socket(this.hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <returns></returns>
        private bool Connect()
        {
            using (SocketAsyncEventArgs args = new SocketAsyncEventArgs())
            {
                args.UserToken = this.clientSocket;
                args.RemoteEndPoint = this.hostEndPoint;
                args.Completed += new EventHandler<SocketAsyncEventArgs>(this.OnConnect);
                this.clientSocket.ConnectAsync(args);
                bool flag = autoConnectEvent.WaitOne(2000);
                if (this.connected)
                {
                    this.lisnterSocketAsyncEventArgs = new SocketAsyncEventArgs();
                    byte[] buffer = new byte[1024];
                    this.lisnterSocketAsyncEventArgs.UserToken = this.clientSocket;
                    this.lisnterSocketAsyncEventArgs.SetBuffer(buffer, 0, buffer.Length);
                    this.lisnterSocketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.OnReceive);
                    this.StartListen();
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 判断有没有连接上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnect(object sender, SocketAsyncEventArgs e)
        {
            this.connected = (e.SocketError == SocketError.Success);
            autoConnectEvent.Set();
        }
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="mes"></param>
        public void Send(byte[] mes)
        {
            if (this.connected)
            {
                EventHandler<SocketAsyncEventArgs> handler = null;
                byte[] buffer = mes;
                SocketAsyncEventArgs senderSocketAsyncEventArgs = null;
                lock (s_lst)
                {
                    if (s_lst.Count > 0)
                    {
                        senderSocketAsyncEventArgs = s_lst[s_lst.Count - 1];
                        s_lst.RemoveAt(s_lst.Count - 1);
                    }
                }
                if (senderSocketAsyncEventArgs == null)
                {
                    senderSocketAsyncEventArgs = new SocketAsyncEventArgs();
                    senderSocketAsyncEventArgs.UserToken = this.clientSocket;
                    senderSocketAsyncEventArgs.RemoteEndPoint = this.clientSocket.RemoteEndPoint;
                    if (handler == null)
                    {
                        handler = delegate(object sender, SocketAsyncEventArgs _e)
                        {
                            lock (s_lst)
                            {
                                s_lst.Add(senderSocketAsyncEventArgs);
                            }
                        };
                    }
                    senderSocketAsyncEventArgs.Completed += handler;
                }
                senderSocketAsyncEventArgs.SetBuffer(buffer, 0, buffer.Length);
                this.clientSocket.SendAsync(senderSocketAsyncEventArgs);
            }
            else
            {
                this.connected = false;
            }
        }
        /// <summary>
        /// 监听服务端
        /// </summary>
        public void Listen()
        {
            if (this.connected && this.clientSocket != null)
            {
                try
                {
                    (lisnterSocketAsyncEventArgs.UserToken as Socket).ReceiveAsync(lisnterSocketAsyncEventArgs);
                }
                catch (Exception)
                {
                }
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        private int Disconnect()
        {
            int res = 0;
            try
            {
                this.clientSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
            }
            try
            {
                this.clientSocket.Close();
            }
            catch (Exception)
            {
            }
            this.connected = false;
            this.Plcstatus = PlcStatus.Close;
            return res;
        }
        /// <summary>
        /// 数据接受
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReceive(object sender, SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0)
            {
                try
                {
                    this.clientSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (this.clientSocket.Connected)
                    {
                        this.clientSocket.Close();
                    }
                }
                byte[] info = new Byte[] { 0 };
                this.OnMsgReceived(info);
            }
            else
            {
                byte[] bf = new byte[e.BytesTransferred];
                Buffer.BlockCopy(e.Buffer, 0, bf, 0, e.BytesTransferred);
                //for (int i = 0; i < e.BytesTransferred; i++)
                //{
                //    buffer[i] = e.Buffer[i];
                //}
                this.OnMsgReceived(bf);
                Listen();
            }
        }
        /// <summary>
        /// 接受完成
        /// </summary>
        /// <param name="info"></param>
        private void OmrlonSocketClient_OnMsgReceived(byte[] info)
        {
            if (info[0] != 0)
            {
                if (this.SendOrRecv == 1)
                {
                    this.SendDataPro = info;
                    this.SendOrRecv = 0;
                }
                else if (this.SendOrRecv == 2)
                {
                    this.SendData = info;
                    this.SendOrRecv = 0;
                }
                else if (this.SendOrRecv == 3)
                {
                    this.RecvDataPro = info;
                    this.SendOrRecv = 0;
                }
                else if (this.SendOrRecv == 4)
                {
                    this.RecvData = info;
                    this.SendOrRecv = 0;
                }
            }
            else
            {
                if (this.SendOrRecv == 1)
                {
                    this.SendDataPro = new Byte[] { 0 };
                    this.SendOrRecv = 0;
                }
                else if (this.SendOrRecv == 2)
                {
                    this.SendData = new Byte[] { 0 };
                    this.SendOrRecv = 0;
                }
                else if (this.SendOrRecv == 3)
                {
                    this.RecvDataPro = new Byte[] { 0 };
                    this.SendOrRecv = 0;
                }
                else if (this.SendOrRecv == 4)
                {
                    this.RecvData = new Byte[] { 0 };
                    this.SendOrRecv = 0;
                }
            }
        }
        /// <summary>
        /// 建立连接的方法
        /// </summary>
        /// <returns></returns>
        public bool OpenLinkPLC()
        {
            bool flag = false;
            this.StartListen += new StartListeHandler(OmrlonSocketClient_StartListen);
            this.OnMsgReceived += new ReceiveMsgHandler(OmrlonSocketClient_OnMsgReceived);
            flag = this.Connect();
            if (!flag)
            {
                return flag;
            }
            Plcstatus = PlcStatus.Run;
            return true;
        }

        /// <summary>
        /// 关闭连接的方法
        /// </summary>
        /// <returns></returns>
        public int CloseLinkPLC()
        {
            this.Plcstatus = PlcStatus.Close;
            return this.Disconnect();
        }
        /// <summary>
        /// 监听的方法
        /// </summary>
        private void OmrlonSocketClient_StartListen()
        {
            this.Listen();
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private string FCS(string Value)
        {
            int num2 = 0;
            for (int i = 0; i < Value.Length; i++)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(Value.Substring(i, 1));
                num2 ^= bytes[0];
            }
            return num2.ToString("X");
        }



        /// <summary>
        /// 写入PLC寄存器值（位）
        /// </summary>
        /// <param name="ItemName">寄存器名称</param>
        /// <param name="WriteCount">数量</param>
        /// <param name="ItemData">写入数组</param>
        /// <returns></returns>
        public bool ax_WriteBit(string ItemName, int WriteCount, ref int[] ItemData)
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(ItemName))
            {
                string Get_Type = ItemName.Substring(0, 1);
                //int Get_Name = int.Parse(ItemName.Substring(1, ItemName.Length - 1));
                string Get_Name = ItemName.Substring(1, ItemName.Length - 1);
                if (Get_Type == "m" | Get_Type == "M" | Get_Type == "b" | Get_Type == "B" | Get_Type == "x" | Get_Type == "X" | Get_Type == "Y" | Get_Type == "y")
                {
                    string TxCommand;
                    string CpuTime = "0010";//cpu监视定时器
                    string GetOrder = Get_Ord_String(4);
                    string SetData = "";
                    for (int j = 0; j < WriteCount; j++)
                    {
                        SetData = SetData + ItemData[j];
                    }
                    string Sendf = string.Format(CpuTime + GetOrder + Get_Type.ToUpper() + "*{0}{1}{2}", Get_Name.PadLeft(6, '0'), WriteCount.ToString("X4"), SetData);
                    TxCommand = "500000FF03FF00" + Sendf.Length.ToString("X4") + Sendf;
                    Byte[] sendBuffer = Encoding.Default.GetBytes(TxCommand.ToCharArray());
                    this.SendOrRecv = 1;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        string Getstring = Encoding.Default.GetString(SendDataPro).Trim('\0');
                        try
                        {
                            string SubHeader = Getstring.Substring(0, 4);
                            string EndCord = Getstring.Substring(18, 4);
                            if (SubHeader == "D000" && EndCord == "0000")//正常
                            {
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {
                           
                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 写入PLC寄存器值（字）
        /// </summary>
        /// <param name="ItemName">寄存器名称</param>
        /// <param name="WriteCount">数量</param>
        /// <param name="ItemData">写入数组</param>
        /// <returns></returns>
        public bool ax_WriteWord(string ItemName, int WriteCount, ref int[] ItemData)
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(ItemName))
            {
                string Get_Type = ItemName.Substring(0, 1);
                //int Get_Name = int.Parse(ItemName.Substring(1, ItemName.Length - 1));
                string Get_Name = ItemName.Substring(1, ItemName.Length - 1);
                if (Get_Type == "d" | Get_Type == "D" | Get_Type == "w" | Get_Type == "W")
                {
                    string TxCommand;
                    string CpuTime = "0010";//cpu监视定时器
                    string GetOrder = Get_Ord_String(2);
                    string SetData = "";
                    for (int j = 0; j < WriteCount; j++)
                    {
                        SetData = SetData + ItemData[j].ToString("X4");
                    }
                    string Sendf = string.Format(CpuTime + GetOrder + Get_Type.ToUpper() + "*{0}{1}{2}", Get_Name.PadLeft(6, '0'), WriteCount.ToString("X4"), SetData);
                    TxCommand = "500000FF03FF00" + Sendf.Length.ToString("X4") + Sendf;
                    Byte[] sendBuffer = Encoding.Default.GetBytes(TxCommand.ToCharArray());
                    this.SendOrRecv = 1;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        string Getstring = Encoding.Default.GetString(SendDataPro).Trim('\0');
                        try
                        {
                            string SubHeader = Getstring.Substring(0, 4);
                            string EndCord = Getstring.Substring(18, 4);
                            if (SubHeader == "D000" && EndCord == "0000")//正常
                            {
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {
                            
                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 读Word
        /// </summary>
        /// <param name="ItemName"></param>
        /// <param name="ReadCount"></param>
        /// <param name="ItemData"></param>
        /// <returns></returns>
        public bool ax_ReadWord(string ItemName, int ReadCount, out int[] ItemData)
        {
            bool ret = false;
            ItemData = new int[ReadCount];
            byte[] recvData = new byte[1024];
            if (!string.IsNullOrEmpty(ItemName))
            {
                string Get_Type = ItemName.Substring(0, 1);
                //int Get_Name = int.Parse(ItemName.Substring(1, ItemName.Length - 1));
                string Get_Name = ItemName.Substring(1, ItemName.Length - 1);
                if (Get_Type == "d" | Get_Type == "D" | Get_Type == "w" | Get_Type == "W" | Get_Type == "b" | Get_Type == "B")
                {
                    string TxCommand;
                    string CpuTime = "0010";//cpu监视定时器
                    string GetOrder = Get_Ord_String(1);
                    string Sendf = string.Format(CpuTime + GetOrder + Get_Type.ToUpper() + "*{0}{1}", Get_Name.PadLeft(6, '0'), ReadCount.ToString("X4"));
                    TxCommand = "500000FF03FF00" + Sendf.Length.ToString("X4") + Sendf;
                    Byte[] sendBuffer = Encoding.Default.GetBytes(TxCommand.ToCharArray());
                    this.SendOrRecv = 3;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        string Getstring = Encoding.Default.GetString(RecvDataPro).Trim('\0');
                        if (Getstring.Length == 22 + ReadCount * 4)
                        {
                            string SubHeader = Getstring.Substring(0, 4);
                            string EndCord = Getstring.Substring(18, 4);
                            if (SubHeader == "D000" && EndCord == "0000")//正常
                            {
                                for (int i = 0; i < ReadCount; i++)
                                {
                                    //D00000FF03FF000060 0000   0000 0000 0000 0000 0000 
                                    //                          0000 0000 0000 0000 0000 
                                    //0000 0000 0000 0000 0000 
                                    //0000 007B 0000 0000 0000 
                                    //0000 0000 0000
                                    ItemData[i] = Convert.ToInt32(Getstring.Substring(22 + (i * 4), 4), 16);
                                }
                                ret = true;
                            }
                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 读位
        /// </summary>
        /// <param name="ItemName"></param>
        /// <param name="ReadCount"></param>
        /// <param name="ItemData"></param>
        /// <returns></returns>
        public bool ax_ReadBit(string ItemName, int ReadCount, out int[] ItemData)
        {
            bool ret = false;
            ItemData = new int[ReadCount];
            byte[] recvData = new byte[1024];
            if (!string.IsNullOrEmpty(ItemName))
            {
                string Get_Type = ItemName.Substring(0, 1);
                //int Get_Name = int.Parse(ItemName.Substring(1, ItemName.Length - 1));
                string Get_Name = ItemName.Substring(1, ItemName.Length - 1);
                if (Get_Type == "m" | Get_Type == "M" | Get_Type == "b" | Get_Type == "B" | Get_Type == "x" | Get_Type == "X" | Get_Type == "Y" | Get_Type == "y")
                {
                    string TxCommand;
                    string CpuTime = "0010";//cpu监视定时器
                    string GetOrder = Get_Ord_String(3);
                    if (Get_Name == "1200")
                    {
                        //500000FF03FF000018001004010001B*0012000030
                    }
                    string Sendf = string.Format(CpuTime + GetOrder + Get_Type.ToUpper() + "*{0}{1}", Get_Name.PadLeft(6, '0'), ReadCount.ToString("X4"));
                    TxCommand = "500000FF03FF00" + Sendf.Length.ToString("X4") + Sendf;
                    Byte[] sendBuffer = Encoding.Default.GetBytes(TxCommand.ToCharArray());
                    this.SendOrRecv = 3;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        string Getstring = Encoding.Default.GetString(RecvDataPro).Trim('\0');
                        if (Getstring.Length == 22 + ReadCount)
                        {
                            string SubHeader = Getstring.Substring(0, 4);
                            string EndCord = Getstring.Substring(18, 4);
                            if (SubHeader == "D000" && EndCord == "0000")//正常
                            {
                                for (int i = 0; i < ReadCount; i++)
                                {
                                    ItemData[i] = int.Parse(Getstring.Substring(22 + i, 1));
                                    if (ItemData[i] == 1)
                                    {

                                    }
                                }
                                ret = true;
                            }
                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 命令+子命令
        /// </summary>
        /// <param name="type">1,读取word;2,写入word;3,读取bit;4,写入bit</param>
        /// <returns></returns>
        private string Get_Ord_String(int type)
        {
            string Rt = "";
            switch (type)
            {
                case 1://读取word
                    Rt = "04010000";
                    break;
                case 2://写入word
                    Rt = "14010000";
                    break;
                case 3://读取bit
                    Rt = "04010001";
                    break;
                case 4://写入bit
                    Rt = "14010001";
                    break;
                default:
                    break;
            }
            return Rt;
        }


        //50 00 副标头
        //00 ff 
        //ff 03 
        //00
        //0c 00 
        //01 00 
        //01 14 
        //00 00
        //A6 27 00
        //A8
        //01 00
        //01 02
        /// <summary>
        /// 写入PLC寄存器值（位）
        /// </summary>
        /// <param name="ItemName">寄存器名称</param>
        /// <param name="WriteCount">数量</param>
        /// <param name="ItemData">写入数组</param>
        /// <returns></returns>
        public bool ax_WriteBit_hex(string ItemName, int WriteCount, ref int[] ItemData)
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(ItemName))
            {
                string Get_Type = ItemName.Substring(0, 1);
                string Get_Name = ItemName.Substring(1, ItemName.Length - 1).PadLeft(6, '0');
                if (Get_Type == "m" | Get_Type == "M")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { (byte)Convert.ToByte((WriteCount / 2 + WriteCount % 2 + 12).ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte((WriteCount / 2 + WriteCount % 2 + 12).ToString("X4").Substring(0, 2), 16) };
                    byte[] Sendtime = new byte[2] { 0x10, 0x00 };
                    byte[] SendOrdF = new byte[4] { 0x01, 0x14, 0x01, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0x90 };//WriteCount.ToString("X4")
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] SendData = new byte[WriteCount / 2 + WriteCount % 2];
                    for (int i = 0; i < WriteCount; i++)
                    {
                        if (i % 2 == 0)
                        {
                            SendData[i / 2] = (byte)(16 * ItemData[i]);
                        }
                        else
                        {
                            SendData[i / 2] = (byte)((int)SendData[i / 2] + ItemData[i]);
                        }
                    }
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length + SendData.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    SendData.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length + SendWordLenth.Length);
                    this.SendOrRecv = 1;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (SendDataPro[0] == 0xd0 && SendDataPro[1] == 0x00 && SendDataPro[9] == 0x00 && SendDataPro[10] == 0x00)//正常
                            {
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
                else if (Get_Type == "b" | Get_Type == "B")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { (byte)Convert.ToByte((WriteCount / 2 + WriteCount % 2 + 12).ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte((WriteCount / 2 + WriteCount % 2 + 12).ToString("X4").Substring(0, 2), 16) };
                    byte[] Sendtime = new byte[2] { 0x10, 0x00 };
                    byte[] SendOrdF = new byte[4] { 0x01, 0x14, 0x01, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0xa0 };//WriteCount.ToString("X4")
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] SendData = new byte[WriteCount / 2 + WriteCount % 2];
                    for (int i = 0; i < WriteCount; i++)
                    {
                        if (i % 2 == 0)
                        {
                            SendData[i / 2] = (byte)(16 * ItemData[i]);
                        }
                        else
                        {
                            SendData[i / 2] = (byte)((int)SendData[i / 2] + ItemData[i]);
                        }
                    }
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length + SendData.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    SendData.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length + SendWordLenth.Length);
                    this.SendOrRecv = 1;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (SendDataPro[0] == 0xd0 && SendDataPro[1] == 0x00 && SendDataPro[9] == 0x00 && SendDataPro[10] == 0x00)//正常
                            {
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
                else if (Get_Type == "x" | Get_Type == "X")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { (byte)Convert.ToByte((WriteCount / 2 + WriteCount % 2 + 12).ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte((WriteCount / 2 + WriteCount % 2 + 12).ToString("X4").Substring(0, 2), 16) };
                    byte[] Sendtime = new byte[2] { 0x10, 0x00 };
                    byte[] SendOrdF = new byte[4] { 0x01, 0x14, 0x01, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0x9c };//WriteCount.ToString("X4")
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] SendData = new byte[WriteCount / 2 + WriteCount % 2];
                    for (int i = 0; i < WriteCount; i++)
                    {
                        if (i % 2 == 0)
                        {
                            SendData[i / 2] = (byte)(16 * ItemData[i]);
                        }
                        else
                        {
                            SendData[i / 2] = (byte)((int)SendData[i / 2] + ItemData[i]);
                        }
                    }
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length + SendData.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    SendData.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length + SendWordLenth.Length);
                    this.SendOrRecv = 1;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (SendDataPro[0] == 0xd0 && SendDataPro[1] == 0x00 && SendDataPro[9] == 0x00 && SendDataPro[10] == 0x00)//正常
                            {
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
                else if (Get_Type == "Y" | Get_Type == "y")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { (byte)Convert.ToByte((WriteCount / 2 + WriteCount % 2 + 12).ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte((WriteCount / 2 + WriteCount % 2 + 12).ToString("X4").Substring(0, 2), 16) };
                    byte[] Sendtime = new byte[2] { 0x10, 0x00 };
                    byte[] SendOrdF = new byte[4] { 0x01, 0x14, 0x01, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0x9d };//WriteCount.ToString("X4")
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] SendData = new byte[WriteCount / 2 + WriteCount % 2];
                    for (int i = 0; i < WriteCount; i++)
                    {
                        if (i % 2 == 0)
                        {
                            SendData[i / 2] = (byte)(16 * ItemData[i]);
                        }
                        else
                        {
                            SendData[i / 2] = (byte)((int)SendData[i / 2] + ItemData[i]);
                        }
                    }
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length + SendData.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    SendData.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length + SendWordLenth.Length);
                    this.SendOrRecv = 1;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (SendDataPro[0] == 0xd0 && SendDataPro[1] == 0x00 && SendDataPro[9] == 0x00 && SendDataPro[10] == 0x00)//正常
                            {
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 写入PLC寄存器值（字）
        /// </summary>
        /// <param name="ItemName">寄存器名称</param>
        /// <param name="WriteCount">数量</param>
        /// <param name="ItemData">写入数组</param>
        /// <returns></returns>
        public bool ax_WriteWord_hex(string ItemName, int WriteCount, ref int[] ItemData)
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(ItemName))
            {
                string Get_Type = ItemName.Substring(0, 1);
                string Get_Name = ItemName.Substring(1, ItemName.Length - 1).PadLeft(6, '0');
                if (Get_Type == "d" | Get_Type == "D")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { (byte)Convert.ToByte((WriteCount * 2 + 12).ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte((WriteCount * 2 + 12).ToString("X4").Substring(0, 2), 16) };
                    byte[] Sendtime = new byte[2] { 0x01, 0x00 };
                    byte[] SendOrdF = new byte[4] { 0x01, 0x14, 0x00, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0xa8 };//WriteCount.ToString("X4")
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] SendData = new byte[2 * WriteCount];
                    for (int i = 0; i < WriteCount; i++)
                    {
                        SendData[i * 2] = (byte)Convert.ToByte(ItemData[i].ToString("X4").Substring(2, 2), 16);
                        SendData[i * 2 + 1] = (byte)Convert.ToByte(ItemData[i].ToString("X4").Substring(0, 2), 16);
                    }
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length + SendData.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    SendData.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length + SendWordLenth.Length);
                    this.SendOrRecv = 1;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (SendDataPro[0] == 0xd0 && SendDataPro[1] == 0x00 && SendDataPro[9] == 0x00 && SendDataPro[10] == 0x00)//正常
                            {
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
                else if (Get_Type == "w" | Get_Type == "W")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { (byte)Convert.ToByte((WriteCount * 2 + 12).ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte((WriteCount * 2 + 12).ToString("X4").Substring(0, 2), 16) };
                    byte[] Sendtime = new byte[2] { 0x01, 0x00 };
                    byte[] SendOrdF = new byte[4] { 0x01, 0x14, 0x00, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0xb4 };//WriteCount.ToString("X4")
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(WriteCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] Senddata = new byte[2 * WriteCount];
                    for (int i = 0; i < WriteCount; i++)
                    {
                        Senddata[i * 2] = (byte)Convert.ToByte(ItemData[i].ToString("X4").Substring(2, 2), 16);
                        Senddata[i * 2 + 1] = (byte)Convert.ToByte(ItemData[i].ToString("X4").Substring(0, 2), 16);
                    }
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length + Senddata.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    Senddata.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length + SendWordLenth.Length);
                    this.SendOrRecv = 1;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (SendDataPro[0] == 0xd0 && SendDataPro[1] == 0x00 && SendDataPro[9] == 0x00 && SendDataPro[10] == 0x00)//正常
                            {
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }

            }
            return ret;
        }

        /// <summary>
        /// 读Word
        /// </summary>
        /// <param name="ItemName"></param>
        /// <param name="ReadCount"></param>
        /// <param name="ItemData"></param>
        /// <returns></returns>
        public bool ax_ReadWord_hex(string ItemName, int ReadCount, out int[] ItemData)
        {
            bool ret = false;
            ItemData = new int[ReadCount];
            byte[] recvData = new byte[1024];
            if (!string.IsNullOrEmpty(ItemName))
            {
                string Get_Type = ItemName.Substring(0, 1);
                string Get_Name = ItemName.Substring(1, ItemName.Length - 1).PadLeft(6, '0');
                if (Get_Type == "d" | Get_Type == "D")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { 0x0c, 0x00 };
                    byte[] Sendtime = new byte[2] { 0x01, 0x00 };
                    byte[] SendOrdF = new byte[4] { 0x01, 0x04, 0x00, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0xa8 };
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    this.SendOrRecv = 3;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                         try
                        {
                            if (RecvDataPro[0] == 0xd0 && RecvDataPro[1] == 0x00 && RecvDataPro[9] == 0x00 && RecvDataPro[10] == 0x00)//正常
                            {
                                for (int i = 0; i < ReadCount; i++)
                                {
                                    ItemData[i] = ((int)RecvDataPro[12 + i * 2]) * 256 + ((int)RecvDataPro[11 + i * 2]);
                                }
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
                else if (Get_Type == "w" | Get_Type == "W")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { 0x0c, 0x00 };
                    byte[] Sendtime = new byte[2] { 0x01, 0x00 };
                    byte[] SendOrdF = new byte[4] { 0x01, 0x04, 0x00, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0xb4 };
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    this.SendOrRecv = 3;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (RecvDataPro[0] == 0xd0 && RecvDataPro[1] == 0x00 && RecvDataPro[9] == 0x00 && RecvDataPro[10] == 0x00)//正常
                            {
                                for (int i = 0; i < ReadCount; i++)
                                {
                                    ItemData[i] = ((int)RecvDataPro[12 + i * 2]) * 256 + ((int)RecvDataPro[11 + i * 2]);
                                }
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
                else if (Get_Type == "b" | Get_Type == "B")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { 0x0c, 0x00 };
                    byte[] Sendtime = new byte[2] { 0x01, 0x00 };
                    byte[] SendOrdF = new byte[4] { 0x01, 0x04, 0x00, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0xa0 };
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    this.SendOrRecv = 3;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (RecvDataPro[0] == 0xd0 && RecvDataPro[1] == 0x00 && RecvDataPro[9] == 0x00 && RecvDataPro[10] == 0x00)//正常
                            {
                                for (int i = 0; i < ReadCount; i++)
                                {
                                    ItemData[i] = ((int)RecvDataPro[12 + i * 2]) * 256 + ((int)RecvDataPro[11 + i * 2]);
                                }
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 读位
        /// </summary>
        /// <param name="ItemName"></param>
        /// <param name="ReadCount"></param>
        /// <param name="ItemData"></param>
        /// <returns></returns>
        public bool ax_ReadBit_hex(string ItemName, int ReadCount, out int[] ItemData)
        {
            bool ret = false;
            ItemData = new int[ReadCount];
            byte[] recvData = new byte[1024];
            if (!string.IsNullOrEmpty(ItemName))
            {
                string Get_Type = ItemName.Substring(0, 1);
                string Get_Name = ItemName.Substring(1, ItemName.Length - 1).PadLeft(6, '0');
                if (Get_Type == "m" | Get_Type == "M")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { 0x0c, 0x00 };
                    byte[] Sendtime = new byte[2] { 0x10, 0x00 };//0x01, 0x00
                    byte[] SendOrdF = new byte[4] { 0x01, 0x04, 0x01, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0x90 };
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);               
                    this.SendOrRecv = 3;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (RecvDataPro[0] == 0xd0 && RecvDataPro[1] == 0x00 && RecvDataPro[9] == 0x00 && RecvDataPro[10] == 0x00)//正常
                            {
                                for (int i = 0; i < ReadCount; i++)//38
                                {
                                    if (i % 2 == 0)
                                    {
                                        ItemData[i] = int.Parse(((int)RecvDataPro[11 + i / 2]).ToString("X2").Substring(0, 1));
                                    }
                                    else
                                    {
                                        ItemData[i] = int.Parse(((int)RecvDataPro[11 + i / 2]).ToString("X2").Substring(1, 1));
                                    }
                                }
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
                else if (Get_Type == "b" | Get_Type == "B")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { 0x0c, 0x00 };
                    byte[] Sendtime = new byte[2] { 0x10, 0x00 };//0x01, 0x00
                    byte[] SendOrdF = new byte[4] { 0x01, 0x04, 0x01, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0xa0 };
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    this.SendOrRecv = 3;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (RecvDataPro[0] == 0xd0 && RecvDataPro[1] == 0x00 && RecvDataPro[9] == 0x00 && RecvDataPro[10] == 0x00)//正常
                            {
                                for (int i = 0; i < ReadCount; i++)//38
                                {
                                    if (i % 2 == 0)
                                    {
                                        ItemData[i] = int.Parse(((int)RecvDataPro[11 + i / 2]).ToString("X2").Substring(0, 1));
                                    }
                                    else
                                    {
                                        ItemData[i] = int.Parse(((int)RecvDataPro[11 + i/ 2]).ToString("X2").Substring(1, 1));
                                    }
                                }
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
                else if (Get_Type == "x" | Get_Type == "X")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { 0x0c, 0x00 };
                    byte[] Sendtime = new byte[2] { 0x10, 0x00 };//0x01, 0x00
                    byte[] SendOrdF = new byte[4] { 0x01, 0x04, 0x01, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0x9c };
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    this.SendOrRecv = 3;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (RecvDataPro[0] == 0xd0 && RecvDataPro[1] == 0x00 && RecvDataPro[9] == 0x00 && RecvDataPro[10] == 0x00)//正常
                            {
                                for (int i = 0; i < ReadCount; i++)//38
                                {
                                    if (i % 2 == 0)
                                    {
                                        ItemData[i] = int.Parse(((int)RecvDataPro[11 + i / 2]).ToString("X2").Substring(0, 1));
                                    }
                                    else
                                    {
                                        ItemData[i] = int.Parse(((int)RecvDataPro[11 + i / 2]).ToString("X2").Substring(1, 1));
                                    }
                                }
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
                else if (Get_Type == "Y" | Get_Type == "y")
                {
                    byte[] SendTag = new byte[2] { 0x50, 0x00 };
                    byte[] SendNetNo = new byte[1] { 0x00 };
                    byte[] SendPLCNo = new byte[1] { 0xff };
                    byte[] SendIONo = new byte[2] { 0xff, 0x03 };
                    byte[] SendSTNo = new byte[1] { 0x00 };
                    byte[] SendDataLenth = new byte[2] { 0x0c, 0x00 };
                    byte[] Sendtime = new byte[2] { 0x10, 0x00 };//0x01, 0x00
                    byte[] SendOrdF = new byte[4] { 0x01, 0x04, 0x01, 0x00 };
                    byte[] SendWordName = new byte[3] { (byte)Convert.ToByte(Get_Name.Substring(4, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(2, 2), 16), (byte)Convert.ToByte(Get_Name.Substring(0, 2), 16) };
                    byte[] SendWordValue = new byte[1] { 0x9d };
                    byte[] SendWordLenth = new byte[2] { (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(2, 2), 16), (byte)Convert.ToByte(ReadCount.ToString("X4").Substring(0, 2), 16) };
                    byte[] sendBuffer = new byte[SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length
                                               + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length
                                               + SendWordLenth.Length];
                    SendTag.CopyTo(sendBuffer, 0);
                    SendNetNo.CopyTo(sendBuffer, SendTag.Length);
                    SendPLCNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length);
                    SendIONo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length);
                    SendSTNo.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length);
                    SendDataLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length);
                    Sendtime.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length);
                    SendOrdF.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length);
                    SendWordName.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length);
                    SendWordValue.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length);
                    SendWordLenth.CopyTo(sendBuffer, SendTag.Length + SendNetNo.Length + SendPLCNo.Length + SendIONo.Length + SendSTNo.Length + SendDataLenth.Length + Sendtime.Length + SendOrdF.Length + SendWordName.Length + SendWordValue.Length);
                    this.SendOrRecv = 3;
                    int numPro = 0;
                    this.Send(sendBuffer);
                    while (this.SendOrRecv != 0 && numPro < 100)
                    {
                        Thread.Sleep(5);
                        numPro++;
                    }
                    if (numPro < 100)
                    {
                        try
                        {
                            if (RecvDataPro[0] == 0xd0 && RecvDataPro[1] == 0x00 && RecvDataPro[9] == 0x00 && RecvDataPro[10] == 0x00)//正常
                            {
                                for (int i = 0; i < ReadCount; i++)//38
                                {
                                    if (i % 2 == 0)
                                    {
                                        ItemData[i] = int.Parse(((int)RecvDataPro[11 + i / 2]).ToString("X2").Substring(0, 1));
                                    }
                                    else
                                    {
                                        ItemData[i] = int.Parse(((int)RecvDataPro[11 + i / 2]).ToString("X2").Substring(1, 1));
                                    }
                                }
                                ret = true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        this.SendOrRecv = 0;
                    }
                }
            }
            return ret;
        }

        #region IDispose member
        public void Dispose()
        {
            if (this.clientSocket.Connected)
            {
                this.clientSocket.Close();
            }
        }
        #endregion    
    }
}
