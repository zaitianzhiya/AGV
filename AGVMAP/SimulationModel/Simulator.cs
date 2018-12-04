using System.Data;
using System.Windows.Forms;
using AGVMAP;
using Model.MDM;
using Model.MSM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using Tools;

namespace SimulationModel
{
    public class Simulator
    {
        public delegate void CarMove(object sender);

        public delegate void CarIni(object sender);

        public event CarIni Car_Ini;

        public event CarMove Car_Move;

        private IList<CarInfo> AllCar = new List<CarInfo>();

        private static IList<AllSegment> AllSegs = new List<AllSegment>();

        private static List<CarMonitor> MoniCars = new List<CarMonitor>();

        private static IList<LandmarkInfo> AllLands = new List<LandmarkInfo>();

        private static IList<StorageInfo> Stores = new List<StorageInfo>();

        public IDictionary<string, string> System = new Dictionary<string, string>();

        private System.Timers.Timer timerStarBeStopedCar = new System.Timers.Timer(1000.0);

        private System.Timers.Timer timerFreshTask = new System.Timers.Timer(5000.0);

        public static object HandleTaskobj = new object();

        public RoutePlanData CountRoute
        {
            get;
            set;
        }

        public TrafficController Traffic
        {
            get;
            set;
        }

        public bool Inital()
        {
            bool result;
            try
            {
                MoniCars.Clear();
                AllCar = DataToObject.TableToEntity<CarInfo>(Function.GetDataInfo("PR_SELECT_TBCAR"));
                AllLands = DataToObject.TableToEntity<LandmarkInfo>(Function.GetDataInfo("PR_SELECT_TBLANDMARK"));
                AllSegs = DataToObject.TableToEntity<AllSegment>(Function.GetDataInfo("P_SELECT_TBALLSEGMENT"));
                Stores = DataToObject.TableToEntity<StorageInfo>(Function.GetDataInfo("P_SELECT_TBLOCATION"));

                DataTable dtPara = Function.GetDataInfo("PR_SELECT_SYSPARAMETER");
                System = new Dictionary<string, string>();
                foreach (DataRow dr in dtPara.Rows)
                {
                    System[dr["ParameterCode"].ToString()] = dr["ParameterValue"].ToString();
                }
                foreach (CarInfo item in AllCar)
                {
                    CarMonitor carMonitor = new CarMonitor();
                    carMonitor.AgvID = item.AgvID;
                    carMonitor.CurrSite = Convert.ToInt32(item.StandbyLandMark);
                    carMonitor.StandbyLandMark = item.StandbyLandMark;
                    double num = 0.0;
                    string value = System["ScalingRate"];
                    try
                    {
                        num = Convert.ToDouble(value);
                    }
                    catch
                    {
                    }
                    if (num > 0.0)
                    {
                        LandmarkInfo landmarkInfo = AllLands.FirstOrDefault(p => p.LandmarkCode == item.StandbyLandMark);
                        if (landmarkInfo != null)
                        {
                            carMonitor.X = (float)(landmarkInfo.LandX * num);
                            carMonitor.Y = (float)(landmarkInfo.LandY * num);
                        }
                    }
                    MoniCars.Add(carMonitor);
                }
                if (Car_Ini != null)
                {
                    Car_Ini(MoniCars);
                }
                CountRoute = new RoutePlanData(AllSegs);
                Traffic = new TrafficController(MoniCars, AllSegs, System, AllLands);
                timerStarBeStopedCar.Enabled = true;
                timerStarBeStopedCar.AutoReset = true;
                timerStarBeStopedCar.Elapsed += new ElapsedEventHandler(TimerStarBeStopedCar_Elapsed);
                timerFreshTask.Enabled = true;
                timerFreshTask.AutoReset = true;
                timerFreshTask.Elapsed += new ElapsedEventHandler(TimerFreshTask_Elapsed);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool StopSimula()
        {
            bool result;
            try
            {
                timerFreshTask.Enabled = false;
                timerStarBeStopedCar.Enabled = false;
                foreach (CarMonitor current in MoniCars)
                {
                    current.Dispose();
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string CreatTask(int CallBoxID, int BtnID)
        {
            string result;
            try
            {
                DataTable dtCallbox = Function.GetCallBoxInfoById(CallBoxID);
                CallBoxInfo callBoxInfo = DataToObject.TableToEntity<CallBoxInfo>(dtCallbox).FirstOrDefault<CallBoxInfo>();
                bool flag = callBoxInfo == null;
                if (flag)
                {
                    result = "未配置按钮盒档案信息";
                }
                else
                {
                    IList<CallBoxDetail> list = DataToObject.TableToEntity<CallBoxDetail>(Function.GetCallBoxDetailInfoById(CallBoxID));
                    if (list == null || list.Count <= 0)
                    {
                        result = "未配置按钮盒明细档案信息";
                    }
                    else
                    {
                        if (callBoxInfo.CallBoxType == 0)
                        {
                            CallBoxDetail CurrBoxDetail = list.FirstOrDefault((CallBoxDetail p) => p.CallBoxID == CallBoxID && p.ButtonID == BtnID);
                            bool flag4 = CurrBoxDetail == null;
                            if (flag4)
                            {
                                result = "当前按钮没有配置信息";
                                return result;
                            }
                            if (CurrBoxDetail.OperaType == 0)
                            {
                                IList<TaskConfigDetail> list2 = DataToObject.TableToEntity<TaskConfigDetail>(Function.GettbTaskConfigDetail(CurrBoxDetail.TaskConditonCode));
                                if (list2 == null || list2.Count <= 0)
                                {
                                    result = "当前按钮未配置任务信息";
                                    return result;
                                }
                                StorageInfo storageInfo = Simulator.Stores.FirstOrDefault((StorageInfo q) => q.ID == CurrBoxDetail.LocationID);
                                if (storageInfo == null)
                                {
                                    result = "未设置当前按钮的监控储位!";
                                    return result;
                                }
                                DataTable dtTemp = Function.ChekAllowCreatTask(CallBoxID, storageInfo.LankMarkCode);
                                if (Convert.ToInt16(dtTemp.Rows[0][0]) > 0)
                                {
                                    result = "存在未完成任务,请稍后再试!";
                                    return result;
                                }
                                string dispatchNo = Guid.NewGuid().ToString();
                                DispatchTaskInfo dispatchTaskInfo = new DispatchTaskInfo();
                                dispatchTaskInfo.dispatchNo = dispatchNo;
                                dispatchTaskInfo.taskType = 0;
                                dispatchTaskInfo.TaskState = 0;
                                dispatchTaskInfo.CallLand = storageInfo.LankMarkCode;
                                dispatchTaskInfo.stationNo = CallBoxID;
                                int num = 1;
                                int num2 = -1;
                                using (IEnumerator<TaskConfigDetail> enumerator = list2.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        TaskConfigDetail item = enumerator.Current;
                                        DispatchTaskDetail dispatchTaskDetail = new DispatchTaskDetail();
                                        dispatchTaskDetail.dispatchNo = dispatchNo;
                                        dispatchTaskDetail.DetailID = num;
                                        StorageInfo storageInfo2;
                                        if (item.ArmOwnArea == -1)
                                        {
                                            storageInfo2 = storageInfo;
                                        }
                                        else
                                        {
                                            storageInfo2 = Simulator.Stores.FirstOrDefault((StorageInfo p) => p.OwnArea == item.ArmOwnArea && p.StorageState == item.StorageState && p.MaterielType == item.MaterialType);
                                        }
                                        if (storageInfo2 == null)
                                        {
                                            result = "任务条件不满足!";
                                            return result;
                                        }
                                        dispatchTaskDetail.LandCode = storageInfo2.LankMarkCode;
                                        dispatchTaskDetail.OperType = item.Action;
                                        dispatchTaskDetail.PutType = ((num2 == -1) ? 0 : ((num2 == 1) ? 0 : 1));
                                        dispatchTaskDetail.IsAllowExcute = item.IsWaitPass;
                                        dispatchTaskDetail.State = 0;
                                        dispatchTaskInfo.TaskDetail.Add(dispatchTaskDetail);
                                        num++;
                                        num2 = storageInfo2.StorageState;
                                    }
                                }
                                OperateReturnInfo operateReturnInfo = Function.SaveTask(dispatchTaskInfo);

                                if (operateReturnInfo.ReturnCode != OperateCodeEnum.Success)
                                {
                                    result = operateReturnInfo.ReturnInfo.ToString();
                                    return result;
                                }
                                using (IEnumerator<TaskConfigDetail> enumerator2 = list2.GetEnumerator())
                                {
                                    while (enumerator2.MoveNext())
                                    {
                                        TaskConfigDetail item = enumerator2.Current;
                                        StorageInfo storageInfo3 = Simulator.Stores.FirstOrDefault((StorageInfo p) => p.OwnArea == item.ArmOwnArea && p.StorageState == item.StorageState && p.MaterielType == item.MaterialType);
                                        bool flag12 = storageInfo3 != null;
                                        if (flag12)
                                        {
                                            storageInfo3.LockState = 2;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (CurrBoxDetail.OperaType == 1)
                                {
                                    StorageInfo CheckStore = Simulator.Stores.FirstOrDefault((StorageInfo p) => p.ID == CurrBoxDetail.LocationID);
                                    if (CheckStore == null)
                                    {
                                        result = "监控放行储位不存在!";
                                        return result;
                                    }
                                    CarMonitor carMonitor = Simulator.MoniCars.FirstOrDefault((CarMonitor p) => p.CurrSite.ToString() == CheckStore.LankMarkCode);
                                    if (carMonitor == null)
                                    {
                                        result = "当前没有放行车辆!";
                                        return result;
                                    }
                                    OperateReturnInfo operateReturnInfo2 = Function.ReleaseCar(carMonitor.ExcuteTaksNo, carMonitor.ArmLand);
                                    if (operateReturnInfo2.ReturnCode != OperateCodeEnum.Success)
                                    {
                                        result = operateReturnInfo2.ReturnInfo.ToString();
                                        return result;
                                    }
                                }
                            }
                        }
                        else
                        {
                            CallBoxDetail CurrBoxDetail = list.FirstOrDefault((CallBoxDetail p) => p.CallBoxID == CallBoxID && p.ButtonID == BtnID);
                            if (CurrBoxDetail == null)
                            {
                                result = "当前按钮没有配置信息";
                                return result;
                            }
                            IList<TaskConfigDetail> list3 = DataToObject.TableToEntity<TaskConfigDetail>(Function.GettbTaskConfigDetail(CurrBoxDetail.TaskConditonCode));
                            if (list3 == null || list3.Count <= 0)
                            {
                                result = "当前按钮未配置任务信息";
                                return result;
                            }
                            StorageInfo storageInfo4 = Simulator.Stores.FirstOrDefault((StorageInfo q) => q.ID == CurrBoxDetail.LocationID);
                            if (storageInfo4 == null)
                            {
                                result = "未设置当前按钮的监控储位!";
                                return result;
                            }
                            storageInfo4.StorageState = CurrBoxDetail.LocationState;
                            Function.UpdateStore(storageInfo4.ID, storageInfo4.StorageState);
                        }
                        result = "操作成功!";
                    }
                }
            }
            catch (Exception ex)
            {
                result = "发送异常!" + ex.Message;
            }
            return result;
        }

        private void TimerFreshTask_Elapsed(object sender, ElapsedEventArgs e)
        {
            LandmarkInfo landmarkInfo;
            try
            {
                try
                {
                    lock (HandleTaskobj)
                    {
                        timerFreshTask.Enabled = false;
                        IList<DispatchTaskInfo> dispatchTaskInfos = Function.LoadDispatchTask(); 
                        if ((dispatchTaskInfos == null || dispatchTaskInfos.Count <= 0))
                        {
                            List<CarMonitor> list = (
                                from p in MoniCars
                                //where (p.Sate == 1 || p.Sate == 2 ? false : !string.IsNullOrEmpty(p.ExcuteTaksNo))
                                where (p.Sate != 1 && p.Sate != 2 &&!string.IsNullOrEmpty(p.ExcuteTaksNo))
                                select p).ToList<CarMonitor>();
                            foreach (CarMonitor carMonitor in list)
                            {
                                carMonitor.ExcuteTaksNo = "";
                            }
                        }
                        else
                        {
                            foreach (DispatchTaskInfo dispatchTaskInfo in dispatchTaskInfos)
                            {
                                if (dispatchTaskInfo.TaskDetail.Count > 0)
                                {
                                    CarMonitor isBack = null;
                                    DispatchTaskDetail dispatchTaskDetail = (
                                        from a in dispatchTaskInfo.TaskDetail
                                        where (a.State == 0 || a.State == 1)
                                        orderby a.DetailID
                                        select a).FirstOrDefault<DispatchTaskDetail>();
                                    if (dispatchTaskInfo.TaskState == 0)
                                    {
                                        if (dispatchTaskDetail != null)
                                        {
                                            isBack = (
                                                from a in MoniCars
                                                where (dispatchTaskInfo.ExeAgvID != 0 || string.IsNullOrEmpty(a.CurrSite.ToString()) || a.Sate != 0 && !a.IsBack || !(a.ExcuteTaksNo == "") ? false : getDistant(a.CurrSite.ToString(), dispatchTaskDetail.LandCode) >= 0)
                                                orderby getDistant(a.CurrSite.ToString(), dispatchTaskDetail.LandCode)
                                                select a).FirstOrDefault<CarMonitor>();
                                        }
                                    }
                                    else if (dispatchTaskInfo.TaskState == 1)
                                    {
                                        isBack = MoniCars.FirstOrDefault(p => (p.AgvID == dispatchTaskInfo.ExeAgvID && (p.Sate == 0 || p.Sate == 2 || p.IsBack)));
                                    }
                                    if ((isBack != null && isBack.CurrSite > 0))
                                    {
                                        if (isBack.IsBack)
                                        {
                                            isBack.IsBack = !isBack.IsBack;
                                        }
                                        LandmarkInfo landmarkInfo1 = null;
                                        if (dispatchTaskDetail == null)
                                        {
                                            isBack.ExcuteTaksNo = "";
                                            if (!string.IsNullOrEmpty(isBack.StandbyLandMark))
                                            {
                                                landmarkInfo = AllLands.FirstOrDefault( p=> p.LandmarkCode == isBack.CurrSite.ToString());
                                                landmarkInfo1 = AllLands.FirstOrDefault( p => p.LandmarkCode == isBack.StandbyLandMark);
                                                if ((landmarkInfo != null&&landmarkInfo1 != null))
                                                {
                                                    isBack.CurrRoute = DataToObject.CreateDeepCopy(CountRoute.GetRoute(landmarkInfo, landmarkInfo1));
                                                    if (isBack.CurrRoute.Count <= 0)
                                                    {
                                                        continue;
                                                    }
                                                    else if (!Traffic.BeforStartTrafficForStop(isBack))
                                                    {
                                                        Traffic.GetTrafficResour(isBack);
                                                        isBack.StepChange += new CarMonitor.CarStepChange(this.DoCar_StepChange);
                                                        isBack.Start();
                                                    }
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        else if ((!(isBack.CurrSite.ToString() == dispatchTaskDetail.LandCode) || dispatchTaskDetail.State != 0 ? dispatchTaskDetail.State != 1 : false))
                                        {
                                            DispatchTaskDetail dispatchTaskDetail1 = (
                                                from a in dispatchTaskInfo.TaskDetail
                                                where a.LandCode == isBack.CurrSite.ToString()
                                                orderby a.DetailID
                                                select a).FirstOrDefault<DispatchTaskDetail>();
                                            if ((dispatchTaskDetail1 == null || dispatchTaskDetail1.IsAllowExcute != 0))
                                            {
                                                Function.TaskHandle(dispatchTaskInfo.dispatchNo, isBack.AgvID, 1, dispatchTaskDetail.LandCode, dispatchTaskDetail.DetailID);
                                                isBack.ExcuteTaksNo = dispatchTaskInfo.dispatchNo;
                                                isBack.TaskDetailID = dispatchTaskDetail.DetailID;
                                                isBack.ArmLand = dispatchTaskDetail.LandCode;
                                                isBack.PutType = dispatchTaskDetail.PutType;
                                                isBack.OperType = dispatchTaskDetail.OperType;
                                                landmarkInfo = AllLands.FirstOrDefault( p => p.LandmarkCode == isBack.CurrSite.ToString());
                                                landmarkInfo1 = AllLands.FirstOrDefault( p => p.LandmarkCode == dispatchTaskDetail.LandCode);
                                                if ((landmarkInfo != null || landmarkInfo1 != null))
                                                {
                                                    isBack.CurrRoute = DataToObject.CreateDeepCopy(CountRoute.GetRoute(landmarkInfo, landmarkInfo1));
                                                    if (isBack.CurrRoute.Count <= 0)
                                                    {
                                                        continue;
                                                    }
                                                    else if (!Traffic.BeforStartTrafficForStop(isBack))
                                                    {
                                                        this.Traffic.GetTrafficResour(isBack);
                                                        isBack.StepChange += new CarMonitor.CarStepChange(this.DoCar_StepChange);
                                                        isBack.Start();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            isBack.OperType = dispatchTaskDetail.OperType;
                                            isBack.PutType = dispatchTaskDetail.PutType;
                                            isBack.ArmLand = dispatchTaskDetail.LandCode;
                                            HandTaskDetail(dispatchTaskDetail.LandCode, dispatchTaskDetail.dispatchNo, dispatchTaskDetail.DetailID);
                                            UnLockStorage(isBack, Stores);
                                            isBack.TaskDetailID = -1;
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                        List<CarMonitor> carMonitors = (
                            from p in MoniCars
                            where (p.Sate == 1 || p.Sate == 2 || !string.IsNullOrEmpty(p.ExcuteTaksNo) ? false : p.CurrSite.ToString() != p.StandbyLandMark)
                            select p).ToList<CarMonitor>();
                        foreach (CarMonitor carMonitor1 in carMonitors)
                        {
                            if (!string.IsNullOrEmpty(carMonitor1.StandbyLandMark))
                            {
                                LandmarkInfo landmarkInfo2 = AllLands.FirstOrDefault( p => p.LandmarkCode == carMonitor1.CurrSite.ToString());
                                LandmarkInfo landmarkInfo3 = AllLands.FirstOrDefault( p => p.LandmarkCode == carMonitor1.StandbyLandMark);
                                if ((landmarkInfo2 == null ? false : landmarkInfo3 != null))
                                {
                                    carMonitor1.CurrRoute = DataToObject.CreateDeepCopy(CountRoute.GetRoute(landmarkInfo2, landmarkInfo3));
                                    if (carMonitor1.CurrRoute.Count > 0)
                                    {
                                        if (!Traffic.BeforStartTrafficForStop(carMonitor1))
                                        {
                                            Traffic.GetTrafficResour(carMonitor1);
                                            carMonitor1.StepChange += new CarMonitor.CarStepChange(DoCar_StepChange);
                                            carMonitor1.Start();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                timerFreshTask.Enabled = true;
            }
        }

        private void TimerStarBeStopedCar_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                timerStarBeStopedCar.Enabled = false;
                new Thread(new ThreadStart(Traffic.HandleTrafficForStart))
                {
                    IsBackground = true
                }.Start();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
            finally
            {
                timerStarBeStopedCar.Enabled = true;
            }
        }

        private void DoCar_StepChange(object sender)
        {
            try
            {
                bool flag = Car_Move != null;
                if (flag)
                {
                    Car_Move(sender);
                }
                CarMonitor carMonitor = sender as CarMonitor;
                bool flag2 = carMonitor != null;
                if (flag2)
                {
                    bool flag3 = carMonitor.OldSite != carMonitor.CurrSite;
                    if (flag3)
                    {
                        Traffic.HandleTrafficForStop(sender as CarMonitor);
                    }
                    bool flag4 = !string.IsNullOrEmpty(carMonitor.ExcuteTaksNo) && carMonitor.TaskDetailID != -1;
                    if (flag4)
                    {
                        bool flag5 = carMonitor.Sate == 0 && carMonitor.CurrSite.ToString() == carMonitor.ArmLand;
                        if (flag5)
                        {
                            UnLockStorage(carMonitor, Stores);
                            HandTaskDetail(carMonitor.CurrSite.ToString(), carMonitor.ExcuteTaksNo, carMonitor.TaskDetailID);
                            carMonitor.TaskDetailID = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double getDistant(string CurrLandCode, string TaskDetailLandCode)
        {
            double result;
            try
            {
                LandmarkInfo landmarkInfo = Simulator.AllLands.FirstOrDefault((LandmarkInfo p) => p.LandmarkCode == CurrLandCode);
                LandmarkInfo landmarkInfo2 = Simulator.AllLands.FirstOrDefault((LandmarkInfo p) => p.LandmarkCode == TaskDetailLandCode);
                bool flag = landmarkInfo != null && landmarkInfo2 != null;
                if (flag)
                {
                    double num = Math.Sqrt(Math.Pow(Math.Abs(landmarkInfo.LandX - landmarkInfo2.LandX), 2.0) + Math.Pow(Math.Abs(landmarkInfo.LandY - landmarkInfo2.LandY), 2.0));
                    result = num;
                    return result;
                }
                LogHelper.WriteLog("领取任务时查找直线最近车辆时找不到车当前地标或任务明细的目的地标");
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
            result = -1.0;
            return result;
        }

        public void UnLockStorage(CarMonitor car, IList<StorageInfo> StorageList)
        {
            StorageInfo storageInfo = null;
            bool flag = !string.IsNullOrEmpty(car.ArmLand) && car.CurrSite.ToString() == car.ArmLand;
            if (flag)
            {
                storageInfo = (from p in StorageList
                               where p.LankMarkCode == car.CurrSite.ToString()
                               select p).FirstOrDefault<StorageInfo>();
            }
            bool flag2 = storageInfo != null;
            if (flag2)
            {
                bool flag3 = car.OperType == 1;
                if (flag3)
                {
                    bool flag4 = car.PutType == 0;
                    if (flag4)
                    {
                        storageInfo.StorageState = 1;
                        storageInfo.LockCar = 0;
                        storageInfo.LockState = 0;
                    }
                    else
                    {
                        storageInfo.StorageState = 2;
                        storageInfo.LockCar = 0;
                        storageInfo.LockState = 0;
                    }
                }
                else
                {
                    storageInfo.StorageState = 0;
                    storageInfo.LockCar = 0;
                    storageInfo.LockState = 0;
                }

                Function.UpdateLocation(car.CurrSite.ToString(), storageInfo.StorageState);
            }
        }

        public void HandTaskDetail(string landCode, string dispatchNo, int taskDetailID)
        {
            Function.UpdateTaskDetailForFinish(landCode, dispatchNo, taskDetailID);
        }
    }
}
