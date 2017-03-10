using StaRTS.Externals.FileManagement;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType407 : $UnityType
	{
		public unsafe $UnityType407()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 442200) = ldftn($Invoke0);
			*(data + 442228) = ldftn($Invoke1);
			*(data + 442256) = ldftn($Invoke2);
			*(data + 442284) = ldftn($Invoke3);
			*(data + 442312) = ldftn($Invoke4);
			*(data + 442340) = ldftn($Invoke5);
			*(data + 442368) = ldftn($Invoke6);
			*(data + 442396) = ldftn($Invoke7);
			*(data + 442424) = ldftn($Invoke8);
			*(data + 442452) = ldftn($Invoke9);
			*(data + 442480) = ldftn($Invoke10);
			*(data + 442508) = ldftn($Invoke11);
			*(data + 442536) = ldftn($Invoke12);
			*(data + 442564) = ldftn($Invoke13);
			*(data + 442592) = ldftn($Invoke14);
		}

		public override object CreateInstance()
		{
			return new FmsOptions((UIntPtr)0);
		}
	}
}
