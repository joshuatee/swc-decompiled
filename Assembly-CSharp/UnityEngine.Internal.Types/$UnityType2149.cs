using StaRTS.Main.Views.Animations;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2149 : $UnityType
	{
		public unsafe $UnityType2149()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 861052) = ldftn($Invoke0);
			*(data + 861080) = ldftn($Invoke1);
			*(data + 861108) = ldftn($Invoke2);
			*(data + 861136) = ldftn($Invoke3);
			*(data + 861164) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new DroidMoment((UIntPtr)0);
		}
	}
}
