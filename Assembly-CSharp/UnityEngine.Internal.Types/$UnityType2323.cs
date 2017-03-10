using StaRTS.Main.Views.UX.Screens;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2323 : $UnityType
	{
		public unsafe $UnityType2323()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 921812) = ldftn($Invoke0);
			*(data + 921840) = ldftn($Invoke1);
			*(data + 921868) = ldftn($Invoke2);
			*(data + 921896) = ldftn($Invoke3);
			*(data + 921924) = ldftn($Invoke4);
			*(data + 921952) = ldftn($Invoke5);
			*(data + 921980) = ldftn($Invoke6);
			*(data + 922008) = ldftn($Invoke7);
			*(data + 922036) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new PayMeScreen((UIntPtr)0);
		}
	}
}
