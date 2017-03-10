using StaRTS.Main.Models;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType1029 : $UnityType
	{
		public unsafe $UnityType1029()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 608660) = ldftn($Invoke0);
			*(data + 608688) = ldftn($Invoke1);
			*(data + 608716) = ldftn($Invoke2);
			*(data + 608744) = ldftn($Invoke3);
			*(data + 608772) = ldftn($Invoke4);
			*(data + 608800) = ldftn($Invoke5);
		}

		public override object CreateInstance()
		{
			return new Deployable((UIntPtr)0);
		}
	}
}
