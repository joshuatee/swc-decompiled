using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2461 : $UnityType
	{
		public unsafe $UnityType2461()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 965492) = ldftn($Invoke0);
			*(data + 965520) = ldftn($Invoke1);
			*(data + 965548) = ldftn($Invoke2);
			*(data + 965576) = ldftn($Invoke3);
			*(data + 965604) = ldftn($Invoke4);
			*(data + 965632) = ldftn($Invoke5);
			*(data + 965660) = ldftn($Invoke6);
			*(data + 965688) = ldftn($Invoke7);
			*(data + 965716) = ldftn($Invoke8);
			*(data + 965744) = ldftn($Invoke9);
			*(data + 965772) = ldftn($Invoke10);
			*(data + 965800) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new SquadScreenWarLogView((UIntPtr)0);
		}
	}
}
