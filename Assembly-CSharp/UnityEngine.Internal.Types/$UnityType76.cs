using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType76 : $UnityType
	{
		public unsafe $UnityType76()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 361280) = ldftn($Invoke0);
			*(data + 361308) = ldftn($Invoke1);
			*(data + 361336) = ldftn($Invoke2);
			*(data + 361364) = ldftn($Invoke3);
			*(data + 361392) = ldftn($Invoke4);
			*(data + 361420) = ldftn($Invoke5);
			*(data + 361448) = ldftn($Invoke6);
			*(data + 361476) = ldftn($Invoke7);
		}

		public override object CreateInstance()
		{
			return new ScaleWobbleBehaviour((UIntPtr)0);
		}
	}
}
