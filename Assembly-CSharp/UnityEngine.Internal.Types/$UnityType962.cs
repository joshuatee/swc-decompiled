using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType962 : $UnityType
	{
		public unsafe $UnityType962()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 599896) = ldftn($Invoke0);
			*(data + 599924) = ldftn($Invoke1);
			*(data + 599952) = ldftn($Invoke2);
			*(data + 599980) = ldftn($Invoke3);
			*(data + 600008) = ldftn($Invoke4);
			*(data + 600036) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new DestroyUnitIdCondition((UIntPtr)0);
		}
	}
}
