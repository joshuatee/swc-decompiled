using StaRTS.Main.Views.UX.Screens.Leaderboard;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2393 : $UnityType
	{
		public unsafe $UnityType2393()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 945388) = ldftn($Invoke0);
			*(data + 945416) = ldftn($Invoke1);
			*(data + 945444) = ldftn($Invoke2);
			*(data + 945472) = ldftn($Invoke3);
			*(data + 945500) = ldftn($Invoke4);
			*(data + 945528) = ldftn($Invoke5);
			*(data + 945556) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new SquadInfoView((UIntPtr)0);
		}
	}
}
