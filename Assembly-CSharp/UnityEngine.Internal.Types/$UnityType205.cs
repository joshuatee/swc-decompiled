using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType205 : $UnityType
	{
		public unsafe $UnityType205()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 406640) = ldftn($Invoke0);
			*(data + 406668) = ldftn($Invoke1);
			*(data + 406696) = ldftn($Invoke2);
			*(data + 406724) = ldftn($Invoke3);
			*(data + 406752) = ldftn($Invoke4);
			*(data + 406780) = ldftn($Invoke5);
			*(data + 1528168) = ldftn($Get0);
			*(data + 1528172) = ldftn($Set0);
			*(data + 1528184) = ldftn($Get1);
			*(data + 1528188) = ldftn($Set1);
			*(data + 1528200) = ldftn($Get2);
			*(data + 1528204) = ldftn($Set2);
			*(data + 1528216) = ldftn($Get3);
			*(data + 1528220) = ldftn($Set3);
			*(data + 1528232) = ldftn($Get4);
			*(data + 1528236) = ldftn($Set4);
		}

		public override object CreateInstance()
		{
			return new UISnapshotPoint((UIntPtr)0);
		}
	}
}
