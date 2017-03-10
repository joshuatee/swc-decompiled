using StaRTS.Main.Views.UX.Screens.Leaderboard;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2389 : $UnityType
	{
		public unsafe $UnityType2389()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 943708) = ldftn($Invoke0);
			*(data + 943736) = ldftn($Invoke1);
			*(data + 943764) = ldftn($Invoke2);
			*(data + 943792) = ldftn($Invoke3);
			*(data + 943820) = ldftn($Invoke4);
			*(data + 943848) = ldftn($Invoke5);
			*(data + 943876) = ldftn($Invoke6);
			*(data + 943904) = ldftn($Invoke7);
			*(data + 943932) = ldftn($Invoke8);
			*(data + 943960) = ldftn($Invoke9);
			*(data + 943988) = ldftn($Invoke10);
			*(data + 944016) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new LeaderboardRowPlayerView((UIntPtr)0);
		}
	}
}
