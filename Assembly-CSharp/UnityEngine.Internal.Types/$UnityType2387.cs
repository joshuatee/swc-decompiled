using StaRTS.Main.Views.UX.Screens.Leaderboard;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2387 : $UnityType
	{
		public unsafe $UnityType2387()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 943428) = ldftn($Invoke0);
			*(data + 943456) = ldftn($Invoke1);
			*(data + 943484) = ldftn($Invoke2);
			*(data + 943512) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new LeaderboardRowCreateSquadView((UIntPtr)0);
		}
	}
}
