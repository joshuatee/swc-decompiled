using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType970 : $UnityType
	{
		public unsafe $UnityType970()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 600988) = ldftn($Invoke0);
			*(data + 601016) = ldftn($Invoke1);
			*(data + 601044) = ldftn($Invoke2);
			*(data + 601072) = ldftn($Invoke3);
			*(data + 601100) = ldftn($Invoke4);
			*(data + 601128) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new RetainBuildingCondition((UIntPtr)0);
		}
	}
}
