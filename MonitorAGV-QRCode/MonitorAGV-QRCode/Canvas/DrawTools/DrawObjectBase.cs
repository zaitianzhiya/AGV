using Canvas.CanvasInterfaces;
using Canvas.Layers;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Canvas.DrawTools
{
	public abstract class DrawObjectBase
	{
		private enum eFlags
		{
			selected = 1,
			highlighted,
			useLayerWidth = 4,
			useLayerColor = 8
		}

		private float m_width;

		private Color m_color;

		private DrawingLayer m_layer;

		private int m_flag = 12;

		[XmlSerializable, Browsable(false)]
		public bool UseLayerWidth
		{
			get
			{
				return false;
			}
			set
			{
				this.SetFlag(DrawObjectBase.eFlags.useLayerWidth, value);
			}
		}

		[XmlSerializable, Browsable(false)]
		public bool UseLayerColor
		{
			get
			{
				return false;
			}
			set
			{
				this.SetFlag(DrawObjectBase.eFlags.useLayerColor, value);
			}
		}

		[XmlSerializable, Browsable(false)]
		public float Width
		{
			get
			{
				return this.m_width;
			}
			set
			{
				this.m_width = value;
			}
		}

		[XmlSerializable]
		public Color Color
		{
			get
			{
				return m_color;
			}
			set
			{
				m_color = value;
			}
		}

		[Browsable(false)]
		public DrawingLayer Layer
		{
			get
			{
				return this.m_layer;
			}
			set
			{
				this.m_layer = value;
			}
		}

		[Browsable(false)]
		public virtual bool Selected
		{
			get
			{
				return this.GetFlag(DrawObjectBase.eFlags.selected);
			}
			set
			{
				this.SetFlag(DrawObjectBase.eFlags.selected, value);
			}
		}

		[Browsable(false)]
		public virtual bool Highlighted
		{
			get
			{
				return this.GetFlag(DrawObjectBase.eFlags.highlighted);
			}
			set
			{
				this.SetFlag(DrawObjectBase.eFlags.highlighted, value);
			}
		}

		private bool GetFlag(DrawObjectBase.eFlags flag)
		{
			return (this.m_flag & (int)flag) > 0;
		}

		private void SetFlag(DrawObjectBase.eFlags flag, bool enable)
		{
			if (enable)
			{
				this.m_flag |= (int)flag;
			}
			else
			{
				this.m_flag &= (int)(~(int)flag);
			}
		}

		public abstract void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap);

		public virtual void Copy(DrawObjectBase acopy)
		{
			this.UseLayerColor = acopy.UseLayerColor;
			this.UseLayerWidth = acopy.UseLayerWidth;
			this.Width = acopy.Width;
			this.Color = acopy.Color;
		}
	}
}
