using StaRTS.Main.Views.UX.Screens.Leaderboard;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2388 : $UnityType
	{
		public unsafe $UnityType2388()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 943540) = ldftn($Invoke0);
			*(data + 943568) = ldftn($Invoke1);
			*(data + 943596) = ldftn($Invoke2);
			*(data + 943624) = ldftn($Invoke3);
			*(data + 943652) = ldftn($Invoke4);
			*(data + 943680) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new LeaderboardRowFacebookView((UIntPtr)0);
		}
	}
}
