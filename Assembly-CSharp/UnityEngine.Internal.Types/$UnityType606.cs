using StaRTS.Externals.Manimal.TransferObjects.Request;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType606 : $UnityType
	{
		public unsafe $UnityType606()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 505060) = ldftn($Invoke0);
			*(data + 505088) = ldftn($Invoke1);
			*(data + 505116) = ldftn($Invoke2);
			*(data + 505144) = ldftn($Invoke3);
			*(data + 505172) = ldftn($Invoke4);
			*(data + 505200) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new Batch((UIntPtr)0);
		}
	}
}
