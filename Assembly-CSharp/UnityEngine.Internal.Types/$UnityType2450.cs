using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2450 : $UnityType
	{
		public unsafe $UnityType2450()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 960844) = ldftn($Invoke0);
			*(data + 960872) = ldftn($Invoke1);
			*(data + 960900) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SquadScreenBasePerkInfoView((UIntPtr)0);
		}
	}
}
