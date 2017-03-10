using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2250 : $UnityType
	{
		public unsafe $UnityType2250()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 895436) = ldftn($Invoke0);
			*(data + 895464) = ldftn($Invoke1);
			*(data + 895492) = ldftn($Invoke2);
			*(data + 895520) = ldftn($Invoke3);
			*(data + 895548) = ldftn($Invoke4);
			*(data + 895576) = ldftn($Invoke5);
			*(data + 895604) = ldftn($Invoke6);
			*(data + 895632) = ldftn($Invoke7);
			*(data + 895660) = ldftn($Invoke8);
			*(data + 895688) = ldftn($Invoke9);
			*(data + 895716) = ldftn($Invoke10);
			*(data + 895744) = ldftn($Invoke11);
			*(data + 895772) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new UXCheckbox((UIntPtr)0);
		}
	}
}
