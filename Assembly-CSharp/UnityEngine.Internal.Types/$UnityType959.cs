using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType959 : $UnityType
	{
		public unsafe $UnityType959()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 599392) = ldftn($Invoke0);
			*(data + 599420) = ldftn($Invoke1);
			*(data + 599448) = ldftn($Invoke2);
			*(data + 599476) = ldftn($Invoke3);
			*(data + 599504) = ldftn($Invoke4);
			*(data + 599532) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DestroyBuildingIdCondition((UIntPtr)0);
		}
	}
}
