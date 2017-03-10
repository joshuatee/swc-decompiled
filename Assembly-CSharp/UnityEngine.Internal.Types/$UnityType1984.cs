using StaRTS.Main.RUF;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1984 : $UnityType
	{
		public unsafe $UnityType1984()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 835152) = ldftn($Invoke0);
			*(data + 835180) = ldftn($Invoke1);
			*(data + 835208) = ldftn($Invoke2);
			*(data + 835236) = ldftn($Invoke3);
			*(data + 835264) = ldftn($Invoke4);
			*(data + 835292) = ldftn($Invoke5);
			*(data + 835320) = ldftn($Invoke6);
			*(data + 835348) = ldftn($Invoke7);
			*(data + 835376) = ldftn($Invoke8);
			*(data + 835404) = ldftn($Invoke9);
			*(data + 835432) = ldftn($Invoke10);
			*(data + 835460) = ldftn($Invoke11);
			*(data + 835488) = ldftn($Invoke12);
			*(data + 835516) = ldftn($Invoke13);
			*(data + 835544) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new RUFManager((UIntPtr)0);
		}
	}
}
