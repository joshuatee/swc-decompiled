using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2179 : $UnityType
	{
		public unsafe $UnityType2179()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 868024) = ldftn($Invoke0);
			*(data + 868052) = ldftn($Invoke1);
			*(data + 868080) = ldftn($Invoke2);
			*(data + 868108) = ldftn($Invoke3);
			*(data + 868136) = ldftn($Invoke4);
			*(data + 868164) = ldftn($Invoke5);
			*(data + 868192) = ldftn($Invoke6);
			*(data + 868220) = ldftn($Invoke7);
			*(data + 868248) = ldftn($Invoke8);
		}

		public override object CreateInstance()
		{
			return new FlashingEntity((UIntPtr)0);
		}
	}
}
