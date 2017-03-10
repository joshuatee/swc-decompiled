using StaRTS.Main.Controllers.Notifications;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType863 : $UnityType
	{
		public unsafe $UnityType863()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 581024) = ldftn($Invoke0);
			*(data + 581052) = ldftn($Invoke1);
			*(data + 581080) = ldftn($Invoke2);
			*(data + 581108) = ldftn($Invoke3);
			*(data + 581136) = ldftn($Invoke4);
			*(data + 581164) = ldftn($Invoke5);
			*(data + 581192) = ldftn($Invoke6);
			*(data + 581220) = ldftn($Invoke7);
			*(data + 581248) = ldftn($Invoke8);
			*(data + 581276) = ldftn($Invoke9);
			*(data + 581304) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new WindowsNotificationManager((UIntPtr)0);
		}
	}
}
