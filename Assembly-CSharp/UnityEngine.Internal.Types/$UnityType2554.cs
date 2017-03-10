using StaRTS.Utils.Core;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2554 : $UnityType
	{
		public unsafe $UnityType2554()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 995340) = ldftn($Invoke0);
			*(data + 995368) = ldftn($Invoke1);
			*(data + 995396) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new RefCount((UIntPtr)0);
		}
	}
}
