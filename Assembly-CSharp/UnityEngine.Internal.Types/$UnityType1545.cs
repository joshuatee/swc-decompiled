using StaRTS.Main.Models.Commands.Pvp;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1545 : $UnityType
	{
		public unsafe $UnityType1545()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 659144) = ldftn($Invoke0);
			*(data + 659172) = ldftn($Invoke1);
			*(data + 659200) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new PvpRevengeRequest((UIntPtr)0);
		}
	}
}
