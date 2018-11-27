using System;
using System.Collections.Generic;
using System.Drawing;

namespace Canvas
{
	public class HitUtil
	{
		public static bool PointInPoint(UnitPoint p, UnitPoint tp, float tpThresHold)
		{
			bool flag = p.IsEmpty || tp.IsEmpty;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = p.X < tp.X - (double)tpThresHold || p.X > tp.X + (double)tpThresHold;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = p.Y < tp.Y - (double)tpThresHold || p.Y > tp.Y + (double)tpThresHold;
					result = !flag3;
				}
			}
			return result;
		}

		public static double Distance(UnitPoint p1, UnitPoint p2)
		{
			return HitUtil.Distance(p1, p2, true);
		}

		public static double Distance(UnitPoint p1, UnitPoint p2, bool abs)
		{
			double num = p1.X - p2.X;
			double num2 = p1.Y - p2.Y;
			if (abs)
			{
				num = Math.Abs(num);
				num2 = Math.Abs(num2);
			}
			bool flag = num == 0.0;
			double result;
			if (flag)
			{
				result = num2;
			}
			else
			{
				bool flag2 = num2 == 0.0;
				if (flag2)
				{
					result = num;
				}
				else
				{
					result = Math.Sqrt(Math.Pow(num, 2.0) + Math.Pow(num2, 2.0));
				}
			}
			return result;
		}

		public static double RadiansToDegrees(double radians)
		{
			return radians * 57.295779513082323;
		}

		public static double DegressToRadians(double degrees)
		{
			return degrees * 0.017453292519943295;
		}

		public static RectangleF CircleBoundingRect(UnitPoint center, float radius)
		{
			RectangleF result;
			try
			{
				RectangleF rectangleF = new RectangleF(center.Point, new SizeF(0f, 0f));
				rectangleF.Inflate(radius, radius);
				result = rectangleF;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool CircleHitPoint(UnitPoint center, float radius, UnitPoint hitpoint)
		{
			bool result;
			try
			{
				double num = center.X - (double)radius;
				double num2 = center.X + (double)radius;
				bool flag = hitpoint.X < num || hitpoint.X > num2;
				if (flag)
				{
					result = false;
				}
				else
				{
					double num3 = center.Y - (double)radius;
					double num4 = center.Y + (double)radius;
					bool flag2 = hitpoint.Y < num3 || hitpoint.Y > num4;
					if (flag2)
					{
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool CircleIntersectWithLine(UnitPoint center, float radius, UnitPoint lp1, UnitPoint lp2)
		{
			bool result;
			try
			{
				bool flag = HitUtil.Distance(center, lp1) < (double)radius && HitUtil.Distance(center, lp2) < (double)radius;
				if (flag)
				{
					result = false;
				}
				else
				{
					UnitPoint p = HitUtil.NearestPointOnLine(lp1, lp2, center);
					double num = HitUtil.Distance(center, p);
					bool flag2 = num <= (double)radius;
					if (flag2)
					{
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool IsPointInCircle(UnitPoint center, float radius, UnitPoint testpoint, float halflinewidth)
		{
			bool result;
			try
			{
				double num = HitUtil.Distance(center, testpoint);
				bool flag = num >= (double)(radius - halflinewidth) && num <= (double)(radius + halflinewidth);
				if (flag)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static UnitPoint NearestPointOnCircle(UnitPoint center, float radius, UnitPoint testpoint, double roundToAngleD)
		{
			UnitPoint result;
			try
			{
				double num = HitUtil.LineAngleR(center, testpoint, HitUtil.DegressToRadians(roundToAngleD));
				double num2 = Math.Cos(num) * (double)radius;
				double num3 = Math.Sin(num) * (double)radius;
				double num4 = Math.Sqrt(Math.Pow(num2, 2.0) + Math.Pow(num3, 2.0));
				result = new UnitPoint((double)((float)(center.X + num2)), (double)((float)(center.Y + num3)));
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static UnitPoint TangentPointOnCircle(UnitPoint center, float radius, UnitPoint testpoint, bool reverse)
		{
			UnitPoint result;
			try
			{
				double num = Math.Sqrt(Math.Pow(center.X - testpoint.X, 2.0) + Math.Pow(center.Y - testpoint.Y, 2.0));
				double num2 = (double)radius;
				bool flag = Math.Abs(num) < num2;
				if (flag)
				{
					result = UnitPoint.Empty;
				}
				else
				{
					double num3 = HitUtil.LineAngleR(center, testpoint, 0.0);
					double num4 = Math.Asin(num2 / num);
					double num5 = Math.Acos(num2 / num);
					double angleR = 1.5707963267948966 + (num3 - num4);
					if (reverse)
					{
						angleR = num3 - num5;
					}
					result = HitUtil.LineEndpoint(center, angleR, (double)radius);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static UnitPoint PointOncircle(UnitPoint center, double radius, double angleR)
		{
			UnitPoint result;
			try
			{
				double num = center.Y + Math.Sin(angleR) * radius;
				double num2 = center.X + Math.Cos(angleR) * radius;
				result = new UnitPoint((double)((float)num2), (double)((float)num));
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static UnitPoint LineEndpoint(UnitPoint lp1, double angleR, double length)
		{
			UnitPoint result;
			try
			{
				double num = Math.Cos(angleR) * length;
				double num2 = Math.Sin(angleR) * length;
				result = new UnitPoint(lp1.X + (double)((float)num), lp1.Y + (double)((float)num2));
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static RectangleF LineBoundingRect(UnitPoint linepoint1, UnitPoint linepoint2, float halflinewidth)
		{
			RectangleF result;
			try
			{
				double x = Math.Min(linepoint1.X, linepoint2.X);
				double y = Math.Min(linepoint1.Y, linepoint2.Y);
				double w = Math.Abs(linepoint1.X - linepoint2.X);
				double h = Math.Abs(linepoint1.Y - linepoint2.Y);
				RectangleF rect = ScreenUtils.GetRect(x, y, w, h);
				rect.Inflate(halflinewidth, halflinewidth);
				result = rect;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static bool IsPointInLine(UnitPoint linepoint1, UnitPoint linepoint2, UnitPoint testpoint, float halflinewidth)
		{
			bool result;
			try
			{
				UnitPoint center = linepoint1;
				UnitPoint center2 = linepoint2;
				UnitPoint hitpoint = testpoint;
				bool flag = HitUtil.CircleHitPoint(center, halflinewidth, hitpoint);
				if (flag)
				{
					result = true;
				}
				else
				{
					bool flag2 = HitUtil.CircleHitPoint(center2, halflinewidth, hitpoint);
					if (flag2)
					{
						result = true;
					}
					else
					{
						bool flag3 = center.Y == center2.Y;
						if (flag3)
						{
							double num = Math.Min(center.Y, center2.Y) - (double)halflinewidth;
							double num2 = Math.Max(center.Y, center2.Y) + (double)halflinewidth;
							bool flag4 = testpoint.Y < num || testpoint.Y > num2;
							if (flag4)
							{
								result = false;
							}
							else
							{
								double num3 = Math.Min(center.X, center2.X) - (double)halflinewidth;
								double num4 = Math.Max(center.X, center2.X) + (double)halflinewidth;
								bool flag5 = hitpoint.X >= num3 && hitpoint.X <= num4;
								if (flag5)
								{
									result = true;
								}
								else
								{
									result = false;
								}
							}
						}
						else
						{
							bool flag6 = center.X == center2.X;
							if (flag6)
							{
								double num5 = Math.Min(center.X, center2.X) - (double)halflinewidth;
								double num6 = Math.Max(center.X, center2.X) + (double)halflinewidth;
								bool flag7 = testpoint.X < num5 || testpoint.X > num6;
								if (flag7)
								{
									result = false;
								}
								else
								{
									double num7 = Math.Min(center.Y, center2.Y) - (double)halflinewidth;
									double num8 = Math.Max(center.Y, center2.Y) + (double)halflinewidth;
									bool flag8 = hitpoint.Y >= num7 && hitpoint.Y <= num8;
									if (flag8)
									{
										result = true;
									}
									else
									{
										result = false;
									}
								}
							}
							else
							{
								double x = Math.Abs(center2.X - hitpoint.X);
								double x2 = Math.Abs(center2.Y - hitpoint.Y);
								double num9 = Math.Pow(x, 2.0) + Math.Pow(x2, 2.0);
								double num10 = Math.Sqrt(num9);
								x = Math.Abs(center.X - center2.X);
								x2 = Math.Abs(center.Y - center2.Y);
								double num11 = Math.Pow(x, 2.0) + Math.Pow(x2, 2.0);
								double num12 = Math.Sqrt(num11);
								x = Math.Abs(center.X - hitpoint.X);
								x2 = Math.Abs(center.Y - hitpoint.Y);
								double num13 = Math.Pow(x, 2.0) + Math.Pow(x2, 2.0);
								double num14 = Math.Sqrt(num13);
								double a = Math.Acos((num9 - num11 - num13) / (-2.0 * num12 * num14));
								double num15 = Math.Sin(a) * num14;
								result = (num15 <= (double)halflinewidth);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static PointF[] draw_bezier_curves(PointF[] points, int count, float step)
		{
			PointF[] result;
			try
			{
				List<PointF> list = new List<PointF>();
				float num = 0f;
				do
				{
					PointF item = HitUtil.bezier_interpolation_func(num, points, count);
					num += step;
					list.Add(item);
				}
				while (num <= 1f && count > 1);
				result = list.ToArray();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private static PointF bezier_interpolation_func(float t, PointF[] points, int count)
		{
			PointF result;
			try
			{
				PointF pointF = default(PointF);
				float[] array = new float[count];
				float num = 0f;
				float num2 = 0f;
				int num5;
				for (int i = 0; i < count; i = num5 + 1)
				{
					int num3 = count - 1;
					ulong num4 = HitUtil.calc_combination_number(num3, i);
					num += (float)((double)(num4 * points[i].X) * Math.Pow((double)(1f - t), (double)(num3 - i)) * Math.Pow((double)t, (double)i));
					num2 += (float)((double)(num4 * points[i].Y) * Math.Pow((double)(1f - t), (double)(num3 - i)) * Math.Pow((double)t, (double)i));
					num5 = i;
				}
				pointF.X = num;
				pointF.Y = num2;
				result = pointF;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private static ulong calc_combination_number(int n, int k)
		{
			ulong result;
			try
			{
				ulong[] array = new ulong[n + 1];
				int num;
				for (int i = 1; i <= n; i = num + 1)
				{
					array[i] = 1uL;
					for (int j = i - 1; j >= 1; j = num - 1)
					{
						array[j] += array[j - 1];
						num = j;
					}
					array[0] = 1uL;
					num = i;
				}
				result = array[k];
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private static bool LinesIntersect(UnitPoint lp1, UnitPoint lp2, UnitPoint lp3, UnitPoint lp4, ref double x, ref double y, bool returnpoint, bool extendA, bool extendB)
		{
			double x2 = lp1.X;
			double x3 = lp2.X;
			double x4 = lp3.X;
			double x5 = lp4.X;
			double y2 = lp1.Y;
			double y3 = lp2.Y;
			double y4 = lp3.Y;
			double y5 = lp4.Y;
			double num = (y5 - y4) * (x3 - x2) - (x5 - x4) * (y3 - y2);
			bool flag = num == 0.0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				double num2 = (x5 - x4) * (y2 - y4) - (y5 - y4) * (x2 - x4);
				double num3 = (x3 - x2) * (y2 - y4) - (y3 - y2) * (x2 - x4);
				double num4 = num2 / num;
				double num5 = num3 / num;
				bool flag2 = !extendA;
				if (flag2)
				{
					bool flag3 = num4 < 0.0 || num4 > 1.0;
					if (flag3)
					{
						result = false;
						return result;
					}
				}
				bool flag4 = !extendB;
				if (flag4)
				{
					bool flag5 = num5 < 0.0 || num5 > 1.0;
					if (flag5)
					{
						result = false;
						return result;
					}
				}
				bool flag6 = extendA | extendB;
				if (flag6)
				{
					x = x2 + num4 * (x3 - x2);
					y = y2 + num4 * (y3 - y2);
					result = true;
				}
				else
				{
					bool flag7 = num4 >= 0.0 && num4 <= 1.0 && num5 >= 0.0 && num5 <= 1.0;
					if (flag7)
					{
						if (returnpoint)
						{
							x = x2 + num4 * (x3 - x2);
							y = y2 + num4 * (y3 - y2);
						}
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		public static bool LinesIntersect(UnitPoint lp1, UnitPoint lp2, UnitPoint lp3, UnitPoint lp4)
		{
			double num = 0.0;
			double num2 = 0.0;
			return HitUtil.LinesIntersect(lp1, lp2, lp3, lp4, ref num, ref num2, false, false, false);
		}

		public static UnitPoint LinesIntersectPoint(UnitPoint lp1, UnitPoint lp2, UnitPoint lp3, UnitPoint lp4)
		{
			double x = 0.0;
			double y = 0.0;
			bool flag = HitUtil.LinesIntersect(lp1, lp2, lp3, lp4, ref x, ref y, true, false, false);
			UnitPoint result;
			if (flag)
			{
				result = new UnitPoint(x, y);
			}
			else
			{
				result = UnitPoint.Empty;
			}
			return result;
		}

		public static UnitPoint FindApparentIntersectPoint(UnitPoint lp1, UnitPoint lp2, UnitPoint lp3, UnitPoint lp4)
		{
			double x = 0.0;
			double y = 0.0;
			bool flag = HitUtil.LinesIntersect(lp1, lp2, lp3, lp4, ref x, ref y, true, true, true);
			UnitPoint result;
			if (flag)
			{
				result = new UnitPoint(x, y);
			}
			else
			{
				result = UnitPoint.Empty;
			}
			return result;
		}

		public static UnitPoint FindApparentIntersectPoint(UnitPoint lp1, UnitPoint lp2, UnitPoint lp3, UnitPoint lp4, bool extendA, bool extendB)
		{
			double x = 0.0;
			double y = 0.0;
			bool flag = HitUtil.LinesIntersect(lp1, lp2, lp3, lp4, ref x, ref y, true, extendA, extendB);
			UnitPoint result;
			if (flag)
			{
				result = new UnitPoint(x, y);
			}
			else
			{
				result = UnitPoint.Empty;
			}
			return result;
		}

		public static bool LineIntersectWithRect(UnitPoint lp1, UnitPoint lp2, RectangleF r)
		{
			bool flag = r.Contains(lp1.Point);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = r.Contains(lp2.Point);
				if (flag2)
				{
					result = true;
				}
				else
				{
					UnitPoint lp3 = new UnitPoint((double)r.Left, (double)r.Top);
					UnitPoint lp4 = new UnitPoint((double)r.Left, (double)r.Bottom);
					bool flag3 = HitUtil.LinesIntersect(lp1, lp2, lp3, lp4);
					if (flag3)
					{
						result = true;
					}
					else
					{
						lp4.Y = (double)r.Top;
						lp4.X = (double)r.Right;
						bool flag4 = HitUtil.LinesIntersect(lp1, lp2, lp3, lp4);
						if (flag4)
						{
							result = true;
						}
						else
						{
							lp3.X = (double)r.Right;
							lp3.Y = (double)r.Top;
							lp4.X = (double)r.Right;
							lp4.Y = (double)r.Bottom;
							bool flag5 = HitUtil.LinesIntersect(lp1, lp2, lp3, lp4);
							result = flag5;
						}
					}
				}
			}
			return result;
		}

		public static UnitPoint LineMidpoint(UnitPoint lp1, UnitPoint lp2)
		{
			return new UnitPoint
			{
				X = (lp1.X + lp2.X) / 2.0,
				Y = (lp1.Y + lp2.Y) / 2.0
			};
		}

		public static double LineAngleR(UnitPoint lp1, UnitPoint lp2, double roundToAngleR)
		{
			bool flag = lp1.X == lp2.X;
			double result;
			if (flag)
			{
				bool flag2 = lp1.Y > lp2.Y;
				if (flag2)
				{
					result = 4.71238898038469;
				}
				else
				{
					bool flag3 = lp1.Y < lp2.Y;
					if (flag3)
					{
						result = 1.5707963267948966;
					}
					else
					{
						result = 0.0;
					}
				}
			}
			else
			{
				double num = lp2.X - lp1.X;
				double num2 = lp2.Y - lp1.Y;
				double num3 = Math.Atan(num2 / num);
				bool flag4 = num < 0.0;
				if (flag4)
				{
					num3 += 3.1415926535897931;
				}
				bool flag5 = num > 0.0 && num2 < 0.0;
				if (flag5)
				{
					num3 += 6.2831853071795862;
				}
				bool flag6 = roundToAngleR != 0.0;
				if (flag6)
				{
					double num4 = Math.Round(num3 / roundToAngleR);
					num3 = roundToAngleR * num4;
				}
				result = num3;
			}
			return result;
		}

		public static UnitPoint OrthoPointD(UnitPoint lp1, UnitPoint lp2, double roundToAngleR)
		{
			return HitUtil.OrthoPointR(lp1, lp2, HitUtil.DegressToRadians(roundToAngleR));
		}

		public static UnitPoint OrthoPointR(UnitPoint lp1, UnitPoint lp2, double roundToAngleR)
		{
			return HitUtil.NearestPointOnLine(lp1, lp2, lp2, roundToAngleR);
		}

		public static UnitPoint NearestPointOnLine(UnitPoint lp1, UnitPoint lp2, UnitPoint tp)
		{
			return HitUtil.NearestPointOnLine(lp1, lp2, tp, false);
		}

		public static UnitPoint NearestPointOnLine(UnitPoint lp1, UnitPoint lp2, UnitPoint tp, bool beyondSegment)
		{
			bool flag = lp1.X == lp2.X;
			UnitPoint result;
			if (flag)
			{
				bool flag2 = !beyondSegment && lp1.Y < tp.Y && lp2.Y < tp.Y;
				if (flag2)
				{
					result = new UnitPoint(lp1.X, Math.Max(lp1.Y, lp2.Y));
				}
				else
				{
					bool flag3 = !beyondSegment && lp1.Y > tp.Y && lp2.Y > tp.Y;
					if (flag3)
					{
						result = new UnitPoint(lp1.X, Math.Min(lp1.Y, lp2.Y));
					}
					else
					{
						result = new UnitPoint(lp1.X, tp.Y);
					}
				}
			}
			else
			{
				bool flag4 = lp1.Y == lp2.Y;
				if (flag4)
				{
					bool flag5 = !beyondSegment && lp1.X < tp.X && lp2.X < tp.X;
					if (flag5)
					{
						result = new UnitPoint(Math.Max(lp1.X, lp2.X), lp1.Y);
					}
					else
					{
						bool flag6 = !beyondSegment && lp1.X > tp.X && lp2.X > tp.X;
						if (flag6)
						{
							result = new UnitPoint(Math.Min(lp1.X, lp2.X), lp1.Y);
						}
						else
						{
							result = new UnitPoint(tp.X, lp1.Y);
						}
					}
				}
				else
				{
					result = HitUtil.NearestPointOnLine(lp1, lp2, tp, 0.0);
				}
			}
			return result;
		}

		private static UnitPoint NearestPointOnLine(UnitPoint lp1, UnitPoint lp2, UnitPoint testpoint, double roundToAngleR)
		{
			bool flag = lp1.X == testpoint.X;
			if (flag)
			{
				UnitPoint unitPoint = lp1;
				lp1 = lp2;
				lp2 = unitPoint;
			}
			double num = HitUtil.LineAngleR(lp1, testpoint, 0.0);
			double num2 = HitUtil.LineAngleR(lp1, lp2, roundToAngleR);
			double d = num - num2;
			double num3 = (lp1.X - testpoint.X) / Math.Cos(num);
			double num4 = Math.Cos(d) * num3;
			double num5 = Math.Cos(num2) * num4;
			double num6 = Math.Sin(num2) * num4;
			num5 = lp1.X - num5;
			num6 = lp1.Y - num6;
			return new UnitPoint((double)((float)num5), (double)((float)num6));
		}

		public static double LineSlope(UnitPoint p1, UnitPoint p2)
		{
			return (p2.Y - p1.Y) / (p2.X - p1.X);
		}

		public static UnitPoint CenterPointFrom3Points(UnitPoint p1, UnitPoint p2, UnitPoint p3)
		{
			UnitPoint result;
			try
			{
				double num = (p2.Y - p1.Y) / (p2.X - p1.X);
				double num2 = (p3.Y - p2.Y) / (p3.X - p2.X);
				bool flag = double.IsInfinity(num);
				if (flag)
				{
					num = 1000000000000.0;
				}
				bool flag2 = double.IsInfinity(num2);
				if (flag2)
				{
					num2 = 1000000000000.0;
				}
				bool flag3 = num == 0.0;
				if (flag3)
				{
					num = 1E-12;
				}
				bool flag4 = num2 == 0.0;
				if (flag4)
				{
					num2 = 1E-12;
				}
				double num3 = (num * num2 * (p1.Y - p3.Y) + num2 * (p1.X + p2.X) - num * (p2.X + p3.X)) / (2.0 * (num2 - num));
				double num4 = -1.0 * (num3 - (p1.X + p2.X) / 2.0) / num + (p1.Y + p2.Y) / 2.0;
				result = new UnitPoint((double)((float)num3), (double)((float)num4));
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
	}
}
