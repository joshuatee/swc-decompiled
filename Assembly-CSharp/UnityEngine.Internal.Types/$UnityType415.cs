using StaRTS.Externals.GameServices;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType415 : $UnityType
	{
		public unsafe $UnityType415()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 443488) = ldftn($Invoke0);
			*(data + 443516) = ldftn($Invoke1);
			*(data + 443544) = ldftn($Invoke2);
			*(data + 443572) = ldftn($Invoke3);
			*(data + 443600) = ldftn($Invoke4);
			*(data + 443628) = ldftn($Invoke5);
			*(data + 443656) = ldftn($Invoke6);
			*(data + 443684) = ldftn($Invoke7);
			*(data + 443712) = ldftn($Invoke8);
			*(data + 443740) = ldftn($Invoke9);
			*(data + 443768) = ldftn($Invoke10);
			*(data + 443796) = ldftn($Invoke11);
			*(data + 443824) = ldftn($Invoke12);
			*(data + 443852) = ldftn($Invoke13);
			*(data + 443880) = ldftn($Invoke14);
			*(data + 443908) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new GameServicesHelper((UIntPtr)0);
		}
	}
}
