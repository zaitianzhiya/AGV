using System;

namespace Canvas.CanvasInterfaces
{
	public interface ICanvasOwner
	{
		void SetPositionInfo(UnitPoint unitpos);

		void SetSnapInfo(ISnapPoint snap);
	}
}
