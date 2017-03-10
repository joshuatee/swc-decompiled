using StaRTS.Utils.Scheduling;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2596 : $UnityType
	{
		public unsafe $UnityType2596()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 999428) = ldftn($Invoke0);
			*(data + 999456) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TimerList((UIntPtr)0);
		}
	}
}
