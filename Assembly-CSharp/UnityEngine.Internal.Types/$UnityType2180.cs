using StaRTS.Main.Views.Entities;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2180 : $UnityType
	{
		public unsafe $UnityType2180()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 868276) = ldftn($Invoke0);
			*(data + 868304) = ldftn($Invoke1);
			*(data + 868332) = ldftn($Invoke2);
			*(data + 868360) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new LinearSpline((UIntPtr)0);
		}
	}
}
