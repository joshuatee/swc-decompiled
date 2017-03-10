using StaRTS.Main.Models.Entities.Components;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1739 : $UnityType
	{
		public unsafe $UnityType1739()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 675384) = ldftn($Invoke0);
			*(data + 675412) = ldftn($Invoke1);
			*(data + 675440) = ldftn($Invoke2);
			*(data + 675468) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TrapComponent((UIntPtr)0);
		}
	}
}
