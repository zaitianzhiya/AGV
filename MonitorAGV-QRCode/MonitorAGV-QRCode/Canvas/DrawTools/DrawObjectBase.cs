using Canvas.CanvasCtrl;
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
				SetFlag(eFlags.useLayerWidth, value);
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
				SetFlag(eFlags.useLayerColor, value);
			}
		}

		[XmlSerializable, Browsable(false)]
		public float Width
		{
			get
			{
				return m_width;
			}
			set
			{
				m_width = value;
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
				return m_layer;
			}
			set
			{
				m_layer = value;
			}
		}

		[Browsable(false)]
		public virtual bool Selected
		{
			get
			{
				return GetFlag(eFlags.selected);
			}
			set
			{
				SetFlag(eFlags.selected, value);
			}
		}

		[Browsable(false)]
		public virtual bool Highlighted
		{
			get
			{
				return GetFlag(eFlags.highlighted);
			}
			set
			{
				SetFlag(eFlags.highlighted, value);
			}
		}

		private bool GetFlag(eFlags flag)
		{
			return (m_flag & (int)flag) > 0;
		}

		private void SetFlag(eFlags flag, bool enable)
		{
			if (enable)
			{
				m_flag |= (int)flag;
			}
			else
			{
				m_flag &= (int)(~(int)flag);
			}
		}

		public abstract void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap);

	    public virtual void InitializeFromModel(CanvasCtrller cc,UnitPoint point, DrawingLayer layer, ISnapPoint snap)
	    {
	        
	    }

		public virtual void Copy(DrawObjectBase acopy)
		{
			UseLayerColor = acopy.UseLayerColor;
			UseLayerWidth = acopy.UseLayerWidth;
			Width = acopy.Width;
			Color = acopy.Color;
		}
	}
}
