using Canvas.DrawTools;
using System;

namespace Canvas.EditTools
{
	public class LinePoints
	{
		private LineTool m_line;

		private UnitPoint m_p1;

		private UnitPoint m_p2;

		public UnitPoint MousePoint;

		public LineTool Line
		{
			get
			{
				return this.m_line;
			}
		}

		public void SetLine(LineTool l)
		{
			try
			{
				this.m_line = l;
				this.m_p1 = l.P1;
				this.m_p2 = l.P2;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void ResetLine()
		{
			try
			{
				this.m_line.P1 = this.m_p1;
				this.m_line.P2 = this.m_p2;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void SetNewPoints(LineTool l, UnitPoint hitpoint, UnitPoint intersectpoint)
		{
			try
			{
				this.SetLine(l);
				double num = HitUtil.Distance(hitpoint, l.P1);
				double num2 = HitUtil.Distance(intersectpoint, l.P1);
				bool flag = num <= num2;
				if (flag)
				{
					this.m_p2 = intersectpoint;
				}
				else
				{
					this.m_p1 = intersectpoint;
				}
				this.ResetLine();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
