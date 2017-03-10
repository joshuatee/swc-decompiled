using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType192 : $UnityType
	{
		public unsafe $UnityType192()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 403308) = ldftn($Invoke0);
			*(data + 403336) = ldftn($Invoke1);
			*(data + 403364) = ldftn($Invoke2);
			*(data + 403392) = ldftn($Invoke3);
			*(data + 403420) = ldftn($Invoke4);
			*(data + 403448) = ldftn($Invoke5);
			*(data + 403476) = ldftn($Invoke6);
			*(data + 403504) = ldftn($Invoke7);
			*(data + 403532) = ldftn($Invoke8);
			*(data + 403560) = ldftn($Invoke9);
			*(data + 403588) = ldftn($Invoke10);
			*(data + 403616) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new UIRect.AnchorPoint((UIntPtr)0);
		}
	}
}
