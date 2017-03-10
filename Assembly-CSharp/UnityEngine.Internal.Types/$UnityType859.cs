using StaRTS.Main.Controllers.Notifications;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType859 : $UnityType
	{
		public unsafe $UnityType859()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 579568) = ldftn($Invoke0);
			*(data + 579596) = ldftn($Invoke1);
			*(data + 579624) = ldftn($Invoke2);
			*(data + 579652) = ldftn($Invoke3);
			*(data + 579680) = ldftn($Invoke4);
			*(data + 579708) = ldftn($Invoke5);
			*(data + 579736) = ldftn($Invoke6);
			*(data + 579764) = ldftn($Invoke7);
			*(data + 579792) = ldftn($Invoke8);
			*(data + 579820) = ldftn($Invoke9);
			*(data + 579848) = ldftn($Invoke10);
			*(data + 579876) = ldftn($Invoke11);
			*(data + 579904) = ldftn($Invoke12);
			*(data + 579932) = ldftn($Invoke13);
			*(data + 579960) = ldftn($Invoke14);
			*(data + 579988) = ldftn($Invoke15);
		}

		public override object CreateInstance()
		{
			return new NotificationController((UIntPtr)0);
		}
	}
}
