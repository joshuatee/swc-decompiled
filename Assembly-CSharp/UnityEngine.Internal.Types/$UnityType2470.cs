using StaRTS.Main.Views.UX.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2470 : $UnityType
	{
		public unsafe $UnityType2470()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 969860) = ldftn($Invoke0);
			*(data + 969888) = ldftn($Invoke1);
			*(data + 969916) = ldftn($Invoke2);
			*(data + 969944) = ldftn($Invoke3);
			*(data + 969972) = ldftn($Invoke4);
			*(data + 970000) = ldftn($Invoke5);
			*(data + 970028) = ldftn($Invoke6);
		}

		public override object CreateInstance()
		{
			return new SquadWarBoardPlayerInfo((UIntPtr)0);
		}
	}
}
