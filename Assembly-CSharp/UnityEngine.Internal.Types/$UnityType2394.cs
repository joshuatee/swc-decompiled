using StaRTS.Main.Views.UX.Screens.Leaderboard;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2394 : $UnityType
	{
		public unsafe $UnityType2394()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 945584) = ldftn($Invoke0);
			*(data + 945612) = ldftn($Invoke1);
			*(data + 945640) = ldftn($Invoke2);
			*(data + 945668) = ldftn($Invoke3);
			*(data + 945696) = ldftn($Invoke4);
			*(data + 945724) = ldftn($Invoke5);
			*(data + 945752) = ldftn($Invoke6);
			*(data + 945780) = ldftn($Invoke7);
			*(data + 945808) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new SquadJoinActionModule((UIntPtr)0);
		}
	}
}
