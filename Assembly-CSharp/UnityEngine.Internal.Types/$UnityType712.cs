using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType712 : $UnityType
	{
		public unsafe $UnityType712()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 535608) = ldftn($Invoke0);
			*(data + 535636) = ldftn($Invoke1);
			*(data + 535664) = ldftn($Invoke2);
			*(data + 535692) = ldftn($Invoke3);
			*(data + 535720) = ldftn($Invoke4);
			*(data + 535748) = ldftn($Invoke5);
			*(data + 535776) = ldftn($Invoke6);
			*(data + 535804) = ldftn($Invoke7);
			*(data + 535832) = ldftn($Invoke8);
			*(data + 535860) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new GarbageCollector((UIntPtr)0);
		}
	}
}
