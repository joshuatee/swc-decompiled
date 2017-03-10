using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType139 : $UnityType
	{
		public unsafe $UnityType139()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 381692) = ldftn($Invoke0);
			*(data + 381720) = ldftn($Invoke1);
			*(data + 381748) = ldftn($Invoke2);
			*(data + 381776) = ldftn($Invoke3);
			*(data + 381804) = ldftn($Invoke4);
			*(data + 381832) = ldftn($Invoke5);
			*(data + 381860) = ldftn($Invoke6);
			*(data + 381888) = ldftn($Invoke7);
			*(data + 381916) = ldftn($Invoke8);
			*(data + 381944) = ldftn($Invoke9);
			*(data + 381972) = ldftn($Invoke10);
			*(data + 382000) = ldftn($Invoke11);
			*(data + 382028) = ldftn($Invoke12);
			*(data + 382056) = ldftn($Invoke13);
			*(data + 1525768) = ldftn($Get0);
			*(data + 1525772) = ldftn($Set0);
			*(data + 1525784) = ldftn($Get1);
			*(data + 1525788) = ldftn($Set1);
		}

		public override object CreateInstance()
		{
			return new UIColorPicker((UIntPtr)0);
		}
	}
}
