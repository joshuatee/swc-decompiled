using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1080 : $UnityType
	{
		public unsafe $UnityType1080()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 627952) = ldftn($Invoke0);
			*(data + 627980) = ldftn($Invoke1);
			*(data + 628008) = ldftn($Invoke2);
			*(data + 628036) = ldftn($Invoke3);
			*(data + 628064) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new TurretTrapEventData((UIntPtr)0);
		}
	}
}
