using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2384 : $UnityType
	{
		public unsafe $UnityType2384()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 941692) = ldftn($Invoke0);
			*(data + 941720) = ldftn($Invoke1);
			*(data + 941748) = ldftn($Invoke2);
			*(data + 941776) = ldftn($Invoke3);
			*(data + 941804) = ldftn($Invoke4);
			*(data + 941832) = ldftn($Invoke5);
			*(data + 941860) = ldftn($Invoke6);
			*(data + 941888) = ldftn($Invoke7);
			*(data + 941916) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new YesNoScreen((UIntPtr)0);
		}
	}
}
