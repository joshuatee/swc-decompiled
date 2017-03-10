using StaRTS.Utils.Diagnostics;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2562 : $UnityType
	{
		public unsafe $UnityType2562()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 995956) = ldftn($Invoke0);
			*(data + 995984) = ldftn($Invoke1);
			*(data + 996012) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new UnityLogAppender((UIntPtr)0);
		}
	}
}
