using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType967 : $UnityType
	{
		public unsafe $UnityType967()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 600624) = ldftn($Invoke0);
			*(data + 600652) = ldftn($Invoke1);
			*(data + 600680) = ldftn($Invoke2);
			*(data + 600708) = ldftn($Invoke3);
			*(data + 600736) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new OwnBuildingCondition((UIntPtr)0);
		}
	}
}
