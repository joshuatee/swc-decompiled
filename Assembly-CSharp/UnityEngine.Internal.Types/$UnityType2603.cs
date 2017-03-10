using StaRTS.Utils.Tween;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2603 : $UnityType
	{
		public unsafe $UnityType2603()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 1000212) = ldftn($Invoke0);
			*(data + 1000240) = ldftn($Invoke1);
			*(data + 1000268) = ldftn($Invoke2);
		}

		public override object CreateInstance()
		{
			return new RotateTween((UIntPtr)0);
		}
	}
}
