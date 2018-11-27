using System;

namespace Model.MDM
{
	[Serializable]
	public class MaterialInfo
	{
		public int MaterialType
		{
			get;
			set;
		}

		public string MaterialName
		{
			get;
			set;
		}

		public MaterialInfo()
		{
			this.MaterialName = "";
		}
	}
}
