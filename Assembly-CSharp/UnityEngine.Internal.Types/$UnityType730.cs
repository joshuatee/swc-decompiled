using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType730 : $UnityType
	{
		public unsafe $UnityType730()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 542076) = ldftn($Invoke0);
			*(data + 542104) = ldftn($Invoke1);
			*(data + 542132) = ldftn($Invoke2);
			*(data + 542160) = ldftn($Invoke3);
			*(data + 542188) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new NeighborVisitManager((UIntPtr)0);
		}
	}
}
