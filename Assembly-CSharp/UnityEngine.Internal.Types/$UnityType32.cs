using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType32 : $UnityType
	{
		public unsafe $UnityType32()
		{
			*(UnityEngine.Internal.$Metadata.data + 351620) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CryptoUtility((UIntPtr)0);
		}
	}
}
