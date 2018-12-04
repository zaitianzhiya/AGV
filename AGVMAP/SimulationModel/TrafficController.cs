using Model.MDM;
using Model.MSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Tools;

namespace SimulationModel
{
	public class TrafficController
	{
		public static readonly object lockStopObj = new object();

		public static readonly object lockStartObj = new object();

		private List<LockResource> lockResource = new List<LockResource>();

		private IList<CarMonitor> CarList = new List<CarMonitor>();

		private IList<AllSegment> AllSeg = new List<AllSegment>();

		private IDictionary<string, string> SysParameter = new Dictionary<string, string>();

		private IList<LandmarkInfo> AllLands = new List<LandmarkInfo>();

		private bool IsHandleBeforStart = false;

		public TrafficController(IList<CarMonitor> cars, IList<AllSegment> allSeg, IDictionary<string, string> sysParameter, IList<LandmarkInfo> allLands)
		{
			this.CarList = cars;
			this.AllSeg = allSeg;
			this.SysParameter = sysParameter;
			this.AllLands = allLands;
		}

		private bool CheckIsStop(CarMonitor CurrCar)
		{
            int agvID;
            bool flag;
            try
            {
                CarMonitor carMonitor = (
                    from a in this.CarList
                    where (a.AgvID == CurrCar.AgvID ? false : (
                        from q in CurrCar.RouteLands
                        where q == a.CurrSite.ToString()
                        select q).Count<string>() > 0)
                    select a).FirstOrDefault<CarMonitor>();
                CarMonitor carMonitor1 = (
                    from a in this.CarList
                    where (a.AgvID == CurrCar.AgvID ? false : CurrCar.RouteLands.Intersect<string>(a.RouteLands).Count<string>() > 0)
                    orderby CurrCar.RouteLands.Intersect<string>(a.RouteLands).Count<string>() descending
                    select a).FirstOrDefault<CarMonitor>();
                CarMonitor carMonitor2 = (
                    from a in this.CarList
                    where (a.AgvID == CurrCar.AgvID ? false :CurrCar.RouteLands.Intersect<string>(a.TurnLands).Count<string>() > 0)
                    select a).FirstOrDefault<CarMonitor>();
                CarMonitor carMonitor3 = (
                    from a in this.CarList
                    where (a.AgvID == CurrCar.AgvID ? false : CurrCar.TurnLands.Contains(a.CurrSite.ToString()))
                    select a).FirstOrDefault<CarMonitor>();
                if (carMonitor == null)
                {
                    if (carMonitor2 != null)
                    {
                        if (carMonitor2.TurnLands.Contains(carMonitor2.CurrSite.ToString()))
                        {
                            if ((
                                from p in this.lockResource
                                where (p.LockCarID != carMonitor2.AgvID ? false : p.BeLockCarID == CurrCar.AgvID)
                                select p).Count<LockResource>() <= 0)
                            {
                                LockResource lockResource = new LockResource()
                                {
                                    BeLockCarID = CurrCar.AgvID,
                                    LockCarID = carMonitor2.AgvID
                                };
                                this.lockResource.Add(lockResource);
                            }
                            string[] str = new string[] { "当前车:", null, null, null, null, null, null, null };
                            agvID = CurrCar.AgvID;
                            str[1] = agvID.ToString();
                            str[2] = "在站点";
                            agvID = CurrCar.CurrSite;
                            str[3] = agvID.ToString();
                            str[4] = "停,因为当前车前方有即将旋转车且在他自己的旋转区域内";
                            agvID = carMonitor2.AgvID;
                            str[5] = agvID.ToString();
                            str[6] = ",目前在站点";
                            agvID = carMonitor2.CurrSite;
                            str[7] = agvID.ToString();
                            LogHelper.WriteTrafficLog(string.Concat(str));
                            flag = true;
                            return flag;
                        }
                        else if (carMonitor2.TurnLands.Contains(carMonitor2.CurrSite.ToString()))
                        {
                            if ((
                                from p in this.lockResource
                                where (p.LockCarID != carMonitor2.AgvID ? false : p.BeLockCarID == CurrCar.AgvID)
                                select p).Count<LockResource>() <= 0)
                            {
                                LockResource lockResource1 = new LockResource()
                                {
                                    BeLockCarID = CurrCar.AgvID,
                                    LockCarID = carMonitor2.AgvID
                                };
                                this.lockResource.Add(lockResource1);
                            }
                            string[] strArrays = new string[] { "当前车:", null, null, null, null, null, null, null };
                            agvID = CurrCar.AgvID;
                            strArrays[1] = agvID.ToString();
                            strArrays[2] = "在站点";
                            agvID = CurrCar.CurrSite;
                            strArrays[3] = agvID.ToString();
                            strArrays[4] = "停,因为当前车前方有即将旋转车";
                            agvID = carMonitor2.AgvID;
                            strArrays[5] = agvID.ToString();
                            strArrays[6] = ",目前在站点";
                            agvID = carMonitor2.CurrSite;
                            strArrays[7] = agvID.ToString();
                            LogHelper.WriteTrafficLog(string.Concat(strArrays));
                            flag = true;
                            return flag;
                        }
                        else
                        {
                            if ((
                                from p in this.lockResource
                                where (p.LockCarID != CurrCar.AgvID ? false : p.BeLockCarID == carMonitor2.AgvID)
                                select p).Count<LockResource>() <= 0)
                            {
                                LockResource lockResource2 = new LockResource()
                                {
                                    BeLockCarID = carMonitor2.AgvID,
                                    LockCarID = CurrCar.AgvID
                                };
                                this.lockResource.Add(lockResource2);
                            }
                            string[] str1 = new string[] { "当前车:", null, null, null, null, null, null, null, null };
                            agvID = carMonitor2.AgvID;
                            str1[1] = agvID.ToString();
                            str1[2] = "在站点";
                            agvID = carMonitor2.CurrSite;
                            str1[3] = agvID.ToString();
                            str1[4] = "停,因为当前车即将旋转车且不在自己的旋转区域内,但有车";
                            agvID = CurrCar.AgvID;
                            str1[5] = agvID.ToString();
                            str1[6] = "站点";
                            agvID = CurrCar.CurrSite;
                            str1[7] = agvID.ToString();
                            str1[8] = "在当前车的旋转区域内";
                            LogHelper.WriteTrafficLog(string.Concat(str1));
                            carMonitor2.Stop();
                            carMonitor2.IsLock = true;
                        }
                    }
                    if (carMonitor3 == null)
                    {
                        if (carMonitor1 != null)
                        {
                            List<string> list = CurrCar.RouteLands.Intersect<string>(carMonitor1.RouteLands).ToList<string>();
                            List<string> strs = carMonitor1.RouteLands.Intersect<string>(CurrCar.RouteLands).ToList<string>();
                            if ((list.Count <= 0 ? false : strs.Count > 0))
                            {
                                int num = CurrCar.RouteLands.FindIndex((string q) => q == list[0]);
                                if (carMonitor1.RouteLands.FindIndex((string q) => q == strs[0]) < num)
                                {
                                    if ((
                                        from p in this.lockResource
                                        where (p.LockCarID != carMonitor1.AgvID ? false : p.BeLockCarID == CurrCar.AgvID)
                                        select p).Count<LockResource>() <= 0)
                                    {
                                        LockResource lockResource3 = new LockResource()
                                        {
                                            BeLockCarID = CurrCar.AgvID,
                                            LockCarID = carMonitor1.AgvID
                                        };
                                        this.lockResource.Add(lockResource3);
                                    }
                                    string[] strArrays1 = new string[] { "交叉路线汇车,远的车", null, null, null, null, null, null, null, null };
                                    agvID = CurrCar.AgvID;
                                    strArrays1[1] = agvID.ToString();
                                    strArrays1[2] = "在站点";
                                    agvID = CurrCar.CurrSite;
                                    strArrays1[3] = agvID.ToString();
                                    strArrays1[4] = "停,车";
                                    agvID = carMonitor1.AgvID;
                                    strArrays1[5] = agvID.ToString();
                                    strArrays1[6] = "在站点";
                                    agvID = carMonitor1.CurrSite;
                                    strArrays1[7] = agvID.ToString();
                                    strArrays1[8] = "走";
                                    LogHelper.WriteTrafficLog(string.Concat(strArrays1));
                                    flag = true;
                                    return flag;
                                }
                                else if ((
                                    from p in this.lockResource
                                    where (p.LockCarID != CurrCar.AgvID ? false : p.BeLockCarID == carMonitor1.AgvID)
                                    select p).Count<LockResource>() <= 0)
                                {
                                    LockResource lockResource4 = new LockResource()
                                    {
                                        BeLockCarID = carMonitor1.AgvID,
                                        LockCarID = CurrCar.AgvID
                                    };
                                    this.lockResource.Add(lockResource4);
                                    carMonitor1.Stop();
                                    carMonitor1.IsLock = true;
                                    string[] str2 = new string[] { "交叉路线汇车,远的车:", null, null, null, null, null, null, null, null };
                                    agvID = carMonitor1.AgvID;
                                    str2[1] = agvID.ToString();
                                    str2[2] = "在站点";
                                    agvID = carMonitor1.CurrSite;
                                    str2[3] = agvID.ToString();
                                    str2[4] = "停,车";
                                    agvID = CurrCar.AgvID;
                                    str2[5] = agvID.ToString();
                                    str2[6] = "在站点";
                                    agvID = CurrCar.CurrSite;
                                    str2[7] = agvID.ToString();
                                    str2[8] = "走";
                                    LogHelper.WriteTrafficLog(string.Concat(str2));
                                }
                            }
                        }
                        flag = false;
                    }
                    else
                    {
                        if ((
                            from p in this.lockResource
                            where (p.LockCarID != carMonitor3.AgvID ? false : p.BeLockCarID == CurrCar.AgvID)
                            select p).Count<LockResource>() <= 0)
                        {
                            LockResource lockResource5 = new LockResource()
                            {
                                BeLockCarID = CurrCar.AgvID,
                                LockCarID = carMonitor3.AgvID
                            };
                            this.lockResource.Add(lockResource5);
                        }
                        string[] strArrays2 = new string[] { "车:", null, null, null, null, null, null, null, null };
                        agvID = CurrCar.AgvID;
                        strArrays2[1] = agvID.ToString();
                        strArrays2[2] = "当前旋转区域内有其他车";
                        agvID = carMonitor3.AgvID;
                        strArrays2[3] = agvID.ToString();
                        strArrays2[4] = "在站点";
                        agvID = carMonitor3.CurrSite;
                        strArrays2[5] = agvID.ToString();
                        strArrays2[6] = ",所以当前车在站点";
                        agvID = CurrCar.CurrSite;
                        strArrays2[7] = agvID.ToString();
                        strArrays2[8] = "停";
                        LogHelper.WriteTrafficLog(string.Concat(strArrays2));
                        flag = true;
                    }
                }
                else
                {
                    if ((
                        from p in this.lockResource
                        where (p.LockCarID != carMonitor.AgvID ? false : p.BeLockCarID == CurrCar.AgvID)
                        select p).Count<LockResource>() <= 0)
                    {
                        LockResource lockResource6 = new LockResource()
                        {
                            BeLockCarID = CurrCar.AgvID,
                            LockCarID = carMonitor.AgvID
                        };
                        this.lockResource.Add(lockResource6);
                    }
                    string[] str3 = new string[] { "跟车:", null, null, null, null, null, null, null, null };
                    agvID = CurrCar.AgvID;
                    str3[1] = agvID.ToString();
                    str3[2] = "在站点";
                    agvID = CurrCar.CurrSite;
                    str3[3] = agvID.ToString();
                    str3[4] = "停,因车";
                    agvID = carMonitor.AgvID;
                    str3[5] = agvID.ToString();
                    str3[6] = "在站点";
                    agvID = carMonitor.CurrSite;
                    str3[7] = agvID.ToString();
                    str3[8] = "阻挡";
                    LogHelper.WriteTrafficLog(string.Concat(str3));
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteTrafficLog(string.Concat("交通管制异常:", exception.Message));
                if ((
                    from p in this.lockResource
                    where p.BeLockCarID == CurrCar.AgvID
                    select p).Count<LockResource>() <= 0)
                {
                    LockResource lockResource7 = new LockResource()
                    {
                        BeLockCarID = CurrCar.AgvID
                    };
                    this.lockResource.Add(lockResource7);
                }
                flag = true;
            }
            return flag;
		}

		public void HandleTrafficForStop(object obj)
		{
			try
			{
				object obj2 = TrafficController.lockStopObj;
				lock (obj2)
				{
					bool flag2 = obj == null;
					if (!flag2)
					{
						CarMonitor carMonitor = obj as CarMonitor;
						bool flag3 = carMonitor == null;
						if (!flag3)
						{
							this.GetTrafficResour(carMonitor);
                            LogHelper.WriteTrafficLog(string.Concat("判断交管停止前,车", carMonitor.AgvID.ToString(), "行走资源集合:", string.Join(",",
                                  from p in carMonitor.RouteLands
                                  select p)));
                            LogHelper.WriteTrafficLog(string.Concat("判断交管停止前,车", carMonitor.AgvID.ToString(), "旋转资源集合:", string.Join(",",
                                from p in carMonitor.TurnLands
                                select p)));
							if (carMonitor.Sate == 1)
							{
								if (CheckIsStop(carMonitor))
								{
									carMonitor.Stop();
									carMonitor.IsLock = true;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogHelper.WriteTrafficLog("交通管制停止异常:" + ex.Message);
			}
		}

		public void HandleTrafficForStart()
		{
            int currSite;
            bool str;
            bool flag;
            bool str1;
            try
            {
                lock (lockStartObj)
                {
                    foreach (LockResource lockResource in DataToObject.CreateDeepCopy<List<LockResource>>(this.lockResource))
                    {
                        CarMonitor carMonitor = this.CarList.FirstOrDefault<CarMonitor>((CarMonitor p) => p.AgvID == lockResource.BeLockCarID);
                        if (carMonitor != null)
                        {
                            int num = carMonitor.CurrRoute.FindIndex((LandmarkInfo p) => p.LandmarkCode == carMonitor.CurrSite.ToString());
                            if (num >= 0)
                            {
                                double length = 0;
                                double length1 = 0;
                                double num1 = 2;
                                carMonitor.RouteLands.Clear();
                                List<string> strs = new List<string>();
                                for (int i = num + 1; i < carMonitor.CurrRoute.Count; i++)
                                {
                                    AllSegment allSegment = (
                                        from p in this.AllSeg
                                        where (p.BeginLandMakCode != carMonitor.CurrRoute[i - 1].LandmarkCode ? false : p.EndLandMarkCode == carMonitor.CurrRoute[i].LandmarkCode)
                                        select p).FirstOrDefault<AllSegment>();
                                    if (allSegment != null)
                                    {
                                        length += allSegment.Length;
                                        if ((length1 > num1 ? true : length < num1))
                                        {
                                            if ((
                                                from p in strs
                                                where p == carMonitor.CurrRoute[i - 1].LandmarkCode
                                                select p).Count<string>() > 0)
                                            {
                                                str = false;
                                            }
                                            else
                                            {
                                                string landmarkCode = carMonitor.CurrRoute[i - 1].LandmarkCode;
                                                currSite = carMonitor.CurrSite;
                                                str = landmarkCode != currSite.ToString();
                                            }
                                            if (str)
                                            {
                                                strs.Add(carMonitor.CurrRoute[i - 1].LandmarkCode);
                                                carMonitor.RouteLands.Add(carMonitor.CurrRoute[i - 1].LandmarkCode);
                                            }
                                            if ((
                                                from p in strs
                                                where p == carMonitor.CurrRoute[i].LandmarkCode
                                                select p).Count<string>() > 0)
                                            {
                                                flag = false;
                                            }
                                            else
                                            {
                                                string landmarkCode1 = carMonitor.CurrRoute[i].LandmarkCode;
                                                currSite = carMonitor.CurrSite;
                                                flag = landmarkCode1 != currSite.ToString();
                                            }
                                            if (flag)
                                            {
                                                strs.Add(carMonitor.CurrRoute[i].LandmarkCode);
                                                carMonitor.RouteLands.Add(carMonitor.CurrRoute[i].LandmarkCode);
                                            }
                                            length1 += allSegment.Length;
                                        }
                                        else
                                        {
                                            if ((
                                                from p in strs
                                                where p == carMonitor.CurrRoute[i].LandmarkCode
                                                select p).Count<string>() > 0)
                                            {
                                                str1 = false;
                                            }
                                            else
                                            {
                                                string landmarkCode2 = carMonitor.CurrRoute[i].LandmarkCode;
                                                currSite = carMonitor.CurrSite;
                                                str1 = landmarkCode2 != currSite.ToString();
                                            }
                                            if (str1)
                                            {
                                                strs.Add(carMonitor.CurrRoute[i].LandmarkCode);
                                                carMonitor.RouteLands.Add(carMonitor.CurrRoute[i].LandmarkCode);
                                            }
                                            break;
                                        }
                                    }
                                }
                                if (!carMonitor.RouteLands.Contains(carMonitor.CurrSite.ToString()))
                                {
                                    List<string> routeLands = carMonitor.RouteLands;
                                    currSite = carMonitor.CurrSite;
                                    routeLands.Insert(0, currSite.ToString());
                                }
                                LogHelper.WriteTrafficLog(string.Concat("被交管停止的车", carMonitor.AgvID.ToString(), "前进资源集合:", string.Join(",",
                                    from p in carMonitor.RouteLands
                                    select p)));
                                carMonitor.TurnLands.Clear();
                                LandmarkInfo landmarkInfo = null;
                                if (carMonitor.RouteLands.Count > 0)
                                {
                                    landmarkInfo = (
                                        from a in carMonitor.CurrRoute
                                        where (!a.IsRotateLand ? false : carMonitor.RouteLands.Contains(a.LandmarkCode))
                                        select a).FirstOrDefault<LandmarkInfo>();
                                }
                                if ((landmarkInfo == null || !landmarkInfo.IsRotateLand || !this.SysParameter.Keys.Contains("TurnLenth") ? false : this.SysParameter.Keys.Contains("ScalingRate")))
                                {
                                    string str2 = this.SysParameter["TurnLenth"].ToString();
                                    string str3 = this.SysParameter["ScalingRate"].ToString();
                                    if ((string.IsNullOrEmpty(str2) ? false : !string.IsNullOrEmpty(str3)))
                                    {
                                        double num2 = Convert.ToDouble(str2);
                                        double num3 = Convert.ToDouble(str3);
                                        List<LandmarkInfo> list = (
                                            from a in this.AllLands
                                            where Math.Round(this.getDistant(a.LandX, a.LandY, landmarkInfo.LandX, landmarkInfo.LandY) * num3, 3) <= Math.Round(num2, 3)
                                            select a).ToList<LandmarkInfo>();
                                        foreach (LandmarkInfo landmarkInfo1 in list)
                                        {
                                            if ((
                                                from p in carMonitor.TurnLands
                                                where p == landmarkInfo1.LandmarkCode
                                                select p).Count<string>() <= 0)
                                            {
                                                carMonitor.TurnLands.Add(landmarkInfo1.LandmarkCode);
                                            }
                                        }
                                    }
                                }
                                if (!this.SameHandleTrafficForStop(carMonitor))
                                {
                                    this.GetTrafficResour(carMonitor);
                                    List<LockResource> lockResources = (
                                        from p in this.lockResource
                                        where p.BeLockCarID == carMonitor.AgvID
                                        select p).ToList<LockResource>();
                                    foreach (LockResource lockResource1 in DataToObject.CreateDeepCopy<List<LockResource>>(lockResources))
                                    {
                                        int num4 = this.lockResource.FindIndex((LockResource p) => p.BeLockCarID == lockResource1.BeLockCarID);
                                        this.lockResource.RemoveAt(num4);
                                    }
                                    carMonitor.Start();
                                    carMonitor.IsLock = false;
                                    string[] strArrays = new string[] { "被管制停的车:", null, null, null, null };
                                    currSite = carMonitor.AgvID;
                                    strArrays[1] = currSite.ToString();
                                    strArrays[2] = "在站点";
                                    currSite = carMonitor.CurrSite;
                                    strArrays[3] = currSite.ToString();
                                    strArrays[4] = "启动";
                                    LogHelper.WriteTrafficLog(string.Concat(strArrays));
                                }
                                else if (carMonitor.CurrRoute.FindIndex((LandmarkInfo p) => p.LandmarkCode == carMonitor.CurrSite.ToString()) + 1 < carMonitor.CurrRoute.Count)
                                {
                                    LandmarkInfo landmarkInfo2 = carMonitor.CurrRoute.FirstOrDefault<LandmarkInfo>((LandmarkInfo p) => p.LandmarkCode == carMonitor.CurrSite.ToString());
                                    if ((landmarkInfo2 == null || !landmarkInfo2.IsRotateLand ? false : this.CarList.FirstOrDefault<CarMonitor>((CarMonitor p) => (p.AgvID == carMonitor.AgvID ? false : carMonitor.RouteLands.Contains(p.CurrSite.ToString()))) == null))
                                    {
                                        List<LockResource> list1 = (
                                            from p in this.lockResource
                                            where p.BeLockCarID == carMonitor.AgvID
                                            select p).ToList<LockResource>();
                                        foreach (LockResource lockResource2 in DataToObject.CreateDeepCopy<List<LockResource>>(list1))
                                        {
                                            int num5 = this.lockResource.FindIndex((LockResource p) => p.BeLockCarID == lockResource2.BeLockCarID);
                                            this.lockResource.RemoveAt(num5);
                                        }
                                        carMonitor.Start();
                                        carMonitor.IsLock = false;
                                        string[] strArrays1 = new string[] { "被管制的车停在了自己的旋转点上:", null, null, null, null };
                                        currSite = carMonitor.AgvID;
                                        strArrays1[1] = currSite.ToString();
                                        strArrays1[2] = ",在地标";
                                        currSite = carMonitor.CurrSite;
                                        strArrays1[3] = currSite.ToString();
                                        strArrays1[4] = "上,往前挪一格";
                                        LogHelper.WriteTrafficLog(string.Concat(strArrays1));
                                    }
                                    else if ((landmarkInfo2 == null || this.CarList.FirstOrDefault<CarMonitor>((CarMonitor q) => (q.AgvID == carMonitor.AgvID || !q.TurnLands.Contains(carMonitor.CurrSite.ToString()) ? false : !this.AllLands.FirstOrDefault<LandmarkInfo>((LandmarkInfo p) => p.LandmarkCode == q.CurrSite.ToString()).IsRotateLand)) == null ? false : this.CarList.FirstOrDefault<CarMonitor>((CarMonitor p) => (p.AgvID == carMonitor.AgvID ? false : carMonitor.RouteLands.Contains(p.CurrSite.ToString()))) == null))
                                    {
                                        List<LockResource> lockResources1 = (
                                            from p in this.lockResource
                                            where p.BeLockCarID == carMonitor.AgvID
                                            select p).ToList<LockResource>();
                                        foreach (LockResource lockResource3 in DataToObject.CreateDeepCopy<List<LockResource>>(lockResources1))
                                        {
                                            int num6 = this.lockResource.FindIndex((LockResource p) => p.BeLockCarID == lockResource3.BeLockCarID);
                                            this.lockResource.RemoveAt(num6);
                                        }
                                        carMonitor.Start();
                                        carMonitor.IsLock = false;
                                        currSite = carMonitor.CurrSite;
                                        LogHelper.WriteTrafficLog(string.Concat("被管制的车停在了别人的旋转集合中:", currSite.ToString(), ",往前挪一格"));
                                    }
                                }
                            }
                            else
                            {
                                List<string> strs1 = DataToObject.CreateDeepCopy<List<string>>(carMonitor.RouteLands);
                                if (carMonitor.RouteLands.Count >= 2)
                                {
                                    carMonitor.RouteLands.Clear();
                                    carMonitor.RouteLands.Add(strs1[0]);
                                    carMonitor.RouteLands.Add(strs1[1]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteTrafficLog(string.Concat("交通管制启动异常:", exception.Message));
            }
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

		public void GetTrafficResour(CarMonitor CurrCar)
		{
   int currSite;
            bool str;
            bool flag;
            bool str1;
            try
            {
                if (CurrCar != null)
                {
                    List<string> strs = new List<string>();
                    List<string> strs1 = new List<string>();
                    if ((!this.SysParameter.Keys.Contains("TrafficLenth") || !this.SysParameter.Keys.Contains("StopLenth") || !this.SysParameter.Keys.Contains("TurnLenth") ? false : this.SysParameter.Keys.Contains("ScalingRate")))
                    {
                        string str2 = this.SysParameter["TrafficLenth"].ToString();
                        string str3 = this.SysParameter["StopLenth"].ToString();
                        string str4 = this.SysParameter["TurnLenth"].ToString();
                        string str5 = this.SysParameter["ScalingRate"].ToString();
                        if ((string.IsNullOrEmpty(str2) || string.IsNullOrEmpty(str3) || string.IsNullOrEmpty(str4) ? false : !string.IsNullOrEmpty(str5)))
                        {
                            double num = Convert.ToDouble(str3);
                            double num1 = num + Convert.ToDouble(str2);
                            double num2 = Convert.ToDouble(str4);
                            double num3 = Convert.ToDouble(str5);
                            if (CurrCar.CurrRoute.Count > 0)
                            {
                                int num4 = CurrCar.CurrRoute.FindIndex((LandmarkInfo p) => p.LandmarkCode == CurrCar.CurrSite.ToString());
                                if (num4 >= 0)
                                {
                                    List<LandmarkInfo> landmarkInfos = new List<LandmarkInfo>();
                                    double length = 0;
                                    double length1 = 0;
                                    for (int i = num4 + 1; i < CurrCar.CurrRoute.Count; i++)
                                    {
                                        AllSegment allSegment = (
                                            from p in this.AllSeg
                                            where (p.BeginLandMakCode != CurrCar.CurrRoute[i - 1].LandmarkCode ? false : p.EndLandMarkCode == CurrCar.CurrRoute[i].LandmarkCode)
                                            select p).FirstOrDefault<AllSegment>();
                                        if (allSegment != null)
                                        {
                                            length += allSegment.Length;
                                            if ((length1 > num1 ? true : length < num1))
                                            {
                                                if ((
                                                    from p in strs
                                                    where p == CurrCar.CurrRoute[i - 1].LandmarkCode
                                                    select p).Count<string>() > 0)
                                                {
                                                    str = false;
                                                }
                                                else
                                                {
                                                    string landmarkCode = CurrCar.CurrRoute[i - 1].LandmarkCode;
                                                    currSite = CurrCar.CurrSite;
                                                    str = landmarkCode != currSite.ToString();
                                                }
                                                if (str)
                                                {
                                                    strs.Add(CurrCar.CurrRoute[i - 1].LandmarkCode);
                                                    landmarkInfos.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i - 1]));
                                                }
                                                if ((
                                                    from p in strs
                                                    where p == CurrCar.CurrRoute[i].LandmarkCode
                                                    select p).Count<string>() > 0)
                                                {
                                                    flag = false;
                                                }
                                                else
                                                {
                                                    string landmarkCode1 = CurrCar.CurrRoute[i].LandmarkCode;
                                                    currSite = CurrCar.CurrSite;
                                                    flag = landmarkCode1 != currSite.ToString();
                                                }
                                                if (flag)
                                                {
                                                    strs.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                                    landmarkInfos.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                                                }
                                                length1 += allSegment.Length;
                                            }
                                            else
                                            {
                                                if ((
                                                    from p in strs
                                                    where p == CurrCar.CurrRoute[i].LandmarkCode
                                                    select p).Count<string>() > 0)
                                                {
                                                    str1 = false;
                                                }
                                                else
                                                {
                                                    string landmarkCode2 = CurrCar.CurrRoute[i].LandmarkCode;
                                                    currSite = CurrCar.CurrSite;
                                                    str1 = landmarkCode2 != currSite.ToString();
                                                }
                                                if (str1)
                                                {
                                                    strs.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                                    landmarkInfos.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    LandmarkInfo landmarkInfo = CurrCar.CurrRoute.FirstOrDefault<LandmarkInfo>((LandmarkInfo q) => q.LandmarkCode == CurrCar.CurrSite.ToString());
                                    if ((landmarkInfo == null ? false : (
                                        from p in landmarkInfos
                                        where p.LandmarkCode == landmarkInfo.LandmarkCode
                                        select p).Count<LandmarkInfo>() <= 0))
                                    {
                                        landmarkInfos.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(landmarkInfo));
                                    }
                                    LandmarkInfo landmarkInfo1 = (
                                        from a in landmarkInfos
                                        where a.IsRotateLand
                                        orderby this.getDistant(a.LandX, a.LandY, landmarkInfo.LandX, landmarkInfo.LandY)
                                        select a).FirstOrDefault<LandmarkInfo>();
                                    if ((landmarkInfo1 == null ? false : landmarkInfo1.IsRotateLand))
                                    {
                                        List<LandmarkInfo> list = (
                                            from a in this.AllLands
                                            where Math.Round(this.getDistant(a.LandX, a.LandY, landmarkInfo1.LandX, landmarkInfo1.LandY) * num3, 3) <= Math.Round(num2, 3)
                                            select a).ToList<LandmarkInfo>();
                                        foreach (LandmarkInfo landmarkInfo2 in list)
                                        {
                                            if ((
                                                from p in strs1
                                                where p == landmarkInfo2.LandmarkCode
                                                select p).Count<string>() <= 0)
                                            {
                                                strs1.Add(landmarkInfo2.LandmarkCode);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    CurrCar.RouteLands = strs;
                    CurrCar.TurnLands = strs1;
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteTrafficLog(string.Concat("计算停止资源集异常:", exception.Message));
            }
		}

		public Hashtable SameGetRouteLandResource(CarMonitor CurrCar)
		{
            Hashtable hashtables;
            int currSite;
            bool str;
            bool flag;
            bool str1;
            try
            {
                if (CurrCar != null)
                {
                    List<string> strs = new List<string>();
                    List<string> strs1 = new List<string>();
                    if ((!this.SysParameter.Keys.Contains("TrafficLenth") || !this.SysParameter.Keys.Contains("StopLenth") || !this.SysParameter.Keys.Contains("TurnLenth") ? false : this.SysParameter.Keys.Contains("ScalingRate")))
                    {
                        string str2 = this.SysParameter["TrafficLenth"].ToString();
                        string str3 = this.SysParameter["StopLenth"].ToString();
                        string str4 = this.SysParameter["TurnLenth"].ToString();
                        string str5 = this.SysParameter["ScalingRate"].ToString();
                        if ((string.IsNullOrEmpty(str2) || string.IsNullOrEmpty(str3) || string.IsNullOrEmpty(str4) ? false : !string.IsNullOrEmpty(str5)))
                        {
                            double num = Convert.ToDouble(str3);
                            double num1 = num + Convert.ToDouble(str2);
                            double num2 = Convert.ToDouble(str4);
                            double num3 = Convert.ToDouble(str5);
                            if (CurrCar.CurrRoute.Count > 0)
                            {
                                int num4 = CurrCar.CurrRoute.FindIndex((LandmarkInfo p) => p.LandmarkCode == CurrCar.CurrSite.ToString());
                                if (num4 >= 0)
                                {
                                    List<LandmarkInfo> landmarkInfos = new List<LandmarkInfo>();
                                    double length = 0;
                                    double length1 = 0;
                                    for (int i = num4 + 1; i < CurrCar.CurrRoute.Count; i++)
                                    {
                                        AllSegment allSegment = (
                                            from p in this.AllSeg
                                            where (p.BeginLandMakCode != CurrCar.CurrRoute[i - 1].LandmarkCode ? false : p.EndLandMarkCode == CurrCar.CurrRoute[i].LandmarkCode)
                                            select p).FirstOrDefault<AllSegment>();
                                        if (allSegment != null)
                                        {
                                            length += allSegment.Length;
                                            if ((length1 > num1 ? true : length < num1))
                                            {
                                                if ((
                                                    from p in strs
                                                    where p == CurrCar.CurrRoute[i - 1].LandmarkCode
                                                    select p).Count<string>() > 0)
                                                {
                                                    str = false;
                                                }
                                                else
                                                {
                                                    string landmarkCode = CurrCar.CurrRoute[i - 1].LandmarkCode;
                                                    currSite = CurrCar.CurrSite;
                                                    str = landmarkCode != currSite.ToString();
                                                }
                                                if (str)
                                                {
                                                    strs.Add(CurrCar.CurrRoute[i - 1].LandmarkCode);
                                                    landmarkInfos.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i - 1]));
                                                }
                                                if ((
                                                    from p in strs
                                                    where p == CurrCar.CurrRoute[i].LandmarkCode
                                                    select p).Count<string>() > 0)
                                                {
                                                    flag = false;
                                                }
                                                else
                                                {
                                                    string landmarkCode1 = CurrCar.CurrRoute[i].LandmarkCode;
                                                    currSite = CurrCar.CurrSite;
                                                    flag = landmarkCode1 != currSite.ToString();
                                                }
                                                if (flag)
                                                {
                                                    strs.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                                    landmarkInfos.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                                                }
                                                length1 += allSegment.Length;
                                            }
                                            else
                                            {
                                                if ((
                                                    from p in strs
                                                    where p == CurrCar.CurrRoute[i].LandmarkCode
                                                    select p).Count<string>() > 0)
                                                {
                                                    str1 = false;
                                                }
                                                else
                                                {
                                                    string landmarkCode2 = CurrCar.CurrRoute[i].LandmarkCode;
                                                    currSite = CurrCar.CurrSite;
                                                    str1 = landmarkCode2 != currSite.ToString();
                                                }
                                                if (str1)
                                                {
                                                    strs.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                                    landmarkInfos.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    LandmarkInfo landmarkInfo = CurrCar.CurrRoute.FirstOrDefault<LandmarkInfo>((LandmarkInfo q) => q.LandmarkCode == CurrCar.CurrSite.ToString());
                                    if ((landmarkInfo == null ? false : (
                                        from p in landmarkInfos
                                        where p.LandmarkCode == landmarkInfo.LandmarkCode
                                        select p).Count<LandmarkInfo>() <= 0))
                                    {
                                        landmarkInfos.Add(DataToObject.CreateDeepCopy<LandmarkInfo>(landmarkInfo));
                                    }
                                    LandmarkInfo landmarkInfo1 = (
                                        from a in landmarkInfos
                                        where a.IsRotateLand
                                        orderby this.getDistant(a.LandX, a.LandY, landmarkInfo.LandX, landmarkInfo.LandY)
                                        select a).FirstOrDefault<LandmarkInfo>();
                                    if ((landmarkInfo1 == null ? false : landmarkInfo1.IsRotateLand))
                                    {
                                        List<LandmarkInfo> list = (
                                            from a in this.AllLands
                                            where Math.Round(this.getDistant(a.LandX, a.LandY, landmarkInfo1.LandX, landmarkInfo1.LandY) * num3, 3) <= Math.Round(num2, 3)
                                            select a).ToList<LandmarkInfo>();
                                        foreach (LandmarkInfo landmarkInfo2 in list)
                                        {
                                            if ((
                                                from p in strs1
                                                where p == landmarkInfo2.LandmarkCode
                                                select p).Count<string>() <= 0)
                                            {
                                                strs1.Add(landmarkInfo2.LandmarkCode);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    hashtables = null;
                                    return hashtables;
                                }
                            }
                            else
                            {
                                hashtables = null;
                                return hashtables;
                            }
                        }
                    }
                    Hashtable hashtables1 = new Hashtable();
                    hashtables1["RouteLands"] = strs;
                    hashtables1["TurnLands"] = strs1;
                    hashtables = hashtables1;
                }
                else
                {
                    hashtables = null;
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteTrafficLog(string.Concat("启动路径前的资源集计算异常:", exception.Message));
                hashtables = null;
            }
            return hashtables;
		}

		private bool SameCheckIsStop(int CurrCarID, int CurrSite, Hashtable hs)
		{
			bool result;
			try
			{
				bool flag = hs == null;
				if (flag)
				{
					result = true;
				}
				else
				{
					List<string> RouteLand = hs["RouteLands"] as List<string>;
					List<string> TurnLands = hs["TurnLands"] as List<string>;
					CarMonitor carMonitor = (from a in this.CarList
					where a.AgvID != CurrCarID && (from q in RouteLand
					where q == a.CurrSite.ToString()
					select q).Count<string>() > 0
					select a).FirstOrDefault<CarMonitor>();
					CarMonitor carMonitor2 = (from a in this.CarList
					where a.AgvID != CurrCarID && RouteLand.Intersect(a.RouteLands).Count<string>() > 0
					orderby RouteLand.Intersect(a.RouteLands).Count<string>() descending
					select a).FirstOrDefault<CarMonitor>();
					CarMonitor carMonitor3 = (from a in this.CarList
					where a.AgvID != CurrCarID && RouteLand.Intersect(a.TurnLands).Count<string>() > 0
					select a).FirstOrDefault<CarMonitor>();
					CarMonitor carMonitor4 = (from a in this.CarList
					where a.AgvID != CurrCarID && TurnLands.Contains(a.CurrSite.ToString())
					select a).FirstOrDefault<CarMonitor>();
					bool flag2 = carMonitor != null;
					if (flag2)
					{
						LogHelper.WriteTrafficLog(string.Concat(new string[]
						{
							"启动路径前判断跟车:",
							CurrCarID.ToString(),
							"在站点",
							CurrSite.ToString(),
							"继续停,因车",
							carMonitor.AgvID.ToString(),
							"在站点",
							carMonitor.CurrSite.ToString(),
							"阻挡"
						}));
						result = true;
					}
					else
					{
						bool flag3 = carMonitor3 != null;
						if (flag3)
						{
							bool flag4 = carMonitor3.TurnLands.Contains(carMonitor3.CurrSite.ToString());
							if (flag4)
							{
								LogHelper.WriteTrafficLog(string.Concat(new string[]
								{
									"启动前判断有旋转且在自己的旋转区域内,所以当前车:",
									CurrCarID.ToString(),
									"在站点",
									CurrSite.ToString(),
									"继续停,车",
									carMonitor3.AgvID.ToString(),
									"在站点",
									carMonitor3.CurrSite.ToString(),
									"阻挡"
								}));
								result = true;
							}
							else
							{
								LogHelper.WriteTrafficLog(string.Concat(new string[]
								{
									"启动路径前判断前方有其他车即将旋转的区域,所以当前车",
									CurrCarID.ToString(),
									"在站点:",
									CurrSite.ToString(),
									"停,车",
									carMonitor3.AgvID.ToString(),
									"在站点",
									carMonitor3.CurrSite.ToString(),
									"阻挡"
								}));
								result = true;
							}
						}
						else
						{
							bool flag5 = carMonitor2 != null;
							if (flag5)
							{
								List<string> CurrInsect = RouteLand.Intersect(carMonitor2.RouteLands).ToList<string>();
								List<string> AnotherInsect = carMonitor2.RouteLands.Intersect(RouteLand).ToList<string>();
								bool flag6 = CurrInsect.Count > 0 && AnotherInsect.Count > 0;
								if (flag6)
								{
									int num = RouteLand.FindIndex((string q) => q == CurrInsect[0]);
									int num2 = carMonitor2.RouteLands.FindIndex((string q) => q == AnotherInsect[0]);
									bool flag7 = num2 < num;
									if (flag7)
									{
										LogHelper.WriteTrafficLog(string.Concat(new string[]
										{
											"启动前判断交叉汇车,车:",
											CurrCarID.ToString(),
											"在站点",
											CurrSite.ToString(),
											"继续停,车",
											carMonitor2.AgvID.ToString(),
											"在站点",
											carMonitor2.CurrSite.ToString(),
											"阻挡"
										}));
										result = true;
										return result;
									}
								}
							}
							result = false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogHelper.WriteTrafficLog("启动路径前判断交通管制异常:" + ex.Message);
				result = true;
			}
			return result;
		}

		public bool SameHandleTrafficForStop(object obj)
		{
			bool result;
			try
			{
				bool flag = obj == null;
				if (flag)
				{
					result = true;
				}
				else
				{
					CarMonitor carMonitor = obj as CarMonitor;
					bool flag2 = carMonitor == null;
					if (flag2)
					{
						result = true;
					}
					else
					{
						Hashtable hashtable = this.SameGetRouteLandResource(carMonitor);
                        LogHelper.WriteTrafficLog(string.Concat("启动停止的车前判断", carMonitor.AgvID.ToString(), "行走资源集合:", string.Join(",",
                          from p in hashtable["RouteLands"] as List<string>
                          select p)));
                        LogHelper.WriteTrafficLog(string.Concat("启动停止的车前判断", carMonitor.AgvID.ToString(), "旋转资源集合:", string.Join(",",
                            from p in hashtable["TurnLands"] as List<string>
                            select p)));
						result = this.SameCheckIsStop(carMonitor.AgvID, carMonitor.CurrSite, hashtable);
					}
				}
			}
			catch (Exception ex)
			{
				LogHelper.WriteTrafficLog("启动路径前判断交通管制异常:" + ex.Message);
				result = true;
			}
			return result;
		}

		public bool BeforStartTrafficForStop(object obj)
		{
			bool result;
			try
			{
				bool flag = !this.IsHandleBeforStart;
				if (flag)
				{
					this.IsHandleBeforStart = true;
					bool flag2 = obj == null;
					if (flag2)
					{
						result = true;
					}
					else
					{
						CarMonitor carMonitor = obj as CarMonitor;
						bool flag3 = carMonitor == null;
						if (flag3)
						{
							result = true;
						}
						else
						{
							Hashtable hashtable = this.SameGetRouteLandResource(carMonitor);
                            LogHelper.WriteTrafficLog(string.Concat("启动前判断车", carMonitor.AgvID.ToString(), "行走资源集合:", string.Join(",",
                                      from p in hashtable["RouteLands"] as List<string>
                                      select p)));
                            LogHelper.WriteTrafficLog(string.Concat("启动前判断车", carMonitor.AgvID.ToString(), "旋转资源集合:", string.Join(",",
                                from p in hashtable["TurnLands"] as List<string>
                                select p)));
							result = this.BeforStartCheckIsStop(carMonitor.AgvID, carMonitor.CurrSite, hashtable);
						}
					}
				}
				else
				{
					result = true;
				}
			}
			catch (Exception ex)
			{
				LogHelper.WriteTrafficLog("启动前判断交通管制异常:" + ex.Message);
				result = true;
			}
			finally
			{
				this.IsHandleBeforStart = false;
			}
			return result;
		}

		public bool BeforStartCheckIsStop(int CurrCarID, int CurrSite, Hashtable hs)
		{
			bool result;
			try
			{
				bool flag = hs == null;
				if (flag)
				{
					result = true;
				}
				else
				{
					List<string> RouteLand = hs["RouteLands"] as List<string>;
					List<string> TurnLands = hs["TurnLands"] as List<string>;
					CarMonitor carMonitor = (from a in this.CarList
					where a.AgvID != CurrCarID && (from q in RouteLand
					where q == a.CurrSite.ToString()
					select q).Count<string>() > 0
					select a).FirstOrDefault<CarMonitor>();
					CarMonitor carMonitor2 = (from a in this.CarList
					where a.AgvID != CurrCarID && RouteLand.Intersect(a.RouteLands).Count<string>() > 0
					orderby RouteLand.Intersect(a.RouteLands).Count<string>() descending
					select a).FirstOrDefault<CarMonitor>();
					CarMonitor carMonitor3 = (from a in this.CarList
					where a.AgvID != CurrCarID && RouteLand.Intersect(a.TurnLands).Count<string>() > 0
					select a).FirstOrDefault<CarMonitor>();
					CarMonitor carMonitor4 = (from a in this.CarList
					where a.AgvID != CurrCarID && TurnLands.Contains(a.CurrSite.ToString())
					select a).FirstOrDefault<CarMonitor>();
					bool flag2 = carMonitor != null;
					if (flag2)
					{
						LogHelper.WriteTrafficLog(string.Concat(new string[]
						{
							"启动前判断跟车:",
							CurrCarID.ToString(),
							"在站点",
							CurrSite.ToString(),
							"继续停,因车",
							carMonitor.AgvID.ToString(),
							"在站点",
							carMonitor.CurrSite.ToString(),
							"阻挡"
						}));
						result = true;
					}
					else
					{
						bool flag3 = carMonitor4 != null;
						if (flag3)
						{
							LogHelper.WriteTrafficLog(string.Concat(new string[]
							{
								"启动前判断我的旋转资源中有车,所以当前车",
								CurrCarID.ToString(),
								"在站点",
								CurrSite.ToString(),
								"继续停,车",
								carMonitor4.AgvID.ToString(),
								"在站点",
								carMonitor4.CurrSite.ToString(),
								"阻挡"
							}));
							result = true;
						}
						else
						{
							bool flag4 = carMonitor3 != null;
							if (flag4)
							{
								LogHelper.WriteTrafficLog(string.Concat(new string[]
								{
									"启动路径前判断前方有其他车即将旋转区域,所以当前车",
									CurrCarID.ToString(),
									"在站点:",
									CurrSite.ToString(),
									"停,即将旋转的车",
									carMonitor3.AgvID.ToString(),
									"在站点",
									carMonitor3.CurrSite.ToString(),
									"继续走"
								}));
								result = true;
							}
							else
							{
								bool flag5 = carMonitor2 != null;
								if (flag5)
								{
									LogHelper.WriteTrafficLog(string.Concat(new string[]
									{
										"启动路径前判断交叉汇车,当前车:",
										CurrCarID.ToString(),
										"在站点",
										CurrSite.ToString(),
										"继续停,车",
										carMonitor2.AgvID.ToString(),
										"在站点",
										carMonitor2.CurrSite.ToString(),
										"阻挡"
									}));
									result = true;
								}
								else
								{
									result = false;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogHelper.WriteTrafficLog("启动路径前判断交通管制异常:" + ex.Message);
				result = true;
			}
			return result;
		}
	}
}
