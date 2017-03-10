using StaRTS.Utils.Scheduling;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2599 : $UnityType
	{
		public unsafe $UnityType2599()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 999708) = ldftn($Invoke0);
			*(data + 999736) = ldftn($Invoke1);
			*(data + 999764) = ldftn($Invoke2);
			*(data + 999792) = ldftn($Invoke3);
			*(data + 999820) = ldftn($Invoke4);
			*(data + 999848) = ldftn($Invoke5);
			*(data + 999876) = ldftn($Invoke6);
			*(data + 999904) = ldftn($Invoke7);
			*(data + 999932) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new ViewTimeEngine((UIntPtr)0);
		}
	}
}
