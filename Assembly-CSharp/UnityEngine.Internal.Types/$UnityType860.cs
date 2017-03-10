using StaRTS.Main.Controllers.Notifications;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType860 : $UnityType
	{
		public unsafe $UnityType860()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 580016) = ldftn($Invoke0);
			*(data + 580044) = ldftn($Invoke1);
			*(data + 580072) = ldftn($Invoke2);
			*(data + 580100) = ldftn($Invoke3);
			*(data + 580128) = ldftn($Invoke4);
			*(data + 580156) = ldftn($Invoke5);
			*(data + 580184) = ldftn($Invoke6);
			*(data + 580212) = ldftn($Invoke7);
			*(data + 580240) = ldftn($Invoke8);
			*(data + 580268) = ldftn($Invoke9);
			*(data + 580296) = ldftn($Invoke10);
			*(data + 580324) = ldftn($Invoke11);
			*(data + 580352) = ldftn($Invoke12);
			*(data + 580380) = ldftn($Invoke13);
			*(data + 580408) = ldftn($Invoke14);
			*(data + 580436) = ldftn($Invoke15);
			*(data + 580464) = ldftn($Invoke16);
			*(data + 580492) = ldftn($Invoke17);
			*(data + 580520) = ldftn($Invoke18);
		}

		public override object CreateInstance()
		{
			return new NotificationEventManager((UIntPtr)0);
		}
	}
}
