using SocketModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace SocketClient
{
	public class TcpClientSever
	{
		public object LockObj = new object();

		private Thread processor;

		private ClientConfig config;

		private Socket _clientsocket;

		private bool keepserver;

		[method: CompilerGenerated]
		public event NetEvent RecvSuccess;

		public ServerStateEnum State
		{
			get;
			set;
		}

		public bool IsConnected
		{
			get
			{
				return this._clientsocket.Connected;
			}
		}

		public bool Setup(ClientConfig _config)
		{
			bool flag = this._clientsocket != null;
			if (flag)
			{
				this._clientsocket.Close();
			}
			this.config = _config;
			this._clientsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.State = ServerStateEnum.NotStarted;
			return true;
		}

		public bool Start()
		{
			bool result;
			try
			{
				this.keepserver = true;
				IPAddress address = IPAddress.Parse(this.config.ServerIP);
				this._clientsocket.Connect(new IPEndPoint(address, this.config.Port));
				this.processor = new Thread(new ThreadStart(this.RecMessage));
				this.processor.IsBackground = true;
				this.processor.Start();
				this.State = ServerStateEnum.Running;
			}
			catch (Exception var_1_72)
			{
				this.State = ServerStateEnum.NotStarted;
				result = false;
				return result;
			}
			result = true;
			return result;
		}

		public bool TestConnect(ClientConfig _config)
		{
			bool result;
			try
			{
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPAddress address = IPAddress.Parse(_config.ServerIP);
				socket.Connect(new IPEndPoint(address, _config.Port));
				socket.Close();
			}
			catch (Exception ex)
			{
				this.State = ServerStateEnum.NotStarted;
				result = false;
				return result;
			}
			result = true;
			return result;
		}

		public void Stop()
		{
			try
			{
				this.keepserver = false;
				this._clientsocket.Shutdown(SocketShutdown.Both);
				bool flag = this.processor != null;
				if (flag)
				{
					this.processor.Abort();
				}
				this.State = ServerStateEnum.NotStarted;
			}
			catch (Exception ex)
			{
				this.State = ServerStateEnum.NotStarted;
			}
		}

		public void RecMessage()
		{
			try
			{
				while (this.keepserver)
				{
					int num = 0;
					int num2 = 12;
					int num3 = 0;
					byte[] array = new byte[12];
					while (num2 - num3 > 0)
					{
						bool flag = num2 - num3 > this.config.ReceiveBufferSize * 1024;
						byte[] array2;
						if (flag)
						{
							array2 = new byte[this.config.ReceiveBufferSize * 1024];
						}
						else
						{
							array2 = new byte[num2 - num3];
						}
						int num4 = this._clientsocket.Receive(array2);
						bool flag2 = num4 <= 0;
						if (flag2)
						{
							bool flag3 = num == 3;
							if (flag3)
							{
								throw new Exception("Socket  错误！");
							}
							num++;
							Thread.Sleep(2000);
						}
						Buffer.BlockCopy(array2, 0, array, num3, num4);
						num3 += num4;
					}
					PackageInfo packageInfo = new PackageInfo();
					string @string = Encoding.UTF8.GetString(array, 0, num2);
					packageInfo = PackParser.ParserHead(@string);
					num = 0;
					int packContentLengh = packageInfo.PackContentLengh;
					num3 = 0;
					byte[] array3 = new byte[packContentLengh];
					while (packContentLengh - num3 > 0)
					{
						bool flag4 = packContentLengh - num3 > this.config.ReceiveBufferSize * 1024;
						byte[] array4;
						if (flag4)
						{
							array4 = new byte[this.config.ReceiveBufferSize * 1024];
						}
						else
						{
							array4 = new byte[packContentLengh - num3];
						}
						int num5 = this._clientsocket.Receive(array4);
						bool flag5 = num5 <= 0;
						if (flag5)
						{
							bool flag6 = num == 3;
							if (flag6)
							{
								throw new Exception("Socket  错误！");
							}
							num++;
							Thread.Sleep(2000);
						}
						Buffer.BlockCopy(array4, 0, array3, num3, num5);
						num3 += num5;
					}
					string string2 = Encoding.UTF8.GetString(array3, 0, packContentLengh);
					packageInfo.PackContent = string2;
					bool flag7 = this.RecvSuccess != null;
					if (flag7)
					{
						this.RecvSuccess(this, packageInfo);
					}
				}
			}
			catch (Exception ex)
			{
				this.Stop();
			}
		}

		private void Log(string msg)
		{
			File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\log.txt", msg + "\r\n");
		}

		public void SendMessage(SuperSocketMsg pack)
		{
			try
			{
				object lockObj = this.LockObj;
				lock (lockObj)
				{
					this._clientsocket.Send(pack.ToBuffer());
				}
			}
			catch (Exception ex)
			{
			}
		}

		public void Exit()
		{
			try
			{
				this._clientsocket.Shutdown(SocketShutdown.Both);
				this._clientsocket.Close();
				this.keepserver = false;
				bool flag = this.processor != null;
				if (flag)
				{
					this.processor.Abort();
				}
				this.State = ServerStateEnum.NotInitialized;
			}
			catch (Exception ex)
			{
				this.State = ServerStateEnum.NotInitialized;
			}
		}
	}
}
