using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2452 : $UnityType
	{
		public unsafe $UnityType2452()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 961348) = ldftn($Invoke0);
			*(data + 961376) = ldftn($Invoke1);
			*(data + 961404) = ldftn($Invoke2);
			*(data + 961432) = ldftn($Invoke3);
			*(data + 961460) = ldftn($Invoke4);
			*(data + 961488) = ldftn($Invoke5);
			*(data + 961516) = ldftn($Invoke6);
			*(data + 961544) = ldftn($Invoke7);
			*(data + 961572) = ldftn($Invoke8);
			*(data + 961600) = ldftn($Invoke9);
			*(data + 961628) = ldftn($Invoke10);
			*(data + 961656) = ldftn($Invoke11);
			*(data + 961684) = ldftn($Invoke12);
			*(data + 961712) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new SquadScreenChatInputView((UIntPtr)0);
		}
	}
}
