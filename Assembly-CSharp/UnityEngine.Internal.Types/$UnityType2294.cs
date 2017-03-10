using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2294 : $UnityType
	{
		public unsafe $UnityType2294()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 913496) = ldftn($Invoke0);
			*(data + 913524) = ldftn($Invoke1);
			*(data + 913552) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new CrateRewardModalScreen((UIntPtr)0);
		}
	}
}
