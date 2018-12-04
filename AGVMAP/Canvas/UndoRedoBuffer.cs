using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;

namespace Canvas
{
	internal class UndoRedoBuffer
	{
		private List<EditCommandBase> m_undoBuffer = new List<EditCommandBase>();

		private List<EditCommandBase> m_redoBuffer = new List<EditCommandBase>();

		private bool m_canCapture = true;

		private bool m_dirty = false;

		public bool Dirty
		{
			get
			{
				return this.m_dirty;
			}
			set
			{
				this.m_dirty = value;
			}
		}

		public bool CanCapture
		{
			get
			{
				return this.m_canCapture;
			}
		}

		public bool CanUndo
		{
			get
			{
				return this.m_undoBuffer.Count > 0;
			}
		}

		public bool CanRedo
		{
			get
			{
				return this.m_redoBuffer.Count > 0;
			}
		}

		public void Clear()
		{
			this.m_undoBuffer.Clear();
			this.m_redoBuffer.Clear();
		}

		public void AddCommand(EditCommandBase command)
		{
			bool flag = this.m_canCapture && command != null;
			if (flag)
			{
				this.m_undoBuffer.Add(command);
				this.m_redoBuffer.Clear();
				this.Dirty = true;
			}
		}

		public bool DoUndo(IModel data)
		{
			bool flag = this.m_undoBuffer.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.m_canCapture = false;
				EditCommandBase editCommandBase = this.m_undoBuffer[this.m_undoBuffer.Count - 1];
				bool flag2 = editCommandBase.DoUndo(data);
				this.m_undoBuffer.RemoveAt(this.m_undoBuffer.Count - 1);
				this.m_redoBuffer.Add(editCommandBase);
				this.m_canCapture = true;
				this.Dirty = true;
				result = flag2;
			}
			return result;
		}

		public bool DoRedo(IModel data)
		{
			bool flag = this.m_redoBuffer.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.m_canCapture = false;
				EditCommandBase editCommandBase = this.m_redoBuffer[this.m_redoBuffer.Count - 1];
				bool flag2 = editCommandBase.DoRedo(data);
				this.m_redoBuffer.RemoveAt(this.m_redoBuffer.Count - 1);
				this.m_undoBuffer.Add(editCommandBase);
				this.m_canCapture = true;
				this.Dirty = true;
				result = flag2;
			}
			return result;
		}
	}
}
