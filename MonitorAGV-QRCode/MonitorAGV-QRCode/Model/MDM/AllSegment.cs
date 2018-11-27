using System;

namespace Model.MDM
{
	public class AllSegment
	{
		public double Length
		{
			get;
			set;
		}

		public string BeginLandMakCode
		{
			get;
			set;
		}

        public LandmarkInfo BeginLandMak
        {
            get;
            set;
        }

		public string EndLandMarkCode
		{
			get;
			set;
		}

        public LandmarkInfo EndLandMark
        {
            get;
            set;
        }

		public int ExcuteAngle
		{
			get;
			set;
		}

	    public int Direct
	    {
	        get
	        {
	            if (BeginLandMak != null && EndLandMark != null)
	            {
	                return LocationCompare(BeginLandMak.LandMidX, EndLandMark.LandMidX, BeginLandMak.LandMidY,
	                    EndLandMark.LandMidY);
	            }
	            return -1;
	        }
	    }

        public int Weight
        {
            get
            {
                if (BeginLandMak != null && EndLandMark != null)
                {
                    return LengthCalc(BeginLandMak.LandMidX, EndLandMark.LandMidX, BeginLandMak.LandMidY,
                        EndLandMark.LandMidY);
                }
                return -1;
            }
        }

		public AllSegment()
		{
			this.BeginLandMakCode = "";
			this.EndLandMarkCode = "";
			this.Length = 0.0;
			this.ExcuteAngle = -1;
		}

        /// 计算两点间位置关系
        /// <summary>
        /// 计算两点间位置关系
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private int LocationCompare(double p1X,double p2X,double p1Y,double p2Y)
        {
            if (p1X == p2X)
            {
                if (p1Y == p2Y)
                {
                    return 0;
                }
                else if (p1Y > p2Y)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else if (p1X > p2X)
            {
                if (p1Y == p2Y)
                {
                    return 3;
                }
                else if (p1Y > p2Y)
                {
                    return 4;
                }
                else
                {
                    return 5;
                }
            }
            else
            {
                if (p1Y == p2Y)
                {
                    return 6;
                }
                else if (p1Y > p2Y)
                {
                    return 7;
                }
                else
                {
                    return 8;
                }
            }
            return -1;
        }

        /// 计算两点间直线距离
        /// <summary>
        /// 计算两点间直线距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private int LengthCalc(double p1X, double p2X, double p1Y, double p2Y)
        {
            double x = Math.Abs(p1X - p2X);
            double y = Math.Abs(p1Y - p2Y);
            return (int)(Math.Sqrt(x * x + y * y) + 0.5);
        }
	}
}
