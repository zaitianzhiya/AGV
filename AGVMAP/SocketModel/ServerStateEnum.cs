using System;

namespace SocketModel
{
	public enum ServerStateEnum
	{
		NotInitialized,
		Initializing,
		NotStarted,
		Starting,
		Running,
		Stopping
	}
}
