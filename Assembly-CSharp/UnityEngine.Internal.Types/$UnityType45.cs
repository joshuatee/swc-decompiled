using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType45 : $UnityType
	{
		public unsafe $UnityType45()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 353300) = ldftn($Invoke0);
			*(data + 353328) = ldftn($Invoke1);
			*(data + 353356) = ldftn($Invoke2);
			*(data + 353384) = ldftn($Invoke3);
			*(data + 353412) = ldftn($Invoke4);
			*(data + 353440) = ldftn($Invoke5);
			*(data + 353468) = ldftn($Invoke6);
			*(data + 353496) = ldftn($Invoke7);
			*(data + 353524) = ldftn($Invoke8);
			*(data + 353552) = ldftn($Invoke9);
			*(data + 353580) = ldftn($Invoke10);
			*(data + 353608) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new InAppPurchaseComponent((UIntPtr)0);
		}
	}
}
