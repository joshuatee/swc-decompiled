using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1004 : $UnityType
	{
		public unsafe $UnityType1004()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 606336) = ldftn($Invoke0);
			*(data + 606364) = ldftn($Invoke1);
			*(data + 606392) = ldftn($Invoke2);
			*(data + 606420) = ldftn($Invoke3);
			*(data + 606448) = ldftn($Invoke4);
			*(data + 606476) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new AdminMessageData((UIntPtr)0);
		}
	}
}
