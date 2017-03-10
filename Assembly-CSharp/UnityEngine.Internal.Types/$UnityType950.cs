using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType950 : $UnityType
	{
		public unsafe $UnityType950()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 598440) = ldftn($Invoke0);
			*(data + 598468) = ldftn($Invoke1);
			*(data + 598496) = ldftn($Invoke2);
			*(data + 598524) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new AbstractCondition((UIntPtr)0);
		}
	}
}
