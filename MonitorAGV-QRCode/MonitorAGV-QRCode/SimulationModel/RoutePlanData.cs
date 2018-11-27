using Model.MDM;
using Model.MSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using Tools;

namespace SimulationModel
{
	public class RoutePlanData
	{
		public List<LandmarkInfo> RouteList = new List<LandmarkInfo>();

		public List<LandmarkInfo> CloseList = new List<LandmarkInfo>();

		public Stack PivotAnotherLand = new Stack();

		private IDictionary dic_Land = new Hashtable();

		private IList<AllSegment> Segments;

		private int OpenCount = 0;

		private LandmarkInfo BeginLand = null;

		private LandmarkInfo EndLand = null;

		private IList<LandmarkInfo> AllLands = new List<LandmarkInfo>();

		public int OpenAmount
		{
			get
			{
				bool flag = this.OpenCount == 0;
				if (flag)
				{
					this.OpenCount = this.AllLands.Count;
				}
				return this.OpenCount;
			}
		}

		public RoutePlanData(IList<AllSegment> Segs)
		{
			this.Segments = Segs;
            DataTable dtLand = Function.GetDataInfo("PR_SELECT_TBLANDMARK");
            this.AllLands = DataToObject.TableToEntity<LandmarkInfo>(dtLand);
		}

		public void RepairRoute()
		{
			try
			{
				bool flag = false;
				this.OnlyAcountDirect(ref this.RouteList);
				List<LandmarkInfo> list = this.CreateDeepCopy<List<LandmarkInfo>>(this.RouteList);
			    List<LandmarkInfo> list2 = (from p in list
			        where p.sway > SwayEnum.None
			        select p).ToList();
				int num3;
				for (int i = 0; i < list2.Count; i = num3 + 1)
				{
					for (int j = i + 1; j < list2.Count; j = num3 + 1)
					{
						LandmarkInfo FirstLand = list2[i];
						LandmarkInfo SecondLand = list2[j];
						if (FirstLand != null && SecondLand != null)
						{
							List<LandmarkInfo> list3 = new List<LandmarkInfo>();
							int num = list.FindIndex((LandmarkInfo p) => p.LandmarkCode == FirstLand.LandmarkCode);
							int num2 = list.FindIndex((LandmarkInfo p) => p.LandmarkCode == SecondLand.LandmarkCode);
							bool flag3 = num >= 0 && num2 >= 0;
							if (flag3)
							{
								for (int k = num; k <= num2; k = num3 + 1)
								{
									list3.Add(this.CreateDeepCopy<LandmarkInfo>(list[k]));
									num3 = k;
								}
							}
							this.CloseList.Clear();
							this.RouteList.Clear();
							this.AcountRoute(FirstLand, SecondLand);
							bool flag4 = this.RouteList.Count > 0 && list3.Count > 0 && this.RouteList.Count < list3.Count;
							if (flag4)
							{
								list.RemoveRange(num, num2 - num + 1);
								list.InsertRange(num, this.RouteList);
								flag = true;
								break;
							}
						}
						num3 = j;
					}
					bool flag5 = flag;
					if (flag5)
					{
						break;
					}
					num3 = i;
				}
				bool flag6 = flag;
				if (flag6)
				{
					this.RouteList = this.CreateDeepCopy<List<LandmarkInfo>>(list);
					this.RepairRoute();
				}
				else
				{
					this.RouteList = this.CreateDeepCopy<List<LandmarkInfo>>(list);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<LandmarkInfo> GetRoute(LandmarkInfo StartLand, LandmarkInfo EndLand)
		{
			List<LandmarkInfo> result;
			try
			{
				this.RouteList.Clear();
				this.CloseList.Clear();
				this.PivotAnotherLand.Clear();
				List<List<LandmarkInfo>> list = new List<List<LandmarkInfo>>();
				this.AcountRoute(StartLand, EndLand);
				list.Add(this.CreateDeepCopy<List<LandmarkInfo>>(this.RouteList));
				List<LandmarkInfo> obj = this.CreateDeepCopy<List<LandmarkInfo>>(this.RouteList);
				List<LandmarkInfo> obj2 = this.CreateDeepCopy<List<LandmarkInfo>>(this.CloseList);
				Stack stack = this.CreateDeepCopy<Stack>(this.PivotAnotherLand);
				while (stack.Count > 0)
				{
					this.PivotAnotherLand.Clear();
					this.RouteList = this.CreateDeepCopy<List<LandmarkInfo>>(obj);
					this.CloseList = this.CreateDeepCopy<List<LandmarkInfo>>(obj2);
					Hashtable hashtable = stack.Pop() as Hashtable;
					ArrayList keyList = new ArrayList(hashtable.Keys);
					LandmarkInfo starLand = hashtable[keyList[0].ToString()] as LandmarkInfo;
					try
					{
						int num = this.RouteList.FindIndex((LandmarkInfo p) => p.LandmarkCode == keyList[0].ToString()) + 1;
						int num2 = this.CloseList.FindIndex((LandmarkInfo p) => p.LandmarkCode == keyList[0].ToString()) + 1;
						bool flag = num >= 0 && num < this.RouteList.Count;
						if (flag)
						{
							this.RouteList.RemoveRange(num, this.RouteList.Count - num);
						}
						bool flag2 = num2 >= 0 && num2 < this.CloseList.Count;
						if (flag2)
						{
							this.CloseList.RemoveRange(num2, this.CloseList.Count - num2);
						}
					}
					catch (Exception ex)
					{
					}
					this.AcountRoute(starLand, EndLand);
					list.Add(this.CreateDeepCopy<List<LandmarkInfo>>(this.RouteList));
				}
                List<LandmarkInfo> list2 = (from a in list
                                            where (
                                                from p in a
                                                where p.LandmarkCode == EndLand.LandmarkCode
                                                select p).Count<LandmarkInfo>() > 0
                                            orderby a.Count
                                            select a).FirstOrDefault<List<LandmarkInfo>>();
				if (list2 == null)
				{
					list2 = new List<LandmarkInfo>();
					result = list2;
				}
				else
				{
					this.RouteList = this.CreateDeepCopy<List<LandmarkInfo>>(list2);
					this.RepairRoute();
					this.AcountDirect(ref this.RouteList);
					result = this.RouteList;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public void AcountRoute(LandmarkInfo StarLand, LandmarkInfo EndLand)
		{
			try
			{
				bool flag = this.Segments.Count > 0;
				if (flag)
				{
					bool flag2 = this.BeginLand == null;
					if (flag2)
					{
						this.BeginLand = this.CreateDeepCopy<LandmarkInfo>(StarLand);
					}
					bool flag3 = this.EndLand == null;
					if (flag3)
					{
						this.EndLand = this.CreateDeepCopy<LandmarkInfo>(EndLand);
					}
					LandmarkInfo landmarkInfo = StarLand;
					bool flag4 = this.CloseList.Count < this.OpenAmount && landmarkInfo.LandmarkCode != EndLand.LandmarkCode;
					if (flag4)
					{
						bool flag5 = (from p in this.CloseList
						where p.LandmarkCode == StarLand.LandmarkCode
						select p).Count<LandmarkInfo>() <= 0;
						if (flag5)
						{
							this.RouteList.Add(StarLand);
							this.CloseList.Add(StarLand);
						}
						List<LandmarkInfo> lands = this.getNextLand(landmarkInfo);
						bool flag6 = lands.Count == 0;
						if (flag6)
						{
							bool flag7 = this.PivotAnotherLand.Count > 0;
							if (!flag7)
							{
								return;
							}
							Hashtable hashtable = this.PivotAnotherLand.Pop() as Hashtable;
							ArrayList keyList = new ArrayList(hashtable.Keys);
							LandmarkInfo landmarkInfo2 = hashtable[keyList[0].ToString()] as LandmarkInfo;
							int num = this.RouteList.FindIndex((LandmarkInfo p) => p.LandmarkCode == keyList[0].ToString()) + 1;
							this.RouteList.RemoveRange(num, this.RouteList.Count - num);
							landmarkInfo = landmarkInfo2;
						}
						else
						{
							bool flag8 = lands.Count == 1;
							if (flag8)
							{
								bool flag9 = (from p in this.CloseList
								where p.LandmarkCode == lands[0].LandmarkCode
								select p).Count<LandmarkInfo>() <= 0;
								if (flag9)
								{
									this.RouteList.Add(lands[0]);
									this.CloseList.Add(lands[0]);
								}
								landmarkInfo = lands[0];
							}
							else
							{
								List<LandmarkInfo> CloseLand = new List<LandmarkInfo>();
							    DataTable dtPara = Function.GetParByCondition("RouteCountMode");
                                bool flag10 = dtPara != null && dtPara.Rows.Count > 0 && dtPara.Rows[0]["ParameterValue"].ToString() == "最少拐弯" && this.RouteList.Count > 1;
								if (flag10)
								{
									LandmarkInfo Frist = this.RouteList[this.RouteList.Count - 2];
									LandmarkInfo Cent = this.RouteList[this.RouteList.Count - 1];
									CloseLand = (from a in lands
									orderby RoutePlanData.Angle(new PointF((float)Cent.LandX, (float)Cent.LandY), new PointF((float)Frist.LandX, (float)Frist.LandY), new PointF((float)a.LandX, (float)a.LandY)) descending
									select a).ToList<LandmarkInfo>();
								}
								else
								{
									CloseLand = (from a in lands
									orderby this.getDistant(a.LandX, a.LandY, EndLand.LandX, EndLand.LandY)
									select a).ToList<LandmarkInfo>();
								}
								bool flag11 = CloseLand == null;
								if (flag11)
								{
									throw new Exception("计算路径异常!");
								}
							    bool flag12 = flag10;
								if (flag12)
								{
									LandmarkInfo landmarkInfo3 = this.RouteList[this.RouteList.Count - 2];
									LandmarkInfo landmarkInfo4 = this.RouteList[this.RouteList.Count - 1];
									bool flag13 = this.getDistant(landmarkInfo4.LandX, landmarkInfo4.LandY, EndLand.LandX, EndLand.LandY) <= this.getDistant(CloseLand[0].LandX, CloseLand[0].LandY, EndLand.LandX, EndLand.LandY);
									if (flag13)
									{
										this.RouteList.Add(CloseLand[1]);
										this.CloseList.Add(CloseLand[1]);
										Hashtable hashtable2 = new Hashtable();
										hashtable2[landmarkInfo.LandmarkCode] = CloseLand[0];
										this.PivotAnotherLand.Push(hashtable2);
										landmarkInfo = CloseLand[1];
									}
									else
									{
										this.RouteList.Add(CloseLand[0]);
										this.CloseList.Add(CloseLand[0]);
										int num2;
										for (int i = 1; i < CloseLand.Count; i = num2 + 1)
										{
											Hashtable hashtable3 = new Hashtable();
											hashtable3[landmarkInfo.LandmarkCode] = CloseLand[i];
											this.PivotAnotherLand.Push(hashtable3);
											num2 = i;
										}
										landmarkInfo = CloseLand[0];
									}
								}
								else
								{
									bool flag14 = (from p in this.CloseList
									where p.LandmarkCode == CloseLand[0].LandmarkCode
									select p).Count<LandmarkInfo>() <= 0;
									if (flag14)
									{
										this.RouteList.Add(CloseLand[0]);
										this.CloseList.Add(CloseLand[0]);
										int num2;
										for (int j = 1; j < CloseLand.Count; j = num2 + 1)
										{
											Hashtable hashtable4 = new Hashtable();
											hashtable4[landmarkInfo.LandmarkCode] = CloseLand[j];
											this.PivotAnotherLand.Push(hashtable4);
											num2 = j;
										}
									}
									landmarkInfo = CloseLand[0];
								}
							}
						}
						this.AcountRoute(landmarkInfo, EndLand);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private List<LandmarkInfo> getNextLand(LandmarkInfo PreLand)
		{
			List<LandmarkInfo> result;
			try
			{
				List<LandmarkInfo> list = new List<LandmarkInfo>();
				List<AllSegment> list2 = (from p in this.Segments
				where p.BeginLandMakCode == PreLand.LandmarkCode
				select p).ToList<AllSegment>();
				using (List<AllSegment>.Enumerator enumerator = list2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AllSegment item = enumerator.Current;
						LandmarkInfo EndLandMark = this.AllLands.FirstOrDefault((LandmarkInfo p) => p.LandmarkCode == item.EndLandMarkCode);
						bool flag = EndLandMark != null && (from p in this.CloseList
						where p.LandmarkCode == EndLandMark.LandmarkCode
						select p).Count<LandmarkInfo>() <= 0;
						if (flag)
						{
							list.Add(this.CreateDeepCopy<LandmarkInfo>(EndLandMark));
						}
					}
				}
				result = list;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private double getDistant(double X1, double Y1, double X2, double Y2)
		{
			double result;
			try
			{
				double num = Math.Sqrt(Math.Pow(Math.Abs(X1 - X2), 2.0) + Math.Pow(Math.Abs(Y1 - Y2), 2.0));
				result = num;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public T CreateDeepCopy<T>(T obj)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			memoryStream.Position = 0L;
			return (T)((object)binaryFormatter.Deserialize(memoryStream));
		}

		public void OnlyAcountDirect(ref List<LandmarkInfo> Routes)
		{
			try
			{
				bool flag = Routes.Count <= 1;
				if (!flag)
				{
					bool flag2 = Routes.Count >= 3;
					if (flag2)
					{
						int num2;
						for (int i = 2; i < Routes.Count; i = num2 + 1)
						{
							LandmarkInfo landmarkInfo = Routes[i - 2];
							LandmarkInfo landmarkInfo2 = Routes[i - 1];
							LandmarkInfo landmarkInfo3 = Routes[i];
							double num = (Math.Round(landmarkInfo.LandX, 3) - Math.Round(landmarkInfo3.LandX, 3)) * (Math.Round(landmarkInfo2.LandY, 3) - Math.Round(landmarkInfo3.LandY, 3)) - (Math.Round(landmarkInfo.LandY, 3) - Math.Round(landmarkInfo3.LandY, 3)) * (Math.Round(landmarkInfo2.LandX, 3) - Math.Round(landmarkInfo3.LandX, 3));
							bool flag3 = num > 0.05;
							if (flag3)
							{
								landmarkInfo2.sway = SwayEnum.Left;
							}
							else
							{
								bool flag4 = num < -0.05;
								if (flag4)
								{
									landmarkInfo2.sway = SwayEnum.Right;
								}
								else
								{
									landmarkInfo2.sway = SwayEnum.None;
								}
							}
							num2 = i;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void AcountDirect(ref List<LandmarkInfo> Routes)
		{
			try
			{
				DataTable dataTable = Function.GetDataInfo("PR_SELECT_COOR_INFO");
			    DataTable dtAllSegment = Function.GetDataInfo("P_SELECT_TBALLSEGMENT");
			    IList<AllSegment> list = DataToObject.TableToEntity<AllSegment>(dtAllSegment);

				LandmarkInfo UpLand = null;
				LandmarkInfo InflectLand = null;
				LandmarkInfo NextLand = null;
				bool flag = Routes.Count <= 1;
				if (!flag)
				{
					bool flag2 = Routes.Count >= 3;
					if (flag2)
					{
						int num2;
						Func<AllSegment, bool> F9__0=null;
						Func<AllSegment, bool> F9__1=null;
						Func<AllSegment, bool> F9__2=null;
						Func<AllSegment, bool> F9__3=null;
						for (int i = 2; i < Routes.Count; i = num2 + 1)
						{
							UpLand = Routes[i - 2];
							InflectLand = Routes[i - 1];
							NextLand = Routes[i];
							double num = (Math.Round(UpLand.LandX, 3) - Math.Round(NextLand.LandX, 3)) * (Math.Round(InflectLand.LandY, 3) - Math.Round(NextLand.LandY, 3)) - (Math.Round(UpLand.LandY, 3) - Math.Round(NextLand.LandY, 3)) * (Math.Round(InflectLand.LandX, 3) - Math.Round(NextLand.LandX, 3));
							bool flag3 = num > 0.05;
							if (flag3)
							{
								InflectLand.sway = SwayEnum.Left;
							}
							else
							{
								bool flag4 = num < -0.05;
								if (flag4)
								{
									InflectLand.sway = SwayEnum.Right;
								}
								else
								{
									InflectLand.sway = SwayEnum.None;
								}
							}
							bool flag5 = i == 2;
							if (flag5)
							{
								bool flag6 = Math.Round(UpLand.LandX, 1, MidpointRounding.AwayFromZero) == Math.Round(InflectLand.LandX, 1, MidpointRounding.AwayFromZero);
								if (flag6)
								{
									IEnumerable<AllSegment> arg_1DA_0 = list;
									Func<AllSegment, bool> arg_1DA_1;
									if ((arg_1DA_1 = F9__0) == null)
									{
										arg_1DA_1 = (F9__0 = ((AllSegment p) => p.BeginLandMakCode == UpLand.LandmarkCode && p.EndLandMarkCode == InflectLand.LandmarkCode));
									}
									AllSegment allSegment = arg_1DA_0.FirstOrDefault(arg_1DA_1);
									bool flag7 = allSegment != null && allSegment.ExcuteAngle != -1;
									if (flag7)
									{
										UpLand.Angle = allSegment.ExcuteAngle;
									}
									else
									{
										bool flag8 = InflectLand.LandY > UpLand.LandY;
										if (flag8)
										{
											bool flag9 = dataTable.Rows.Count > 0;
											if (flag9)
											{
												DataRow dataRow = dataTable.Select("Direction=" + 0).FirstOrDefault<DataRow>();
												UpLand.Angle = (int)Convert.ToInt16(dataRow["Angle"]);
											}
											else
											{
												UpLand.Angle = 90;
											}
										}
										else
										{
											bool flag10 = dataTable.Rows.Count > 0;
											if (flag10)
											{
												DataRow dataRow2 = dataTable.Select("Direction=" + 2).FirstOrDefault<DataRow>();
												UpLand.Angle = (int)Convert.ToInt16(dataRow2["Angle"]);
											}
											else
											{
												UpLand.Angle = 270;
											}
										}
									}
								}
								else
								{
									bool flag11 = Math.Round(UpLand.LandY, 1, MidpointRounding.AwayFromZero) == Math.Round(InflectLand.LandY, 1, MidpointRounding.AwayFromZero);
									if (flag11)
									{
										IEnumerable<AllSegment> arg_35A_0 = list;
										Func<AllSegment, bool> arg_35A_1;
										if ((arg_35A_1 = F9__1) == null)
										{
											arg_35A_1 = (F9__1 = ((AllSegment p) => p.BeginLandMakCode == UpLand.LandmarkCode && p.EndLandMarkCode == InflectLand.LandmarkCode));
										}
										AllSegment allSegment2 = arg_35A_0.FirstOrDefault(arg_35A_1);
										bool flag12 = allSegment2 != null && allSegment2.ExcuteAngle != -1;
										if (flag12)
										{
											UpLand.Angle = allSegment2.ExcuteAngle;
										}
										else
										{
											bool flag13 = InflectLand.LandX > UpLand.LandX;
											if (flag13)
											{
												bool flag14 = dataTable.Rows.Count > 0;
												if (flag14)
												{
													DataRow dataRow3 = dataTable.Select("Direction=" + 1).FirstOrDefault<DataRow>();
													UpLand.Angle = (int)Convert.ToInt16(dataRow3["Angle"]);
												}
												else
												{
													UpLand.Angle = 0;
												}
											}
											else
											{
												bool flag15 = dataTable.Rows.Count > 0;
												if (flag15)
												{
													DataRow dataRow4 = dataTable.Select("Direction=" + 3).FirstOrDefault<DataRow>();
													UpLand.Angle = (int)Convert.ToInt16(dataRow4["Angle"]);
												}
												else
												{
													UpLand.Angle = 180;
												}
											}
										}
									}
								}
							}
							bool flag16 = i == Routes.Count - 1;
							if (flag16)
							{
								NextLand.Angle = InflectLand.Angle;
							}
							bool flag17 = Math.Round(InflectLand.LandX, 1, MidpointRounding.AwayFromZero) == Math.Round(NextLand.LandX, 1, MidpointRounding.AwayFromZero);
							if (flag17)
							{
								IEnumerable<AllSegment> arg_501_0 = list;
								Func<AllSegment, bool> arg_501_1;
								if ((arg_501_1 = F9__2) == null)
								{
									arg_501_1 = (F9__2 = ((AllSegment p) => p.BeginLandMakCode == InflectLand.LandmarkCode && p.EndLandMarkCode == NextLand.LandmarkCode));
								}
								AllSegment allSegment3 = arg_501_0.FirstOrDefault(arg_501_1);
								bool flag18 = allSegment3 != null && allSegment3.ExcuteAngle != -1;
								if (flag18)
								{
									InflectLand.Angle = allSegment3.ExcuteAngle;
								}
								else
								{
									bool flag19 = NextLand.LandY > InflectLand.LandY;
									if (flag19)
									{
										bool flag20 = dataTable.Rows.Count > 0;
										if (flag20)
										{
											DataRow dataRow5 = dataTable.Select("Direction=" + 0).FirstOrDefault<DataRow>();
											InflectLand.Angle = (int)Convert.ToInt16(dataRow5["Angle"]);
										}
										else
										{
											InflectLand.Angle = 90;
										}
									}
									else
									{
										bool flag21 = dataTable.Rows.Count > 0;
										if (flag21)
										{
											DataRow dataRow6 = dataTable.Select("Direction=" + 2).FirstOrDefault<DataRow>();
											InflectLand.Angle = (int)Convert.ToInt16(dataRow6["Angle"]);
										}
										else
										{
											InflectLand.Angle = 270;
										}
									}
								}
							}
							else
							{
								bool flag22 = Math.Round(InflectLand.LandY, 1, MidpointRounding.AwayFromZero) == Math.Round(NextLand.LandY, 1, MidpointRounding.AwayFromZero);
								if (flag22)
								{
									IEnumerable<AllSegment> arg_681_0 = list;
									Func<AllSegment, bool> arg_681_1;
									if ((arg_681_1 = F9__3) == null)
									{
										arg_681_1 = (F9__3 = ((AllSegment p) => p.BeginLandMakCode == InflectLand.LandmarkCode && p.EndLandMarkCode == NextLand.LandmarkCode));
									}
									AllSegment allSegment4 = arg_681_0.FirstOrDefault(arg_681_1);
									bool flag23 = allSegment4 != null && allSegment4.ExcuteAngle != -1;
									if (flag23)
									{
										InflectLand.Angle = allSegment4.ExcuteAngle;
									}
									else
									{
										bool flag24 = NextLand.LandX > InflectLand.LandX;
										if (flag24)
										{
											bool flag25 = dataTable.Rows.Count > 0;
											if (flag25)
											{
												DataRow dataRow7 = dataTable.Select("Direction=" + 1).FirstOrDefault<DataRow>();
												InflectLand.Angle = (int)Convert.ToInt16(dataRow7["Angle"]);
											}
											else
											{
												InflectLand.Angle = 0;
											}
										}
										else
										{
											bool flag26 = dataTable.Rows.Count > 0;
											if (flag26)
											{
												DataRow dataRow8 = dataTable.Select("Direction=" + 3).FirstOrDefault<DataRow>();
												InflectLand.Angle = (int)Convert.ToInt16(dataRow8["Angle"]);
											}
											else
											{
												InflectLand.Angle = 180;
											}
										}
									}
								}
							}
							bool flag27 = UpLand.Angle != InflectLand.Angle;
							if (flag27)
							{
								InflectLand.sway = SwayEnum.ExcuteLand;
							}
							num2 = i;
						}
					}
					else
					{
						InflectLand = Routes[0];
						NextLand = Routes[1];
						bool flag28 = Math.Round(InflectLand.LandX, 1, MidpointRounding.AwayFromZero) == Math.Round(NextLand.LandX, 1, MidpointRounding.AwayFromZero);
						if (flag28)
						{
							AllSegment allSegment5 = list.FirstOrDefault((AllSegment p) => p.BeginLandMakCode == InflectLand.LandmarkCode && p.EndLandMarkCode == NextLand.LandmarkCode);
							bool flag29 = allSegment5 != null && allSegment5.ExcuteAngle != -1;
							if (flag29)
							{
								InflectLand.Angle = allSegment5.ExcuteAngle;
							}
							else
							{
								bool flag30 = NextLand.LandY > InflectLand.LandY;
								if (flag30)
								{
									bool flag31 = dataTable.Rows.Count > 0;
									if (flag31)
									{
										DataRow dataRow9 = dataTable.Select("Direction=" + 0).FirstOrDefault<DataRow>();
										InflectLand.Angle = (int)Convert.ToInt16(dataRow9["Angle"]);
									}
									else
									{
										InflectLand.Angle = 90;
									}
								}
								else
								{
									bool flag32 = dataTable.Rows.Count > 0;
									if (flag32)
									{
										DataRow dataRow10 = dataTable.Select("Direction=" + 2).FirstOrDefault<DataRow>();
										InflectLand.Angle = (int)Convert.ToInt16(dataRow10["Angle"]);
									}
									else
									{
										InflectLand.Angle = 270;
									}
								}
							}
						}
						else
						{
							bool flag33 = Math.Round(InflectLand.LandY, 1, MidpointRounding.AwayFromZero) == Math.Round(NextLand.LandY, 1, MidpointRounding.AwayFromZero);
							if (flag33)
							{
								AllSegment allSegment6 = list.FirstOrDefault((AllSegment p) => p.BeginLandMakCode == InflectLand.LandmarkCode && p.EndLandMarkCode == NextLand.LandmarkCode);
								bool flag34 = allSegment6 != null && allSegment6.ExcuteAngle != -1;
								if (flag34)
								{
									InflectLand.Angle = allSegment6.ExcuteAngle;
								}
								else
								{
									bool flag35 = NextLand.LandX > InflectLand.LandX;
									if (flag35)
									{
										bool flag36 = dataTable.Rows.Count > 0;
										if (flag36)
										{
											DataRow dataRow11 = dataTable.Select("Direction=" + 1).FirstOrDefault<DataRow>();
											InflectLand.Angle = (int)Convert.ToInt16(dataRow11["Angle"]);
										}
										else
										{
											InflectLand.Angle = 0;
										}
									}
									else
									{
										bool flag37 = dataTable.Rows.Count > 0;
										if (flag37)
										{
											DataRow dataRow12 = dataTable.Select("Direction=" + 3).FirstOrDefault<DataRow>();
											InflectLand.Angle = (int)Convert.ToInt16(dataRow12["Angle"]);
										}
										else
										{
											InflectLand.Angle = 180;
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private int AcountInflect(List<LandmarkInfo> route)
		{
			int result;
			try
			{
				int num = 0;
				bool flag = route.Count >= 3;
				if (flag)
				{
					int num3;
					for (int i = 2; i < route.Count; i = num3 + 1)
					{
						LandmarkInfo landmarkInfo = route[i - 2];
						LandmarkInfo landmarkInfo2 = route[i - 1];
						LandmarkInfo landmarkInfo3 = route[i];
						double num2 = (Math.Round(landmarkInfo.LandX, 3) - Math.Round(landmarkInfo3.LandX, 3)) * (Math.Round(landmarkInfo2.LandY, 3) - Math.Round(landmarkInfo3.LandY, 3)) - (Math.Round(landmarkInfo.LandY, 3) - Math.Round(landmarkInfo3.LandY, 3)) * (Math.Round(landmarkInfo2.LandX, 3) - Math.Round(landmarkInfo3.LandX, 3));
						bool flag2 = num2 > 0.05 || num2 < -0.05;
						if (flag2)
						{
							num++;
						}
						num3 = i;
					}
				}
				result = num;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public List<string> GetRouteStr(List<LandmarkInfo> route)
		{
			List<string> result;
			try
			{
				List<LandmarkInfo> list = this.CreateDeepCopy<List<LandmarkInfo>>(route);
				List<LandmarkInfo> list2 = this.CreateDeepCopy<List<LandmarkInfo>>(route);
				using (List<LandmarkInfo>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LandmarkInfo item = enumerator.Current;
						bool flag = item.sway > SwayEnum.None;
						if (flag)
						{
							List<LandmarkInfo> list3 = (from a in this.AllLands
							where a.LandmarkCode != item.LandmarkCode
							orderby this.getDistant(a.LandX, a.LandY, item.LandX, item.LandY)
							select a).ToList<LandmarkInfo>();
							int num = list2.FindIndex((LandmarkInfo p) => p.LandmarkCode == item.LandmarkCode);
							int num2 = 1;
							int num3 = 1;
							using (List<LandmarkInfo>.Enumerator enumerator2 = list3.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									LandmarkInfo Copy = enumerator2.Current;
									bool flag2 = num2 == 5;
									if (flag2)
									{
										break;
									}
									bool flag3 = (from p in list2
									where p.LandmarkCode == Copy.LandmarkCode
									select p).Count<LandmarkInfo>() <= 0;
									int num4;
									if (flag3)
									{
										LandmarkInfo item2 = this.CreateDeepCopy<LandmarkInfo>(Copy);
										list2.Insert(num + num3, item2);
										num4 = num3;
										num3 = num4 + 1;
									}
									num4 = num2;
									num2 = num4 + 1;
								}
							}
						}
					}
				}
		
			    string text = string.Join(",", (from p in list2.Distinct<LandmarkInfo>()
			        select p.LandmarkCode));
				List<string> list4 = text.Split(new char[]
				{
					','
				}).ToList<string>();
				result = list4;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static double Angle(PointF cen, PointF first, PointF second)
		{
			double result;
			try
			{
				double num = (double)(first.X - cen.X);
				double num2 = (double)(first.Y - cen.Y);
				double num3 = (double)(second.X - cen.X);
				double num4 = (double)(second.Y - cen.Y);
				double num5 = num * num3 + num2 * num4;
				double num6 = Math.Sqrt(num * num + num2 * num2);
				double num7 = Math.Sqrt(num3 * num3 + num4 * num4);
				double d = num5 / (num6 * num7);
				double value = Math.Acos(d) * 180.0 / 3.1415926535897;
				result = Math.Round(value, 1, MidpointRounding.AwayFromZero);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
	}
}
