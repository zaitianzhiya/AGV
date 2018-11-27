using System.Data;
using Model.MDM;
using Model.MSM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;

namespace SimulationModel
{
	public class CarMonitor
	{
		public delegate void CarStepChange(object sender);

		public System.Timers.Timer CarActive_Timer;

		public static object locker = new object();

		[method: CompilerGenerated]
		public event CarStepChange StepChange;

		public bool IsLock
		{
			get;
			set;
		}

		public int AgvID
		{
			get;
			set;
		}

		public LandmarkInfo CurrLand
		{
			get;
			set;
		}

		public int CurrSite
		{
			get;
			set;
		}

		public int OldSite
		{
			get;
			set;
		}

		public float X
		{
			get;
			set;
		}

		public float Y
		{
			get;
			set;
		}

		public LandmarkInfo NextLand
		{
			get;
			set;
		}

		public int Sate
		{
			get;
			set;
		}

		public List<LandmarkInfo> CurrRoute
		{
			get;
			set;
		}

		public List<string> RouteLands
		{
			get;
			set;
		}

		public List<string> TurnLands
		{
			get;
			set;
		}

		public string StandbyLandMark
		{
			get;
			set;
		}

		public string ExcuteTaksNo
		{
			get;
			set;
		}

		public bool IsBack
		{
			get;
			set;
		}

		public int TaskDetailID
		{
			get;
			set;
		}

		public string ArmLand
		{
			get;
			set;
		}

		public int OperType
		{
			get;
			set;
		}

		public int PutType
		{
			get;
			set;
		}

		public CarMonitor()
		{
			IsLock = false;
			Sate = 0;
			CurrLand = new LandmarkInfo();
			NextLand = new LandmarkInfo();
			CurrRoute = new List<LandmarkInfo>();
			RouteLands = new List<string>();
			TurnLands = new List<string>();
			StandbyLandMark = "";
			ExcuteTaksNo = "";
			ArmLand = "";
			CurrSite = -1;
			CarActive_Timer = new System.Timers.Timer
			{
				Enabled = false
			};
			CarActive_Timer.Elapsed += new ElapsedEventHandler(CarActive);
			CarActive_Timer.Interval = 100.0;
		}

		public void Move()
		{
			try
			{
				object obj = locker;
				lock (obj)
				{
					CarActive_Timer.Enabled = false;
					if (CurrRoute.Any())
					{
						double ScalingRate = 0.0;
                        DataTable dtPara = Function.GetParByCondition("ScalingRate");
						if (dtPara != null && dtPara.Rows.Count>0)
						{
							try
							{
                                ScalingRate = Convert.ToDouble(dtPara.Rows[0]["ParameterValue"]);
							}
							catch
							{
								return;
							}
						}
                        if (ScalingRate > 0.0)
						{
							CurrRoute = CurrRoute.Distinct().ToList();
							if ( Math.Abs(X - CurrRoute.Last().LandX * ScalingRate) <= 0.2 && Math.Abs(Y - CurrRoute.Last().LandY * ScalingRate) <= 0.2)
							{
								CurrLand = CurrRoute.Last();
								X = (float)(CurrLand.LandX * ScalingRate);
								Y = (float)(CurrLand.LandY * ScalingRate);
								NextLand = null;
								CurrSite = Convert.ToInt16(CurrLand.LandmarkCode);
								CurrRoute.Clear();
								Sate = 0;
								CarActive_Timer.Enabled = false;
								if (StepChange != null)
								{
									StepChange(this);
								}
							}
							else
							{
								if (Sate == 0)
								{
									CurrLand = CurrRoute[0];
									X = (float)(CurrLand.LandX * ScalingRate);
									Y = (float)(CurrLand.LandY * ScalingRate);
									OldSite = CurrSite;
									CurrSite = Convert.ToInt16(CurrLand.LandmarkCode);
									int num = CurrRoute.FindIndex(p => p.LandmarkCode == CurrLand.LandmarkCode);
									if ( num + 1 < CurrRoute.Count)
									{
										NextLand = CurrRoute[num + 1];
									}
									Sate = 1;
								}
								else
								{
									LandmarkInfo land = CurrRoute.FirstOrDefault( p => Math.Abs(p.LandX * ScalingRate - X) <= 0.2 && Math.Abs(p.LandY * ScalingRate - Y) <= 0.2);
									if (land != null && NextLand != null && land.LandmarkCode == NextLand.LandmarkCode)
									{
										X = (float)(land.LandX * ScalingRate);
										Y = (float)(land.LandY * ScalingRate);
										int num2 = CurrRoute.FindIndex(p => p.LandmarkCode == land.LandmarkCode);
										CurrLand = CurrRoute[num2];
										OldSite = CurrSite;
										CurrSite = Convert.ToInt16(CurrLand.LandmarkCode);
										if (num2 + 1 < CurrRoute.Count)
										{
											NextLand = CurrRoute[num2 + 1];
										}
										Sate = 1;
									}
									else
									{
										if ( CurrLand != null && NextLand != null)
										{
											if (Math.Abs(CurrLand.LandX - NextLand.LandX) <= 0.3)
											{
												if (NextLand.LandY > CurrLand.LandY)
												{
													Y += 0.05f;
												}
												else
												{
													Y -= 0.05f;
												}
											}
											else
											{
												if (NextLand.LandX > CurrLand.LandX)
												{
													X += 0.05f;
												}
												else
												{
													X -= 0.05f;
												}
											}
											Sate = 1;
										}
									}
								}
								if (StepChange != null)
								{
									StepChange(this);
								}
							}
						}
					}
					else
					{
						Sate = 0;
						CarActive_Timer.Enabled = false;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				CarActive_Timer.Enabled = true;
			}
		}

		public void Run()
		{
			try
			{
				new Thread(new ThreadStart(this.Move))
				{
					IsBackground = true
				}.Start();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Stop()
		{
			CarActive_Timer.Enabled = false;
		}

		public void Dispose()
		{
			CarActive_Timer.Enabled = false;
		}

		public void Start()
		{
			try
			{
				CarActive_Timer.Enabled = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void CarActive(object sender, ElapsedEventArgs e)
		{
			try
			{
				CarActive_Timer.Enabled = false;
				Run();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				CarActive_Timer.Enabled = true;
			}
		}
	}
}
