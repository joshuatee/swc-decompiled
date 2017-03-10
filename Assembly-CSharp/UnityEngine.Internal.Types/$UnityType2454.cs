using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2454 : $UnityType
	{
		public unsafe $UnityType2454()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 961992) = ldftn($Invoke0);
			*(data + 962020) = ldftn($Invoke1);
			*(data + 962048) = ldftn($Invoke2);
			*(data + 962076) = ldftn($Invoke3);
			*(data + 962104) = ldftn($Invoke4);
			*(data + 962132) = ldftn($Invoke5);
			*(data + 962160) = ldftn($Invoke6);
			*(data + 962188) = ldftn($Invoke7);
			*(data + 962216) = ldftn($Invoke8);
			*(data + 962244) = ldftn($Invoke9);
			*(data + 962272) = ldftn($Invoke10);
			*(data + 962300) = ldftn($Invoke11);
			*(data + 962328) = ldftn($Invoke12);
			*(data + 962356) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new SquadScreenChatView((UIntPtr)0);
		}
	}
}
