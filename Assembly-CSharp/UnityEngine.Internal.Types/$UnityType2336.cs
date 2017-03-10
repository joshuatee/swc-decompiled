using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2336 : $UnityType
	{
		public unsafe $UnityType2336()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 927748) = ldftn($Invoke0);
			*(data + 927776) = ldftn($Invoke1);
			*(data + 927804) = ldftn($Invoke2);
			*(data + 927832) = ldftn($Invoke3);
			*(data + 927860) = ldftn($Invoke4);
			*(data + 927888) = ldftn($Invoke5);
			*(data + 927916) = ldftn($Invoke6);
			*(data + 927944) = ldftn($Invoke7);
			*(data + 927972) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new SetCallsignScreen((UIntPtr)0);
		}
	}
}
