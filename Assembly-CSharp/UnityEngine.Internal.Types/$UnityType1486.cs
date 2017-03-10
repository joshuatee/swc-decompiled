using StaRTS.Main.Models.Commands.Player.Building.Contracts;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1486 : $UnityType
	{
		public unsafe $UnityType1486()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 656792) = ldftn($Invoke0);
			*(data + 656820) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new BuildingContractRequest((UIntPtr)0);
		}
	}
}
