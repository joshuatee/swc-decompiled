using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType46 : $UnityType
	{
		public unsafe $UnityType46()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 353636) = ldftn($Invoke0);
			*(data + 353664) = ldftn($Invoke1);
			*(data + 353692) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new InAppPurchaseReceipt((UIntPtr)0);
		}
	}
}
