using StaRTS.Main.Models.Commands.Player.Building.Move;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1494 : $UnityType
	{
		public unsafe $UnityType1494()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 657100) = ldftn($Invoke0);
			*(data + 657128) = ldftn($Invoke1);
			*(data + 657156) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new BuildingMultiMoveRequest((UIntPtr)0);
		}
	}
}
