using System;

namespace Model.MDM
{
	[Serializable]
	public class StorageInfo
	{
		public int ID
		{
			get;
			set;
		}

		public string StorageName
		{
			get;
			set;
		}

		public int OwnArea
		{
			get;
			set;
		}

		public int SubOwnArea
		{
			get;
			set;
		}

		public int matterType
		{
			get;
			set;
		}

		public string LankMarkCode
		{
			get;
			set;
		}

		public int StorageState
		{
			get;
			set;
		}

		public int LockState
		{
			get;
			set;
		}

		public int LockCar
		{
			get;
			set;
		}

		public int MaterielType
		{
			get;
			set;
		}

		public StorageInfo()
		{
			this.StorageName = "";
			this.LankMarkCode = "";
		}
	}
}
