using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2448 : $UnityType
	{
		public unsafe $UnityType2448()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 960060) = ldftn($Invoke0);
			*(data + 960088) = ldftn($Invoke1);
			*(data + 960116) = ldftn($Invoke2);
			*(data + 960144) = ldftn($Invoke3);
			*(data + 960172) = ldftn($Invoke4);
			*(data + 960200) = ldftn($Invoke5);
			*(data + 960228) = ldftn($Invoke6);
			*(data + 960256) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new SquadScreenActivationInfoView((UIntPtr)0);
		}
	}
}
