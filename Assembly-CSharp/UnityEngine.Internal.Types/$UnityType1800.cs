using StaRTS.Main.Models.Leaderboard;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1800 : $UnityType
	{
		public unsafe $UnityType1800()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 685324) = ldftn($Invoke0);
			*(data + 685352) = ldftn($Invoke1);
			*(data + 685380) = ldftn($Invoke2);
			*(data + 685408) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new LeaderboardBattleHistory((UIntPtr)0);
		}
	}
}
