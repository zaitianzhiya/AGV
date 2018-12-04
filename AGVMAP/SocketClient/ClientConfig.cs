using System;

namespace SocketClient
{
	public class ClientConfig
	{
		private const string DefauleServerIP = "192.168.1.230";

		private const int DefaultPort = 6001;

		private const int DefaultTimeOut = 60;

		private const int DefaultBufferSize = 1024;

		public string ServerIP
		{
			get;
			set;
		}

		public int Port
		{
			get;
			set;
		}

		public int TimeOut
		{
			get;
			set;
		}

		public int ReceiveBufferSize
		{
			get;
			set;
		}

		public ClientConfig()
		{
			this.ServerIP = "192.168.1.230";
			this.Port = 6001;
			this.TimeOut = 60;
			this.ReceiveBufferSize = 1024;
		}

		public ClientConfig(int port) : this()
		{
			this.Port = port;
		}
	}
}
