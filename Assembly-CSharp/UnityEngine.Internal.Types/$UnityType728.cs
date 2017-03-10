using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType728 : $UnityType
	{
		public unsafe $UnityType728()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 541516) = ldftn($Invoke0);
			*(data + 541544) = ldftn($Invoke1);
			*(data + 541572) = ldftn($Invoke2);
			*(data + 541600) = ldftn($Invoke3);
			*(data + 541628) = ldftn($Invoke4);
			*(data + 541656) = ldftn($Invoke5);
			*(data + 541684) = ldftn($Invoke6);
			*(data + 541712) = ldftn($Invoke7);
			*(data + 541740) = ldftn($Invoke8);
			*(data + 541768) = ldftn($Invoke9);
			*(data + 541796) = ldftn($Invoke10);
			*(data + 541824) = ldftn($Invoke11);
			*(data + 541852) = ldftn($Invoke12);
			*(data + 541880) = ldftn($Invoke13);
			*(data + 541908) = ldftn($Invoke14);
			*(data + 541936) = ldftn($Invoke15);
			*(data + 541964) = ldftn($Invoke16);
		}

		public override object CreateInstance()
		{
			return new LootController((UIntPtr)0);
		}
	}
}
