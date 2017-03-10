using StaRTS.Main.Controllers.Holonet;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType847 : $UnityType
	{
		public unsafe $UnityType847()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 576264) = ldftn($Invoke0);
			*(data + 576292) = ldftn($Invoke1);
			*(data + 576320) = ldftn($Invoke2);
			*(data + 576348) = ldftn($Invoke3);
			*(data + 576376) = ldftn($Invoke4);
			*(data + 576404) = ldftn($Invoke5);
			*(data + 576432) = ldftn($Invoke6);
			*(data + 576460) = ldftn($Invoke7);
			*(data + 576488) = ldftn($Invoke8);
			*(data + 576516) = ldftn($Invoke9);
			*(data + 576544) = ldftn($Invoke10);
			*(data + 576572) = ldftn($Invoke11);
			*(data + 576600) = ldftn($Invoke12);
			*(data + 576628) = ldftn($Invoke13);
			*(data + 576656) = ldftn($Invoke14);
			*(data + 576684) = ldftn($Invoke15);
			*(data + 576712) = ldftn($Invoke16);
			*(data + 576740) = ldftn($Invoke17);
			*(data + 576768) = ldftn($Invoke18);
			*(data + 576796) = ldftn($Invoke19);
			*(data + 576824) = ldftn($Invoke20);
			*(data + 576852) = ldftn($Invoke21);
		}

		public override object CreateInstance()
		{
			return new TransmissionsHolonetController((UIntPtr)0);
		}
	}
}
