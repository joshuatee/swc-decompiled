using StaRTS.Main.Controllers.Notifications;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType861 : $UnityType
	{
		public unsafe $UnityType861()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 580548) = ldftn($Invoke0);
			*(data + 580576) = ldftn($Invoke1);
			*(data + 580604) = ldftn($Invoke2);
			*(data + 580632) = ldftn($Invoke3);
			*(data + 580660) = ldftn($Invoke4);
			*(data + 580688) = ldftn($Invoke5);
			*(data + 580716) = ldftn($Invoke6);
			*(data + 580744) = ldftn($Invoke7);
			*(data + 580772) = ldftn($Invoke8);
			*(data + 580800) = ldftn($Invoke9);
			*(data + 580828) = ldftn($Invoke10);
			*(data + 580856) = ldftn($Invoke11);
			*(data + 580884) = ldftn($Invoke12);
			*(data + 580912) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new NotificationObject((UIntPtr)0);
		}
	}
}
