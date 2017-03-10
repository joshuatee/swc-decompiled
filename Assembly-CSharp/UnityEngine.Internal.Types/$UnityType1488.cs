using StaRTS.Main.Models.Commands.Player.Building.Contracts;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1488 : $UnityType
	{
		public unsafe $UnityType1488()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 656876) = ldftn($Invoke0);
			*(data + 656904) = ldftn($Invoke1);
			*(data + 656932) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BuildingUpgradeAllWallsRequest((UIntPtr)0);
		}
	}
}
