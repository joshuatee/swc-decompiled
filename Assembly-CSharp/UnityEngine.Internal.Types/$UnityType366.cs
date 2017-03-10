using StaRTS.Audio;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType366 : $UnityType
	{
		public unsafe $UnityType366()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 432512) = ldftn($Invoke0);
			*(data + 432540) = ldftn($Invoke1);
			*(data + 432568) = ldftn($Invoke2);
			*(data + 432596) = ldftn($Invoke3);
			*(data + 432624) = ldftn($Invoke4);
		}

		public override object CreateInstance()
		{
			return new AudioFader((UIntPtr)0);
		}
	}
}
