using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType960 : $UnityType
	{
		public unsafe $UnityType960()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 599560) = ldftn($Invoke0);
			*(data + 599588) = ldftn($Invoke1);
			*(data + 599616) = ldftn($Invoke2);
			*(data + 599644) = ldftn($Invoke3);
			*(data + 599672) = ldftn($Invoke4);
			*(data + 599700) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DestroyBuildingTypeCondition((UIntPtr)0);
		}
	}
}
