using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType745 : $UnityType
	{
		public unsafe $UnityType745()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 546500) = ldftn($Invoke0);
			*(data + 546528) = ldftn($Invoke1);
			*(data + 546556) = ldftn($Invoke2);
			*(data + 546584) = ldftn($Invoke3);
			*(data + 546612) = ldftn($Invoke4);
			*(data + 546640) = ldftn($Invoke5);
			*(data + 546668) = ldftn($Invoke6);
			*(data + 546696) = ldftn($Invoke7);
			*(data + 546724) = ldftn($Invoke8);
			*(data + 546752) = ldftn($Invoke9);
			*(data + 546780) = ldftn($Invoke10);
			*(data + 546808) = ldftn($Invoke11);
			*(data + 546836) = ldftn($Invoke12);
			*(data + 546864) = ldftn($Invoke13);
			*(data + 546892) = ldftn($Invoke14);
			*(data + 546920) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new QuestController((UIntPtr)0);
		}
	}
}
