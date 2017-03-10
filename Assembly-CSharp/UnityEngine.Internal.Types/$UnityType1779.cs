using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1779 : $UnityType
	{
		public unsafe $UnityType1779()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 682244) = ldftn($Invoke0);
			*(data + 682272) = ldftn($Invoke1);
			*(data + 682300) = ldftn($Invoke2);
			*(data + 682328) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new ScoutTowerNode((UIntPtr)0);
		}
	}
}
