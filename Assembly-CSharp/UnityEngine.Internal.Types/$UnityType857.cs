using StaRTS.Main.Controllers.Notifications;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType857 : $UnityType
	{
		public unsafe $UnityType857()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 579008) = ldftn($Invoke0);
			*(data + 579036) = ldftn($Invoke1);
			*(data + 579064) = ldftn($Invoke2);
			*(data + 579092) = ldftn($Invoke3);
			*(data + 579120) = ldftn($Invoke4);
			*(data + 579148) = ldftn($Invoke5);
			*(data + 579176) = ldftn($Invoke6);
			*(data + 579204) = ldftn($Invoke7);
			*(data + 579232) = ldftn($Invoke8);
			*(data + 579260) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new DefaultNotificationManager((UIntPtr)0);
		}
	}
}
