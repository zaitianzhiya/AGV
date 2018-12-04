using Canvas.CanvasInterfaces;
using System;

namespace Canvas
{
	internal class EditCommandEditTool : EditCommandBase
	{
		private IEditTool m_tool;

		public EditCommandEditTool(IEditTool tool)
		{
			this.m_tool = tool;
		}

		public override bool DoUndo(IModel data)
		{
			this.m_tool.Undo();
			return true;
		}

		public override bool DoRedo(IModel data)
		{
			this.m_tool.Redo();
			return true;
		}
	}
}
