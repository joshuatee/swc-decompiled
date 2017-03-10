using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2368 : $UnityType
	{
		public unsafe $UnityType2368()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 936400) = ldftn($Invoke0);
			*(data + 936428) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new TournamentTierChangedScreen((UIntPtr)0);
		}
	}
}
