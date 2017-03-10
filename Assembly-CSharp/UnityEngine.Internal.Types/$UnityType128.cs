using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType128 : $UnityType
	{
		public unsafe $UnityType128()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 378192) = ldftn($Invoke0);
			*(data + 378220) = ldftn($Invoke1);
			*(data + 378248) = ldftn($Invoke2);
			*(data + 378276) = ldftn($Invoke3);
			*(data + 378304) = ldftn($Invoke4);
			*(data + 378332) = ldftn($Invoke5);
			*(data + 378360) = ldftn($Invoke6);
			*(data + 378388) = ldftn($Invoke7);
			*(data + 378416) = ldftn($Invoke8);
			*(data + 378444) = ldftn($Invoke9);
			*(data + 378472) = ldftn($Invoke10);
			*(data + 378500) = ldftn($Invoke11);
			*(data + 378528) = ldftn($Invoke12);
			*(data + 1525272) = ldftn($Get0);
			*(data + 1525276) = ldftn($Set0);
			*(data + 1525288) = ldftn($Get1);
			*(data + 1525292) = ldftn($Set1);
			*(data + 1525304) = ldftn($Get2);
			*(data + 1525308) = ldftn($Set2);
			*(data + 1525320) = ldftn($Get3);
			*(data + 1525324) = ldftn($Set3);
		}

		public override object CreateInstance()
		{
			return new UIButtonOffset((UIntPtr)0);
		}
	}
}
