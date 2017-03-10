using StaRTS.Main.Controllers.VictoryConditions;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType972 : $UnityType
	{
		public unsafe $UnityType972()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 601324) = ldftn($Invoke0);
			*(data + 601352) = ldftn($Invoke1);
			*(data + 601380) = ldftn($Invoke2);
			*(data + 601408) = ldftn($Invoke3);
			*(data + 601436) = ldftn($Invoke4);
			*(data + 601464) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new TrainUnitCondition((UIntPtr)0);
		}
	}
}
