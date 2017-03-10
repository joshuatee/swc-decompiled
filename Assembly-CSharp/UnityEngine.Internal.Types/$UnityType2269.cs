using StaRTS.Main.Views.UX.Elements;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2269 : $UnityType
	{
		public unsafe $UnityType2269()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 904480) = ldftn($Invoke0);
			*(data + 904508) = ldftn($Invoke1);
			*(data + 904536) = ldftn($Invoke2);
			*(data + 904564) = ldftn($Invoke3);
			*(data + 904592) = ldftn($Invoke4);
			*(data + 904620) = ldftn($Invoke5);
			*(data + 904648) = ldftn($Invoke6);
			*(data + 904676) = ldftn($Invoke7);
			*(data + 904704) = ldftn($Invoke8);
			*(data + 904732) = ldftn($Invoke9);
			*(data + 904760) = ldftn($Invoke10);
			*(data + 904788) = ldftn($Invoke11);
			*(data + 904816) = ldftn($Invoke12);
		}

		public override object CreateInstance()
		{
			return new UXTexture((UIntPtr)0);
		}
	}
}
