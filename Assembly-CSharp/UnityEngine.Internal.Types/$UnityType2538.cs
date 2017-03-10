using StaRTS.Utils;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2538 : $UnityType
	{
		public unsafe $UnityType2538()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 990300) = ldftn($Invoke0);
			*(data + 990328) = ldftn($Invoke1);
			*(data + 990356) = ldftn($Invoke2);
			*(data + 990384) = ldftn($Invoke3);
			*(data + 990412) = ldftn($Invoke4);
			*(data + 990440) = ldftn($Invoke5);
			*(data + 990468) = ldftn($Invoke6);
			*(data + 990496) = ldftn($Invoke7);
			*(data + 990524) = ldftn($Invoke8);
			*(data + 990552) = ldftn($Invoke9);
			*(data + 990580) = ldftn($Invoke10);
			*(data + 990608) = ldftn($Invoke11);
			*(data + 990636) = ldftn($Invoke12);
			*(data + 990664) = ldftn($Invoke13);
			*(data + 990692) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new JoeFile((UIntPtr)0);
		}
	}
}
