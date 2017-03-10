using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1911 : $UnityType
	{
		public unsafe $UnityType1911()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 744432) = ldftn($Invoke0);
			*(data + 744460) = ldftn($Invoke1);
			*(data + 744488) = ldftn($Invoke2);
			*(data + 744516) = ldftn($Invoke3);
			*(data + 744544) = ldftn($Invoke4);
			*(data + 744572) = ldftn($Invoke5);
			*(data + 744600) = ldftn($Invoke6);
			*(data + 744628) = ldftn($Invoke7);
			*(data + 744656) = ldftn($Invoke8);
			*(data + 744684) = ldftn($Invoke9);
			*(data + 744712) = ldftn($Invoke10);
			*(data + 744740) = ldftn($Invoke11);
			*(data + 744768) = ldftn($Invoke12);
			*(data + 744796) = ldftn($Invoke13);
			*(data + 744824) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new DataCardTierVO((UIntPtr)0);
		}
	}
}
