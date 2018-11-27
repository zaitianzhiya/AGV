using System;
using System.Collections.Generic;

namespace Model.MSM
{
	public class JunctionInfo
	{
		public int JunctionID
		{
			get;
			set;
		}

		public List<int> EnterP
		{
			get;
			set;
		}

		public List<string> EnterLandCode
		{
			get;
			set;
		}

		public List<int> JunctionP
		{
			get;
			set;
		}

		public List<string> JunctionLandMarkCodes
		{
			get;
			set;
		}

		public string RealseLandMarkCode
		{
			get;
			set;
		}

		public int OwerAGVID
		{
			get;
			set;
		}

		public int AreaAmount
		{
			get;
			set;
		}

		public JunctionInfo()
		{
			this.JunctionLandMarkCodes = new List<string>();
			this.EnterLandCode = new List<string>();
			this.RealseLandMarkCode = "";
			this.EnterP = new List<int>();
			this.JunctionP = new List<int>();
			this.OwerAGVID = -1;
		}
	}
}
