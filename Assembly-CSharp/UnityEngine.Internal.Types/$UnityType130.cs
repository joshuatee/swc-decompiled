using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType130 : $UnityType
	{
		public unsafe $UnityType130()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 378864) = ldftn($Invoke0);
			*(data + 378892) = ldftn($Invoke1);
			*(data + 378920) = ldftn($Invoke2);
			*(data + 378948) = ldftn($Invoke3);
			*(data + 378976) = ldftn($Invoke4);
			*(data + 379004) = ldftn($Invoke5);
			*(data + 379032) = ldftn($Invoke6);
			*(data + 379060) = ldftn($Invoke7);
			*(data + 379088) = ldftn($Invoke8);
			*(data + 379116) = ldftn($Invoke9);
			*(data + 379144) = ldftn($Invoke10);
			*(data + 1525400) = ldftn($Get0);
			*(data + 1525404) = ldftn($Set0);
			*(data + 1525416) = ldftn($Get1);
			*(data + 1525420) = ldftn($Set1);
			*(data + 1525432) = ldftn($Get2);
			*(data + 1525436) = ldftn($Set2);
			*(data + 1525448) = ldftn($Get3);
			*(data + 1525452) = ldftn($Set3);
		}

		public override object CreateInstance()
		{
			return new UIButtonScale((UIntPtr)0);
		}
	}
}
