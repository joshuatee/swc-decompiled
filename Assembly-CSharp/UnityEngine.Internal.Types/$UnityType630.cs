using StaRTS.FX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType630 : $UnityType
	{
		public unsafe $UnityType630()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 509288) = ldftn($Invoke0);
			*(data + 509316) = ldftn($Invoke1);
			*(data + 509344) = ldftn($Invoke2);
			*(data + 509372) = ldftn($Invoke3);
			*(data + 509400) = ldftn($Invoke4);
			*(data + 509428) = ldftn($Invoke5);
			*(data + 509456) = ldftn($Invoke6);
			*(data + 509484) = ldftn($Invoke7);
			*(data + 509512) = ldftn($Invoke8);
			*(data + 509540) = ldftn($Invoke9);
			*(data + 509568) = ldftn($Invoke10);
			*(data + 509596) = ldftn($Invoke11);
			*(data + 509624) = ldftn($Invoke12);
			*(data + 509652) = ldftn($Invoke13);
			*(data + 509680) = ldftn($Invoke14);
			*(data + 509708) = ldftn($Invoke15);
			*(data + 509736) = ldftn($Invoke16);
			*(data + 509764) = ldftn($Invoke17);
			*(data + 509792) = ldftn($Invoke18);
			*(data + 509820) = ldftn($Invoke19);
			*(data + 509848) = ldftn($Invoke20);
			*(data + 509876) = ldftn($Invoke21);
		}

		public override object CreateInstance()
		{
			return new FXManager((UIntPtr)0);
		}
	}
}
