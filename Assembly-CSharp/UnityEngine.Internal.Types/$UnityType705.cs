using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType705 : $UnityType
	{
		public unsafe $UnityType705()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 533648) = ldftn($Invoke0);
			*(data + 533676) = ldftn($Invoke1);
			*(data + 533704) = ldftn($Invoke2);
			*(data + 533732) = ldftn($Invoke3);
			*(data + 533760) = ldftn($Invoke4);
			*(data + 533788) = ldftn($Invoke5);
			*(data + 533816) = ldftn($Invoke6);
			*(data + 533844) = ldftn($Invoke7);
			*(data + 533872) = ldftn($Invoke8);
			*(data + 533900) = ldftn($Invoke9);
			*(data + 533928) = ldftn($Invoke10);
			*(data + 533956) = ldftn($Invoke11);
			*(data + 533984) = ldftn($Invoke12);
			*(data + 534012) = ldftn($Invoke13);
			*(data + 534040) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new DroidController((UIntPtr)0);
		}
	}
}
