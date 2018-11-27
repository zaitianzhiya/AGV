using System;

namespace Model.Comoon
{
	[Serializable]
	public class DetailQueryFiled
	{
		public string FiledCode
		{
			get;
			set;
		}

		public string FiledName
		{
			get;
			set;
		}

		public int SummaryType
		{
			get;
			set;
		}

		public string SummaryTypeUI
		{
			get
			{
				string result;
				switch (this.SummaryType)
				{
				case 0:
					result = "无";
					break;
				case 1:
					result = "计数";
					break;
				case 2:
					result = "求和";
					break;
				case 3:
					result = "平均值";
					break;
				default:
					result = "无";
					break;
				}
				return result;
			}
			set
			{
				if (!(value == "无"))
				{
					if (!(value == "计数"))
					{
						if (!(value == "求和"))
						{
							if (!(value == "平均值"))
							{
								this.SummaryType = 0;
							}
							else
							{
								this.SummaryType = 3;
							}
						}
						else
						{
							this.SummaryType = 2;
						}
					}
					else
					{
						this.SummaryType = 1;
					}
				}
				else
				{
					this.SummaryType = 0;
				}
			}
		}

		public DetailQueryFiled()
		{
			this.FiledCode = "";
			this.FiledName = "";
		}
	}
}
