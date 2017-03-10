using StaRTS.Externals.Maker.MRSS;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType442 : $UnityType
	{
		public unsafe $UnityType442()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 449116) = ldftn($Invoke0);
			*(data + 449144) = ldftn($Invoke1);
			*(data + 449172) = ldftn($Invoke2);
			*(data + 449200) = ldftn($Invoke3);
			*(data + 449228) = ldftn($Invoke4);
			*(data + 449256) = ldftn($Invoke5);
			*(data + 449284) = ldftn($Invoke6);
			*(data + 449312) = ldftn($Invoke7);
			*(data + 449340) = ldftn($Invoke8);
			*(data + 449368) = ldftn($Invoke9);
			*(data + 449396) = ldftn($Invoke10);
			*(data + 449424) = ldftn($Invoke11);
		}

		public override object CreateInstance()
		{
			return new QueryData((UIntPtr)0);
		}
	}
}
