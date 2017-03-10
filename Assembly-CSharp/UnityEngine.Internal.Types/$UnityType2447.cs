using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2447 : $UnityType
	{
		public unsafe $UnityType2447()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 959892) = ldftn($Invoke0);
			*(data + 959920) = ldftn($Invoke1);
			*(data + 959948) = ldftn($Invoke2);
			*(data + 959976) = ldftn($Invoke3);
			*(data + 960004) = ldftn($Invoke4);
			*(data + 960032) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new SquadLevelUpCelebrationScreen((UIntPtr)0);
		}
	}
}
