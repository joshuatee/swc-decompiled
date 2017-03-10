using StaRTS.Main.Views.UX.Screens.Leaderboard;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2391 : $UnityType
	{
		public unsafe $UnityType2391()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 944184) = ldftn($Invoke0);
			*(data + 944212) = ldftn($Invoke1);
			*(data + 944240) = ldftn($Invoke2);
			*(data + 944268) = ldftn($Invoke3);
			*(data + 944296) = ldftn($Invoke4);
			*(data + 944324) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new LeaderboardRowSquadView((UIntPtr)0);
		}
	}
}
