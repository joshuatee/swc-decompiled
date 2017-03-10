using StaRTS.Main.Views.Animations;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2150 : $UnityType
	{
		public unsafe $UnityType2150()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 861192) = ldftn($Invoke0);
			*(data + 861220) = ldftn($Invoke1);
			*(data + 861248) = ldftn($Invoke2);
			*(data + 861276) = ldftn($Invoke3);
			*(data + 861304) = ldftn($Invoke4);
			*(data + 861332) = ldftn($Invoke5);
			*(data + 861360) = ldftn($Invoke6);
			*(data + 861388) = ldftn($Invoke7);
			*(data + 861416) = ldftn($Invoke8);
			*(data + 861444) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new IntroCameraAnimation((UIntPtr)0);
		}
	}
}
