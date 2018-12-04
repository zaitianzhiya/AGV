using System;

namespace SocketModel
{
	public class PackParser
	{
		public static PackageInfo ParserHead(string packhead)
		{
			string command = packhead.Substring(7, 4);
			int packContentLengh = int.Parse(packhead.Substring(0, 8));
			return new PackageInfo
			{
				Command = command,
				PackHead = packhead,
				PackContentLengh = packContentLengh
			};
		}
	}
}
