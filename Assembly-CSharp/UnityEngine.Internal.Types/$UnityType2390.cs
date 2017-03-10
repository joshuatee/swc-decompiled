using StaRTS.Main.Views.UX.Screens.Leaderboard;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2390 : $UnityType
	{
		public unsafe $UnityType2390()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 944044) = ldftn($Invoke0);
			*(data + 944072) = ldftn($Invoke1);
			*(data + 944100) = ldftn($Invoke2);
			*(data + 944128) = ldftn($Invoke3);
			*(data + 944156) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new LeaderboardRowSquadInviteView((UIntPtr)0);
		}
	}
}
