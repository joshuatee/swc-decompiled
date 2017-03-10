using StaRTS.Main.Views.UX;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2221 : $UnityType
	{
		public unsafe $UnityType2221()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 885300) = ldftn($Invoke0);
			*(data + 885328) = ldftn($Invoke1);
			*(data + 885356) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new RegionHighlight((UIntPtr)0);
		}
	}
}
