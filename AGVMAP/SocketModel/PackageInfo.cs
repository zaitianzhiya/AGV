using System;

namespace SocketModel
{
	public class PackageInfo
	{
		public string PackHead
		{
			get;
			set;
		}

		public string PackContent
		{
			get;
			set;
		}

		public string Command
		{
			get;
			set;
		}

		public int PackContentLengh
		{
			get;
			set;
		}

		public PackageInfo()
		{
			this.PackHead = "";
			this.PackContent = "";
			this.Command = "";
			this.PackContentLengh = 0;
		}
	}
}
