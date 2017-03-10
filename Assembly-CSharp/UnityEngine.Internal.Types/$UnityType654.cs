using StaRTS.GameBoard;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType654 : $UnityType
	{
		public unsafe $UnityType654()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 515672) = ldftn($Invoke0);
			*(data + 515700) = ldftn($Invoke1);
			*(data + 515728) = ldftn($Invoke2);
			*(data + 515756) = ldftn($Invoke3);
			*(data + 515784) = ldftn($Invoke4);
			*(data + 515812) = ldftn($Invoke5);
			*(data + 515840) = ldftn($Invoke6);
			*(data + 515868) = ldftn($Invoke7);
			*(data + 515896) = ldftn($Invoke8);
			*(data + 515924) = ldftn($Invoke9);
		}

		public override object CreateInstance()
		{
			return new ConstraintRegion((UIntPtr)0);
		}
	}
}
