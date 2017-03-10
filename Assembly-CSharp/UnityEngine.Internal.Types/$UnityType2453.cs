using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2453 : $UnityType
	{
		public unsafe $UnityType2453()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 961740) = ldftn($Invoke0);
			*(data + 961768) = ldftn($Invoke1);
			*(data + 961796) = ldftn($Invoke2);
			*(data + 961824) = ldftn($Invoke3);
			*(data + 961852) = ldftn($Invoke4);
			*(data + 961880) = ldftn($Invoke5);
			*(data + 961908) = ldftn($Invoke6);
			*(data + 961936) = ldftn($Invoke7);
			*(data + 961964) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new SquadScreenChatTroopDonationProgressView((UIntPtr)0);
		}
	}
}
