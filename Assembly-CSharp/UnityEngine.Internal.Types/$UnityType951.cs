using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType951 : $UnityType
	{
		public unsafe $UnityType951()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 598552) = ldftn($Invoke0);
			*(data + 598580) = ldftn($Invoke1);
			*(data + 598608) = ldftn($Invoke2);
			*(data + 598636) = ldftn($Invoke3);
			*(data + 598664) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new CollectCurrencyCondition((UIntPtr)0);
		}
	}
}
