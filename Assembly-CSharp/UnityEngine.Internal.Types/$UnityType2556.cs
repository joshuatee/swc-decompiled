using StaRTS.Utils.Core;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2556 : $UnityType
	{
		public unsafe $UnityType2556()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 995452) = ldftn($Invoke0);
			*(data + 995480) = ldftn($Invoke1);
			*(data + 995508) = ldftn($Invoke2);
			*(data + 995536) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new WWWManager((UIntPtr)0);
		}
	}
}
