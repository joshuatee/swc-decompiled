using StaRTS.Audio;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType363 : $UnityType
	{
		public unsafe $UnityType363()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 431700) = ldftn($Invoke0);
			*(data + 431728) = ldftn($Invoke1);
		}

		public override object CreateInstance()
		{
			return new AudioData((UIntPtr)0);
		}
	}
}
