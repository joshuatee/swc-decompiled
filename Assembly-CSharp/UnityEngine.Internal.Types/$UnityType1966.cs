using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1966 : $UnityType
	{
		public unsafe $UnityType1966()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 799508) = ldftn($Invoke0);
			*(data + 799536) = ldftn($Invoke1);
			*(data + 799564) = ldftn($Invoke2);
			*(data + 799592) = ldftn($Invoke3);
			*(data + 799620) = ldftn($Invoke4);
			*(data + 799648) = ldftn($Invoke5);
			*(data + 799676) = ldftn($Invoke6);
			*(data + 799704) = ldftn($Invoke7);
			*(data + 799732) = ldftn($Invoke8);
			*(data + 799760) = ldftn($Invoke9);
			*(data + 799788) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new SupplyCardTypeVO((UIntPtr)0);
		}
	}
}
