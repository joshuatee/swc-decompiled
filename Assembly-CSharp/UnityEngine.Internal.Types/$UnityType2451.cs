using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2451 : $UnityType
	{
		public unsafe $UnityType2451()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 960928) = ldftn($Invoke0);
			*(data + 960956) = ldftn($Invoke1);
			*(data + 960984) = ldftn($Invoke2);
			*(data + 961012) = ldftn($Invoke3);
			*(data + 961040) = ldftn($Invoke4);
			*(data + 961068) = ldftn($Invoke5);
			*(data + 961096) = ldftn($Invoke6);
			*(data + 961124) = ldftn($Invoke7);
			*(data + 961152) = ldftn($Invoke8);
			*(data + 961180) = ldftn($Invoke9);
			*(data + 961208) = ldftn($Invoke10);
			*(data + 961236) = ldftn($Invoke11);
			*(data + 961264) = ldftn($Invoke12);
			*(data + 961292) = ldftn($Invoke13);
			*(data + 961320) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new SquadScreenChatFilterView((UIntPtr)0);
		}
	}
}
