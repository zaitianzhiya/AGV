using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using CommonTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Canvas.CanvasCtrl
{
	public class CanvasCtrller : UserControl
	{
		public eCommandType m_commandType = eCommandType.select;

        public event EventHandler m_commandTypeChange;

		public bool IsChooseSpecial = false;

		public ICanvasOwner m_owner;

		public CursorCollection m_cursors = new CursorCollection();

		public IModel m_model;

		private MoveHelper m_moveHelper = null;

		private NodeMoveHelper m_nodeMoveHelper = null;

		private CanvasWrapper m_canvaswrapper;

		private bool m_runningSnaps = true;

		private Type[] m_runningSnapTypes = null;

		private PointF m_mousedownPoint;

		private IDrawObject m_newObject = null;

		private IEditTool m_editTool = null;

		private SelectionRectangle m_selection = null;

		private string m_drawObjectId = string.Empty;

		private string m_editToolId = string.Empty;

		private Bitmap m_staticImage = null;

		private bool m_staticDirty = true;

		private UnitPoint m_lastCenterPoint;

		private PointF m_panOffset = new PointF(0f, 0f);

		private PointF m_dragOffset = new PointF(0f, 0f);

		private float m_screenResolution = 100f;

		private ISnapPoint m_snappoint = null;

		private SmoothingMode m_smoothingMode = SmoothingMode.HighQuality;

		private Dictionary<Keys, Type> m_QuickSnap = new Dictionary<Keys, Type>();

		private IContainer components = null;

		public Type[] RunningSnaps
		{
			get
			{
				return this.m_runningSnapTypes;
			}
			set
			{
				this.m_runningSnapTypes = value;
			}
		}

		public bool RunningSnapsEnabled
		{
			get
			{
				return this.m_runningSnaps;
			}
			set
			{
				this.m_runningSnaps = value;
			}
		}

		public SmoothingMode SmoothingMode
		{
			get
			{
				return this.m_smoothingMode;
			}
			set
			{
				this.m_smoothingMode = value;
			}
		}

		public IModel Model
		{
			get
			{
				return this.m_model;
			}
			set
			{
				this.m_model = value;
			}
		}

		public IDrawObject NewObject
		{
			get
			{
				return this.m_newObject;
			}
		}

		public CanvasCtrller(ICanvasOwner owner, IModel datamodel)
		{
			this.m_canvaswrapper = new CanvasWrapper(this);
			this.m_owner = owner;
			this.m_model = datamodel;
			this.InitializeComponent();
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.m_commandType = eCommandType.select;
            //m_commandTypeChange(null, null);
			this.m_cursors.AddCursor(eCommandType.select, Cursors.Arrow);
			this.m_cursors.AddCursor(eCommandType.draw, Cursors.Cross);
			this.m_cursors.AddCursor(eCommandType.pan, "hmove.cur");
			this.m_cursors.AddCursor(eCommandType.move, Cursors.SizeAll);
			this.m_cursors.AddCursor(eCommandType.edit, Cursors.Cross);
			this.UpdateCursor();
			this.m_moveHelper = new MoveHelper(this);
			this.m_nodeMoveHelper = new NodeMoveHelper(this.m_canvaswrapper);
		}

		public UnitPoint GetMousePoint()
		{
			Point p = base.PointToClient(Control.MousePosition);
			return this.ToUnit(p);
		}

		public void SetCenter(UnitPoint unitPoint)
		{
			PointF screenPoint = this.ToScreen(unitPoint);
			this.m_lastCenterPoint = unitPoint;
			this.SetCenterScreen(screenPoint, false);
		}

		private void UpdateCursor()
		{
			this.Cursor = this.m_cursors.GetCursor(this.m_commandType);
		}

		public void SetCenter()
		{
			Point p = base.PointToClient(Control.MousePosition);
			this.SetCenterScreen(p, true);
		}

		public UnitPoint GetCenter()
		{
			return this.ToUnit(new PointF((float)(base.ClientRectangle.Width / 2), (float)(base.ClientRectangle.Height / 2)));
		}

		protected void SetCenterScreen(PointF screenPoint, bool setCursor)
		{
			float num = (float)(base.ClientRectangle.Width / 2);
			this.m_panOffset.X = this.m_panOffset.X + (num - screenPoint.X);
			float num2 = (float)(base.ClientRectangle.Height / 2);
			this.m_panOffset.Y = this.m_panOffset.Y + (num2 - screenPoint.Y);
			if (setCursor)
			{
				Cursor.Position = base.PointToScreen(new Point((int)num, (int)num2));
			}
			this.DoInvalidate(true);
		}

		public double ToUnit(float screenvalue)
		{
			return Math.Round((double)screenvalue / (double)(this.m_screenResolution * this.m_model.Zoom), 5);
		}

		public UnitPoint ToUnit(PointF screenpoint)
		{
			float num = this.m_panOffset.X + this.m_dragOffset.X;
			float num2 = this.m_panOffset.Y + this.m_dragOffset.Y;
			float num3 = (screenpoint.X - num) / (this.m_screenResolution * this.m_model.Zoom);
			float num4 = this.ScreenHeight() - (screenpoint.Y - num2) / (this.m_screenResolution * this.m_model.Zoom);
			return new UnitPoint((double)num3, (double)num4);
		}

		public PointF ToScreen(UnitPoint point)
		{
			PointF result;
			try
			{
				PointF pointF = this.Translate(point);
				pointF.Y = this.ScreenHeight() - pointF.Y;
				pointF.Y *= this.m_screenResolution * this.m_model.Zoom;
				pointF.X *= this.m_screenResolution * this.m_model.Zoom;
				pointF.X += this.m_panOffset.X + this.m_dragOffset.X;
				pointF.Y += this.m_panOffset.Y + this.m_dragOffset.Y;
				result = pointF;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private PointF Translate(UnitPoint point)
		{
			return point.Point;
		}

		private float ScreenHeight()
		{
			return (float)(this.ToUnit((float)base.ClientRectangle.Height) / (double)this.m_model.Zoom);
		}

		public Pen CreatePen(Color color, float unitWidth)
		{
			return new Pen(color, unitWidth);
		}

		public void DoInvalidate(bool dostatic)
		{
			if (dostatic)
			{
				this.m_staticDirty = true;
			}
			base.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				Tracing.StartTrack(Tracing.TracePaint);
				e.Graphics.SmoothingMode = this.m_smoothingMode;
				CanvasWrapper canvasWrapper = new CanvasWrapper(this, e.Graphics, base.ClientRectangle);
				Rectangle rectangle = e.ClipRectangle;
				bool flag = this.m_staticImage == null;
				if (flag)
				{
					rectangle = base.ClientRectangle;
					this.m_staticImage = new Bitmap(base.ClientRectangle.Width, base.ClientRectangle.Height);
					this.m_staticDirty = true;
				}
				RectangleF rectangleF = ScreenUtils.ToUnitNormalized(canvasWrapper, rectangle);
				bool flag2 = float.IsNaN(rectangleF.Width) || float.IsInfinity(rectangleF.Width);
				if (!flag2)
				{
					bool staticDirty = this.m_staticDirty;
					if (staticDirty)
					{
						this.m_staticDirty = false;
						CanvasWrapper canvasWrapper2 = new CanvasWrapper(this, Graphics.FromImage(this.m_staticImage), base.ClientRectangle);
						canvasWrapper2.Graphics.SmoothingMode = this.m_smoothingMode;
						try
						{
							this.m_model.BackgroundLayer.Draw(canvasWrapper2, rectangleF);
						}
						catch (Exception ex)
						{
							throw ex;
						}
						try
						{
							bool enabled = this.m_model.GridLayer.Enabled;
							if (enabled)
							{
								this.m_model.GridLayer.Draw(canvasWrapper2, rectangleF);
							}
						}
						catch (Exception ex2)
						{
							throw ex2;
						}
						PointF pointF = this.ToScreen(new UnitPoint(0.0, 0.0));
						try
						{
							canvasWrapper2.Graphics.DrawLine(Pens.Blue, pointF.X - 50f, pointF.Y, pointF.X + 50f, pointF.Y);
							canvasWrapper2.Graphics.DrawLine(Pens.Blue, pointF.X, pointF.Y - 50f, pointF.X, pointF.Y + 50f);
						}
						catch (Exception var_12_1DF)
						{
						}
						try
						{
							ICanvasLayer[] layers = this.m_model.Layers;
							int num;
							for (int i = layers.Length - 1; i >= 0; i = num - 1)
							{
								bool flag3 = layers[i] != this.m_model.ActiveLayer && layers[i].Visible;
								if (flag3)
								{
									layers[i].Draw(canvasWrapper2, rectangleF);
								}
								num = i;
							}
							bool flag4 = this.m_model.ActiveLayer != null;
							if (flag4)
							{
								this.m_model.ActiveLayer.Draw(canvasWrapper2, rectangleF);
							}
							canvasWrapper2.Dispose();
						}
						catch (Exception ex3)
						{
							throw ex3;
						}
					}
					try
					{
						e.Graphics.DrawImage(this.m_staticImage, rectangle, rectangle, GraphicsUnit.Pixel);
					}
					catch (Exception ex4)
					{
						throw ex4;
					}
					foreach (IDrawObject current in this.m_model.SelectedObjects)
					{
						current.Draw(canvasWrapper, rectangleF);
					}
					try
					{
						bool flag5 = this.m_newObject != null;
						if (flag5)
						{
							this.m_newObject.Draw(canvasWrapper, rectangleF);
						}
					}
					catch (Exception ex5)
					{
						throw ex5;
					}
					try
					{
						bool flag6 = this.m_snappoint != null;
						if (flag6)
						{
							this.m_snappoint.Draw(canvasWrapper);
						}
					}
					catch (Exception ex6)
					{
						throw ex6;
					}
					bool flag7 = this.m_selection != null;
					if (flag7)
					{
						this.m_selection.Reset();
						this.m_selection.SetMousePoint(e.Graphics, base.PointToClient(Control.MousePosition));
					}
					bool flag8 = !this.m_moveHelper.IsEmpty;
					if (flag8)
					{
						this.m_moveHelper.DrawObjects(canvasWrapper, rectangleF);
					}
					bool flag9 = !this.m_nodeMoveHelper.IsEmpty;
					if (flag9)
					{
						this.m_nodeMoveHelper.DrawObjects(canvasWrapper, rectangleF);
					}
					canvasWrapper.Dispose();
					Tracing.EndTrack(Tracing.TracePaint, "OnPaint complete");
				}
			}
			catch (Exception ex7)
			{
				throw ex7;
			}
		}

		private void RepaintStatic(Rectangle r)
		{
			bool flag = this.m_staticImage == null;
			if (!flag)
			{
				Graphics graphics = Graphics.FromHwnd(base.Handle);
				bool flag2 = r.X < 0;
				if (flag2)
				{
					r.X = 0;
				}
				bool flag3 = r.X > this.m_staticImage.Width;
				if (flag3)
				{
					r.X = 0;
				}
				bool flag4 = r.Y < 0;
				if (flag4)
				{
					r.Y = 0;
				}
				bool flag5 = r.Y > this.m_staticImage.Height;
				if (flag5)
				{
					r.Y = 0;
				}
				bool flag6 = r.Width > this.m_staticImage.Width || r.Width < 0;
				if (flag6)
				{
					r.Width = this.m_staticImage.Width;
				}
				bool flag7 = r.Height > this.m_staticImage.Height || r.Height < 0;
				if (flag7)
				{
					r.Height = this.m_staticImage.Height;
				}
				graphics.DrawImage(this.m_staticImage, r, r, GraphicsUnit.Pixel);
				graphics.Dispose();
			}
		}

		private void RepaintSnappoint(ISnapPoint snappoint)
		{
			bool flag = snappoint == null;
			if (!flag)
			{
				CanvasWrapper canvasWrapper = new CanvasWrapper(this, Graphics.FromHwnd(base.Handle), base.ClientRectangle);
				snappoint.Draw(canvasWrapper);
				canvasWrapper.Graphics.Dispose();
				canvasWrapper.Dispose();
			}
		}

		private void RepaintObject(IDrawObject obj)
		{
			bool flag = obj == null;
			if (!flag)
			{
				CanvasWrapper canvasWrapper = new CanvasWrapper(this, Graphics.FromHwnd(base.Handle), base.ClientRectangle);
				RectangleF unitrect = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(canvasWrapper, obj.GetBoundingRect(canvasWrapper)));
				obj.Draw(canvasWrapper, unitrect);
				canvasWrapper.Graphics.Dispose();
				canvasWrapper.Dispose();
			}
		}

		public void DoInvalidate(bool dostatic, RectangleF rect)
		{
			if (dostatic)
			{
				this.m_staticDirty = true;
			}
			base.Invalidate(ScreenUtils.ConvertRect(rect));
		}

		protected void HandleSelection(List<IDrawObject> selected)
		{
			bool flag = Control.ModifierKeys == Keys.Shift;
			bool flag2 = Control.ModifierKeys == Keys.Control;
			bool flag3 = false;
			bool flag4 = false;
			int num = 0;
			bool flag5 = selected != null;
			if (flag5)
			{
				num = selected.Count;
			}
			bool flag6 = this.m_model.SelectedObjects.Count<IDrawObject>() > 0;
			if (flag6)
			{
				flag4 = true;
			}
			bool flag7 = flag2 && num > 0;
			if (flag7)
			{
				flag3 = true;
				foreach (IDrawObject current in selected)
				{
					bool isChooseSpecial = this.IsChooseSpecial;
					if (isChooseSpecial)
					{
						bool flag8 = current.Id != "ButtonTool";
						if (flag8)
						{
							this.m_model.RemoveSelectedObject(current);
						}
						else
						{
							this.m_model.AddSelectedObject(current);
						}
					}
					else
					{
						bool flag9 = this.m_model.IsSelected(current);
						if (flag9)
						{
							this.m_model.RemoveSelectedObject(current);
						}
						else
						{
							this.m_model.AddSelectedObject(current);
						}
					}
				}
			}
			bool flag10 = flag && num > 0;
			if (flag10)
			{
				flag3 = true;
				foreach (IDrawObject current2 in selected)
				{
					bool isChooseSpecial2 = this.IsChooseSpecial;
					if (isChooseSpecial2)
					{
						bool flag11 = current2.Id != "ButtonTool";
						if (flag11)
						{
							this.m_model.RemoveSelectedObject(current2);
						}
						else
						{
							this.m_model.AddSelectedObject(current2);
						}
					}
					else
					{
						this.m_model.AddSelectedObject(current2);
					}
				}
			}
			bool flag12 = !flag && !flag2 && num > 0;
			if (flag12)
			{
				flag3 = true;
				this.m_model.ClearSelectedObjects();
				foreach (IDrawObject current3 in selected)
				{
					bool isChooseSpecial3 = this.IsChooseSpecial;
					if (isChooseSpecial3)
					{
						bool flag13 = current3.Id != "ButtonTool";
						if (flag13)
						{
							this.m_model.RemoveSelectedObject(current3);
						}
						else
						{
							this.m_model.AddSelectedObject(current3);
						}
					}
					else
					{
						this.m_model.AddSelectedObject(current3);
					}
				}
			}
			bool flag14 = (!flag && !flag2 && num == 0) & flag4;
			if (flag14)
			{
				flag3 = true;
				this.m_model.ClearSelectedObjects();
			}
			bool flag15 = flag3;
			if (flag15)
			{
				this.DoInvalidate(false);
			}
		}

		private void FinishNodeEdit()
		{
			this.m_commandType = eCommandType.select;
            m_commandTypeChange(null, null);
			this.m_snappoint = null;
		}

		protected virtual void HandleMouseDownWhenDrawing(UnitPoint mouseunitpoint, ISnapPoint snappoint)
		{
			try
			{
				if (this.m_commandType == eCommandType.draw)
				{
					if (this.m_newObject == null)
					{
						this.m_newObject = this.m_model.CreateObject(this.m_drawObjectId, mouseunitpoint, snappoint);
						this.DoInvalidate(false, this.m_newObject.GetBoundingRect(this.m_canvaswrapper));
						if (this.m_newObject.Id == "LandMark" || this.m_newObject.Id == "ImgeTool" || this.m_newObject.Id == "TextTool" || this.m_newObject.Id == "StorageTool" || this.m_newObject.Id == "AGVTool" || this.m_newObject.Id == "ButtonTool")
						{
							this.m_newObject = this.m_model.CreateObject(this.m_drawObjectId, mouseunitpoint, snappoint);
							this.m_newObject.OnMouseDown(this.m_canvaswrapper, mouseunitpoint, snappoint);
							this.m_model.AddObject(this.m_model.ActiveLayer, this.m_newObject);
							this.m_newObject = null;
							this.DoInvalidate(true);
						}
						if (this.m_drawObjectId == "PointBezier")
						{
							if ((this.m_newObject as BezierTool).CurrentPoint == BezierTool.eCurrentPoint.p1 || (this.m_newObject as BezierTool).CurrentPoint == BezierTool.eCurrentPoint.p2)
							{
								if (this.m_snappoint == null || (this.m_snappoint != null && ((this.m_snappoint.Owner != null && this.m_snappoint.Owner.Id != "LandMark") || this.m_snappoint.Owner == null)))
								{
									this.m_newObject = null;
									this.DoInvalidate(true);
								}
							}
						}
					}
					else
					{
						if (this.m_drawObjectId == "PointBezier")
						{
							if ( (this.m_newObject as BezierTool).CurrentPoint == BezierTool.eCurrentPoint.p1 || (this.m_newObject as BezierTool).CurrentPoint == BezierTool.eCurrentPoint.p2)
							{
								if (this.m_snappoint == null || (this.m_snappoint != null && ((this.m_snappoint.Owner != null && this.m_snappoint.Owner.Id != "LandMark") || this.m_snappoint.Owner == null)))
								{
									this.m_newObject = null;
									this.DoInvalidate(true);
									return;
								}
							}
						}
						switch (this.m_newObject.OnMouseDown(this.m_canvaswrapper, mouseunitpoint, snappoint))
						{
						case eDrawObjectMouseDownEnum.Done:
							this.m_model.AddObject(this.m_model.ActiveLayer, this.m_newObject);
							this.m_newObject = null;
							this.DoInvalidate(true);
							break;
						case eDrawObjectMouseDownEnum.DoneRepeat:
							this.m_model.AddObject(this.m_model.ActiveLayer, this.m_newObject);
							this.m_newObject = this.m_model.CreateObject(this.m_newObject.Id, this.m_newObject.RepeatStartingPoint, null);
							this.DoInvalidate(true);
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	    private eCommandType? beforeECommandType=null;
		protected override void OnMouseDown(MouseEventArgs e)
		{
			try
			{
			    if (e.Button == MouseButtons.Right)
			    {
			        beforeECommandType = m_commandType;
			        m_commandType = eCommandType.pan;
                    m_commandTypeChange(null, null);
			        UpdateCursor();
			    }
			    else
			    {
			        beforeECommandType = null;
			    }
				if (m_commandType == eCommandType.select && e.Button == MouseButtons.Right)
				{
					m_commandType = eCommandType.pan;
                    m_commandTypeChange(null, null);
					UpdateCursor();
				}
				m_mousedownPoint = new PointF((float)e.X, (float)e.Y);
				m_dragOffset = new PointF(0f, 0f);
				UnitPoint unitPoint = ToUnit(m_mousedownPoint);
				if (m_snappoint != null)
				{
					unitPoint = m_snappoint.SnapPoint;
				}
				if (m_commandType == eCommandType.editNode)
				{
					bool flag4 = false;
					if (this.m_nodeMoveHelper.HandleMouseDown(unitPoint, ref flag4))
					{
						if (this.m_drawObjectId == "PointLine")
						{
							if (this.m_snappoint == null || (this.m_snappoint != null && ((this.m_snappoint.Owner != null && this.m_snappoint.Owner.Id != "LandMark") || this.m_snappoint.Owner == null)))
							{
								this.m_newObject = null;
								this.DoInvalidate(true);
								return;
							}
						}
						this.FinishNodeEdit();
						base.OnMouseDown(e);
						return;
					}
				}
				if (this.m_commandType == eCommandType.select && e.Button == MouseButtons.Left)
				{
                    if (Control.ModifierKeys == Keys.Alt)
                    {
                        this.m_moveHelper.HandleMouseDownForMove(unitPoint, this.m_snappoint);
                    }
                    else
				    {
				        bool flag9 = false;
				        if (this.m_nodeMoveHelper.HandleMouseDown(unitPoint, ref flag9))
				        {
				            this.m_commandType = eCommandType.editNode;
				            this.m_snappoint = null;
				            base.OnMouseDown(e);
				            return;
				        }
				        this.m_selection = new SelectionRectangle(this.m_mousedownPoint);
				    }
				}
				if (this.m_commandType == eCommandType.move)
				{
					this.m_moveHelper.HandleMouseDownForMove(unitPoint, this.m_snappoint);
				}
				if (this.m_commandType == eCommandType.draw)
				{
					if (e.Button == MouseButtons.Right)
					{
						this.m_newObject = null;
						this.DoInvalidate(true);
						return;
					}
					if (this.m_drawObjectId == "PointLine")
					{
						if (this.m_snappoint == null || (this.m_snappoint != null && ((this.m_snappoint.Owner != null && this.m_snappoint.Owner.Id != "LandMark") || this.m_snappoint.Owner == null)))
						{
							this.m_newObject = null;
							this.DoInvalidate(true);
							return;
						}
					}
					this.HandleMouseDownWhenDrawing(unitPoint, null);
					this.DoInvalidate(true);
				}
				if (this.m_commandType == eCommandType.edit)
				{
					if ( this.m_editTool == null)
					{
						this.m_editTool = this.m_model.GetEditTool(this.m_editToolId);
					}
					if (this.m_editTool != null)
					{
						if (this.m_editTool.SupportSelection)
						{
							this.m_selection = new SelectionRectangle(this.m_mousedownPoint);
						}
						eDrawObjectMouseDownEnum eDrawObjectMouseDownEnum = this.m_editTool.OnMouseDown(this.m_canvaswrapper, unitPoint, this.m_snappoint);
						if (eDrawObjectMouseDownEnum == eDrawObjectMouseDownEnum.Done)
						{
							this.m_editTool.Finished();
							this.m_editTool = this.m_model.GetEditTool(this.m_editToolId);
							bool supportSelection2 = this.m_editTool.SupportSelection;
							if (supportSelection2)
							{
								this.m_selection = new SelectionRectangle(this.m_mousedownPoint);
							}
						}
					}
					this.DoInvalidate(true);
					this.UpdateCursor();
				}
				base.OnMouseDown(e);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			try
			{
				if (this.m_commandType == eCommandType.pan && e.Button == MouseButtons.Right)
				{
                    //this.m_commandType = eCommandType.select;
				    if (beforeECommandType != null)
				    {
				        this.m_commandType = (eCommandType) beforeECommandType;
				    }
				    else
				    {
                        this.m_commandType = eCommandType.select;
                        m_commandTypeChange(null, null);
				    }
				    this.UpdateCursor();
				}
				if (this.m_commandType == eCommandType.pan || e.Button == MouseButtons.Right)
				{
					this.m_panOffset.X = this.m_panOffset.X + this.m_dragOffset.X;
					this.m_panOffset.Y = this.m_panOffset.Y + this.m_dragOffset.Y;
					this.m_dragOffset = new PointF(0f, 0f);
				}
				List<IDrawObject> list = null;
				Rectangle left = Rectangle.Empty;
                if (this.m_selection != null)
				{
					left = this.m_selection.ScreenRect();
					RectangleF rectangleF = this.m_selection.Selection(this.m_canvaswrapper);
					if (rectangleF != RectangleF.Empty)
					{
						list = this.m_model.GetHitObjects(this.m_canvaswrapper, rectangleF, this.m_selection.AnyPoint());
						this.DoInvalidate(true);
					}
					else
					{
						UnitPoint point = this.ToUnit(new PointF((float)e.X, (float)e.Y));
						list = this.m_model.GetHitObjects(this.m_canvaswrapper, point);
					}
					this.m_selection = null;
				}
				if (this.m_commandType == eCommandType.select && e.Button == MouseButtons.Left)
				{
					bool flag6 = list != null;
					if (flag6)
					{
						this.HandleSelection(list);
					}
				}
				if ( this.m_commandType == eCommandType.edit && this.m_editTool != null)
				{
					UnitPoint unitPoint = this.ToUnit(this.m_mousedownPoint);
					if (this.m_snappoint != null)
					{
						unitPoint = this.m_snappoint.SnapPoint;
					}
					if (left != Rectangle.Empty)
					{
						this.m_editTool.SetHitObjects(unitPoint, list);
					}
					this.m_editTool.OnMouseUp(this.m_canvaswrapper, unitPoint, this.m_snappoint);
				}
				if (this.m_commandType == eCommandType.draw && this.m_newObject != null)
				{
					UnitPoint point2 = this.ToUnit(this.m_mousedownPoint);
					if (this.m_snappoint != null)
					{
						point2 = this.m_snappoint.SnapPoint;
					}
					this.m_newObject.OnMouseUp(this.m_canvaswrapper, point2, this.m_snappoint);
				}
				base.OnMouseUp(e);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			try
			{
				base.OnMouseMove(e);
				if (this.m_selection != null)
				{
                    Graphics graphics = Graphics.FromHwnd(base.Handle);
                    this.m_selection.SetMousePoint(graphics, new PointF((float)e.X, (float)e.Y));
                    graphics.Dispose();
				}
				else
				{
					if ((this.m_commandType == eCommandType.pan && e.Button == MouseButtons.Left) || e.Button == MouseButtons.Right)
					{
						this.m_dragOffset.X = -(this.m_mousedownPoint.X - (float)e.X);
						this.m_dragOffset.Y = -(this.m_mousedownPoint.Y - (float)e.Y);
						this.m_lastCenterPoint = this.CenterPointUnit();
						this.DoInvalidate(true);
					}
					UnitPoint positionInfo = this.ToUnit(new PointF((float)e.X, (float)e.Y));
					UnitPoint unitPoint;
					if (this.m_commandType == eCommandType.draw || this.m_commandType == eCommandType.move || !this.m_nodeMoveHelper.IsEmpty)
					{
						Rectangle rectangle = Rectangle.Empty;
						ISnapPoint snapPoint = null;
						unitPoint = this.GetMousePoint();
						if (this.RunningSnapsEnabled)
						{
							snapPoint = this.m_model.SnapPoint(this.m_canvaswrapper, unitPoint, this.m_runningSnapTypes, null);
						}
						if (snapPoint == null)
						{
							snapPoint = this.m_model.GridLayer.SnapPoint(this.m_canvaswrapper, unitPoint, null);
						}
						if (this.m_snappoint != null && (snapPoint == null || snapPoint.SnapPoint != this.m_snappoint.SnapPoint || this.m_snappoint.GetType() != snapPoint.GetType()))
						{
							rectangle = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(this.m_canvaswrapper, this.m_snappoint.BoundingRect));
							rectangle.Inflate(2, 2);
							this.RepaintStatic(rectangle);
							this.m_snappoint = snapPoint;
						}
						if (this.m_commandType == eCommandType.move)
						{
							base.Invalidate(rectangle);
						}
						if (this.m_snappoint == null)
						{
							this.m_snappoint = snapPoint;
						}
					}
					this.m_owner.SetPositionInfo(positionInfo);
					this.m_owner.SetSnapInfo(this.m_snappoint);
					if (this.m_snappoint != null)
					{
						unitPoint = this.m_snappoint.SnapPoint;
					}
					else
					{
						unitPoint = this.GetMousePoint();
					}
					if (this.m_newObject != null)
					{
						Rectangle r = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(this.m_canvaswrapper, this.m_newObject.GetBoundingRect(this.m_canvaswrapper)));
						r.Inflate(2, 2);
						this.RepaintStatic(r);
						this.m_newObject.OnMouseMove(this.m_canvaswrapper, unitPoint);
						this.RepaintObject(this.m_newObject);
					}
					if (this.m_snappoint != null)
					{
						this.RepaintSnappoint(this.m_snappoint);
					}
					if (this.m_moveHelper.HandleMouseMoveForMove(unitPoint))
					{
						this.Refresh();
					}
					RectangleF rectangleF = this.m_nodeMoveHelper.HandleMouseMoveForNode(unitPoint);
					if (rectangleF != RectangleF.Empty)
					{
						Rectangle r2 = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(this.m_canvaswrapper, rectangleF));
						this.RepaintStatic(r2);
						CanvasWrapper canvasWrapper = new CanvasWrapper(this, Graphics.FromHwnd(base.Handle), base.ClientRectangle);
						canvasWrapper.Graphics.Clip = new Region(base.ClientRectangle);
						this.m_nodeMoveHelper.DrawObjects(canvasWrapper, rectangleF);
						if (this.m_snappoint != null)
						{
							this.RepaintSnappoint(this.m_snappoint);
						}
						canvasWrapper.Graphics.Dispose();
						canvasWrapper.Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			try
			{
				UnitPoint mousePoint = this.GetMousePoint();
				float num = 120f;
				float num2 = 1.1f * ((float)Math.Abs(e.Delta) / num);
				bool flag = e.Delta < 0;
				float num3;
				if (flag)
				{
					num3 = this.m_model.Zoom / num2;
				}
				else
				{
					num3 = this.m_model.Zoom * num2;
				}
				bool flag2 = float.IsNaN(num3) || float.IsInfinity(num3) || Math.Round((double)num3, 6) <= 1E-06 || num3 > 20000f;
				if (!flag2)
				{
					this.m_model.Zoom = num3;
					this.SetCenterScreen(this.ToScreen(mousePoint), true);
					this.DoInvalidate(true);
					base.OnMouseWheel(e);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			try
			{
				base.OnResize(e);
				this.SetCenterScreen(this.ToScreen(this.m_lastCenterPoint), false);
				this.m_lastCenterPoint = this.CenterPointUnit();
				this.m_staticImage = null;
				this.DoInvalidate(true);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawLine(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2)
		{
			try
			{
				PointF pt = this.ToScreen(p1);
				PointF pt2 = this.ToScreen(p2);
				canvas.Graphics.DrawLine(pen, pt, pt2);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawLandMark(ICanvas canvas, Brush pen, string code, UnitPoint Point)
		{
			try
			{
				bool isEmpty = Point.IsEmpty;
				if (!isEmpty)
				{
					bool flag = canvas.Graphics == null;
					if (!flag)
					{
						PointF pointF = this.ToScreen(Point);
						float num = this.ToScreen(0.2);
						canvas.Graphics.FillEllipse(pen, pointF.X, pointF.Y, num, num);
						pen = Brushes.White;
						float num2 = this.ToScreen(0.10000000149011612);
						float num3 = num2 * (float)(code.Length + 1);
						float num4 = this.ToScreen(0.15000000596046448);
						float num5 = num3 / 2f - this.ToScreen(0.1);
						StringFormat stringFormat = new StringFormat();
						stringFormat.Alignment = StringAlignment.Center;
						Rectangle r = new Rectangle((int)(pointF.X - num5), (int)(pointF.Y - num4), (int)num3, (int)num4);
						Font font = new Font("宋体", num2);
						canvas.Graphics.DrawString(code, font, pen, r, stringFormat);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawImge(ICanvas canvas, Pen pen, UnitPoint Location, float Widht, float Hight, Image img, string values)
		{
			try
			{
				bool isEmpty = Location.IsEmpty;
				if (!isEmpty)
				{
					bool flag = canvas.Graphics != null;
					if (flag)
					{
						PointF location = this.ToScreen(Location);
						float num = this.ToScreen((double)(Widht / 96f));
						float num2 = this.ToScreen((double)(Hight / 96f));
						canvas.Graphics.DrawRectangle(pen, location.X, location.Y, num, num2);
						canvas.Graphics.DrawImage(img, new RectangleF(location, new Size((int)num, (int)num2)));
						bool flag2 = !string.IsNullOrEmpty(values);
						if (flag2)
						{
							StringFormat stringFormat = new StringFormat();
							stringFormat.Alignment = StringAlignment.Center;
							float num3 = 0f;
							bool flag3 = values.Length == 1;
							float emSize;
							float num4;
							if (flag3)
							{
								emSize = this.ToScreen(0.3125);
								num4 = -this.ToScreen(0.26041666666666669);
							}
							else
							{
								bool flag4 = values.Length == 2;
								if (flag4)
								{
									emSize = this.ToScreen(0.3125);
									num4 = -this.ToScreen(0.28125);
								}
								else
								{
									emSize = this.ToScreen(0.20833333333333334);
									num4 = -this.ToScreen(0.28125);
									num3 = -this.ToScreen(0.10416666666666667);
								}
							}
							Font font = new Font("Arial Black", emSize, FontStyle.Regular, GraphicsUnit.Point, 0);
							SolidBrush brush = new SolidBrush(Color.White);
							canvas.Graphics.DrawString(values, font, brush, location.X - num4, location.Y - num3, stringFormat);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawTxt(ICanvas canvas, string code, UnitPoint Point, int FontSize, Color fontColor)
		{
			try
			{
				bool isEmpty = Point.IsEmpty;
				if (!isEmpty)
				{
					bool flag = canvas.Graphics != null;
					if (flag)
					{
						PointF pointF = this.ToScreen(Point);
						StringFormat stringFormat = new StringFormat();
						stringFormat.Alignment = StringAlignment.Near;
						float emSize = this.ToScreen((double)((float)FontSize / 96f));
						Font font = new Font("Arial Black", emSize, FontStyle.Regular, GraphicsUnit.Point, 0);
						SolidBrush brush = new SolidBrush(fontColor);
						canvas.Graphics.DrawString(code, font, brush, pointF.X, pointF.Y, stringFormat);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawBizer(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2, UnitPoint p3, UnitPoint p4)
		{
			try
			{
				PointF pt = this.ToScreen(p1);
				PointF pt2 = this.ToScreen(p2);
				PointF pt3 = this.ToScreen(p3);
				PointF pt4 = this.ToScreen(p4);
				bool flag = canvas.Graphics != null;
				if (flag)
				{
					canvas.Graphics.DrawBezier(pen, pt, pt2, pt3, pt4);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawStorage(ICanvas canvas, Brush Pen, string code, UnitPoint Point)
		{
			try
			{
				if (!Point.IsEmpty)
				{
					if (canvas.Graphics != null)
					{
						PointF pointF = this.ToScreen(Point);
						float num = this.ToScreen(0.5);
						Rectangle rect = new Rectangle((int)pointF.X, (int)pointF.Y, (int)num, (int)num);
						canvas.Graphics.FillRectangle(Pen, rect);
                        //float num2 = this.ToScreen(0.10000000149011612);
						float num2 = this.ToScreen(0.20000000149011612);
						float num3 = num2 * (float)code.Length + num2;
                        //float num4 = this.ToScreen(0.15000000596046448);
                        float num4 = this.ToScreen(0.30000000596046448);
						float num5 = num / 2f - num3 / 2f;
						StringFormat stringFormat = new StringFormat();
						stringFormat.Alignment = StringAlignment.Center;
						Font font = new Font("宋体", num2);
						Brush gray = Brushes.Gray;
                        //Rectangle r = new Rectangle((int)(pointF.X + num5), (int)(pointF.Y + this.ToScreen(0.15000000596046448)), (int)num3, (int)num4);
                        Rectangle r = new Rectangle((int)(pointF.X + num5), (int)(pointF.Y + this.ToScreen(0.10000000596046448)), (int)num3, (int)num4);
                        //canvas.Graphics.FillRectangle(Brushes.Red,r);
						canvas.Graphics.DrawString(code, font, gray, r, stringFormat);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawBtnBox(ICanvas canvas, float Radius, UnitPoint Point, bool Selected)
		{
			try
			{
                if (!Point.IsEmpty)
				{
                    if (canvas.Graphics != null)
					{
						PointF pointF = this.ToScreen(Point);
						float num = this.ToScreen((double)Radius);
						Color color = Color.FromArgb(47, 79, 79);
						Brush brush = new SolidBrush(color);
						canvas.Graphics.FillEllipse(brush, pointF.X, pointF.Y, num, num);
						color = Color.FromArgb(105, 105, 105);
						brush = new SolidBrush(color);
						if (Selected)
						{
							brush = Brushes.Magenta;
						}
						num -= this.ToScreen(0.20000000298023224);
						bool flag2 = num > 0f;
						if (flag2)
						{
							canvas.Graphics.FillEllipse(brush, pointF.X + this.ToScreen(0.10000000149011612), pointF.Y + this.ToScreen(0.10000000149011612), num, num);
						}
						brush = Brushes.DarkGray;
						float emSize = this.ToScreen((double)(((Radius >= 1f) ? (0.2f * Radius) : 0.14f) - 0.05f));
						StringFormat stringFormat = new StringFormat();
						stringFormat.Alignment = StringAlignment.Center;
						Rectangle r = new Rectangle((int)(this.ToScreen(Point).X + this.ToScreen((double)(Radius / 3f))), (int)(this.ToScreen(Point).Y + this.ToScreen((double)(Radius / 3f))), (int)this.ToScreen((double)(Radius / 3f)), (int)this.ToScreen((double)(Radius / 3f * 2f)));
						Font font = new Font("宋体", emSize);
						canvas.Graphics.DrawString("呼叫", font, brush, r, stringFormat);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void DrawAGV(ICanvas canvas, Pen pen, UnitPoint p, string Code)
		{
			try
			{
				PointF pointF = this.ToScreen(p);
				SolidBrush brush = new SolidBrush(pen.Color);
				RectangleF rect = new RectangleF(pointF.X - this.ToScreen(0.15), pointF.Y - this.ToScreen(0.15), this.ToScreen(0.5), this.ToScreen(0.5));
				bool flag = !rect.Location.IsEmpty && !float.IsInfinity(rect.X) && !float.IsInfinity(rect.Y);
				if (flag)
				{
					using (GraphicsPath graphicsPath = this.CreateRoundedRectanglePath(rect, (int)this.ToScreen(0.06)))
					{
						bool flag2 = graphicsPath != null;
						if (flag2)
						{
							canvas.Graphics.FillPath(brush, graphicsPath);
						}
					}
					float emSize = this.ToScreen(0.30000001192092896);
					StringFormat stringFormat = new StringFormat();
					stringFormat.Alignment = StringAlignment.Center;
					Font font = new Font("宋体", emSize);
					Brush yellow = Brushes.Yellow;
					float num = this.ToScreen(0.05) + (float)(Code.Length - 1) * this.ToScreen(0.1);
					float num2 = this.ToScreen(0.1);
					canvas.Graphics.DrawString(Code, font, yellow, new PointF((float)((int)(pointF.X - num)), (float)((int)(pointF.Y - num2))));
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private GraphicsPath CreateRoundedRectanglePath(RectangleF rect, int cornerRadius)
		{
			GraphicsPath result;
			try
			{
				GraphicsPath graphicsPath = new GraphicsPath();
				graphicsPath.AddArc(rect.X, rect.Y, (float)(cornerRadius * 2), (float)(cornerRadius * 2), 180f, 90f);
				graphicsPath.AddLine(rect.X + (float)cornerRadius, rect.Y, rect.Right - (float)(cornerRadius * 2), rect.Y);
				graphicsPath.AddArc(rect.X + rect.Width - (float)(cornerRadius * 2), rect.Y, (float)(cornerRadius * 2), (float)(cornerRadius * 2), 270f, 90f);
				graphicsPath.AddLine(rect.Right, rect.Y + (float)(cornerRadius * 2), rect.Right, rect.Y + rect.Height - (float)(cornerRadius * 2));
				graphicsPath.AddArc(rect.X + rect.Width - (float)(cornerRadius * 2), rect.Y + rect.Height - (float)(cornerRadius * 2), (float)(cornerRadius * 2), (float)(cornerRadius * 2), 0f, 90f);
				graphicsPath.AddLine(rect.Right - (float)(cornerRadius * 2), rect.Bottom, rect.X + (float)(cornerRadius * 2), rect.Bottom);
				graphicsPath.AddArc(rect.X, rect.Bottom - (float)(cornerRadius * 2), (float)(cornerRadius * 2), (float)(cornerRadius * 2), 90f, 90f);
				graphicsPath.AddLine(rect.X, rect.Bottom - (float)(cornerRadius * 2), rect.X, rect.Y + (float)(cornerRadius * 2));
				graphicsPath.CloseFigure();
				result = graphicsPath;
				return result;
			}
			catch
			{
			}
			result = null;
			return result;
		}

		public UnitPoint CenterPointUnit()
		{
			UnitPoint result;
			try
			{
				UnitPoint unitPoint = this.ScreenTopLeftToUnitPoint();
				UnitPoint unitPoint2 = this.ScreenBottomRightToUnitPoint();
				result = new UnitPoint
				{
					X = (unitPoint.X + unitPoint2.X) / 2.0,
					Y = (unitPoint.Y + unitPoint2.Y) / 2.0
				};
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public UnitPoint ScreenTopLeftToUnitPoint()
		{
			return this.ToUnit(new PointF(0f, 0f));
		}

		public UnitPoint ScreenBottomRightToUnitPoint()
		{
			return this.ToUnit(new PointF((float)base.ClientRectangle.Width, (float)base.ClientRectangle.Height));
		}

		public float ToScreen(double value)
		{
			return (float)Math.Round(value * (double)this.m_screenResolution * (double)this.m_model.Zoom, 5);
		}

		public void AddQuickSnapType(Keys key, Type snaptype)
		{
			this.m_QuickSnap.Add(key, snaptype);
		}

		public void CommandSelectDrawTool(string drawobjectid)
		{
			try
			{
				this.CommandEscape(false);
				this.m_model.ClearSelectedObjects();
				this.m_commandType = eCommandType.draw;
				this.m_drawObjectId = drawobjectid;
				this.UpdateCursor();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void CommandEscape(bool eve)
		{
			try
			{
				bool dostatic = this.m_newObject != null || this.m_snappoint != null;
				this.m_newObject = null;
				this.m_snappoint = null;
				if (this.m_editTool != null)
				{
					this.m_editTool.Finished();
				}
				this.m_editTool = null;
				this.m_commandType = eCommandType.select;
			    if (eve)
			    {
			        m_commandTypeChange(null, null);
			    }
			    this.m_moveHelper.HandleCancelMove();
				this.m_nodeMoveHelper.HandleCancelMove();
				this.DoInvalidate(dostatic);
				this.UpdateCursor();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void CommandPan()
		{
			try
			{
				this.m_commandType = eCommandType.pan;
                m_commandTypeChange(null, null);
				this.UpdateCursor();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void CommandMove(bool handleImmediately)
		{
			try
			{
				bool flag = this.m_model.SelectedCount > 0;
				if (flag)
				{
					bool flag2 = handleImmediately && this.m_commandType == eCommandType.move;
					if (flag2)
					{
						this.m_moveHelper.HandleMouseDownForMove(this.GetMousePoint(), this.m_snappoint);
					}
					this.m_commandType = eCommandType.move;
                    m_commandTypeChange(null, null);
					this.UpdateCursor();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void CommandDeleteSelected()
		{
			try
			{
				this.m_model.DeleteObjects(this.m_model.SelectedObjects);
				this.m_model.ClearSelectedObjects();
				this.DoInvalidate(true);
				this.UpdateCursor();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void CommandEdit(string editid)
		{
			try
			{
				this.CommandEscape(false);
				this.m_model.ClearSelectedObjects();
				this.m_commandType = eCommandType.edit;
				this.m_editToolId = editid;
				this.m_editTool = this.m_model.GetEditTool(this.m_editToolId);
				this.UpdateCursor();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void HandleQuickSnap(KeyEventArgs e)
		{
			try
			{
				bool flag = this.m_commandType == eCommandType.select || this.m_commandType == eCommandType.pan;
				if (!flag)
				{
					ISnapPoint snapPoint = null;
					UnitPoint mousePoint = this.GetMousePoint();
					bool flag2 = this.m_QuickSnap.ContainsKey(e.KeyCode);
					if (flag2)
					{
						snapPoint = this.m_model.SnapPoint(this.m_canvaswrapper, mousePoint, null, this.m_QuickSnap[e.KeyCode]);
					}
					bool flag3 = snapPoint != null;
					if (flag3)
					{
						bool flag4 = this.m_commandType == eCommandType.draw;
						if (flag4)
						{
							this.HandleMouseDownWhenDrawing(snapPoint.SnapPoint, snapPoint);
							bool flag5 = this.m_newObject != null;
							if (flag5)
							{
								this.m_newObject.OnMouseMove(this.m_canvaswrapper, this.GetMousePoint());
							}
							this.DoInvalidate(true);
							e.Handled = true;
						}
						bool flag6 = this.m_commandType == eCommandType.move;
						if (flag6)
						{
							this.m_moveHelper.HandleMouseDownForMove(snapPoint.SnapPoint, snapPoint);
							e.Handled = true;
						}
						bool flag7 = !this.m_nodeMoveHelper.IsEmpty;
						if (flag7)
						{
							bool flag8 = false;
							this.m_nodeMoveHelper.HandleMouseDown(snapPoint.SnapPoint, ref flag8);
							this.FinishNodeEdit();
							e.Handled = true;
						}
						bool flag9 = this.m_commandType == eCommandType.edit;
						if (flag9)
						{
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			bool isChooseSpecial = this.IsChooseSpecial;
			if (!isChooseSpecial)
			{
				this.HandleQuickSnap(e);
				bool flag = !this.m_nodeMoveHelper.IsEmpty;
				if (flag)
				{
					this.m_nodeMoveHelper.OnKeyDown(this.m_canvaswrapper, e);
					bool handled = e.Handled;
					if (handled)
					{
						return;
					}
				}
				base.OnKeyDown(e);
				bool handled2 = e.Handled;
				if (handled2)
				{
					this.UpdateCursor();
				}
				else
				{
					bool flag2 = this.m_editTool != null;
					if (flag2)
					{
						this.m_editTool.OnKeyDown(this.m_canvaswrapper, e);
						bool handled3 = e.Handled;
						if (handled3)
						{
							return;
						}
					}
					bool flag3 = this.m_newObject != null;
					if (flag3)
					{
						this.m_newObject.OnKeyDown(this.m_canvaswrapper, e);
						bool handled4 = e.Handled;
						if (handled4)
						{
							return;
						}
					}
					foreach (IDrawObject current in this.m_model.SelectedObjects)
					{
						current.OnKeyDown(this.m_canvaswrapper, e);
						bool handled5 = e.Handled;
						if (handled5)
						{
							return;
						}
					}
					bool flag4 = (Control.ModifierKeys & Keys.Control) == Keys.Control;
					if (flag4)
					{
						bool flag5 = e.KeyCode == Keys.G;
						if (flag5)
						{
							this.m_model.GridLayer.Enabled = !this.m_model.GridLayer.Enabled;
							this.DoInvalidate(true);
						}
						bool flag6 = e.KeyCode == Keys.C;
						if (flag6)
						{
							this.RunningSnapsEnabled = !this.RunningSnapsEnabled;
							bool flag7 = !this.RunningSnapsEnabled;
							if (flag7)
							{
								this.m_snappoint = null;
							}
							this.DoInvalidate(false);
						}
					}
					else
					{
						bool flag8 = e.KeyCode == Keys.Escape;
						if (flag8)
						{
							this.CommandEscape(true);
						}
						bool flag9 = e.KeyCode == Keys.P;
						if (flag9)
						{
							this.CommandPan();
						}
						bool flag10 = e.KeyCode == Keys.S;
						if (flag10)
						{
							this.CommandEscape(true);
						}
						bool flag11 = e.KeyCode == Keys.M;
						if (flag11)
						{
							this.CommandMove(true);
						}
						bool flag12 = e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9;
						if (flag12)
						{
							int num = e.KeyCode - Keys.D1;
							bool flag13 = num >= 0 && num < this.m_model.Layers.Length;
							if (flag13)
							{
								this.m_model.ActiveLayer = this.m_model.Layers[num];
								this.DoInvalidate(true);
							}
						}
						bool flag14 = e.KeyCode == Keys.Delete;
						if (flag14)
						{
							this.CommandDeleteSelected();
						}
						bool flag15 = e.KeyCode == Keys.O;
						if (flag15)
						{
							this.CommandEdit("linesmeet");
						}
						this.UpdateCursor();
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode =System.Windows.Forms.AutoScaleMode.Font;
			base.Name = "Canvas";
			base.Size = new Size(598, 403);
			base.ResumeLayout(false);
		}
	}
}
