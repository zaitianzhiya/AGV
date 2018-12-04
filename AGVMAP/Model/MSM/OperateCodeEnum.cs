using System;

namespace Model.MSM
{
	public enum OperateCodeEnum
	{
		UnknownError,
		Success,
		Failed,
		CodeIsExist,
		CodeLengthIsWrong,
		SaveCodeLengthError,
		IsNotExist,
		IsUsed
	}
}
