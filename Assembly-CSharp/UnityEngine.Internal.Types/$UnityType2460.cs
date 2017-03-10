using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2460 : $UnityType
	{
		public unsafe $UnityType2460()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 965296) = ldftn($Invoke0);
			*(data + 965324) = ldftn($Invoke1);
			*(data + 965352) = ldftn($Invoke2);
			*(data + 965380) = ldftn($Invoke3);
			*(data + 965408) = ldftn($Invoke4);
			*(data + 965436) = ldftn($Invoke5);
			*(data + 965464) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new SquadScreenWarButtonView((UIntPtr)0);
		}
	}
}
