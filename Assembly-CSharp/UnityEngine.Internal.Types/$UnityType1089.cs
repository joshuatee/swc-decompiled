using StaRTS.Main.Models.Battle;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1089 : $UnityType
	{
		public unsafe $UnityType1089()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 635652) = ldftn($Invoke0);
			*(data + 635680) = ldftn($Invoke1);
			*(data + 635708) = ldftn($Invoke2);
			*(data + 635736) = ldftn($Invoke3);
			*(data + 635764) = ldftn($Invoke4);
			*(data + 635792) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new HealthFragment((UIntPtr)0);
		}
	}
}
