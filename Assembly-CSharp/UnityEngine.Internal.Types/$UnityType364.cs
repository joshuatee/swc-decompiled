using StaRTS.Audio;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType364 : $UnityType
	{
		public unsafe $UnityType364()
		{
			IntPtr data = UnityEngine.Internal.$Metadata.data;
			*(data + 431756) = ldftn($Invoke0);
			*(data + 431784) = ldftn($Invoke1);
			*(data + 431812) = ldftn($Invoke2);
			*(data + 431840) = ldftn($Invoke3);
		}

		public override object CreateInstance()
		{
			return new AudioEffectLoop((UIntPtr)0);
		}
	}
}
