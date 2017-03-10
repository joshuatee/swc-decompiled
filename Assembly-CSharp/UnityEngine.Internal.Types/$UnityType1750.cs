using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1750 : $UnityType
	{
		public unsafe $UnityType1750()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 676672) = ldftn($Invoke0);
			*(data + 676700) = ldftn($Invoke1);
			*(data + 676728) = ldftn($Invoke2);
			*(data + 676756) = ldftn($Invoke3);
			*(data + 676784) = ldftn($Invoke4);
			*(data + 676812) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new AreaTriggerBuildingNode((UIntPtr)0);
		}
	}
}
