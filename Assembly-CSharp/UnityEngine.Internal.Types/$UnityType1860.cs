using StaRTS.Main.Models.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1860 : $UnityType
	{
		public unsafe $UnityType1860()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 705456) = ldftn($Invoke0);
			*(data + 705484) = ldftn($Invoke1);
			*(data + 705512) = ldftn($Invoke2);
			*(data + 705540) = ldftn($Invoke3);
			*(data + 705568) = ldftn($Invoke4);
			*(data + 705596) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new SquadDonatedTroop((UIntPtr)0);
		}
	}
}
