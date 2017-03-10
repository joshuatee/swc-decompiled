using System;

namespace UnityEngine.Internal.Types
{
	public sealed class $UnityType31 : $UnityType
	{
		public unsafe $UnityType31()
		{
			*(UnityEngine.Internal.$Metadata.data + 351592) = ldftn($Invoke0);
		}

		public override object CreateInstance()
		{
			return new CompressionUtility((UIntPtr)0);
		}
	}
}
