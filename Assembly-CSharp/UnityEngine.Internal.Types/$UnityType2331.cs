using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2331 : $UnityType
	{
		public unsafe $UnityType2331()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 925732) = ldftn($Invoke0);
			*(data + 925760) = ldftn($Invoke1);
			*(data + 925788) = ldftn($Invoke2);
			*(data + 925816) = ldftn($Invoke3);
			*(data + 925844) = ldftn($Invoke4);
			*(data + 925872) = ldftn($Invoke5);
			*(data + 925900) = ldftn($Invoke6);
			*(data + 925928) = ldftn($Invoke7);
			*(data + 925956) = ldftn($Invoke8);
			*(data + 925984) = ldftn($Invoke9);
			*(data + 926012) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new RateAppScreen((UIntPtr)0);
		}
	}
}
