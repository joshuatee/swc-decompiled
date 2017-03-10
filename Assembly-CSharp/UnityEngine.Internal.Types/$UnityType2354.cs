using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2354 : $UnityType
	{
		public unsafe $UnityType2354()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 931472) = ldftn($Invoke0);
			*(data + 931500) = ldftn($Invoke1);
			*(data + 931528) = ldftn($Invoke2);
			*(data + 931556) = ldftn($Invoke3);
			*(data + 931584) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new SquadWarMatchMakeScreen((UIntPtr)0);
		}
	}
}
