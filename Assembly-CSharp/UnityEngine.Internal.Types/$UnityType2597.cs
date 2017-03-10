using StaRTS.Utils.Scheduling;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2597 : $UnityType
	{
		public unsafe $UnityType2597()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 999484) = ldftn($Invoke0);
			*(data + 999512) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TimerManager((UIntPtr)0);
		}
	}
}
