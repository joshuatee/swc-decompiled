using StaRTS.Externals.EnvironmentManager;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType386 : $UnityType
	{
		public unsafe $UnityType386()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 437524) = ldftn($Invoke0);
			*(data + 437552) = ldftn($Invoke1);
			*(data + 437580) = ldftn($Invoke2);
			*(data + 437608) = ldftn($Invoke3);
			*(data + 437636) = ldftn($Invoke4);
			*(data + 437664) = ldftn($Invoke5);
			*(data + 437692) = ldftn($Invoke6);
			*(data + 437720) = ldftn($Invoke7);
			*(data + 437748) = ldftn($Invoke8);
			*(data + 437776) = ldftn($Invoke9);
			*(data + 437804) = ldftn($Invoke10);
			*(data + 437832) = ldftn($Invoke11);
			*(data + 437860) = ldftn($Invoke12);
			*(data + 437888) = ldftn($Invoke13);
			*(data + 437916) = ldftn($Invoke14);
			*(data + 437944) = ldftn($Invoke15);
			*(data + 437972) = ldftn($Invoke16);
			*(data + 438000) = ldftn($Invoke17);
			*(data + 438028) = ldftn($Invoke18);
		}

		public override object CreateInstance()
		{
			return new DefaultEnvironmentManager((UIntPtr)0);
		}
	}
}
