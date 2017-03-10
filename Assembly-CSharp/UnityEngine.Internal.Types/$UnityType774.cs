using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType774 : $UnityType
	{
		public unsafe $UnityType774()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 564000) = ldftn($Invoke0);
			*(data + 564028) = ldftn($Invoke1);
			*(data + 564056) = ldftn($Invoke2);
			*(data + 564084) = ldftn($Invoke3);
			*(data + 564112) = ldftn($Invoke4);
			*(data + 564140) = ldftn($Invoke5);
			*(data + 564168) = ldftn($Invoke6);
			*(data + 564196) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new TrapController((UIntPtr)0);
		}
	}
}
