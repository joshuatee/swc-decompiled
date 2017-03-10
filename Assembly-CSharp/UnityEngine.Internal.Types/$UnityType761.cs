using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType761 : $UnityType
	{
		public unsafe $UnityType761()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 552100) = ldftn($Invoke0);
			*(data + 552128) = ldftn($Invoke1);
			*(data + 552156) = ldftn($Invoke2);
			*(data + 552184) = ldftn($Invoke3);
			*(data + 552212) = ldftn($Invoke4);
			*(data + 552240) = ldftn($Invoke5);
			*(data + 552268) = ldftn($Invoke6);
			*(data + 552296) = ldftn($Invoke7);
			*(data + 552324) = ldftn($Invoke8);
			*(data + 552352) = ldftn($Invoke9);
			*(data + 552380) = ldftn($Invoke10);
			*(data + 552408) = ldftn($Invoke11);
			*(data + 552436) = ldftn($Invoke12);
			*(data + 552464) = ldftn($Invoke13);
			*(data + 552492) = ldftn($Invoke14);
			*(data + 552520) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new ShuttleController((UIntPtr)0);
		}
	}
}
