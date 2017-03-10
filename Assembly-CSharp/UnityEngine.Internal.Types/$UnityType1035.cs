using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1035 : $UnityType
	{
		public unsafe $UnityType1035()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 609556) = ldftn($Invoke0);
			*(data + 609584) = ldftn($Invoke1);
			*(data + 609612) = ldftn($Invoke2);
			*(data + 609640) = ldftn($Invoke3);
			*(data + 609668) = ldftn($Invoke4);
			*(data + 609696) = ldftn($Invoke5);
			*(data + 609724) = ldftn($Invoke6);
			*(data + 609752) = ldftn($Invoke7);
			*(data + 609780) = ldftn($Invoke8);
			*(data + 609808) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new EventTickerObject((UIntPtr)0);
		}
	}
}
