using StaRTS.Main.Controllers.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType916 : $UnityType
	{
		public unsafe $UnityType916()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 595976) = ldftn($Invoke0);
			*(data + 596004) = ldftn($Invoke1);
			*(data + 596032) = ldftn($Invoke2);
			*(data + 596060) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new TroopDonationTrackController((UIntPtr)0);
		}
	}
}
