using StaRTS.Main.Views.UX.Controls;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2237 : $UnityType
	{
		public unsafe $UnityType2237()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 890536) = ldftn($Invoke0);
			*(data + 890564) = ldftn($Invoke1);
			*(data + 890592) = ldftn($Invoke2);
			*(data + 890620) = ldftn($Invoke3);
			*(data + 890648) = ldftn($Invoke4);
			*(data + 890676) = ldftn($Invoke5);
			*(data + 890704) = ldftn($Invoke6);
			*(data + 890732) = ldftn($Invoke7);
			*(data + 890760) = ldftn($Invoke8);
			*(data + 890788) = ldftn($Invoke9);
			*(data + 890816) = ldftn($Invoke10);
			*(data + 890844) = ldftn($Invoke11);
			*(data + 890872) = ldftn($Invoke12);
			*(data + 890900) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new SliderControl((UIntPtr)0);
		}
	}
}
