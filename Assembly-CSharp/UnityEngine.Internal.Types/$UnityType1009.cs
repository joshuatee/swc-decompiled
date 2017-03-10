using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1009 : $UnityType
	{
		public unsafe $UnityType1009()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 606504) = ldftn($Invoke0);
			*(data + 606532) = ldftn($Invoke1);
			*(data + 606560) = ldftn($Invoke2);
			*(data + 606588) = ldftn($Invoke3);
			*(data + 606616) = ldftn($Invoke4);
			*(data + 606644) = ldftn($Invoke5);
			*(data + 606672) = ldftn($Invoke6);
			*(data + 606700) = ldftn($Invoke7);
			*(data + 606728) = ldftn($Invoke8);
			*(data + 606756) = ldftn($Invoke9);
			*(data + 606784) = ldftn($Invoke10);
		}

		public override object CreateInstance()
		{
			return new Buff((UIntPtr)0);
		}
	}
}
