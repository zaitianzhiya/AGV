using System;
using System.Drawing;

namespace Model.Comoon
{
	[Serializable]
	public class DetailCondition
	{
		public string ConditionCode
		{
			get;
			set;
		}

		public bool IsSystem
		{
			get;
			set;
		}

		public string ConditionValue
		{
			get;
			set;
		}

		public string RealyValue
		{
			get;
			set;
		}

		public string ConditionName
		{
			get;
			set;
		}

		public int X
		{
			get;
			set;
		}

		public int Y
		{
			get;
			set;
		}

		public Point Location
		{
			get
			{
				return new Point(this.X, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		public int control_type
		{
			get;
			set;
		}

		public string control_type_ui
		{
			get
			{
				string result;
				switch (this.control_type)
				{
				case 0:
					result = "输入框";
					break;
				case 1:
					result = "日期";
					break;
				case 2:
					result = "选择框";
					break;
				default:
					result = "";
					break;
				}
				return result;
			}
			set
			{
				if (!(value == "输入框"))
				{
					if (!(value == "日期"))
					{
						if (value == "选择框")
						{
							this.control_type = 2;
						}
					}
					else
					{
						this.control_type = 1;
					}
				}
				else
				{
					this.control_type = 0;
				}
			}
		}

		public DetailCondition()
		{
			this.ConditionCode = "";
			this.ConditionName = "";
			this.ConditionValue = "";
			this.Location = Point.Empty;
			this.control_type_ui = "";
			this.RealyValue = "";
			this.IsSystem = false;
		}
	}
}
