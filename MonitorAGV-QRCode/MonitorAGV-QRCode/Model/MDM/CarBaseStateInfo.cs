using System;
using System.Collections.Generic;

namespace Model.MDM
{
	[Serializable]
	public class CarBaseStateInfo
	{
		public int AgvID
		{
			get;
			set;
		}

		public int IsUpLand
		{
			get;
			set;
		}

		public string IsUpLandStr
		{
			get
			{
				return (this.IsUpLand == 1) ? "在" : "不在";
			}
		}

		public int CarType
		{
			get;
			set;
		}

		public string CarTypeStr
		{
			get
			{
				string result;
				switch (this.CarType)
				{
				case 0:
					result = "单向";
					break;
				case 1:
					result = "双向";
					break;
				case 2:
					result = "全向";
					break;
				default:
					result = "单向";
					break;
				}
				return result;
			}
			set
			{
				if (!(value == "单向"))
				{
					if (!(value == "双向"))
					{
						if (!(value == "全向"))
						{
							this.CarType = 0;
						}
						else
						{
							this.CarType = 2;
						}
					}
					else
					{
						this.CarType = 1;
					}
				}
				else
				{
					this.CarType = 0;
				}
			}
		}

		public string CarIP
		{
			get;
			set;
		}

		public string CarPort
		{
			get;
			set;
		}

		public double X
		{
			get;
			set;
		}

		public double Y
		{
			get;
			set;
		}

		public double fVolt
		{
			get;
			set;
		}

		public int speed
		{
			get;
			set;
		}

		public bool bIsCommBreak
		{
			get;
			set;
		}

		public bool BackModel
		{
			get;
			set;
		}

		public bool ForwordModel
		{
			get;
			set;
		}

		public int PBSValue
		{
			get;
			set;
		}

		public bool RightModel
		{
			get;
			set;
		}

		public bool LeftModel
		{
			get;
			set;
		}

		public int CurrSite
		{
			get;
			set;
		}

		public int CarState
		{
			get;
			set;
		}

		public string CarStateStr
		{
			get
			{
				bool flag = this.CarState == 0;
				string result;
				if (flag)
				{
					result = "待命";
				}
				else
				{
					bool flag2 = this.CarState == 1;
					if (flag2)
					{
						result = "任务执行中";
					}
					else
					{
						bool flag3 = this.CarState == 2;
						if (flag3)
						{
							result = "任务完成";
						}
						else
						{
							bool flag4 = this.CarState == 3;
							if (flag4)
							{
								result = "任务暂停";
							}
							else
							{
								bool flag5 = this.CarState == 4;
								if (flag5)
								{
									result = "到达目标点";
								}
								else
								{
									bool flag6 = this.CarState == 5;
									if (flag6)
									{
										result = "请求重新规划路径";
									}
									else
									{
										bool flag7 = this.CarState == 6;
										if (flag7)
										{
											result = "机械防撞触发";
										}
										else
										{
											bool flag8 = this.CarState == 7;
											if (flag8)
											{
												result = "光电避障传感器触发";
											}
											else
											{
												bool flag9 = this.CarState == 8;
												if (flag9)
												{
													result = "电量不足";
												}
												else
												{
													bool flag10 = this.CarState == 255;
													if (flag10)
													{
														result = "未准备";
													}
													else
													{
														result = "";
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				return result;
			}
		}

		public int Angel
		{
			get;
			set;
		}

		public bool OffLineP
		{
			get;
			set;
		}

		public bool LowPower
		{
			get;
			set;
		}

		public bool NearObstacle
		{
			get;
			set;
		}

		public bool FindObstacle
		{
			get;
			set;
		}

		public bool PhotoelectricitError
		{
			get;
			set;
		}

		public bool bIsHookonTDC
		{
			get;
			set;
		}

		public bool bIsHookonBDC
		{
			get;
			set;
		}

		public int CurrRoute
		{
			get;
			set;
		}

		public int TaskCon
		{
			get;
			set;
		}

		public LogicEnum CurrLogic
		{
			get;
			set;
		}

		public LogicEnum CurrLogicSys
		{
			get;
			set;
		}

		public int CurrLogicState
		{
			get;
			set;
		}

		public int CurrActionSite
		{
			get;
			set;
		}

		public int WarnType
		{
			get;
			set;
		}

		public string WarnTypeStr
		{
			get
			{
				string result;
				switch (this.WarnType)
				{
				case 0:
					result = "24C02错误";
					return result;
				case 1:
					result = "电量检测异常";
					return result;
				case 2:
					result = "WiFi通信异常";
					return result;
				case 3:
					result = "433M通信异常";
					return result;
				case 5:
					result = "PGV100异常";
					return result;
				case 6:
					result = "二维码导航异常";
					return result;
				case 7:
					result = "里程计异常";
					return result;
				case 8:
					result = "电机异常";
					return result;
				case 10:
					result = "安全传感器异常";
					return result;
				}
				result = "";
				return result;
			}
		}

		public string WarnBinaryCode
		{
			get;
			set;
		}

		public List<LandmarkInfo> Route
		{
			get;
			set;
		}

		public LandmarkInfo NextLand
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

		public CarInfo LockCar
		{
			get;
			set;
		}

		public bool IsLock
		{
			get;
			set;
		}

		public string ErrorMessage
		{
			get;
			set;
		}

		public string ArmLand
		{
			get;
			set;
		}

		public int PutType
		{
			get;
			set;
		}

		public int OperType
		{
			get;
			set;
		}

		public bool IsBack
		{
			get;
			set;
		}

		public int LockCarID
		{
			get;
			set;
		}

		public CarBaseStateInfo()
		{
			this.CarIP = "";
			this.CarPort = "";
			this.X = 0.0;
			this.Y = 0.0;
			this.IsUpLand = 1;
			this.LockCarID = -1;
			this.ErrorMessage = "";
			this.ArmLand = "";
			this.IsBack = false;
			this.NextLand = new LandmarkInfo();
			this.Route = new List<LandmarkInfo>();
			this.RouteLands = new List<string>();
			this.TurnLands = new List<string>();
			this.CurrLogic = LogicEnum.None;
		}

		public bool IsChange(CarBaseStateInfo car)
		{
			return this.CurrSite != car.CurrSite || this.ErrorMessage != car.ErrorMessage || this.bIsCommBreak != car.bIsCommBreak;
		}

		public void GetValue(CarBaseStateInfo car)
		{
			this.speed = car.speed;
			this.Angel = car.Angel;
			this.fVolt = car.fVolt;
			this.PBSValue = car.PBSValue;
			this.CurrSite = car.CurrSite;
			this.CarState = car.CarState;
			this.OffLineP = car.OffLineP;
			this.NearObstacle = car.NearObstacle;
			this.FindObstacle = car.FindObstacle;
			this.PhotoelectricitError = car.PhotoelectricitError;
			this.bIsHookonTDC = car.bIsHookonTDC;
			this.bIsHookonBDC = car.bIsHookonBDC;
			this.ForwordModel = car.ForwordModel;
			this.BackModel = car.BackModel;
			this.bIsCommBreak = car.bIsCommBreak;
			this.CurrRoute = car.CurrRoute;
			this.TaskCon = car.TaskCon;
			this.RightModel = car.RightModel;
			this.LeftModel = car.LeftModel;
			this.CurrLogic = car.CurrLogic;
			this.CurrLogicState = car.CurrLogicState;
			this.CurrActionSite = car.CurrActionSite;
			this.ErrorMessage = car.ErrorMessage;
			this.IsUpLand = car.IsUpLand;
			bool flag = this.Route != null && this.Route.Count > 0;
			if (flag)
			{
				int num = this.Route.FindIndex((LandmarkInfo p) => p.LandmarkCode == this.CurrSite.ToString());
				bool flag2 = num >= 0 && num + 1 <= this.Route.Count - 1;
				if (flag2)
				{
					this.NextLand = this.Route[num + 1];
				}
			}
		}
	}
}
