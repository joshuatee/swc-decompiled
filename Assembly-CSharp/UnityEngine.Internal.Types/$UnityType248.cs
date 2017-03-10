using MD5;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType248 : $UnityType
	{
		public unsafe $UnityType248()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 416216) = ldftn($Invoke0);
			*(data + 416244) = ldftn($Invoke1);
			*(data + 416272) = ldftn($Invoke2);
			*(data + 416300) = ldftn($Invoke3);
			*(data + 416328) = ldftn($Invoke4);
			*(data + 416356) = ldftn($Invoke5);
			*(data + 416384) = ldftn($Invoke6);
			*(data + 416412) = ldftn($Invoke7);
			*(data + 416440) = ldftn($Invoke8);
			*(data + 416468) = ldftn($Invoke9);
			*(data + 416496) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new MD5((UIntPtr)0);
		}
	}
}
