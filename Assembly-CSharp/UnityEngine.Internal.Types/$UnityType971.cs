using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType971 : $UnityType
	{
		public unsafe $UnityType971()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 601156) = ldftn($Invoke0);
			*(data + 601184) = ldftn($Invoke1);
			*(data + 601212) = ldftn($Invoke2);
			*(data + 601240) = ldftn($Invoke3);
			*(data + 601268) = ldftn($Invoke4);
			*(data + 601296) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new RetainUnitCondition((UIntPtr)0);
		}
	}
}
