using System;
using System.Text;

namespace SocketModel
{
	public class MessagePackage
	{
		private string _messagetime;

		private string _contentlengh;

		public string Message
		{
			get;
			set;
		}

		public string MessageType
		{
			get;
			set;
		}

		public string Command
		{
			get;
			set;
		}

		public string MessageTime
		{
			get
			{
				return this._messagetime;
			}
		}

		public string ContentLengh
		{
			get
			{
				return this._contentlengh;
			}
		}

		public MessagePackage()
		{
			this.Message = "";
			this._messagetime = "";
			this._contentlengh = "";
			this.MessageType = "0";
			this.Command = "0001";
			this._messagetime = DateTime.Now.ToString("yyyyMMddHHmmss");
		}

		public MessagePackage(string message, string command) : this()
		{
			this.Message = message;
			this.MessageType = "0";
			this.Command = command;
		}

		public MessagePackage(string message, string messagetype, string command) : this()
		{
			this.Message = message;
			this.MessageType = messagetype;
			this.Command = command;
		}

		public override string ToString()
		{
			byte[] bytes = Encoding.UTF8.GetBytes(this.Message);
			this._contentlengh = bytes.Length.ToString().PadLeft(8, '0');
			return this._contentlengh + this.Command + this.Message;
		}

		public byte[] ToBuffer()
		{
			return Encoding.UTF8.GetBytes(this.ToString());
		}
	}
}
