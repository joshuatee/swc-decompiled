using StaRTS.Main.Controllers;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType677 : $UnityType
	{
		public unsafe $UnityType677()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 517632) = ldftn($Invoke0);
			*(data + 517660) = ldftn($Invoke1);
			*(data + 517688) = ldftn($Invoke2);
			*(data + 517716) = ldftn($Invoke3);
			*(data + 517744) = ldftn($Invoke4);
			*(data + 517772) = ldftn($Invoke5);
			*(data + 517800) = ldftn($Invoke6);
			*(data + 517828) = ldftn($Invoke7);
			*(data + 517856) = ldftn($Invoke8);
			*(data + 517884) = ldftn($Invoke9);
			*(data + 517912) = ldftn($Invoke10);
			*(data + 517940) = ldftn($Invoke11);
			*(data + 517968) = ldftn($Invoke12);
			*(data + 517996) = ldftn($Invoke13);
			*(data + 518024) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new AccountSyncController((UIntPtr)0);
		}
	}
}
