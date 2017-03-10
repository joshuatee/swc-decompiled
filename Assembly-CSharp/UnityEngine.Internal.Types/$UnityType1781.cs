using StaRTS.Main.Models.Entities.Nodes;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1781 : $UnityType
	{
		public unsafe $UnityType1781()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 682580) = ldftn($Invoke0);
			*(data + 682608) = ldftn($Invoke1);
			*(data + 682636) = ldftn($Invoke2);
			*(data + 682664) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new SquadBuildingNode((UIntPtr)0);
		}
	}
}
