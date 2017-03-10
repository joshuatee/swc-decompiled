using StaRTS.Utils;
using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType2536 : $UnityType
	{
		public unsafe $UnityType2536()
		{
			*(UnityEngine.Internal.$Metadata.data + 990048) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new EncryptionUtil((UIntPtr)0);
		}
	}
}
