using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACTETHERLib;

namespace PLCControl
{
    public class PlcControl
    {
        public  int[] LData;        
        public  string ErrorNo;
        public  PlcStatus Plcstatus = PlcStatus.Init;
        ActQJ71E71TCP act = new ActQJ71E71TCP();
        public PlcControl()
        {
            act.ActHostAddress = FileControl.SetFileControl.ReadIniValue("SET", "HostAddress", Application.StartupPath + @"\Plc.ini");
            act.ActCpuType = int.Parse(FileControl.SetFileControl.ReadIniValue("SET", "CpuType", Application.StartupPath + @"\Plc.ini"));
            act.ActSourceNetworkNumber = int.Parse(FileControl.SetFileControl.ReadIniValue("SET", "SourceNetworkNumber", Application.StartupPath + @"\Plc.ini"));
            act.ActSourceStationNumber = int.Parse(FileControl.SetFileControl.ReadIniValue("SET", "SourceStationNumber", Application.StartupPath + @"\Plc.ini"));
            act.ActNetworkNumber = int.Parse(FileControl.SetFileControl.ReadIniValue("SET", "NetworkNumber", Application.StartupPath + @"\Plc.ini"));
            act.ActStationNumber = int.Parse(FileControl.SetFileControl.ReadIniValue("SET", "StationNumber", Application.StartupPath + @"\Plc.ini"));
            act.ActTimeOut = int.Parse(FileControl.SetFileControl.ReadIniValue("SET", "TimeOut", Application.StartupPath + @"\Plc.ini"));
        }

        public  bool ControlPLC()
        {
            long Plcopen = act.Open();
            int i = 0;
            ErrorNo = "";

            while (Plcopen != 0 && i < 1)
            {
                Plcopen = act.Open();
                i++;
            }

            string mm = Convert.ToString(Plcopen, 16);
            for (int len = mm.Length; len < 8; len++)
            {
                mm = "0" + mm;
            }
            ErrorNo = "0x" + mm;

            if (Plcopen != 0)
            {
                Plcstatus = PlcStatus.Close;
                return false;
            }
            else
            {
                Plcstatus = PlcStatus.Run;
                return true;
            }
        }


        public  void ClosePLC()
        {
            int Plcopen = act.Close();
            int i = 0;
            ErrorNo = "";

            while (Plcopen != 0 && i < 1) 
            {
                Plcopen = act.Close();
                i++;
            }
            string mm = Convert.ToString(Plcopen, 16);
            for (int len = mm.Length; len < 8; len++)
            {
                mm = "0" + mm;
            }
            ErrorNo = "0x" + mm;

            if (Plcopen != 0)
            {
                //MessageBox.Show("通信错误，错误码：" + ErrorNo, "提示", MessageBoxButtons.OK);
            }
            else
            {
                Plcstatus = PlcStatus.Close;
            }
        }

        public bool subReadRandom(string strDevice, int lSize, int intQuantity,out int[] Ldata)
        {
            bool ret = false;
            Ldata = new int[intQuantity];
            try
            {
                long lRet;
                ErrorNo = "";
                int i;
                lRet = act.ReadDeviceRandom(strDevice, lSize, out Ldata[0]);
                i = 0;
                while (lRet != 0 && i < 3)
                {
                    lRet = act.ReadDeviceRandom(strDevice, lSize, out Ldata[0]);
                    i = i + 1;
                }
                if (i != 3)
                {
                    ret = true;
                }
            }
            catch (Exception)
            {
               
            }
            return ret;
        }

        public bool subReadBlock(string strDevice, int lSize, int intQuantity, out int[] Ldata)
        {
            bool ret = false;
            Ldata = new int[intQuantity];
            try
            {
                long lRet;
                ErrorNo = "";
                int i;
                lRet = act.ReadDeviceBlock(strDevice, lSize, out Ldata[0]);
                i = 0;
                while (lRet != 0 && i < 3)
                {
                    lRet = act.ReadDeviceBlock(strDevice, lSize, out Ldata[0]);
                    i = i + 1;
                }
                if (i != 3)
                {
                    ret = true;
                }
            }
            catch (Exception)
            {
            }
            return ret;
        }

        public bool subWriteRandom(string strDevice, int lSize, int intValue)
        {
            bool ret = false;
            try
            {
                long lRet;
                int i;
                ErrorNo = "";
                lRet = act.WriteDeviceRandom(strDevice, lSize, ref intValue);
                i = 0;
                while (lRet != 0 && i < 3)
                {
                    lRet = act.WriteDeviceRandom(strDevice, lSize, ref intValue);
                    i = i + 1;
                }
                if (i != 3)
                {
                    ret = true;
                }
            }
            catch (Exception)
            {
            }
            return ret;
        }

        public  bool subWriteBlock(string strDevice, int lSize, int intValue)
        {
            bool ret = false;
            try
            {
                long lRet;
                int i = 0;
                ErrorNo = "";
                lRet = act.WriteDeviceBlock(strDevice, lSize, ref intValue);
                while (lRet != 0 && i < 3)
                {
                    lRet = act.WriteDeviceBlock(strDevice, lSize, ref intValue);
                    i++;
                }
                if (i != 3)
                {
                    ret = true;
                }
            }
            catch (Exception)
            {
            }
            return ret;
        }
    }
}
