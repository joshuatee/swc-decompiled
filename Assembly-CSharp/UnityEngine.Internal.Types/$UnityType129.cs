using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType129 : $UnityType
	{
		public unsafe $UnityType129()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 378556) = ldftn($Invoke0);
			*(data + 378584) = ldftn($Invoke1);
			*(data + 378612) = ldftn($Invoke2);
			*(data + 378640) = ldftn($Invoke3);
			*(data + 378668) = ldftn($Invoke4);
			*(data + 378696) = ldftn($Invoke5);
			*(data + 378724) = ldftn($Invoke6);
			*(data + 378752) = ldftn($Invoke7);
			*(data + 378780) = ldftn($Invoke8);
			*(data + 378808) = ldftn($Invoke9);
			*(data + 378836) = ldftn($Invoke10);
			*(data + 1525336) = ldftn($Get0);
			*(data + 1525340) = ldftn($Set0);
			*(data + 1525352) = ldftn($Get1);
			*(data + 1525356) = ldftn($Set1);
			*(data + 1525368) = ldftn($Get2);
			*(data + 1525372) = ldftn($Set2);
			*(data + 1525384) = ldftn($Get3);
			*(data + 1525388) = ldftn($Set3);
		}

		public override object CreateInstance()
		{
			return new UIButtonRotation((UIntPtr)0);
		}
	}
}
