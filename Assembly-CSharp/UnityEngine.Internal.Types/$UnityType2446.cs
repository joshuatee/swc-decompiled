using StaRTS.Main.Views.UX.Screens.Squads;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2446 : $UnityType
	{
		public unsafe $UnityType2446()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 959752) = ldftn($Invoke0);
			*(data + 959780) = ldftn($Invoke1);
			*(data + 959808) = ldftn($Invoke2);
			*(data + 959836) = ldftn($Invoke3);
			*(data + 959864) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new SquadJoinRequestScreen((UIntPtr)0);
		}
	}
}
