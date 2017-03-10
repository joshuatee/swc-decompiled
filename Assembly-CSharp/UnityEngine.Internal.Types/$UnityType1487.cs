using StaRTS.Main.Models.Commands.Player.Building.Contracts;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1487 : $UnityType
	{
		public unsafe $UnityType1487()
		{
			*(UnityEngine.Internal.$Metadata.data + 656848) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new BuildingInstantUpgradeRequest((UIntPtr)0);
		}
	}
}
