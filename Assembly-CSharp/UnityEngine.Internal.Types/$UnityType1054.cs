using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1054 : $UnityType
	{
		public unsafe $UnityType1054()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 626300) = ldftn($Invoke0);
			*(data + 626328) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new OperationProgress((UIntPtr)0);
		}
	}
}
