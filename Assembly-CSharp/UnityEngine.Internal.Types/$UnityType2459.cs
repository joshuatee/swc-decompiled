using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2459 : $UnityType
	{
		public unsafe $UnityType2459()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 965184) = ldftn($Invoke0);
			*(data + 965212) = ldftn($Invoke1);
			*(data + 965240) = ldftn($Invoke2);
			*(data + 965268) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new SquadScreenUpgradeCelebPerkInfoView((UIntPtr)0);
		}
	}
}
