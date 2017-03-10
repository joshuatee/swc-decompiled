using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType966 : $UnityType
	{
		public unsafe $UnityType966()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 600484) = ldftn($Invoke0);
			*(data + 600512) = ldftn($Invoke1);
			*(data + 600540) = ldftn($Invoke2);
			*(data + 600568) = ldftn($Invoke3);
			*(data + 600596) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new LootCurrencyCondition((UIntPtr)0);
		}
	}
}
