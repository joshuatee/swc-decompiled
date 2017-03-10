using StaRTS.Main.Models.Commands.Player.Building.Swap;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1500 : $UnityType
	{
		public unsafe $UnityType1500()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 657352) = ldftn($Invoke0);
			*(data + 657380) = ldftn($Invoke1);
			*(data + 657408) = ldftn($Invoke2);
			*(data + 657436) = ldftn($Invoke3);
			*(data + 657464) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new BuildingSwapRequest((UIntPtr)0);
		}
	}
}
