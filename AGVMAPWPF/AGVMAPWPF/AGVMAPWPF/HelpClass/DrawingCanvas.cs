using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Pen = System.Windows.Media.Pen;

namespace AGVMAPWPF
{
    public class DrawingCanvas : Canvas
    {
        private List<Visual> visuals = new List<Visual>();

        public List<Visual> AllVisuals
        {
            get { return visuals; }
        }

        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= visuals.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            return visuals[index];
        }

        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);
            base.AddVisualChild(visual); 
            base.AddLogicalChild(visual);
        }

        public void DeleteVisual(Visual visual)
        {
            visuals.Remove(visual);
            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        public void ClearVisual()
        {
            for (int i = visuals.Count - 1; i >= 0; i--)
            {
                base.RemoveVisualChild(visuals[i]);
                base.RemoveLogicalChild(visuals[i]);
            }
            visuals.Clear();
        }

        public DrawingVisual GetVisual(System.Windows.Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as DrawingVisual;
        }

        //使用DrawVisual画Line
        public DrawingVisual Getline(System.Windows.Point point1, System.Windows.Point point2, Brush color, double thinkness)
        {
            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext dc = visual.RenderOpen())
            {
                Pen pen = new Pen(color, thinkness);
                pen.Freeze(); //冻结画笔，这样能加快绘图速度
                dc.DrawLine(pen, point1, point2);
            }
            return visual;
        }

        //使用DrawVisual画点
        public DrawingVisual GetPoint(System.Windows.Point pointCenter, double radius, Brush color, double thinkness)
        {
            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext dc = visual.RenderOpen())
            {
                Pen pen = new Pen(color, thinkness);
                pen.Freeze(); //冻结画笔，这样能加快绘图速度
                dc.DrawEllipse(color, pen, pointCenter, radius, radius);
            }
            return visual;
        }
    }
}
