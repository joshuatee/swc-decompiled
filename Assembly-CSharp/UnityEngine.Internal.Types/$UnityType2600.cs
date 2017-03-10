using StaRTS.Utils.Scheduling;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2600 : $UnityType
	{
		public unsafe $UnityType2600()
		{
			*(UnityEngine.Internal.$Metadata.data + 999960) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new ViewTimerManager((UIntPtr)0);
		}
	}
}
