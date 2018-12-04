using System;
using System.Text;

namespace SocketModel
{
	public class SuperSocketMsg
	{
		public const string msg = "{0}MFLAG{1}ENDFLAG";

		public string Command
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}

		public SuperSocketMsg(string content, string command)
		{
			this.Content = content;
			this.Command = command;
		}

		public byte[] ToBuffer()
		{
			return Encoding.UTF8.GetBytes(string.Format("{0}MFLAG{1}ENDFLAG", this.Command, this.Content));
		}
	}
}
