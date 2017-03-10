using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2304 : $UnityType
	{
		public unsafe $UnityType2304()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 917220) = ldftn($Invoke0);
			*(data + 917248) = ldftn($Invoke1);
			*(data + 917276) = ldftn($Invoke2);
			*(data + 917304) = ldftn($Invoke3);
			*(data + 917332) = ldftn($Invoke4);
			*(data + 917360) = ldftn($Invoke5);
			*(data + 917388) = ldftn($Invoke6);
			*(data + 917416) = ldftn($Invoke7);
			*(data + 917444) = ldftn($Invoke8);
			*(data + 917472) = ldftn($Invoke9);
			*(data + 917500) = ldftn($Invoke10);
			*(data + 917528) = ldftn($Invoke11);
			*(data + 917556) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new FinishNowScreen((UIntPtr)0);
		}
	}
}
