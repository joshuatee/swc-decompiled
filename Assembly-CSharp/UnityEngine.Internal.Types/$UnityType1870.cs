using StaRTS.Main.Models.Squads.War;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1870 : $UnityType
	{
		public unsafe $UnityType1870()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 706940) = ldftn($Invoke0);
			*(data + 706968) = ldftn($Invoke1);
			*(data + 706996) = ldftn($Invoke2);
			*(data + 707024) = ldftn($Invoke3);
			*(data + 707052) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new SquadMemberWarData((UIntPtr)0);
		}
	}
}
