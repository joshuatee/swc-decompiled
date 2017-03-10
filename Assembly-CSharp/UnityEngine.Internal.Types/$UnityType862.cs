using StaRTS.Main.Controllers.Notifications;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType862 : $UnityType
	{
		public unsafe $UnityType862()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 580940) = ldftn($Invoke0);
			*(data + 580968) = ldftn($Invoke1);
			*(data + 580996) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new SocialPushNotificationController((UIntPtr)0);
		}
	}
}
