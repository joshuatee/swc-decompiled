using StaRTS.Main.Models.ValueObjects;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1953 : $UnityType
	{
		public unsafe $UnityType1953()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 783464) = ldftn($Invoke0);
			*(data + 783492) = ldftn($Invoke1);
			*(data + 783520) = ldftn($Invoke2);
			*(data + 783548) = ldftn($Invoke3);
			*(data + 783576) = ldftn($Invoke4);
			*(data + 783604) = ldftn($Invoke5);
			*(data + 783632) = ldftn($Invoke6);
			*(data + 783660) = ldftn($Invoke7);
			*(data + 783688) = ldftn($Invoke8);
			*(data + 783716) = ldftn($Invoke9);
			*(data + 783744) = ldftn($Invoke10);
			*(data + 783772) = ldftn($Invoke11);
			*(data + 783800) = ldftn($Invoke12);
			*(data + 783828) = ldftn($Invoke13);
		}

		public override object CreateInstance()
		{
			return new SaleItemTypeVO((UIntPtr)0);
		}
	}
}
